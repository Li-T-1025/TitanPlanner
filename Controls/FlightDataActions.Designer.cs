namespace MissionPlanner.Controls
{
    partial class FlightDataActions
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlightDataActions));
            this.scrollPanel = new System.Windows.Forms.Panel();
            this.mainFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.grpCommand = new System.Windows.Forms.GroupBox();
            this.tableCommand = new System.Windows.Forms.TableLayoutPanel();
            this.cmbAction = new System.Windows.Forms.ComboBox();
            this.btnDoAction = new MissionPlanner.Controls.MyButton();
            this.cmbWaypoint = new System.Windows.Forms.ComboBox();
            this.btnSetWP = new MissionPlanner.Controls.MyButton();
            this.cmbModes = new System.Windows.Forms.ComboBox();
            this.btnSetMode = new MissionPlanner.Controls.MyButton();
            this.cmbMount = new System.Windows.Forms.ComboBox();
            this.btnSetMount = new MissionPlanner.Controls.MyButton();
            this.btnRestartMission = new MissionPlanner.Controls.MyButton();
            this.btnResumeMission = new MissionPlanner.Controls.MyButton();
            this.grpSetpoints = new System.Windows.Forms.GroupBox();
            this.tableSetpoints = new System.Windows.Forms.TableLayoutPanel();
            this.lblHomeAlt = new System.Windows.Forms.Label();
            this.btnHomeAlt = new MissionPlanner.Controls.MyButton();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.modSetSpeed = new MissionPlanner.Controls.ModifyandSet();
            this.lblAlt = new System.Windows.Forms.Label();
            this.modSetAlt = new MissionPlanner.Controls.ModifyandSet();
            this.lblLoiterRad = new System.Windows.Forms.Label();
            this.modSetLoiterRad = new MissionPlanner.Controls.ModifyandSet();
            this.grpTools = new System.Windows.Forms.GroupBox();
            this.tableTools = new System.Windows.Forms.TableLayoutPanel();
            this.btnRawSensor = new MissionPlanner.Controls.MyButton();
            this.btnJoystick = new MissionPlanner.Controls.MyButton();
            this.btnMessage = new MissionPlanner.Controls.MyButton();
            this.btnClearTrack = new MissionPlanner.Controls.MyButton();
            this.btnReboot = new MissionPlanner.Controls.MyButton();
            this.btnAbortLanding = new MissionPlanner.Controls.MyButton();
            this.grpPayload = new System.Windows.Forms.GroupBox();
            this.tablePayload = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxPitch = new System.Windows.Forms.GroupBox();
            this.trackBarPitch = new System.Windows.Forms.TrackBar();
            this.txtPitchPos = new System.Windows.Forms.TextBox();
            this.groupBoxYaw = new System.Windows.Forms.GroupBox();
            this.txtYawPos = new System.Windows.Forms.TextBox();
            this.trackBarYaw = new System.Windows.Forms.TrackBar();
            this.groupBoxRoll = new System.Windows.Forms.GroupBox();
            this.txtRollPos = new System.Windows.Forms.TextBox();
            this.trackBarRoll = new System.Windows.Forms.TrackBar();
            this.btnResetGimbal = new MissionPlanner.Controls.MyButton();
            this.btnGimbalVideo = new MissionPlanner.Controls.MyButton();
            this.scrollPanel.SuspendLayout();
            this.mainFlowLayout.SuspendLayout();
            this.grpCommand.SuspendLayout();
            this.tableCommand.SuspendLayout();
            this.grpSetpoints.SuspendLayout();
            this.tableSetpoints.SuspendLayout();
            this.grpTools.SuspendLayout();
            this.tableTools.SuspendLayout();
            this.grpPayload.SuspendLayout();
            this.tablePayload.SuspendLayout();
            this.groupBoxPitch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPitch)).BeginInit();
            this.groupBoxYaw.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarYaw)).BeginInit();
            this.groupBoxRoll.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRoll)).BeginInit();
            this.SuspendLayout();
            // 
            // scrollPanel
            // 
            resources.ApplyResources(this.scrollPanel, "scrollPanel");
            this.scrollPanel.Controls.Add(this.mainFlowLayout);
            this.scrollPanel.Name = "scrollPanel";
            // 
            // mainFlowLayout
            // 
            resources.ApplyResources(this.mainFlowLayout, "mainFlowLayout");
            this.mainFlowLayout.Controls.Add(this.grpCommand);
            this.mainFlowLayout.Controls.Add(this.grpSetpoints);
            this.mainFlowLayout.Controls.Add(this.grpTools);
            this.mainFlowLayout.Controls.Add(this.grpPayload);
            this.mainFlowLayout.Name = "mainFlowLayout";
            // 
            // grpCommand
            // 
            resources.ApplyResources(this.grpCommand, "grpCommand");
            this.grpCommand.Controls.Add(this.tableCommand);
            this.grpCommand.Name = "grpCommand";
            this.grpCommand.TabStop = false;
            // 
            // tableCommand
            // 
            resources.ApplyResources(this.tableCommand, "tableCommand");
            this.tableCommand.Controls.Add(this.cmbAction, 0, 0);
            this.tableCommand.Controls.Add(this.btnDoAction, 1, 0);
            this.tableCommand.Controls.Add(this.cmbWaypoint, 0, 1);
            this.tableCommand.Controls.Add(this.btnSetWP, 1, 1);
            this.tableCommand.Controls.Add(this.cmbModes, 0, 2);
            this.tableCommand.Controls.Add(this.btnSetMode, 1, 2);
            this.tableCommand.Controls.Add(this.cmbMount, 0, 3);
            this.tableCommand.Controls.Add(this.btnSetMount, 1, 3);
            this.tableCommand.Controls.Add(this.btnRestartMission, 0, 4);
            this.tableCommand.Controls.Add(this.btnResumeMission, 1, 4);
            this.tableCommand.Name = "tableCommand";
            // 
            // cmbAction
            // 
            resources.ApplyResources(this.cmbAction, "cmbAction");
            this.cmbAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAction.DropDownWidth = 250;
            this.cmbAction.FormattingEnabled = true;
            this.cmbAction.Name = "cmbAction";
            // 
            // btnDoAction
            // 
            resources.ApplyResources(this.btnDoAction, "btnDoAction");
            this.btnDoAction.Name = "btnDoAction";
            this.btnDoAction.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnDoAction.UseVisualStyleBackColor = true;
            // 
            // cmbWaypoint
            // 
            resources.ApplyResources(this.cmbWaypoint, "cmbWaypoint");
            this.cmbWaypoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWaypoint.DropDownWidth = 150;
            this.cmbWaypoint.FormattingEnabled = true;
            this.cmbWaypoint.Name = "cmbWaypoint";
            // 
            // btnSetWP
            // 
            resources.ApplyResources(this.btnSetWP, "btnSetWP");
            this.btnSetWP.Name = "btnSetWP";
            this.btnSetWP.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnSetWP.UseVisualStyleBackColor = true;
            // 
            // cmbModes
            // 
            resources.ApplyResources(this.cmbModes, "cmbModes");
            this.cmbModes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModes.DropDownWidth = 150;
            this.cmbModes.FormattingEnabled = true;
            this.cmbModes.Name = "cmbModes";
            // 
            // btnSetMode
            // 
            resources.ApplyResources(this.btnSetMode, "btnSetMode");
            this.btnSetMode.Name = "btnSetMode";
            this.btnSetMode.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnSetMode.UseVisualStyleBackColor = true;
            // 
            // cmbMount
            // 
            resources.ApplyResources(this.cmbMount, "cmbMount");
            this.cmbMount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMount.DropDownWidth = 150;
            this.cmbMount.FormattingEnabled = true;
            this.cmbMount.Name = "cmbMount";
            // 
            // btnSetMount
            // 
            resources.ApplyResources(this.btnSetMount, "btnSetMount");
            this.btnSetMount.Name = "btnSetMount";
            this.btnSetMount.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnSetMount.UseVisualStyleBackColor = true;
            // 
            // btnRestartMission
            // 
            resources.ApplyResources(this.btnRestartMission, "btnRestartMission");
            this.btnRestartMission.Name = "btnRestartMission";
            this.btnRestartMission.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnRestartMission.UseVisualStyleBackColor = true;
            // 
            // btnResumeMission
            // 
            resources.ApplyResources(this.btnResumeMission, "btnResumeMission");
            this.btnResumeMission.Name = "btnResumeMission";
            this.btnResumeMission.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnResumeMission.UseVisualStyleBackColor = true;
            // 
            // grpSetpoints
            // 
            resources.ApplyResources(this.grpSetpoints, "grpSetpoints");
            this.grpSetpoints.Controls.Add(this.tableSetpoints);
            this.grpSetpoints.Name = "grpSetpoints";
            this.grpSetpoints.TabStop = false;
            // 
            // tableSetpoints
            // 
            resources.ApplyResources(this.tableSetpoints, "tableSetpoints");
            this.tableSetpoints.Controls.Add(this.lblHomeAlt, 0, 0);
            this.tableSetpoints.Controls.Add(this.btnHomeAlt, 1, 0);
            this.tableSetpoints.Controls.Add(this.lblSpeed, 0, 1);
            this.tableSetpoints.Controls.Add(this.modSetSpeed, 1, 1);
            this.tableSetpoints.Controls.Add(this.lblAlt, 0, 2);
            this.tableSetpoints.Controls.Add(this.modSetAlt, 1, 2);
            this.tableSetpoints.Controls.Add(this.lblLoiterRad, 0, 3);
            this.tableSetpoints.Controls.Add(this.modSetLoiterRad, 1, 3);
            this.tableSetpoints.Name = "tableSetpoints";
            // 
            // lblHomeAlt
            // 
            resources.ApplyResources(this.lblHomeAlt, "lblHomeAlt");
            this.lblHomeAlt.Name = "lblHomeAlt";
            // 
            // btnHomeAlt
            // 
            resources.ApplyResources(this.btnHomeAlt, "btnHomeAlt");
            this.btnHomeAlt.Name = "btnHomeAlt";
            this.btnHomeAlt.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnHomeAlt.UseVisualStyleBackColor = true;
            // 
            // lblSpeed
            // 
            resources.ApplyResources(this.lblSpeed, "lblSpeed");
            this.lblSpeed.Name = "lblSpeed";
            // 
            // modSetSpeed
            // 
            resources.ApplyResources(this.modSetSpeed, "modSetSpeed");
            this.modSetSpeed.ButtonText = "应用";
            this.modSetSpeed.DecimalPlaces = 0;
            this.modSetSpeed.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.modSetSpeed.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.modSetSpeed.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.modSetSpeed.Name = "modSetSpeed";
            this.modSetSpeed.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // lblAlt
            // 
            resources.ApplyResources(this.lblAlt, "lblAlt");
            this.lblAlt.Name = "lblAlt";
            // 
            // modSetAlt
            // 
            resources.ApplyResources(this.modSetAlt, "modSetAlt");
            this.modSetAlt.ButtonText = "应用";
            this.modSetAlt.DecimalPlaces = 0;
            this.modSetAlt.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.modSetAlt.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.modSetAlt.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.modSetAlt.Name = "modSetAlt";
            this.modSetAlt.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // lblLoiterRad
            // 
            resources.ApplyResources(this.lblLoiterRad, "lblLoiterRad");
            this.lblLoiterRad.Name = "lblLoiterRad";
            // 
            // modSetLoiterRad
            // 
            resources.ApplyResources(this.modSetLoiterRad, "modSetLoiterRad");
            this.modSetLoiterRad.ButtonText = "应用";
            this.modSetLoiterRad.DecimalPlaces = 0;
            this.modSetLoiterRad.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.modSetLoiterRad.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.modSetLoiterRad.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.modSetLoiterRad.Name = "modSetLoiterRad";
            this.modSetLoiterRad.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // grpTools
            // 
            resources.ApplyResources(this.grpTools, "grpTools");
            this.grpTools.Controls.Add(this.tableTools);
            this.grpTools.Name = "grpTools";
            this.grpTools.TabStop = false;
            // 
            // tableTools
            // 
            resources.ApplyResources(this.tableTools, "tableTools");
            this.tableTools.Controls.Add(this.btnRawSensor, 0, 0);
            this.tableTools.Controls.Add(this.btnJoystick, 1, 0);
            this.tableTools.Controls.Add(this.btnMessage, 0, 1);
            this.tableTools.Controls.Add(this.btnClearTrack, 1, 1);
            this.tableTools.Controls.Add(this.btnReboot, 0, 2);
            this.tableTools.Controls.Add(this.btnAbortLanding, 1, 2);
            this.tableTools.Name = "tableTools";
            // 
            // btnRawSensor
            // 
            resources.ApplyResources(this.btnRawSensor, "btnRawSensor");
            this.btnRawSensor.Name = "btnRawSensor";
            this.btnRawSensor.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnRawSensor.UseVisualStyleBackColor = true;
            // 
            // btnJoystick
            // 
            resources.ApplyResources(this.btnJoystick, "btnJoystick");
            this.btnJoystick.Name = "btnJoystick";
            this.btnJoystick.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnJoystick.UseVisualStyleBackColor = true;
            // 
            // btnMessage
            // 
            resources.ApplyResources(this.btnMessage, "btnMessage");
            this.btnMessage.Name = "btnMessage";
            this.btnMessage.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnMessage.UseVisualStyleBackColor = true;
            // 
            // btnClearTrack
            // 
            resources.ApplyResources(this.btnClearTrack, "btnClearTrack");
            this.btnClearTrack.Name = "btnClearTrack";
            this.btnClearTrack.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnClearTrack.UseVisualStyleBackColor = true;
            // 
            // btnReboot
            // 
            resources.ApplyResources(this.btnReboot, "btnReboot");
            this.btnReboot.Name = "btnReboot";
            this.btnReboot.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnReboot.UseVisualStyleBackColor = true;
            // 
            // btnAbortLanding
            // 
            resources.ApplyResources(this.btnAbortLanding, "btnAbortLanding");
            this.btnAbortLanding.Name = "btnAbortLanding";
            this.btnAbortLanding.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnAbortLanding.UseVisualStyleBackColor = true;
            // 
            // grpPayload
            // 
            resources.ApplyResources(this.grpPayload, "grpPayload");
            this.grpPayload.Controls.Add(this.tablePayload);
            this.grpPayload.Name = "grpPayload";
            this.grpPayload.TabStop = false;
            // 
            // tablePayload
            // 
            resources.ApplyResources(this.tablePayload, "tablePayload");
            this.tablePayload.Controls.Add(this.groupBoxPitch, 0, 0);
            this.tablePayload.Controls.Add(this.groupBoxYaw, 1, 0);
            this.tablePayload.Controls.Add(this.groupBoxRoll, 1, 1);
            this.tablePayload.Controls.Add(this.btnResetGimbal, 0, 2);
            this.tablePayload.Controls.Add(this.btnGimbalVideo, 1, 2);
            this.tablePayload.Name = "tablePayload";
            // 
            // groupBoxPitch
            // 
            resources.ApplyResources(this.groupBoxPitch, "groupBoxPitch");
            this.groupBoxPitch.Controls.Add(this.trackBarPitch);
            this.groupBoxPitch.Controls.Add(this.txtPitchPos);
            this.groupBoxPitch.Name = "groupBoxPitch";
            this.tablePayload.SetRowSpan(this.groupBoxPitch, 2);
            this.groupBoxPitch.TabStop = false;
            // 
            // trackBarPitch
            // 
            resources.ApplyResources(this.trackBarPitch, "trackBarPitch");
            this.trackBarPitch.LargeChange = 10;
            this.trackBarPitch.Maximum = 90;
            this.trackBarPitch.Minimum = -90;
            this.trackBarPitch.Name = "trackBarPitch";
            this.trackBarPitch.SmallChange = 5;
            this.trackBarPitch.TickFrequency = 10;
            // 
            // txtPitchPos
            // 
            resources.ApplyResources(this.txtPitchPos, "txtPitchPos");
            this.txtPitchPos.Name = "txtPitchPos";
            // 
            // groupBoxYaw
            // 
            resources.ApplyResources(this.groupBoxYaw, "groupBoxYaw");
            this.groupBoxYaw.Controls.Add(this.txtYawPos);
            this.groupBoxYaw.Controls.Add(this.trackBarYaw);
            this.groupBoxYaw.Name = "groupBoxYaw";
            this.groupBoxYaw.TabStop = false;
            // 
            // txtYawPos
            // 
            resources.ApplyResources(this.txtYawPos, "txtYawPos");
            this.txtYawPos.Name = "txtYawPos";
            // 
            // trackBarYaw
            // 
            resources.ApplyResources(this.trackBarYaw, "trackBarYaw");
            this.trackBarYaw.LargeChange = 10;
            this.trackBarYaw.Maximum = 180;
            this.trackBarYaw.Minimum = -180;
            this.trackBarYaw.Name = "trackBarYaw";
            this.trackBarYaw.TickFrequency = 10;
            // 
            // groupBoxRoll
            // 
            resources.ApplyResources(this.groupBoxRoll, "groupBoxRoll");
            this.groupBoxRoll.Controls.Add(this.txtRollPos);
            this.groupBoxRoll.Controls.Add(this.trackBarRoll);
            this.groupBoxRoll.Name = "groupBoxRoll";
            this.groupBoxRoll.TabStop = false;
            // 
            // txtRollPos
            // 
            resources.ApplyResources(this.txtRollPos, "txtRollPos");
            this.txtRollPos.Name = "txtRollPos";
            // 
            // trackBarRoll
            // 
            resources.ApplyResources(this.trackBarRoll, "trackBarRoll");
            this.trackBarRoll.LargeChange = 10;
            this.trackBarRoll.Maximum = 90;
            this.trackBarRoll.Minimum = -90;
            this.trackBarRoll.Name = "trackBarRoll";
            this.trackBarRoll.TickFrequency = 10;
            // 
            // btnResetGimbal
            // 
            resources.ApplyResources(this.btnResetGimbal, "btnResetGimbal");
            this.btnResetGimbal.Name = "btnResetGimbal";
            this.btnResetGimbal.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnResetGimbal.UseVisualStyleBackColor = true;
            // 
            // btnGimbalVideo
            // 
            resources.ApplyResources(this.btnGimbalVideo, "btnGimbalVideo");
            this.btnGimbalVideo.Name = "btnGimbalVideo";
            this.btnGimbalVideo.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnGimbalVideo.UseVisualStyleBackColor = true;
            // 
            // FlightDataActions
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scrollPanel);
            this.Name = "FlightDataActions";
            this.scrollPanel.ResumeLayout(false);
            this.scrollPanel.PerformLayout();
            this.mainFlowLayout.ResumeLayout(false);
            this.grpCommand.ResumeLayout(false);
            this.tableCommand.ResumeLayout(false);
            this.grpSetpoints.ResumeLayout(false);
            this.tableSetpoints.ResumeLayout(false);
            this.tableSetpoints.PerformLayout();
            this.grpTools.ResumeLayout(false);
            this.tableTools.ResumeLayout(false);
            this.grpPayload.ResumeLayout(false);
            this.tablePayload.ResumeLayout(false);
            this.groupBoxPitch.ResumeLayout(false);
            this.groupBoxPitch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPitch)).EndInit();
            this.groupBoxYaw.ResumeLayout(false);
            this.groupBoxYaw.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarYaw)).EndInit();
            this.groupBoxRoll.ResumeLayout(false);
            this.groupBoxRoll.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRoll)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel scrollPanel;
        private System.Windows.Forms.FlowLayoutPanel mainFlowLayout;

        // Command group
        private System.Windows.Forms.GroupBox grpCommand;
        private System.Windows.Forms.TableLayoutPanel tableCommand;
        private System.Windows.Forms.ComboBox cmbAction;
        private MissionPlanner.Controls.MyButton btnDoAction;
        private System.Windows.Forms.ComboBox cmbWaypoint;
        private MissionPlanner.Controls.MyButton btnSetWP;
        private System.Windows.Forms.ComboBox cmbModes;
        private MissionPlanner.Controls.MyButton btnSetMode;
        private System.Windows.Forms.ComboBox cmbMount;
        private MissionPlanner.Controls.MyButton btnSetMount;
        private MissionPlanner.Controls.MyButton btnRestartMission;
        private MissionPlanner.Controls.MyButton btnResumeMission;

        // Setpoints group
        private System.Windows.Forms.GroupBox grpSetpoints;
        private System.Windows.Forms.TableLayoutPanel tableSetpoints;
        private System.Windows.Forms.Label lblHomeAlt;
        private MissionPlanner.Controls.MyButton btnHomeAlt;
        private System.Windows.Forms.Label lblSpeed;
        private MissionPlanner.Controls.ModifyandSet modSetSpeed;
        private System.Windows.Forms.Label lblAlt;
        private MissionPlanner.Controls.ModifyandSet modSetAlt;
        private System.Windows.Forms.Label lblLoiterRad;
        private MissionPlanner.Controls.ModifyandSet modSetLoiterRad;

        // Tools group
        private System.Windows.Forms.GroupBox grpTools;
        private System.Windows.Forms.TableLayoutPanel tableTools;
        private MissionPlanner.Controls.MyButton btnRawSensor;
        private MissionPlanner.Controls.MyButton btnJoystick;
        private MissionPlanner.Controls.MyButton btnMessage;
        private MissionPlanner.Controls.MyButton btnClearTrack;
        private MissionPlanner.Controls.MyButton btnReboot;
        private MissionPlanner.Controls.MyButton btnAbortLanding;

        // Payload group
        private System.Windows.Forms.GroupBox grpPayload;
        private System.Windows.Forms.TableLayoutPanel tablePayload;
        private System.Windows.Forms.GroupBox groupBoxPitch;
        private System.Windows.Forms.TrackBar trackBarPitch;
        private System.Windows.Forms.TextBox txtPitchPos;
        private System.Windows.Forms.GroupBox groupBoxYaw;
        private System.Windows.Forms.TrackBar trackBarYaw;
        private System.Windows.Forms.TextBox txtYawPos;
        private System.Windows.Forms.GroupBox groupBoxRoll;
        private System.Windows.Forms.TrackBar trackBarRoll;
        private System.Windows.Forms.TextBox txtRollPos;
        private MissionPlanner.Controls.MyButton btnResetGimbal;
        private MissionPlanner.Controls.MyButton btnGimbalVideo;
    }
}
