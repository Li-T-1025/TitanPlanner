using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MissionPlanner.Controls
{
    public partial class OpenDroneID_Map_Status : UserControl
    {

        public static OpenDroneID_Map_Status Instance;
        public OpenDroneID_UI _parent_ODID { get; set; } = null;

        public OpenDroneID_Map_Status()
        {
            Instance = this;
            

            InitializeComponent();
        }

        public void setStatusOK()
        {
            LED_ODID_Status.Color = Color.Green;
            LBL_ODID_OK.Text = Strings.RemoteIDOK;
            LBL_ODID_reason.Text = Strings.DBLCLKEmergency;
        }

        public void setStatusAlert(string alertReason)
        {
            LED_ODID_Status.Color = Color.Red;
            LBL_ODID_OK.Text = Strings.RemoteIDFail;
            LBL_ODID_reason.Text = alertReason;
        }

        public void setStatusEmergency(string alertReason)
        {
            LED_ODID_Status.Color = Color.Red;
            //LED_ODID_Status.Blink(200); 
            LBL_ODID_OK.Text = Strings.RIDEmergency;
            LBL_ODID_reason.Text = alertReason;
        }

        private void OpenDroneID_Map_Status_DoubleClick(object sender, EventArgs e)
        {
            if (CustomMessageBox.Show(Strings.SureWantDeclareRemoteIDEmergency, Strings.SureWantDeclareRemoteIDEmergencyTitle, CustomMessageBox.MessageBoxButtons.YesNo) == CustomMessageBox.DialogResult.Yes)
            {
                try
                {
                    if (_parent_ODID != null)
                     _parent_ODID.setEmergencyFromMap();
                } catch
                {
                    Console.WriteLine("ODID - Error setting Emergency from Map");
                }
            }
        }

    }

    
}
