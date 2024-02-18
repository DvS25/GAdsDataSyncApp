using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace GAdsDataSyncApp
{
    public partial class Form1 : Form
    {
        static string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/GAdsDataSyncApp/";
        public Form1()
        {
            InitializeComponent();

            string json = File.ReadAllText(path + "OtherSettings.json");
            AppSettings AS = JsonConvert.DeserializeObject<AppSettings>(json);
            this.txtDBServerName.Text = AS.DBServerName;
            this.txtDBName.Text = AS.DatabaseName;
            this.txtUserName.Text = AS.UserName;
            this.txtDBPassword.Text = AS.Password;
            this.txtPort.Text = AS.Port;
            this.numAutoSyncTime.Value = Convert.ToInt16(AS.AutoSyncTime);
            this.txtGAdAccCustomerId.Text = AS.GAdAccCustomerId;
            this.numPastRecordsDays.Text = AS.PastRecordsDays;

            string json1 = File.ReadAllText(path + "GoogleAdsApi.json");
            GAPIParameters GS = JsonConvert.DeserializeObject<GAPIParameters>(json1);
            this.txtGAPIDeveloperToken.Text = GS.DeveloperToken;
            this.txtOAuth2ClientId.Text = GS.OAuth2ClientId;
            this.txtOAuth2ClientSecret.Text = GS.OAuth2ClientSecret;
            this.txtOAuth2RefreshToken.Text = GS.OAuth2RefreshToken;
            this.txtGAPILoginCustomerId.Text = GS.LoginCustomerId;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren(ValidationConstraints.Enabled))
            {
                MessageBox.Show("All fields are required.", "Warning!");
                return;
            }

            /*-------------------Save Google API settings in json file------------------*/
            GAPIParameters GAPIData = new GAPIParameters();
            GAPIData.ProxyServer = "";
            GAPIData.ProxyUser = "";
            GAPIData.ProxyPassword = "";
            GAPIData.ProxyDomain = "";
            GAPIData.DeveloperToken = this.txtGAPIDeveloperToken.Text;
            GAPIData.OAuth2ClientId = this.txtOAuth2ClientId.Text;
            GAPIData.OAuth2ClientSecret = this.txtOAuth2ClientSecret.Text;
            GAPIData.OAuth2Mode = "APPLICATION";
            GAPIData.OAuth2RefreshToken = this.txtOAuth2RefreshToken.Text;
            GAPIData.LoginCustomerId = this.txtGAPILoginCustomerId.Text;

            string json = JsonConvert.SerializeObject(GAPIData);
            File.WriteAllText(path + "GoogleAdsApi.json", json);
            /*-------------------Save Google API settings in json file------------------*/

            /*-------------------Save Other Application settings in json file------------------*/
            AppSettings AppSettingData = new AppSettings();
            AppSettingData.DBServerName = this.txtDBServerName.Text;
            AppSettingData.DatabaseName = this.txtDBName.Text;
            AppSettingData.UserName = this.txtUserName.Text;
            AppSettingData.Password = this.txtDBPassword.Text;
            AppSettingData.Port = this.txtPort.Text;
            AppSettingData.AutoSyncTime = this.numAutoSyncTime.Text;
            AppSettingData.GAdAccCustomerId = this.txtGAdAccCustomerId.Text;
            AppSettingData.PastRecordsDays = this.numPastRecordsDays.Text;

            string json1 = JsonConvert.SerializeObject(AppSettingData);
            File.WriteAllText(path + "OtherSettings.json", json1);
            /*-------------------Save Other Application settings in json file------------------*/


            MyCustomApplicationContext.WriteLogToFile("Configuration settings are updated.");
            this.Close();
            Application.Restart();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtDBServerName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDBServerName.Text))
            {
                e.Cancel = true;
                txtDBServerName.Focus();
                errorProviderApp.SetError(txtDBServerName, "Server Name should not be left blank!");
            }
            else
            {
                e.Cancel = false;
                errorProviderApp.SetError(txtDBServerName, "");
            }
        }

        private void txtDBName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDBName.Text))
            {
                e.Cancel = true;
                txtDBName.Focus();
                errorProviderApp.SetError(txtDBName, "Database Name should not be left blank!");
            }
            else
            {
                e.Cancel = false;
                errorProviderApp.SetError(txtDBName, "");
            }
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                e.Cancel = true;
                txtUserName.Focus();
                errorProviderApp.SetError(txtUserName, "User Name should not be left blank!");
            }
            else
            {
                e.Cancel = false;
                errorProviderApp.SetError(txtUserName, "");
            }
        }

        private void txtDBPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDBPassword.Text))
            {
                e.Cancel = true;
                txtDBPassword.Focus();
                errorProviderApp.SetError(txtDBPassword, "Password should not be left blank!");
            }
            else
            {
                e.Cancel = false;
                errorProviderApp.SetError(txtDBPassword, "");
            }
        }

        private void txtGAPIDeveloperToken_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtGAPIDeveloperToken.Text))
            {
                e.Cancel = true;
                txtGAPIDeveloperToken.Focus();
                errorProviderApp.SetError(txtGAPIDeveloperToken, "Developer token should not be left blank!");
            }
            else
            {
                e.Cancel = false;
                errorProviderApp.SetError(txtGAPIDeveloperToken, "");
            }
        }

        private void txtOAuth2ClientId_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOAuth2ClientId.Text))
            {
                e.Cancel = true;
                txtOAuth2ClientId.Focus();
                errorProviderApp.SetError(txtOAuth2ClientId, "OAuth2 client id should not be left blank!");
            }
            else
            {
                e.Cancel = false;
                errorProviderApp.SetError(txtOAuth2ClientId, "");
            }
        }

        private void txtOAuth2ClientSecret_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOAuth2ClientSecret.Text))
            {
                e.Cancel = true;
                txtOAuth2ClientSecret.Focus();
                errorProviderApp.SetError(txtOAuth2ClientSecret, "OAuth2 client secret should not be left blank!");
            }
            else
            {
                e.Cancel = false;
                errorProviderApp.SetError(txtOAuth2ClientSecret, "");
            }
        }

        private void txtOAuth2RefreshToken_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOAuth2RefreshToken.Text))
            {
                e.Cancel = true;
                txtOAuth2RefreshToken.Focus();
                errorProviderApp.SetError(txtOAuth2RefreshToken, "OAuth2 refresh token should not be left blank!");
            }
            else
            {
                e.Cancel = false;
                errorProviderApp.SetError(txtOAuth2RefreshToken, "");
            }
        }

        private void txtGAPILoginCustomerId_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtGAPILoginCustomerId.Text))
            {
                e.Cancel = true;
                txtGAPILoginCustomerId.Focus();
                errorProviderApp.SetError(txtGAPILoginCustomerId, "Manager's customer id should not be left blank!");
            }
            else
            {
                e.Cancel = false;
                errorProviderApp.SetError(txtGAPILoginCustomerId, "");
            }
        }

        private void txtGAdAccCustomerId_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtGAdAccCustomerId.Text))
            {
                e.Cancel = true;
                txtGAdAccCustomerId.Focus();
                errorProviderApp.SetError(txtGAdAccCustomerId, "GAd Account customer id should not be left blank!");
            }
            else
            {
                e.Cancel = false;
                errorProviderApp.SetError(txtGAdAccCustomerId, "");
            }
        }

        private void numAutoSyncTime_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(numAutoSyncTime.Text))
            {
                e.Cancel = true;
                numAutoSyncTime.Focus();
                errorProviderApp.SetError(numAutoSyncTime, "Auto sync time should not be left blank!");
            }
            else if (Convert.ToInt16(numAutoSyncTime.Text) < 0 || Convert.ToInt16(numAutoSyncTime.Text) > 23)
            {
                e.Cancel = true;
                numAutoSyncTime.Focus();
                errorProviderApp.SetError(numAutoSyncTime, "Auto sync time should be between 0 to 23!");
            }
            else
            {
                e.Cancel = false;
                errorProviderApp.SetError(numAutoSyncTime, "");
            }
        }

        private void numPastRecordsDays_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(numPastRecordsDays.Text))
            {
                e.Cancel = true;
                numPastRecordsDays.Focus();
                errorProviderApp.SetError(numPastRecordsDays, "Days should not be left blank!");
            }
            else if (Convert.ToInt16(numPastRecordsDays.Text) <= 0)
            {
                e.Cancel = true;
                numPastRecordsDays.Focus();
                errorProviderApp.SetError(numPastRecordsDays, "Days should be greater than 0!");
            }
            else
            {
                e.Cancel = false;
                errorProviderApp.SetError(numPastRecordsDays, "");
            }
        }

        private void txtPort_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPort.Text))
            {
                e.Cancel = true;
                txtPort.Focus();
                errorProviderApp.SetError(txtPort, "Database Port should not be left blank!");
            }
            else
            {
                e.Cancel = false;
                errorProviderApp.SetError(txtPort, "");
            }
        }
    }
}
