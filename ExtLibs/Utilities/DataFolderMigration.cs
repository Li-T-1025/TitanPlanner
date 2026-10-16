using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;

namespace MissionPlanner.Utilities
{
    /// <summary>
    /// First-run copy of settings and data from the folders a previous build used
    /// (<see cref="Settings.PreviousAppConfigName"/>, e.g. "Mission Planner") into this build's folders
    /// (<see cref="Settings.AppConfigName"/>).
    ///
    /// Files are copied, never moved or deleted, so another program still using the old folders keeps
    /// working, and a file that already exists in the new folders is never overwritten. Settings are
    /// copied before startup continues; everything else (logs, map tiles, offline maps, elevation data,
    /// caches) copies on a background thread. A marker file in each new folder records that the copy
    /// finished; until it exists, every launch resumes where the last one stopped.
    /// </summary>
    public static class DataFolderMigration
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string MarkerName = ".migration-complete";
        private const string TempSuffix = ".migrating";
        private const long FreeSpaceMarginBytes = 512L * 1024 * 1024;

        private class FolderPair
        {
            public string From;
            public string To;
            public bool IsUserFolder;
        }

        /// <summary>
        /// Copies the old user folder's top-level files (config.xml, configuration profiles) and the
        /// offline-maps list. Call before anything reads <see cref="Settings.Instance"/>.
        /// </summary>
        public static void CopySettings()
        {
            try
            {
                foreach (var pair in PendingPairs())
                {
                    Directory.CreateDirectory(pair.To);

                    if (pair.IsUserFolder)
                    {
                        foreach (var file in Directory.GetFiles(pair.From))
                            CopyIfMissing(file, Path.Combine(pair.To, Path.GetFileName(file)));
                    }

                    // tiny, and has to arrive before the map writes a new one
                    var offlineMaps = Path.Combine(pair.From, "gmapcache", "offlinemaps.json");
                    if (File.Exists(offlineMaps))
                        CopyIfMissing(offlineMaps, Path.Combine(pair.To, "gmapcache", "offlinemaps.json"));
                }
            }
            catch (Exception ex)
            {
                log.Error("Copying settings from the previous data folder failed", ex);
            }
        }

        /// <summary>Copies everything else on a low-priority background thread.</summary>
        public static void StartBackgroundCopy()
        {
            List<FolderPair> pending;
            try
            {
                pending = PendingPairs();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return;
            }

            if (pending.Count == 0)
                return;

            new Thread(() => CopyAll(pending))
            {
                IsBackground = true,
                Name = "Data folder migration",
                Priority = ThreadPriority.BelowNormal
            }.Start();
        }

        /// <summary>Old/new folder pairs whose copy has not finished yet.</summary>
        private static List<FolderPair> PendingPairs()
        {
            var result = new List<FolderPair>();
            var previous = Settings.PreviousAppConfigName;
            var current = Settings.AppConfigName;
            if (string.IsNullOrEmpty(previous) || previous == current)
                return result;

            string newUser = Trim(Settings.GetUserDataDirectory());
            string newData = Trim(Settings.GetDataDirectory());
            string oldUser, oldData;
            try
            {
                Settings.AppConfigName = previous;
                oldUser = Trim(Settings.GetUserDataDirectory());
                oldData = Trim(Settings.GetDataDirectory());
            }
            finally
            {
                Settings.AppConfigName = current;
            }

            // on Mono both point at the same folder
            var pairs = new List<FolderPair> { new FolderPair { From = oldUser, To = newUser, IsUserFolder = true } };
            if (!string.Equals(oldData, oldUser, StringComparison.OrdinalIgnoreCase))
                pairs.Add(new FolderPair { From = oldData, To = newData });

            foreach (var pair in pairs)
            {
                if (Directory.Exists(pair.From) && !File.Exists(Path.Combine(pair.To, MarkerName)))
                    result.Add(pair);
            }

            return result;
        }

        private static void CopyAll(List<FolderPair> pairs)
        {
            try
            {
                var todo = new List<KeyValuePair<string, string>>();
                var bytesByRoot = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);

                foreach (var pair in pairs)
                {
                    foreach (var src in Directory.EnumerateFiles(pair.From, "*", SearchOption.AllDirectories))
                    {
                        if (src.EndsWith(TempSuffix, StringComparison.OrdinalIgnoreCase))
                            continue;
                        var dst = Path.Combine(pair.To, src.Substring(pair.From.Length).TrimStart('\\', '/'));
                        if (File.Exists(dst))
                            continue;

                        long size = 0;
                        try
                        {
                            size = new FileInfo(src).Length;
                        }
                        catch
                        {
                        }

                        todo.Add(new KeyValuePair<string, string>(src, dst));
                        var root = Path.GetPathRoot(pair.To) ?? "";
                        bytesByRoot[root] = (bytesByRoot.TryGetValue(root, out var b) ? b : 0) + size;
                    }
                }

                foreach (var kv in bytesByRoot)
                {
                    long free;
                    try
                    {
                        free = new DriveInfo(kv.Key).AvailableFreeSpace;
                    }
                    catch
                    {
                        continue; // unknown drive type: try anyway
                    }

                    if (free < kv.Value + FreeSpaceMarginBytes)
                    {
                        var msg = string.Format(
                            "Not enough free disk space to copy your data from the previous \"{0}\" folders into \"{1}\" ({2:0.0} GB needed, {3:0.0} GB free).\n\n" +
                            "Free up space and restart to finish the copy. Your old data is untouched in:\n{4}",
                            Settings.PreviousAppConfigName, Settings.AppConfigName, kv.Value / 1073741824.0, free / 1073741824.0,
                            string.Join("\n", pairs.Select(p => p.From)));
                        log.Warn(msg);
                        CustomMessageBox.Show(msg, "Data migration");
                        return; // no marker: retried on the next launch
                    }
                }

                log.InfoFormat("Data folder migration: copying {0} files ({1:0.0} MB)", todo.Count,
                    bytesByRoot.Values.Sum() / 1048576.0);

                int failed = 0, done = 0;
                foreach (var item in todo)
                {
                    try
                    {
                        CopyIfMissing(item.Key, item.Value);
                    }
                    catch (Exception ex)
                    {
                        failed++;
                        log.Debug("Data folder migration: could not copy " + item.Key, ex);
                    }

                    if (++done % 1000 == 0)
                        log.InfoFormat("Data folder migration: {0} / {1}", done, todo.Count);
                }

                if (failed > 0)
                {
                    log.WarnFormat("Data folder migration: {0} file(s) could not be copied; retrying on the next launch", failed);
                    return;
                }

                foreach (var pair in pairs)
                    File.WriteAllText(Path.Combine(pair.To, MarkerName),
                        "Copied from " + pair.From + " on " + DateTime.Now.ToString("u") + Environment.NewLine);
                log.Info("Data folder migration complete");
            }
            catch (Exception ex)
            {
                log.Error("Data folder migration failed; retrying on the next launch", ex);
            }
        }

        /// <summary>Copies via a temporary name so an interrupted copy never leaves a truncated file behind.</summary>
        private static void CopyIfMissing(string src, string dst)
        {
            if (File.Exists(dst))
                return;

            Directory.CreateDirectory(Path.GetDirectoryName(dst));
            var tmp = dst + TempSuffix;
            File.Copy(src, tmp, true);
            try
            {
                File.Move(tmp, dst);
            }
            catch (IOException) when (File.Exists(dst))
            {
                // the app created it meanwhile; keep the newer one
                File.Delete(tmp);
            }
        }

        private static string Trim(string path)
        {
            return path.TrimEnd('\\', '/');
        }
    }
}
