
namespace MissionPlanner.Controls
{
    partial class OpenDroneID_UI
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenDroneID_UI));
            this.txt_UserID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TXT_UAS_ID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label4_info = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.LED_ArmedError = new Bulb.LedBulb();
            this.label13 = new System.Windows.Forms.Label();
            this.LED_RemoteID_Messages = new Bulb.LedBulb();
            this.LED_gps_valid = new Bulb.LedBulb();
            this.label14 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.CMB_uas_id_type = new System.Windows.Forms.ComboBox();
            this.CMB_uas_type = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.CMB_self_id_type = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label18_info = new System.Windows.Forms.Label();
            this.TXT_self_id_TXT = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ODOD_tabs = new System.Windows.Forms.TabControl();
            this.tabStatus = new System.Windows.Forms.TabPage();
            this.LBL_version = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.TXT_RID_Status_Msg = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TXT_ODID_Status = new System.Windows.Forms.RichTextBox();
            this.tab_uid = new System.Windows.Forms.TabPage();
            this.tab_ops = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.CMB_op_id_type = new System.Windows.Forms.ComboBox();
            this.nmea_GPS_1 = new MissionPlanner.NMEA_GPS_Connection();
            this.myODID_Status = new MissionPlanner.Controls.OpenDroneID_Map_Status();
            this.groupBox2.SuspendLayout();
            this.ODOD_tabs.SuspendLayout();
            this.tabStatus.SuspendLayout();
            this.tab_uid.SuspendLayout();
            this.tab_ops.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_UserID
            // 
            resources.ApplyResources(this.txt_UserID, "txt_UserID");
            this.txt_UserID.Name = "txt_UserID";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // TXT_UAS_ID
            // 
            resources.ApplyResources(this.TXT_UAS_ID, "TXT_UAS_ID");
            this.TXT_UAS_ID.Name = "TXT_UAS_ID";
            this.TXT_UAS_ID.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label4_info
            // 
            resources.ApplyResources(this.label4_info, "label4_info");
            this.label4_info.Name = "label4_info";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // LED_ArmedError
            // 
            this.LED_ArmedError.Color = System.Drawing.Color.White;
            resources.ApplyResources(this.LED_ArmedError, "LED_ArmedError");
            this.LED_ArmedError.Name = "LED_ArmedError";
            this.LED_ArmedError.On = true;
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // LED_RemoteID_Messages
            // 
            this.LED_RemoteID_Messages.Color = System.Drawing.Color.White;
            resources.ApplyResources(this.LED_RemoteID_Messages, "LED_RemoteID_Messages");
            this.LED_RemoteID_Messages.Name = "LED_RemoteID_Messages";
            this.LED_RemoteID_Messages.On = true;
            // 
            // LED_gps_valid
            // 
            this.LED_gps_valid.Color = System.Drawing.Color.White;
            resources.ApplyResources(this.LED_gps_valid, "LED_gps_valid");
            this.LED_gps_valid.Name = "LED_gps_valid";
            this.LED_gps_valid.On = true;
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // CMB_uas_id_type
            // 
            resources.ApplyResources(this.CMB_uas_id_type, "CMB_uas_id_type");
            this.CMB_uas_id_type.FormattingEnabled = true;
            this.CMB_uas_id_type.Name = "CMB_uas_id_type";
            // 
            // CMB_uas_type
            // 
            resources.ApplyResources(this.CMB_uas_type, "CMB_uas_type");
            this.CMB_uas_type.FormattingEnabled = true;
            this.CMB_uas_type.Name = "CMB_uas_type";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // CMB_self_id_type
            // 
            resources.ApplyResources(this.CMB_self_id_type, "CMB_self_id_type");
            this.CMB_self_id_type.FormattingEnabled = true;
            this.CMB_self_id_type.Name = "CMB_self_id_type";
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // label18_info
            // 
            resources.ApplyResources(this.label18_info, "label18_info");
            this.label18_info.Name = "label18_info";
            // 
            // TXT_self_id_TXT
            // 
            resources.ApplyResources(this.TXT_self_id_TXT, "TXT_self_id_TXT");
            this.TXT_self_id_TXT.Name = "TXT_self_id_TXT";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.LED_ArmedError);
            this.groupBox2.Controls.Add(this.LED_RemoteID_Messages);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.LED_gps_valid);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // ODOD_tabs
            // 
            this.ODOD_tabs.Controls.Add(this.tabStatus);
            this.ODOD_tabs.Controls.Add(this.tab_uid);
            this.ODOD_tabs.Controls.Add(this.tab_ops);
            resources.ApplyResources(this.ODOD_tabs, "ODOD_tabs");
            this.ODOD_tabs.Name = "ODOD_tabs";
            this.ODOD_tabs.SelectedIndex = 0;
            // 
            // tabStatus
            // 
            this.tabStatus.Controls.Add(this.LBL_version);
            this.tabStatus.Controls.Add(this.label8);
            this.tabStatus.Controls.Add(this.TXT_RID_Status_Msg);
            this.tabStatus.Controls.Add(this.label5);
            this.tabStatus.Controls.Add(this.TXT_ODID_Status);
            resources.ApplyResources(this.tabStatus, "tabStatus");
            this.tabStatus.Name = "tabStatus";
            this.tabStatus.UseVisualStyleBackColor = true;
            // 
            // LBL_version
            // 
            resources.ApplyResources(this.LBL_version, "LBL_version");
            this.LBL_version.Name = "LBL_version";
            this.LBL_version.DoubleClick += new System.EventHandler(this.LBL_version_DoubleClick);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // TXT_RID_Status_Msg
            // 
            resources.ApplyResources(this.TXT_RID_Status_Msg, "TXT_RID_Status_Msg");
            this.TXT_RID_Status_Msg.Name = "TXT_RID_Status_Msg";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // TXT_ODID_Status
            // 
            resources.ApplyResources(this.TXT_ODID_Status, "TXT_ODID_Status");
            this.TXT_ODID_Status.Name = "TXT_ODID_Status";
            // 
            // tab_uid
            // 
            this.tab_uid.Controls.Add(this.TXT_UAS_ID);
            this.tab_uid.Controls.Add(this.label2);
            this.tab_uid.Controls.Add(this.label4);
            this.tab_uid.Controls.Add(this.label4_info);
            this.tab_uid.Controls.Add(this.CMB_uas_id_type);
            this.tab_uid.Controls.Add(this.CMB_uas_type);
            this.tab_uid.Controls.Add(this.label3);
            resources.ApplyResources(this.tab_uid, "tab_uid");
            this.tab_uid.Name = "tab_uid";
            this.tab_uid.UseVisualStyleBackColor = true;
            // 
            // tab_ops
            // 
            this.tab_ops.Controls.Add(this.label7);
            this.tab_ops.Controls.Add(this.label6);
            this.tab_ops.Controls.Add(this.CMB_op_id_type);
            this.tab_ops.Controls.Add(this.label17);
            this.tab_ops.Controls.Add(this.label1);
            this.tab_ops.Controls.Add(this.txt_UserID);
            this.tab_ops.Controls.Add(this.CMB_self_id_type);
            this.tab_ops.Controls.Add(this.label18);
            this.tab_ops.Controls.Add(this.label18_info);
            this.tab_ops.Controls.Add(this.TXT_self_id_TXT);
            resources.ApplyResources(this.tab_ops, "tab_ops");
            this.tab_ops.Name = "tab_ops";
            this.tab_ops.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // CMB_op_id_type
            // 
            resources.ApplyResources(this.CMB_op_id_type, "CMB_op_id_type");
            this.CMB_op_id_type.FormattingEnabled = true;
            this.CMB_op_id_type.Name = "CMB_op_id_type";
            // 
            // nmea_GPS_1
            // 
            resources.ApplyResources(this.nmea_GPS_1, "nmea_GPS_1");
            this.nmea_GPS_1.Name = "nmea_GPS_1";
            // 
            // myODID_Status
            // 
            this.myODID_Status._parent_ODID = null;
            resources.ApplyResources(this.myODID_Status, "myODID_Status");
            this.myODID_Status.Name = "myODID_Status";
            // 
            // OpenDroneID_UI
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ODOD_tabs);
            this.Controls.Add(this.nmea_GPS_1);
            this.Controls.Add(this.myODID_Status);
            this.Controls.Add(this.groupBox2);
            this.Name = "OpenDroneID_UI";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ODOD_tabs.ResumeLayout(false);
            this.tabStatus.ResumeLayout(false);
            this.tabStatus.PerformLayout();
            this.tab_uid.ResumeLayout(false);
            this.tab_uid.PerformLayout();
            this.tab_ops.ResumeLayout(false);
            this.tab_ops.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txt_UserID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TXT_UAS_ID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label4_info;
        private System.Windows.Forms.Label label12;
        private Bulb.LedBulb LED_ArmedError;
        private System.Windows.Forms.Label label13;
        private Bulb.LedBulb LED_RemoteID_Messages;
        private Bulb.LedBulb LED_gps_valid;
        private System.Windows.Forms.Label label14;        
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox CMB_uas_id_type;
        private System.Windows.Forms.ComboBox CMB_uas_type;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox CMB_self_id_type;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label18_info;
        private System.Windows.Forms.TextBox TXT_self_id_TXT;
        private System.Windows.Forms.GroupBox groupBox2;
        private OpenDroneID_Map_Status myODID_Status;
        private NMEA_GPS_Connection nmea_GPS_1;
        private System.Windows.Forms.TabControl ODOD_tabs;
        private System.Windows.Forms.TabPage tab_uid;
        private System.Windows.Forms.TabPage tab_ops;
        private System.Windows.Forms.TabPage tabStatus;
        private System.Windows.Forms.RichTextBox TXT_ODID_Status;
        private System.Windows.Forms.TextBox TXT_RID_Status_Msg;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox CMB_op_id_type;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label LBL_version;
    }
}
