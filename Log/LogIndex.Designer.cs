namespace MissionPlanner.Log
{
    partial class LogIndex
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogIndex));
            this.objectListView1 = new BrightIdeasSoftware.FastObjectListView();
            this.olvColumnImage = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnDate = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumndir = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnFrame = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnSysid = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnduration = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnSize = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnHome = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnTimeInAir = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnDistTraveled = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnCamMSG = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.BUT_changedir = new MissionPlanner.Controls.MyButton();
            this.btnDeleteLog = new MissionPlanner.Controls.MyButton();
            this.lbStats = new MissionPlanner.Controls.MyLabel();
            this.but_defaultlogdir = new MissionPlanner.Controls.MyButton();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.SuspendLayout();
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.olvColumnImage);
            this.objectListView1.AllColumns.Add(this.olvColumnDate);
            this.objectListView1.AllColumns.Add(this.olvColumndir);
            this.objectListView1.AllColumns.Add(this.olvColumnFrame);
            this.objectListView1.AllColumns.Add(this.olvColumnSysid);
            this.objectListView1.AllColumns.Add(this.olvColumnduration);
            this.objectListView1.AllColumns.Add(this.olvColumnName);
            this.objectListView1.AllColumns.Add(this.olvColumnSize);
            this.objectListView1.AllColumns.Add(this.olvColumnHome);
            this.objectListView1.AllColumns.Add(this.olvColumnTimeInAir);
            this.objectListView1.AllColumns.Add(this.olvColumnDistTraveled);
            this.objectListView1.AllColumns.Add(this.olvColumnCamMSG);
            this.objectListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnImage,
            this.olvColumnDate,
            this.olvColumndir,
            this.olvColumnFrame,
            this.olvColumnSysid,
            this.olvColumnduration,
            this.olvColumnName,
            this.olvColumnSize,
            this.olvColumnHome,
            this.olvColumnTimeInAir,
            this.olvColumnDistTraveled,
            this.olvColumnCamMSG});
            this.objectListView1.FullRowSelect = true;
            this.objectListView1.HideSelection = false;
            this.objectListView1.Location = new System.Drawing.Point(12, 42);
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.RowHeight = 150;
            this.objectListView1.ShowGroups = false;
            this.objectListView1.Size = new System.Drawing.Size(1153, 460);
            this.objectListView1.TabIndex = 0;
            this.objectListView1.UseCellFormatEvents = true;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            this.objectListView1.VirtualMode = true;
            this.objectListView1.FormatCell += new System.EventHandler<BrightIdeasSoftware.FormatCellEventArgs>(this.objectListView1_FormatCell);
            this.objectListView1.SelectedIndexChanged += new System.EventHandler(this.objectListView1_SelectedIndexChanged);
            // 
            // olvColumnImage
            // 
            this.olvColumnImage.AspectName = "";
            this.olvColumnImage.CellPadding = null;
            this.olvColumnImage.Text = "";
            this.olvColumnImage.Width = 150;
            // 
            // olvColumnDate
            // 
            this.olvColumnDate.AspectName = "Date";
            this.olvColumnDate.AspectToStringFormat = "{0:yyyy/MM/dd hh\\:mm}";
            this.olvColumnDate.CellPadding = null;
            resources.ApplyResources(this.olvColumnDate, "olvColumnDate");
            this.olvColumnDate.Width = 100;
            // 
            // olvColumndir
            // 
            this.olvColumndir.AspectName = "Directory";
            this.olvColumndir.CellPadding = null;
            resources.ApplyResources(this.olvColumndir, "olvColumndir");
            this.olvColumndir.Width = 258;
            // 
            // olvColumnFrame
            // 
            this.olvColumnFrame.AspectName = "Frame";
            this.olvColumnFrame.CellPadding = null;
            resources.ApplyResources(this.olvColumnFrame, "olvColumnFrame");
            // 
            // olvColumnSysid
            // 
            this.olvColumnSysid.AspectName = "Aircraft";
            this.olvColumnSysid.CellPadding = null;
            resources.ApplyResources(this.olvColumnSysid, "olvColumnSysid");
            // 
            // olvColumnduration
            // 
            this.olvColumnduration.AspectName = "Duration";
            this.olvColumnduration.AspectToStringFormat = "{0:hh\\:mm\\:ss}";
            this.olvColumnduration.CellPadding = null;
            resources.ApplyResources(this.olvColumnduration, "olvColumnduration");
            this.olvColumnduration.Width = 100;
            // 
            // olvColumnName
            // 
            this.olvColumnName.AspectName = "Name";
            this.olvColumnName.CellPadding = null;
            resources.ApplyResources(this.olvColumnName, "olvColumnName");
            this.olvColumnName.Width = 178;
            // 
            // olvColumnSize
            // 
            this.olvColumnSize.AspectName = "Size";
            this.olvColumnSize.CellPadding = null;
            resources.ApplyResources(this.olvColumnSize, "olvColumnSize");
            // 
            // olvColumnHome
            // 
            this.olvColumnHome.AspectName = "Home";
            this.olvColumnHome.CellPadding = null;
            resources.ApplyResources(this.olvColumnHome, "olvColumnHome");
            // 
            // olvColumnTimeInAir
            // 
            this.olvColumnTimeInAir.AspectName = "TimeInAir";
            this.olvColumnTimeInAir.CellPadding = null;
            resources.ApplyResources(this.olvColumnTimeInAir, "olvColumnTimeInAir");
            // 
            // olvColumnDistTraveled
            // 
            this.olvColumnDistTraveled.AspectName = "DistTraveled";
            this.olvColumnDistTraveled.CellPadding = null;
            resources.ApplyResources(this.olvColumnDistTraveled, "olvColumnDistTraveled");
            // 
            // olvColumnCamMSG
            // 
            this.olvColumnCamMSG.AspectName = "CamMSG";
            this.olvColumnCamMSG.CellPadding = null;
            resources.ApplyResources(this.olvColumnCamMSG, "olvColumnCamMSG");
            // 
            // BUT_changedir
            // 
            this.BUT_changedir.AutoSize = true;
            this.BUT_changedir.Location = new System.Drawing.Point(117, 13);
            this.BUT_changedir.Name = "BUT_changedir";
            this.BUT_changedir.Size = new System.Drawing.Size(99, 23);
            this.BUT_changedir.TabIndex = 1;
            resources.ApplyResources(this.BUT_changedir, "BUT_changedir");
            this.BUT_changedir.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.BUT_changedir.UseVisualStyleBackColor = true;
            this.BUT_changedir.Click += new System.EventHandler(this.BUT_changedir_Click);
            // 
            // btnDeleteLog
            // 
            this.btnDeleteLog.Enabled = false;
            this.btnDeleteLog.Location = new System.Drawing.Point(1061, 13);
            this.btnDeleteLog.Name = "btnDeleteLog";
            this.btnDeleteLog.Size = new System.Drawing.Size(104, 23);
            this.btnDeleteLog.TabIndex = 2;
            resources.ApplyResources(this.btnDeleteLog, "btnDeleteLog");
            this.btnDeleteLog.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnDeleteLog.UseVisualStyleBackColor = true;
            this.btnDeleteLog.Click += new System.EventHandler(this.btnDeleteLog_Click);
            // 
            // lbStats
            // 
            this.lbStats.Location = new System.Drawing.Point(683, 12);
            this.lbStats.Name = "lbStats";
            this.lbStats.resize = false;
            this.lbStats.Size = new System.Drawing.Size(372, 23);
            this.lbStats.TabIndex = 3;
            resources.ApplyResources(this.lbStats, "lbStats");
            // 
            // but_defaultlogdir
            // 
            this.but_defaultlogdir.AutoSize = true;
            this.but_defaultlogdir.Location = new System.Drawing.Point(12, 13);
            this.but_defaultlogdir.Name = "but_defaultlogdir";
            this.but_defaultlogdir.Size = new System.Drawing.Size(99, 23);
            this.but_defaultlogdir.TabIndex = 4;
            resources.ApplyResources(this.but_defaultlogdir, "but_defaultlogdir");
            this.but_defaultlogdir.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.but_defaultlogdir.UseVisualStyleBackColor = true;
            this.but_defaultlogdir.Click += new System.EventHandler(this.but_defaultlogdir_Click);
            // 
            // LogIndex
            // 
            this.ClientSize = new System.Drawing.Size(1177, 514);
            this.Controls.Add(this.but_defaultlogdir);
            this.Controls.Add(this.lbStats);
            this.Controls.Add(this.btnDeleteLog);
            this.Controls.Add(this.BUT_changedir);
            this.Controls.Add(this.objectListView1);
            this.Name = "LogIndex";
            resources.ApplyResources(this, "$this");
            this.Load += new System.EventHandler(this.LogIndex_Load);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BrightIdeasSoftware.FastObjectListView objectListView1;
        private BrightIdeasSoftware.OLVColumn olvColumnImage;
        private BrightIdeasSoftware.OLVColumn olvColumnName;
        private BrightIdeasSoftware.OLVColumn olvColumnduration;
        private BrightIdeasSoftware.OLVColumn olvColumndir;
        private Controls.MyButton BUT_changedir;
        private BrightIdeasSoftware.OLVColumn olvColumnDate;
        private BrightIdeasSoftware.OLVColumn olvColumnSysid;
        private BrightIdeasSoftware.OLVColumn olvColumnSize;
        private BrightIdeasSoftware.OLVColumn olvColumnHome;
        private BrightIdeasSoftware.OLVColumn olvColumnTimeInAir;
        private BrightIdeasSoftware.OLVColumn olvColumnFrame;
        private BrightIdeasSoftware.OLVColumn olvColumnDistTraveled;
        private BrightIdeasSoftware.OLVColumn olvColumnCamMSG;
        private Controls.MyButton btnDeleteLog;
        private Controls.MyLabel lbStats;
        private Controls.MyButton but_defaultlogdir;
    }
}