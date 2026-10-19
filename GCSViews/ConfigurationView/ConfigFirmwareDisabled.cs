using MissionPlanner.Controls;
using System;
using System.Windows.Forms;

namespace MissionPlanner.GCSViews.ConfigurationView
{
    public partial class ConfigFirmwareDisabled : MyUserControl, IActivate
    {
        public ConfigFirmwareDisabled()
        {
            InitializeComponent();
        }

        public void Activate()
        {
        }

        private void but_bootloaderupdate_Click(object sender, System.EventArgs e)
        {
            var mav = MainV2.comPort;

            if (!mav.BaseStream.IsOpen)
                return;

            if (CustomMessageBox.Show(Strings.SureWantUpgradeBootloaderCanBrick,
                Strings.SureWantUpgradeBootloaderCanBrickTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == (int) DialogResult.Yes)
                if (CustomMessageBox.Show(
                    Strings.SureWantUpgradeBootloaderCanBrick1,
                    Strings.SureWantUpgradeBootloaderCanBrickTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == (int) DialogResult.Yes)
                    try
                    {
                        if (mav.doCommand(MAVLink.MAV_CMD.FLASH_BOOTLOADER, 0, 0, 0, 0, 290876, 0, 0))
                        {
                            CustomMessageBox.Show(Strings.UpgradedBootloader);
                        }
                        else
                        {
                            CustomMessageBox.Show(Strings.FailedUpgradeBootloader);
                        }
                    }
                    catch (Exception ex)
                    {
                        CustomMessageBox.Show(ex.ToString(), Strings.ERROR);
                    }
        }
    }
}