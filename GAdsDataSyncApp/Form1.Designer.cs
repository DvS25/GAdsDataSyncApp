using Newtonsoft.Json;
using System.IO;

namespace GAdsDataSyncApp
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.gbDBConnectionSettings = new System.Windows.Forms.GroupBox();
            this.txtDBPassword = new System.Windows.Forms.TextBox();
            this.lblDBPassword = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.txtDBName = new System.Windows.Forms.TextBox();
            this.lblDBName = new System.Windows.Forms.Label();
            this.txtDBServerName = new System.Windows.Forms.TextBox();
            this.lblDBServerName = new System.Windows.Forms.Label();
            this.gbGAPISettings = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numPastRecordsDays = new System.Windows.Forms.NumericUpDown();
            this.lblGAdsAccCustomerId = new System.Windows.Forms.Label();
            this.txtGAdAccCustomerId = new System.Windows.Forms.TextBox();
            this.lblPastRecordDays = new System.Windows.Forms.Label();
            this.lblGAPILoginCustomerId = new System.Windows.Forms.Label();
            this.txtGAPILoginCustomerId = new System.Windows.Forms.TextBox();
            this.txtOAuth2RefreshToken = new System.Windows.Forms.TextBox();
            this.lblOAuth2RefreshToken = new System.Windows.Forms.Label();
            this.txtOAuth2ClientSecret = new System.Windows.Forms.TextBox();
            this.lblOAuth2ClientSecret = new System.Windows.Forms.Label();
            this.txtOAuth2ClientId = new System.Windows.Forms.TextBox();
            this.lblOAuth2ClientId = new System.Windows.Forms.Label();
            this.txtGAPIDeveloperToken = new System.Windows.Forms.TextBox();
            this.lblDeveloperToken = new System.Windows.Forms.Label();
            this.gbAutoSyncSettings = new System.Windows.Forms.GroupBox();
            this.lblAutoSyncMsg = new System.Windows.Forms.Label();
            this.numAutoSyncTime = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.errorProviderApp = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblPort = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.gbDBConnectionSettings.SuspendLayout();
            this.gbGAPISettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPastRecordsDays)).BeginInit();
            this.gbAutoSyncSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoSyncTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderApp)).BeginInit();
            this.SuspendLayout();
            // 
            // gbDBConnectionSettings
            // 
            this.gbDBConnectionSettings.Controls.Add(this.txtPort);
            this.gbDBConnectionSettings.Controls.Add(this.lblPort);
            this.gbDBConnectionSettings.Controls.Add(this.txtDBPassword);
            this.gbDBConnectionSettings.Controls.Add(this.lblDBPassword);
            this.gbDBConnectionSettings.Controls.Add(this.txtUserName);
            this.gbDBConnectionSettings.Controls.Add(this.lblUserName);
            this.gbDBConnectionSettings.Controls.Add(this.txtDBName);
            this.gbDBConnectionSettings.Controls.Add(this.lblDBName);
            this.gbDBConnectionSettings.Controls.Add(this.txtDBServerName);
            this.gbDBConnectionSettings.Controls.Add(this.lblDBServerName);
            this.gbDBConnectionSettings.Location = new System.Drawing.Point(3, 0);
            this.gbDBConnectionSettings.Name = "gbDBConnectionSettings";
            this.gbDBConnectionSettings.Size = new System.Drawing.Size(529, 104);
            this.gbDBConnectionSettings.TabIndex = 0;
            this.gbDBConnectionSettings.TabStop = false;
            this.gbDBConnectionSettings.Text = "DB Connection Settings";
            // 
            // txtDBPassword
            // 
            this.txtDBPassword.Location = new System.Drawing.Point(358, 49);
            this.txtDBPassword.Name = "txtDBPassword";
            this.txtDBPassword.Size = new System.Drawing.Size(149, 20);
            this.txtDBPassword.TabIndex = 7;
            this.txtDBPassword.Validating += new System.ComponentModel.CancelEventHandler(this.txtDBPassword_Validating);
            // 
            // lblDBPassword
            // 
            this.lblDBPassword.AutoSize = true;
            this.lblDBPassword.Location = new System.Drawing.Point(268, 52);
            this.lblDBPassword.Name = "lblDBPassword";
            this.lblDBPassword.Size = new System.Drawing.Size(53, 13);
            this.lblDBPassword.TabIndex = 6;
            this.lblDBPassword.Text = "Password";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(99, 49);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(150, 20);
            this.txtUserName.TabIndex = 5;
            this.txtUserName.Validating += new System.ComponentModel.CancelEventHandler(this.txtUserName_Validating);
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(6, 52);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(60, 13);
            this.lblUserName.TabIndex = 4;
            this.lblUserName.Text = "User Name";
            // 
            // txtDBName
            // 
            this.txtDBName.Location = new System.Drawing.Point(358, 23);
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.Size = new System.Drawing.Size(149, 20);
            this.txtDBName.TabIndex = 3;
            this.txtDBName.Validating += new System.ComponentModel.CancelEventHandler(this.txtDBName_Validating);
            // 
            // lblDBName
            // 
            this.lblDBName.AutoSize = true;
            this.lblDBName.Location = new System.Drawing.Point(268, 26);
            this.lblDBName.Name = "lblDBName";
            this.lblDBName.Size = new System.Drawing.Size(84, 13);
            this.lblDBName.TabIndex = 2;
            this.lblDBName.Text = "Database Name";
            // 
            // txtDBServerName
            // 
            this.txtDBServerName.Location = new System.Drawing.Point(99, 23);
            this.txtDBServerName.Name = "txtDBServerName";
            this.txtDBServerName.Size = new System.Drawing.Size(150, 20);
            this.txtDBServerName.TabIndex = 1;
            this.txtDBServerName.Validating += new System.ComponentModel.CancelEventHandler(this.txtDBServerName_Validating);
            // 
            // lblDBServerName
            // 
            this.lblDBServerName.AutoSize = true;
            this.lblDBServerName.Location = new System.Drawing.Point(6, 26);
            this.lblDBServerName.Name = "lblDBServerName";
            this.lblDBServerName.Size = new System.Drawing.Size(87, 13);
            this.lblDBServerName.TabIndex = 0;
            this.lblDBServerName.Text = "DB Server Name";
            // 
            // gbGAPISettings
            // 
            this.gbGAPISettings.Controls.Add(this.label2);
            this.gbGAPISettings.Controls.Add(this.numPastRecordsDays);
            this.gbGAPISettings.Controls.Add(this.lblGAdsAccCustomerId);
            this.gbGAPISettings.Controls.Add(this.txtGAdAccCustomerId);
            this.gbGAPISettings.Controls.Add(this.lblPastRecordDays);
            this.gbGAPISettings.Controls.Add(this.lblGAPILoginCustomerId);
            this.gbGAPISettings.Controls.Add(this.txtGAPILoginCustomerId);
            this.gbGAPISettings.Controls.Add(this.txtOAuth2RefreshToken);
            this.gbGAPISettings.Controls.Add(this.lblOAuth2RefreshToken);
            this.gbGAPISettings.Controls.Add(this.txtOAuth2ClientSecret);
            this.gbGAPISettings.Controls.Add(this.lblOAuth2ClientSecret);
            this.gbGAPISettings.Controls.Add(this.txtOAuth2ClientId);
            this.gbGAPISettings.Controls.Add(this.lblOAuth2ClientId);
            this.gbGAPISettings.Controls.Add(this.txtGAPIDeveloperToken);
            this.gbGAPISettings.Controls.Add(this.lblDeveloperToken);
            this.gbGAPISettings.Location = new System.Drawing.Point(3, 120);
            this.gbGAPISettings.Name = "gbGAPISettings";
            this.gbGAPISettings.Size = new System.Drawing.Size(529, 211);
            this.gbGAPISettings.TabIndex = 1;
            this.gbGAPISettings.TabStop = false;
            this.gbGAPISettings.Text = "Google API Settings";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(274, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "days";
            // 
            // numPastRecordsDays
            // 
            this.numPastRecordsDays.Location = new System.Drawing.Point(222, 179);
            this.numPastRecordsDays.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numPastRecordsDays.Name = "numPastRecordsDays";
            this.numPastRecordsDays.Size = new System.Drawing.Size(46, 20);
            this.numPastRecordsDays.TabIndex = 8;
            this.numPastRecordsDays.Validating += new System.ComponentModel.CancelEventHandler(this.numPastRecordsDays_Validating);
            // 
            // lblGAdsAccCustomerId
            // 
            this.lblGAdsAccCustomerId.AutoSize = true;
            this.lblGAdsAccCustomerId.Location = new System.Drawing.Point(6, 156);
            this.lblGAdsAccCustomerId.Name = "lblGAdsAccCustomerId";
            this.lblGAdsAccCustomerId.Size = new System.Drawing.Size(137, 13);
            this.lblGAdsAccCustomerId.TabIndex = 11;
            this.lblGAdsAccCustomerId.Text = "GAd Account\'s Customer Id";
            // 
            // txtGAdAccCustomerId
            // 
            this.txtGAdAccCustomerId.Location = new System.Drawing.Point(170, 153);
            this.txtGAdAccCustomerId.Name = "txtGAdAccCustomerId";
            this.txtGAdAccCustomerId.Size = new System.Drawing.Size(154, 20);
            this.txtGAdAccCustomerId.TabIndex = 10;
            this.txtGAdAccCustomerId.Validating += new System.ComponentModel.CancelEventHandler(this.txtGAdAccCustomerId_Validating);
            // 
            // lblPastRecordDays
            // 
            this.lblPastRecordDays.AutoSize = true;
            this.lblPastRecordDays.Location = new System.Drawing.Point(6, 181);
            this.lblPastRecordDays.Name = "lblPastRecordDays";
            this.lblPastRecordDays.Size = new System.Drawing.Size(210, 13);
            this.lblPastRecordDays.TabIndex = 5;
            this.lblPastRecordDays.Text = "Past how much day\'s records should sync?";
            // 
            // lblGAPILoginCustomerId
            // 
            this.lblGAPILoginCustomerId.AutoSize = true;
            this.lblGAPILoginCustomerId.Location = new System.Drawing.Point(6, 129);
            this.lblGAPILoginCustomerId.Name = "lblGAPILoginCustomerId";
            this.lblGAPILoginCustomerId.Size = new System.Drawing.Size(158, 13);
            this.lblGAPILoginCustomerId.TabIndex = 9;
            this.lblGAPILoginCustomerId.Text = "Manager Account\'s Customer Id";
            // 
            // txtGAPILoginCustomerId
            // 
            this.txtGAPILoginCustomerId.Location = new System.Drawing.Point(170, 126);
            this.txtGAPILoginCustomerId.Name = "txtGAPILoginCustomerId";
            this.txtGAPILoginCustomerId.Size = new System.Drawing.Size(154, 20);
            this.txtGAPILoginCustomerId.TabIndex = 8;
            this.txtGAPILoginCustomerId.Validating += new System.ComponentModel.CancelEventHandler(this.txtGAPILoginCustomerId_Validating);
            // 
            // txtOAuth2RefreshToken
            // 
            this.txtOAuth2RefreshToken.Location = new System.Drawing.Point(129, 100);
            this.txtOAuth2RefreshToken.Name = "txtOAuth2RefreshToken";
            this.txtOAuth2RefreshToken.Size = new System.Drawing.Size(378, 20);
            this.txtOAuth2RefreshToken.TabIndex = 7;
            this.txtOAuth2RefreshToken.Validating += new System.ComponentModel.CancelEventHandler(this.txtOAuth2RefreshToken_Validating);
            // 
            // lblOAuth2RefreshToken
            // 
            this.lblOAuth2RefreshToken.AutoSize = true;
            this.lblOAuth2RefreshToken.Location = new System.Drawing.Point(6, 103);
            this.lblOAuth2RefreshToken.Name = "lblOAuth2RefreshToken";
            this.lblOAuth2RefreshToken.Size = new System.Drawing.Size(117, 13);
            this.lblOAuth2RefreshToken.TabIndex = 6;
            this.lblOAuth2RefreshToken.Text = "OAuth2 Refresh Token";
            // 
            // txtOAuth2ClientSecret
            // 
            this.txtOAuth2ClientSecret.Location = new System.Drawing.Point(129, 74);
            this.txtOAuth2ClientSecret.Name = "txtOAuth2ClientSecret";
            this.txtOAuth2ClientSecret.Size = new System.Drawing.Size(378, 20);
            this.txtOAuth2ClientSecret.TabIndex = 5;
            this.txtOAuth2ClientSecret.Validating += new System.ComponentModel.CancelEventHandler(this.txtOAuth2ClientSecret_Validating);
            // 
            // lblOAuth2ClientSecret
            // 
            this.lblOAuth2ClientSecret.AutoSize = true;
            this.lblOAuth2ClientSecret.Location = new System.Drawing.Point(6, 77);
            this.lblOAuth2ClientSecret.Name = "lblOAuth2ClientSecret";
            this.lblOAuth2ClientSecret.Size = new System.Drawing.Size(106, 13);
            this.lblOAuth2ClientSecret.TabIndex = 4;
            this.lblOAuth2ClientSecret.Text = "OAuth2 Client Secret";
            // 
            // txtOAuth2ClientId
            // 
            this.txtOAuth2ClientId.Location = new System.Drawing.Point(129, 48);
            this.txtOAuth2ClientId.Name = "txtOAuth2ClientId";
            this.txtOAuth2ClientId.Size = new System.Drawing.Size(378, 20);
            this.txtOAuth2ClientId.TabIndex = 3;
            this.txtOAuth2ClientId.Validating += new System.ComponentModel.CancelEventHandler(this.txtOAuth2ClientId_Validating);
            // 
            // lblOAuth2ClientId
            // 
            this.lblOAuth2ClientId.AutoSize = true;
            this.lblOAuth2ClientId.Location = new System.Drawing.Point(6, 51);
            this.lblOAuth2ClientId.Name = "lblOAuth2ClientId";
            this.lblOAuth2ClientId.Size = new System.Drawing.Size(84, 13);
            this.lblOAuth2ClientId.TabIndex = 2;
            this.lblOAuth2ClientId.Text = "OAuth2 Client Id";
            // 
            // txtGAPIDeveloperToken
            // 
            this.txtGAPIDeveloperToken.Location = new System.Drawing.Point(129, 22);
            this.txtGAPIDeveloperToken.Name = "txtGAPIDeveloperToken";
            this.txtGAPIDeveloperToken.Size = new System.Drawing.Size(195, 20);
            this.txtGAPIDeveloperToken.TabIndex = 1;
            this.txtGAPIDeveloperToken.Validating += new System.ComponentModel.CancelEventHandler(this.txtGAPIDeveloperToken_Validating);
            // 
            // lblDeveloperToken
            // 
            this.lblDeveloperToken.AutoSize = true;
            this.lblDeveloperToken.Location = new System.Drawing.Point(6, 25);
            this.lblDeveloperToken.Name = "lblDeveloperToken";
            this.lblDeveloperToken.Size = new System.Drawing.Size(90, 13);
            this.lblDeveloperToken.TabIndex = 0;
            this.lblDeveloperToken.Text = "Developer Token";
            // 
            // gbAutoSyncSettings
            // 
            this.gbAutoSyncSettings.Controls.Add(this.lblAutoSyncMsg);
            this.gbAutoSyncSettings.Controls.Add(this.numAutoSyncTime);
            this.gbAutoSyncSettings.Controls.Add(this.label1);
            this.gbAutoSyncSettings.Location = new System.Drawing.Point(3, 347);
            this.gbAutoSyncSettings.Name = "gbAutoSyncSettings";
            this.gbAutoSyncSettings.Size = new System.Drawing.Size(529, 47);
            this.gbAutoSyncSettings.TabIndex = 2;
            this.gbAutoSyncSettings.TabStop = false;
            this.gbAutoSyncSettings.Text = "Auto Sync Settings";
            // 
            // lblAutoSyncMsg
            // 
            this.lblAutoSyncMsg.AutoSize = true;
            this.lblAutoSyncMsg.Location = new System.Drawing.Point(184, 22);
            this.lblAutoSyncMsg.Name = "lblAutoSyncMsg";
            this.lblAutoSyncMsg.Size = new System.Drawing.Size(21, 13);
            this.lblAutoSyncMsg.TabIndex = 3;
            this.lblAutoSyncMsg.Text = "hrs";
            // 
            // numAutoSyncTime
            // 
            this.numAutoSyncTime.Location = new System.Drawing.Point(121, 20);
            this.numAutoSyncTime.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.numAutoSyncTime.Name = "numAutoSyncTime";
            this.numAutoSyncTime.Size = new System.Drawing.Size(57, 20);
            this.numAutoSyncTime.TabIndex = 2;
            this.numAutoSyncTime.Validating += new System.ComponentModel.CancelEventHandler(this.numAutoSyncTime_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Daily Auto Sync Time";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(457, 400);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(376, 400);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // errorProviderApp
            // 
            this.errorProviderApp.ContainerControl = this;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(6, 78);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(75, 13);
            this.lblPort.TabIndex = 8;
            this.lblPort.Text = "Database Port";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(99, 75);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(150, 20);
            this.txtPort.TabIndex = 9;
            this.txtPort.Validating += new System.ComponentModel.CancelEventHandler(this.txtPort_Validating);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 428);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbAutoSyncSettings);
            this.Controls.Add(this.gbGAPISettings);
            this.Controls.Add(this.gbDBConnectionSettings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.gbDBConnectionSettings.ResumeLayout(false);
            this.gbDBConnectionSettings.PerformLayout();
            this.gbGAPISettings.ResumeLayout(false);
            this.gbGAPISettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPastRecordsDays)).EndInit();
            this.gbAutoSyncSettings.ResumeLayout(false);
            this.gbAutoSyncSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoSyncTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderApp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDBConnectionSettings;
        private System.Windows.Forms.Label lblDBServerName;
        private System.Windows.Forms.TextBox txtDBServerName;
        private System.Windows.Forms.TextBox txtDBPassword;
        private System.Windows.Forms.Label lblDBPassword;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.TextBox txtDBName;
        private System.Windows.Forms.Label lblDBName;
        private System.Windows.Forms.GroupBox gbGAPISettings;
        private System.Windows.Forms.Label lblDeveloperToken;
        private System.Windows.Forms.TextBox txtGAPIDeveloperToken;
        private System.Windows.Forms.TextBox txtOAuth2ClientSecret;
        private System.Windows.Forms.Label lblOAuth2ClientSecret;
        private System.Windows.Forms.TextBox txtOAuth2ClientId;
        private System.Windows.Forms.Label lblOAuth2ClientId;
        private System.Windows.Forms.TextBox txtGAPILoginCustomerId;
        private System.Windows.Forms.TextBox txtOAuth2RefreshToken;
        private System.Windows.Forms.Label lblOAuth2RefreshToken;
        private System.Windows.Forms.Label lblGAPILoginCustomerId;
        private System.Windows.Forms.Label lblGAdsAccCustomerId;
        private System.Windows.Forms.TextBox txtGAdAccCustomerId;
        private System.Windows.Forms.GroupBox gbAutoSyncSettings;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblAutoSyncMsg;
        private System.Windows.Forms.NumericUpDown numAutoSyncTime;
        private System.Windows.Forms.ErrorProvider errorProviderApp;
        private System.Windows.Forms.NumericUpDown numPastRecordsDays;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblPastRecordDays;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lblPort;
    }
}

