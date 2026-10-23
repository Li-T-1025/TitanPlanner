using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MissionPlanner.Utilities;
using MissionPlanner.Controls;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using MissionPlanner;
using System.Drawing;
using GMap.NET.WindowsForms;
using MissionPlanner.GCSViews;
using MissionPlanner.Maps;


namespace hudonoff
{
    public class Plugin : MissionPlanner.Plugin.Plugin
    {

        public override string Name
        {
            get { return "HUD"; }
        }

        public override string Version
        {
            get { return "0.2"; }
        }

        public override string Author
        {
            get { return "Michael Oborne"; }
        }

        public override bool Init()
        {
            return true;
        }

        private string configname = "HudItems";

        public override bool Loaded()
        {
            var rootbut = new ToolStripMenuItem(Strings.HUDItems);
            ToolStripItemCollection col = Host.FDMenuHud.Items;
            col.Add(rootbut);

            // key = HUD 属性名（存储/反射用），value = 存储于 config.xml 的英文标识
            // （保持英文 key 存储，与工程"显示中文、存储英文 key"决策一致）
            var items = new Dictionary<string,string>()
            {
                {"displayheading", "Heading"},
                {"displayspeed", "Speed"},
                {"displayalt", "Alt"},
                {"displayconninfo", "Connection"},
                {"displayxtrack", "X-Track"},
                {"displayrollpitch", "Roll/Pitch"},
                {"displaygps", "GPS"},
                {"batteryon", "Battery"},
                {"batteryon2", "Battery2"},
                {"displayekf", "EKF"},
                {"displayvibe", "Vibe"},
                {"displayprearm", "Prearm Status"},
                {"displayAOASSA", "AOA"},
            };

            // 菜单显示名称（中文化），仅用于界面展示
            var displayNames = new Dictionary<string,string>()
            {
                {"displayheading", Strings.HUDItemHeading},
                {"displayspeed", Strings.HUDItemSpeed},
                {"displayalt", Strings.HUDItemAlt},
                {"displayconninfo", Strings.HUDItemConnection},
                {"displayxtrack", Strings.HUDItemXTrack},
                {"displayrollpitch", Strings.HUDItemRollPitch},
                {"displaygps", Strings.HUDItemGPS},
                {"batteryon", Strings.HUDItemBattery},
                {"batteryon2", Strings.HUDItemBattery2},
                {"displayekf", Strings.HUDItemEKF},
                {"displayvibe", Strings.HUDItemVibe},
                {"displayprearm", Strings.HUDItemPrearm},
                {"displayAOASSA", Strings.HUDItemAOA},
            };

            var hide = Settings.Instance.GetList(configname);

            foreach (var item in items)
            {
                var but = new ToolStripMenuItem(displayNames[item.Key]);
                but.CheckOnClick = true;
                but.Checked = true;
                but.Click += (s,e)=>
                {
                    FlightData.myhud.GetType().GetProperty(item.Key).SetValue(FlightData.myhud, but.Checked);
                    if (!but.Checked)
                        Settings.Instance.AppendList(configname, item.Value);
                    if (but.Checked)
                        Settings.Instance.RemoveList(configname, item.Value);
                };
                if (hide.Contains(item.Value))
                {
                    but.Checked = false;
                    FlightData.myhud.GetType().GetProperty(item.Key).SetValue(FlightData.myhud, but.Checked);
                }
                rootbut.DropDownItems.Add(but);
            }

            return true;
        }



        public override bool Loop()
        {
            return true;
        }

        public override bool Exit()
        {
            return true;
        }
    }
}