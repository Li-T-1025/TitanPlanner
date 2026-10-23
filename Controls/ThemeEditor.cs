using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MissionPlanner.Utilities;

namespace MissionPlanner.Controls
{
    public partial class ThemeEditor : Form
    {
        private static readonly Dictionary<string, string> ThemeColorCnMap = new Dictionary<string, string>
        {
            {"Background", "背景"},
            {"Control Background", "控件背景"},
            {"Text", "文本"},
            {"TextBox Background", "文本框背景"},
            {"Button Text", "按钮文本"},
            {"Button Background top", "按钮背景（顶部）"},
            {"Button Background bottom", "按钮背景（底部）"},
            {"ProgressBar Top", "进度条顶部"},
            {"ProgressBar Bottom", "进度条底部"},
            {"ProgressBar Outline", "进度条边框"},
            {"BannerColor1", "横幅颜色 1"},
            {"BannerColor2", "横幅颜色 2"},
            {"Disabled Button", "禁用按钮"},
            {"Button Mouseover", "按钮悬停"},
            {"Button Mousedown", "按钮按下"},
            {"CurrentPPM Background", "当前 PPM 背景"},
            {"Graph Chart Fill", "图表填充"},
            {"Graph Pane Fill", "图表面板填充"},
            {"Graph Legend Fill", "图例填充"},
            {"Rich Text Box text", "富文本框文本"},
            {"BackStageView Button Area", "后台视图按钮区域"},
            {"BSV Unselected Text", "BSV 未选中文本"},
            {"Horizontal ProgressBar", "水平进度条"},
            {"HUD text and drawings", "HUD 文本与图形"},
            {"HUD Ground top", "HUD 地面顶部"},
            {"HUD Ground bottom", "HUD 地面底部"},
            {"HUD Sky top", "HUD 天空顶部"},
            {"HUD Sky bottom", "HUD 天空底部"},
        };

        private static string ThemeColorCn(string en)
        {
            return ThemeColorCnMap.ContainsKey(en) ? ThemeColorCnMap[en] : en;
        }

        private static void SetupCnListBox(ListBox lb)
        {
            lb.DrawMode = DrawMode.OwnerDrawFixed;
            lb.DrawItem += (s, e) =>
            {
                e.DrawBackground();
                string txt = e.Index >= 0 && e.Index < lb.Items.Count && lb.Items[e.Index] != null
                    ? ThemeColorCn(lb.Items[e.Index].ToString())
                    : "";
                TextRenderer.DrawText(e.Graphics, txt, e.Font, e.Bounds,
                    e.ForeColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
                e.DrawFocusRectangle();
            };
        }

        public ThemeEditor()
        {
            InitializeComponent();
            Utilities.ThemeManager.ApplyThemeTo(this);

            SetupCnListBox(listboxThemeItems);
            listboxThemeItems.Items.Clear();
            foreach (ThemeColor coloritem in ThemeManager.thmColor.colors)
            {
                listboxThemeItems.Items.Add(coloritem.strColorItemName);
            }
            listboxThemeItems.SelectedIndex = 0;

            lblThemeName.Text = ThemeManager.thmColor.strThemeName;

            if (ThemeManager.thmColor.iconSet == ThemeColorTable.IconSet.HighContrastIconSet)
            {
                cbIconSet.Checked = true;
            } else
            {
                cbIconSet.Checked = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listboxThemeItems_SelectedIndexChanged(object sender, EventArgs e)
        {

            string itemName = listboxThemeItems.Items[listboxThemeItems.SelectedIndex].ToString();
            lblItemName.Text = ThemeColorCn(itemName);
            lblColorName.Text = ThemeManager.thmColor.colors.Find(x => x.strColorItemName.Contains(itemName)).clrColor.ToString() ;
            colorPatch.BackColor = ThemeManager.thmColor.colors.Find(x => x.strColorItemName.Contains(itemName)).clrColor;

        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            //Create a new theme based on the current theme. Nem theme will allways be a .mpusertheme
            string name = Path.GetFileNameWithoutExtension(ThemeManager.thmColor.strThemeName);

            if (DialogResult.Cancel == InputBox.Show(Strings.CreateThemeCopy, Strings.EnterNemThemeNameWithoutExtension, ref name))
                return;

            name = name + ".mpusertheme";
            var match = ThemeManager.ThemeNames.FirstOrDefault(stringToCheck => stringToCheck.Equals(name,StringComparison.OrdinalIgnoreCase));
            if (match != null)
            {
                CustomMessageBox.Show(Strings.UserThemeNamed + " " + Path.GetFileNameWithoutExtension(name) + " exists.");
                return;
            }

            string strFiletoSave = Settings.GetUserDataDirectory() + name;
            string tmpThemeName = ThemeManager.thmColor.strThemeName;
            try
            {
                ThemeManager.thmColor.strThemeName = name;
                ThemeManager.WriteToXmlFile<ThemeColorTable>(strFiletoSave,ThemeManager.thmColor);
                lblThemeName.Text = ThemeManager.thmColor.strThemeName;
            }
            catch
            {
                ThemeManager.thmColor.strThemeName = tmpThemeName;
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            ThemeManager.thmColor.SetTheme();
            ThemeManager.ApplyThemeTo(MainV2.instance);

        }

        private void colorPatch_dblclick(object sender, MouseEventArgs e)
        {
            colorSelectDialog.Color = colorPatch.BackColor;
            colorSelectDialog.ShowDialog();
            colorPatch.BackColor = colorSelectDialog.Color;
            string itemName = listboxThemeItems.SelectedItem.ToString();
            ThemeManager.thmColor.colors.Find(x => x.strColorItemName.Contains(itemName)).clrColor = colorSelectDialog.Color;
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            //Restore the theme from disk
            ThemeManager.LoadTheme(ThemeManager.thmColor.strThemeName);
            ThemeManager.ApplyThemeTo(MainV2.instance);
        }

        private void btnSaveApply_Click(object sender, EventArgs e)
        {

            if (ThemeManager.thmColor.strThemeName.Contains(".mpsystheme"))
                return;

            string strFiletoSave = Settings.GetUserDataDirectory() + ThemeManager.thmColor.strThemeName;
            try
            {
                ThemeManager.WriteToXmlFile<ThemeColorTable>(strFiletoSave, ThemeManager.thmColor);
                Settings.Instance["theme"] = ThemeManager.thmColor.strThemeName;
            }
            catch
            {
            }

            ThemeManager.LoadTheme(ThemeManager.thmColor.strThemeName);
            ThemeManager.ApplyThemeTo(MainV2.instance);
            CustomMessageBox.Show(Strings.MayNeedSelectAnotherTabRestart);
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //Get current theme and reapply
            ThemeManager.LoadTheme(Settings.Instance["theme"]);
            ThemeManager.ApplyThemeTo(MainV2.instance);
        }

        private void cbIconSet_CheckedChanged(object sender, EventArgs e)
        {
            if (cbIconSet.Checked)
            {
                ThemeManager.thmColor.iconSet = ThemeColorTable.IconSet.HighContrastIconSet; 
            }
            else
            {
                ThemeManager.thmColor.iconSet = ThemeColorTable.IconSet.BurnKermitIconSet;
            }
        }
    }
    
}
