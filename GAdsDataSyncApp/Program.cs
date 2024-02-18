using GAdsDataSyncApp.Properties;
using Google.Ads.Gax.Util;
using Google.Ads.GoogleAds;
using Google.Ads.GoogleAds.Config;
using Google.Ads.GoogleAds.Lib;
using Google.Ads.GoogleAds.V11.Errors;
using Google.Ads.GoogleAds.V11.Resources;
using Google.Ads.GoogleAds.V11.Services;
using Google.Protobuf;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using static Google.Ads.GoogleAds.V11.Enums.ChangeEventResourceTypeEnum.Types;
using static Google.Ads.GoogleAds.V11.Enums.ResourceChangeOperationEnum.Types;
using static Google.Ads.GoogleAds.V11.Resources.ChangeEvent.Types;

namespace GAdsDataSyncApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                MyCustomApplicationContext.WriteLogToFile("Application is started.");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //Application.Run(new Form1());
                Application.Run(new MyCustomApplicationContext());

                //for add this application on windows startup
                RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                rk.SetValue("GAdsDataSyncApp", Application.ExecutablePath);
            }
            catch (Exception ex)
            {
                MyCustomApplicationContext.WriteLogToFile(ex.Message);
            }
        }
    }

    public class MyCustomApplicationContext : ApplicationContext
    {
        private NotifyIcon trayIcon;
        System.Timers.Timer timer = new System.Timers.Timer(); // name space(using System.Timers;) 
        string customerId = "";
        static string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/GAdsDataSyncApp/";

        public MyCustomApplicationContext()
        {
            try
            {
                MenuItem SyncMenuItem = new MenuItem("Sync", new EventHandler(ManualSyncData));
                MenuItem SyncChangeHistoryMenuItem = new MenuItem("Sync Change History", new EventHandler(ManualSyncChangeHistory));
                MenuItem configMenuItem = new MenuItem("Configuration", new EventHandler(ShowConfig));
                MenuItem exitMenuItem = new MenuItem("Exit", new EventHandler(Exit));

                // Initialize Tray Icon
                trayIcon = new NotifyIcon()
                {
                    Text = "GAdsDataSyncApp",
                    Icon = Resources.AppIcon,
                    ContextMenu = new ContextMenu(new MenuItem[] { SyncMenuItem, SyncChangeHistoryMenuItem, configMenuItem, exitMenuItem }),
                    Visible = true
                };

                string json = File.ReadAllText(path + "OtherSettings.json");
                AppSettings AS = JsonConvert.DeserializeObject<AppSettings>(json);

                timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
                DateTime scheduleTime = DateTime.Today.AddDays(0).AddHours(Convert.ToInt16(AS.AutoSyncTime)); // Schedule to run once a day at given time
                if (DateTime.Now > scheduleTime)
                    scheduleTime = scheduleTime.AddHours(24);

                timer.Interval = scheduleTime.Subtract(DateTime.Now).TotalSeconds * 1000; //number in miliseconds  
                timer.Enabled = true;

                WriteLogToFile("Service timer is started.");
            }
            catch (Exception ex)
            {
                WriteLogToFile(ex.Message);
            }
        }

        void ManualSyncData(object sender, EventArgs e)
        {
            WriteLogToFile("Google Ads data manual synchronization is started.");
            StartDataSync();
            WriteLogToFile("Google Ads data manual synchronization is ended.");
        }

        void ManualSyncChangeHistory(object sender, EventArgs e)
        {
            WriteLogToFile("Google Ads Change History manual synchronization is started.");
            GetChangeHistory();
            WriteLogToFile("Google Ads Change History manual synchronization is ended.");
        }

        Form1 configWindow = new Form1();
        void ShowConfig(object sender, EventArgs e)
        {

            // If we are already showing the window, merely focus it.
            if (configWindow.Visible)
            {
                configWindow.Activate();
            }
            else
            {
                configWindow.ShowDialog();
            }
        }

        void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;
            timer.Enabled = false;
            WriteLogToFile("Service timer is stopped.");

            //for remove this application from windows startup
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            rk.DeleteValue("GAdsDataSyncApp", false);

            WriteLogToFile("Application is closed.");
            Application.Exit();
        }

        public static void WriteLogToFile(string LogText)
        {
            // Create a writer and open the file:
            StreamWriter log;

            if (!File.Exists(path + "logfile.txt"))
            {
                log = new StreamWriter(path + "logfile.txt");
            }
            else
            {
                log = File.AppendText(path + "logfile.txt");
            }

            // Write to the file:
            log.WriteLine("Date: " + DateTime.Now + ",   Log: " + LogText);
            log.WriteLine();

            // Close the stream:
            log.Close();
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            WriteLogToFile("Google Ads data auto synchronization is started.");
            StartDataSync();
            GetChangeHistory();
            WriteLogToFile("Google Ads data auto synchronization is ended.");

            // If tick for the first time, reset next run to every 24 hours
            if (timer.Interval != 24 * 60 * 60 * 1000)
            {
                timer.Interval = 24 * 60 * 60 * 1000;
            }
        }


        /*---------------------Functions for get data from api and store to my sql database----------------------*/
        void StartDataSync()
        {
            string json = File.ReadAllText(path + "OtherSettings.json");
            AppSettings AS = JsonConvert.DeserializeObject<AppSettings>(json);

            customerId = AS.GAdAccCustomerId;
            int PastRecDays = Convert.ToInt16(AS.PastRecordsDays);
            string StartDate = DateTime.Today.AddDays(-1 * (PastRecDays - 1)).ToString("yyyy-MM-dd");
            string EndDate = DateTime.Today.ToString("yyyy-MM-dd");

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(path + "GoogleAdsApi.json");
            IConfigurationRoot configRoot = builder.Build();

            GoogleAdsConfig config = new GoogleAdsConfig(configRoot);
            GoogleAdsClient client = new GoogleAdsClient(config);
            //GoogleAdsClient client = new GoogleAdsClient();
            // Get the GoogleAdsService.
            GoogleAdsServiceClient googleAdsService = client.GetService(
                Services.V11.GoogleAdsService);

            string query =
                @"SELECT 
                    segments.date,
                    campaign.start_date, 
                    campaign.id, 
                    campaign.name, 
                    campaign.campaign_budget,
                    location_view.resource_name, 
                    campaign_criterion.location.geo_target_constant, 
                    campaign.status, 
                    campaign.target_cpa.target_cpa_micros, 
                    metrics.cost_micros, 
                    metrics.cost_per_conversion, 
                    metrics.conversions, 
                    metrics.ctr 
                FROM location_view
                WHERE segments.date >= '" + StartDate + "' AND segments.date <= '" + EndDate + "' ORDER BY segments.date ASC, campaign.id";

            try
            {
                // Issue a search request.
                googleAdsService.SearchStream(customerId, query,
                    delegate (SearchGoogleAdsStreamResponse resp)
                    {
                        // Display the results.
                        WriteLogToFile(resp.Results.Count + " records found from the google ads api.");

                        if (resp.Results.Count > 0)
                        {

                            /*------------------------for get country list-----------------------*/
                            SearchGoogleAdsStreamResponse geo_target_constant_list = new SearchGoogleAdsStreamResponse();
                            string query_geo =
                                @"SELECT 
                                    geo_target_constant.canonical_name, 
                                    geo_target_constant.country_code,
                                    geo_target_constant.id, 
                                    geo_target_constant.name, 
                                    geo_target_constant.resource_name, 
                                    geo_target_constant.target_type 
                                FROM geo_target_constant
                                WHERE
                                    geo_target_constant.target_type IN ('Country','Region')
                                ORDER BY
                                    geo_target_constant.id";

                            try
                            {
                                // Issue a search request for country names.
                                googleAdsService.SearchStream(customerId, query_geo,
                                    delegate (SearchGoogleAdsStreamResponse resp_geo)
                                    {
                                        geo_target_constant_list = resp_geo;
                                    }
                                );
                            }
                            catch (GoogleAdsException ex)
                            {
                                WriteLogToFile("Google API Failure in get data of geo_target_constant:");
                                WriteLogToFile("Message: " + ex.Message);
                                WriteLogToFile("Failure: " + ex.Failure);
                                WriteLogToFile("Request ID: " + ex.RequestId);
                            }
                            /*------------------------/for get country list-----------------------*/


                            /*------------------------for get Campaign Budget list-----------------------*/
                            SearchGoogleAdsStreamResponse budget_list = new SearchGoogleAdsStreamResponse();
                            string query_budget =
                                @"SELECT 
                                    segments.date, 
                                    campaign_budget.resource_name, 
                                    campaign_budget.amount_micros 
                                FROM campaign_budget 
                                WHERE segments.date >= '" + StartDate + "' AND segments.date <= '" + EndDate + "'";

                            try
                            {
                                // Issue a search request for campaign_budget.
                                googleAdsService.SearchStream(customerId, query_budget,
                                    delegate (SearchGoogleAdsStreamResponse resp_budget)
                                    {
                                        budget_list = resp_budget;
                                    }
                                );
                            }
                            catch (GoogleAdsException ex)
                            {
                                WriteLogToFile("Google API Failure in get data of campaign_budget:");
                                WriteLogToFile("Message: " + ex.Message);
                                WriteLogToFile("Failure: " + ex.Failure);
                                WriteLogToFile("Request ID: " + ex.RequestId);
                            }
                            /*------------------------/for get Campaign Budget list-----------------------*/


                            string DBQuery = "CREATE TEMPORARY TABLE TMP_CAMPAIGN_DATA(`date` date,campingid varchar(100),campaign varchar(250),country varchar(250),`status` varchar(250),bid float,cost float,costdivideconversion float,conversion varchar(20),ctr varchar(50),budget float); ";
                            //DBQuery += "insert into TMP_CAMPAIGN_DATA(date,campingid,campaign,country,status,bid,cost,costdivideconversion,conversion,ctr,budget) VALUES";
                            string temp = "";
                            foreach (GoogleAdsRow rowobj in resp.Results)
                            {
                                long CampaignBudget = 0;
                                string Country = "";

                                List<GoogleAdsRow> Budget = budget_list.Results.ToList().FindAll(x => x.CampaignBudget.ResourceName == rowobj.Campaign.CampaignBudget && x.Segments.Date == rowobj.Segments.Date);
                                if (Budget.Count() > 0)
                                {
                                    CampaignBudget = Budget[0].CampaignBudget.AmountMicros;
                                }

                                List<GoogleAdsRow> Location = geo_target_constant_list.Results.ToList().FindAll(x => x.GeoTargetConstant.ResourceName == rowobj.CampaignCriterion.Location.GeoTargetConstant);
                                if (Location.Count() > 0)
                                {
                                    Country = Location[0].GeoTargetConstant.CanonicalName;
                                }

                                //temp += ",('" + rowobj.Segments.Date + "','" + rowobj.Campaign.Id.ToString() + "','" + (rowobj.Campaign.Name).Replace("'", "''") + "','" + (Country).Replace("'", "''") + "','" + rowobj.Campaign.Status.ToString() + "'," + (rowobj.Campaign.TargetCpa != null ? ((rowobj.Campaign.TargetCpa.TargetCpaMicros / 10000.00) / 100.00).ToString() : "0") + "," + ((rowobj.Metrics.CostMicros / 10000.00) / 100.00).ToString() + "," + ((rowobj.Metrics.CostPerConversion / 10000.00) / 100.00).ToString() + ",'" + rowobj.Metrics.Conversions.ToString() + "','" + rowobj.Metrics.Ctr.ToString() + "'," + ((CampaignBudget / 10000.00) / 100.00).ToString() + ") ";
                                DBQuery += "insert into TMP_CAMPAIGN_DATA(`date`,campingid,campaign,country,`status`,bid,cost,costdivideconversion,conversion,ctr,budget) ";
                                DBQuery += "VALUES('" + rowobj.Segments.Date + "','" + rowobj.Campaign.Id.ToString() + "','" + (rowobj.Campaign.Name).Replace("'", "''") + "','" + (Country).Replace("'", "''") + "','" + rowobj.Campaign.Status.ToString() + "'," + (rowobj.Campaign.TargetCpa != null ? ((rowobj.Campaign.TargetCpa.TargetCpaMicros / 10000.00) / 100.00).ToString() : "0") + "," + ((rowobj.Metrics.CostMicros / 10000.00) / 100.00).ToString() + "," + ((rowobj.Metrics.CostPerConversion / 10000.00) / 100.00).ToString() + ",'" + rowobj.Metrics.Conversions.ToString() + "','" + rowobj.Metrics.Ctr.ToString() + "'," + ((CampaignBudget / 10000.00) / 100.00).ToString() + "); ";

                            }

                            //DBQuery = DBQuery + temp.Substring(1) + "; ";

                            /*--------------------UPDATE COUNTRY IN TEMP TABLE---------------------*/
                            DBQuery += " UPDATE TMP_CAMPAIGN_DATA cd INNER JOIN tblcountryreplace cr ON cd.country = cr.country SET cd.country = cr.utfreplacekeyword; ";
                            /*--------------------/UPDATE COUNTRY IN TEMP TABLE---------------------*/

                            /*--------------INSERT CAMPAIGNS WHICH ARE NOT AVAILABLE IN tblcampaign---------------*/
                            //DBQuery += "INSERT INTO tblcampaign(campaignid,campaignname,entry_date) SELECT A1.campingid,A1.campaign,CURDATE() FROM( SELECT distinct campingid, campaign FROM TMP_CAMPAIGN_DATA )A1 LEFT JOIN tblcampaign C ON A1.campingid = C.campaignid WHERE C.id IS NULL; ";
                            DBQuery += "INSERT INTO tblcampaign(campaignid,campaignname,entry_date) SELECT A1.campingid,A1.campaign,CURDATE() FROM TMP_CAMPAIGN_DATA A1 LEFT JOIN tblcampaign C ON A1.campingid = C.campaignid WHERE C.id IS NULL GROUP BY A1.campingid,A1.campaign; ";
                            /*--------------/INSERT CAMPAIGNS WHICH ARE NOT AVAILABLE IN tblcampaign---------------*/

                            /*--------------DELETE OLD DATA AND INSERT NEW FOR GIVEN DATE RANGE---------------*/
                            //DBQuery += "DELETE cd FROM tblcampaigndata cd INNER JOIN TMP_CAMPAIGN_DATA tmp ON STR_TO_DATE(cd.date, '%d-%m-%Y') = tmp.date AND cd.campingid = tmp.campingid WHERE STR_TO_DATE(cd.date, '%d-%m-%Y') >= '" + StartDate + "' AND STR_TO_DATE(cd.date, '%d-%m-%Y') <= '" + EndDate + "'; ";
                            //DBQuery += "DELETE FROM tblcampaigndata cd INNER JOIN TMP_CAMPAIGN_DATA tmp ON STR_TO_DATE(cd.date, '%d-%m-%Y') = tmp.date AND cd.campingid = tmp.campingid WHERE STR_TO_DATE(cd.date, '%d-%m-%Y') >= '" + StartDate + "' AND STR_TO_DATE(cd.date, '%d-%m-%Y') <= '" + EndDate + "'; ";
                            DBQuery += "DELETE FROM tblcampaigndata WHERE (STR_TO_DATE(date, '%d-%m-%Y') BETWEEN '" + StartDate + "' AND '" + EndDate + "') AND EXISTS( SELECT tmp.campingid FROM TMP_CAMPAIGN_DATA tmp WHERE tmp.date = STR_TO_DATE(tblcampaigndata.date, '%d-%m-%Y') AND tmp.campingid = tblcampaigndata.campingid AND tmp.country = tblcampaigndata.country ); ";
                            DBQuery += "INSERT INTO tblcampaigndata(`date`,campingid,campaign,country,`status`,bid,cost,costdivideconversion,conversion,ctr,budget) SELECT DATE_FORMAT(`date`, '%d-%m-%Y'),campingid,campaign,country,`status`,bid,cost,costdivideconversion,conversion,ctr,budget FROM TMP_CAMPAIGN_DATA; ";
                            /*--------------/DELETE OLD DATA AND INSERT NEW FOR GIVEN DATE RANGE---------------*/

                            /*--------------UPDATE APP_ID IN tblcampaigndata---------------*/
                            DBQuery += "UPDATE tblcampaigndata c INNER JOIN tbl_appwise_campaign a ON a.campaignid = c.campingid SET c.app_id = a.app_id WHERE IFNULL(c.app_id, 0) = 0 AND (STR_TO_DATE(c.date, '%d-%m-%Y') BETWEEN '" + StartDate + "' AND '" + EndDate + "'); ";
                            /*--------------/UPDATE APP_ID IN tblcampaigndata---------------*/

                            /*--------------UPDATE campaignid in tblcsvdata.---------------*/
                            DBQuery += "UPDATE tblcsvdata c JOIN( SELECT tc.app_id, tc.country, tc.date, GROUP_CONCAT(DISTINCT c.campingid) AS campaignid FROM tblcsvdata tc LEFT JOIN tblcampaigndata c ON c.app_id = tc.app_id AND tc.date = c.date AND tc.country = c.country WHERE STR_TO_DATE(tc.date, '%d-%m-%Y') BETWEEN '" + StartDate + "' AND '" + EndDate + "' GROUP BY c.date, c.app_id, c.country ) tmp ON tmp.app_id = c.app_id AND tmp.date = c.date AND tmp.country = c.country SET c.campaignid = tmp.campaignid WHERE STR_TO_DATE(c.date, '%d-%m-%Y') BETWEEN '" + StartDate + "' AND '" + EndDate + "'; ";
                            /*--------------/UPDATE campaignid in tblcsvdata.---------------*/

                            DBQuery += "DROP TABLE TMP_CAMPAIGN_DATA; ";
                            //WriteLogToFile("Query:[" + DBQuery + "]"); // for syntax error tracking 
                            DBOperation(DBQuery, resp.Results.Count);
                        }
                    }
                );

            }
            catch (GoogleAdsException ex)
            {
                WriteLogToFile("Google API Failure:");
                WriteLogToFile("Message: " + ex.Message);
                WriteLogToFile("Failure: " + ex.Failure);
                WriteLogToFile("Request ID: " + ex.RequestId);
            }

        }

        void DBOperation(string DBQuery, int totalrecords)
        {
            string json = File.ReadAllText(path + "OtherSettings.json");
            AppSettings AS = JsonConvert.DeserializeObject<AppSettings>(json);
            string connectionString = "server=" + AS.DBServerName + ";port=" + AS.Port + ";userid=" + AS.UserName + ";password=" + AS.Password + ";database=" + AS.DatabaseName;
            //string cs = System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

            MySqlConnection conn = null;

            //WriteLogToFile("Database connection is started.");
            try
            {
                conn = new MySqlConnection(connectionString);

                string DBResponse = string.Empty;

                MySqlCommand cmd = new MySqlCommand(DBQuery, conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                conn.Open();
                MySqlDataReader MyReader = cmd.ExecuteReader();     // Here our query will be executed and data saved into the database.

                while (MyReader.Read())
                {
                    DBResponse = MyReader.GetString(0);
                }
                conn.Close();

                //WriteLogToFile("Database Response: " + (string)cmd.Parameters["@Result"].Value);
                WriteLogToFile(totalrecords + " records are updated to database.");
            }
            catch (Exception ex)
            {
                WriteLogToFile("DB Error: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        void GetChangeHistory()
        {
            string json = File.ReadAllText(path + "OtherSettings.json");
            AppSettings AS = JsonConvert.DeserializeObject<AppSettings>(json);

            customerId = AS.GAdAccCustomerId;
            string connectionString = "server=" + AS.DBServerName + ";port=" + AS.Port + ";userid=" + AS.UserName + ";password=" + AS.Password + ";database=" + AS.DatabaseName;

            string StartDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm");
            string EndDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(path + "GoogleAdsApi.json");
            IConfigurationRoot configRoot = builder.Build();

            GoogleAdsConfig config = new GoogleAdsConfig(configRoot);
            GoogleAdsClient client = new GoogleAdsClient(config);
            //GoogleAdsClient client = new GoogleAdsClient();
            // Get the GoogleAdsService.
            GoogleAdsServiceClient googleAdsService = client.GetService(
                Services.V11.GoogleAdsService);

            string query =
                @"SELECT 
                    change_event.change_date_time, 
                    change_event.resource_name,
                    change_event.changed_fields, 
                    change_event.new_resource, 
                    change_event.old_resource, 
                    change_event.resource_change_operation, 
                    change_event.change_resource_name, 
                    change_event.change_resource_type, 
                    change_event.ad_group, 
                    change_event.asset, 
                    change_event.user_email,
                    change_event.campaign, 
                    campaign.name,
                    campaign.id,
                    ad_group.name,
                    ad_group.id
                FROM change_event 
                WHERE change_event.change_date_time >= '" + StartDate + "' AND change_event.change_date_time <= '" + EndDate + "' ORDER BY change_event.change_date_time DESC LIMIT 10000";

            try
            {
                // Issue a search request.
                googleAdsService.SearchStream(customerId, query,
                    delegate (SearchGoogleAdsStreamResponse resp)
                    {
                        // Display the results.
                        WriteLogToFile(resp.Results.Count + " records found from the google ads change history.");

                        string DBQuery = "", temp = "";

                        DBQuery += "CREATE TEMPORARY TABLE TMP_CHANGE_HISTORY( acc_customer_id varchar(50), change_date_time varchar(50), user_email varchar(100), change_resource_type varchar(200), resource_change_operation varchar(200), change_message LONGTEXT, campaignid varchar(50), campaign varchar(100), ad_group_id varchar(50), ad_group varchar(100) ); ";
                        DBQuery += "insert into TMP_CHANGE_HISTORY(acc_customer_id,change_date_time,user_email,change_resource_type,resource_change_operation,change_message,campaignid,campaign,ad_group_id,ad_group) values";

                        if (resp.Results.Count > 0)
                        {
                            string message = "";
                            foreach (GoogleAdsRow rowobj in resp.Results)
                            {
                                ChangeEvent changeEvent = rowobj.ChangeEvent;
                                ChangedResource oldResource = changeEvent.OldResource;
                                ChangedResource newResource = changeEvent.NewResource;

                                bool knownResourceType = true;
                                IMessage oldResourceEntity = null;
                                IMessage newResourceEntity = null;
                                switch (changeEvent.ChangeResourceType)
                                {
                                    case ChangeEventResourceType.Ad:
                                        oldResourceEntity = oldResource.Ad;
                                        newResourceEntity = newResource.Ad;
                                        break;

                                    case ChangeEventResourceType.AdGroup:
                                        oldResourceEntity = oldResource.AdGroup;
                                        newResourceEntity = newResource.AdGroup;
                                        break;

                                    case ChangeEventResourceType.AdGroupAd:
                                        oldResourceEntity = oldResource.AdGroupAd;
                                        newResourceEntity = newResource.AdGroupAd;
                                        break;

                                    case ChangeEventResourceType.AdGroupAsset:
                                        oldResourceEntity = oldResource.AdGroupAsset;
                                        newResourceEntity = newResource.AdGroupAsset;
                                        break;

                                    case ChangeEventResourceType.AdGroupBidModifier:
                                        oldResourceEntity = oldResource.AdGroupBidModifier;
                                        newResourceEntity = newResource.AdGroupBidModifier;
                                        break;

                                    case ChangeEventResourceType.AdGroupCriterion:
                                        oldResourceEntity = oldResource.AdGroupCriterion;
                                        newResourceEntity = newResource.AdGroupCriterion;
                                        break;

                                    case ChangeEventResourceType.AdGroupFeed:
                                        oldResourceEntity = oldResource.AdGroupFeed;
                                        newResourceEntity = newResource.AdGroupFeed;
                                        break;

                                    case ChangeEventResourceType.Asset:
                                        oldResourceEntity = oldResource.Asset;
                                        newResourceEntity = newResource.Asset;
                                        break;

                                    case ChangeEventResourceType.AssetSet:
                                        oldResourceEntity = oldResource.AssetSet;
                                        newResourceEntity = newResource.AssetSet;
                                        break;

                                    case ChangeEventResourceType.AssetSetAsset:
                                        oldResourceEntity = oldResource.AssetSetAsset;
                                        newResourceEntity = newResource.AssetSetAsset;
                                        break;

                                    case ChangeEventResourceType.Campaign:
                                        oldResourceEntity = oldResource.Campaign;
                                        newResourceEntity = newResource.Campaign;
                                        break;

                                    case ChangeEventResourceType.CampaignAsset:
                                        oldResourceEntity = oldResource.CampaignAsset;
                                        newResourceEntity = newResource.CampaignAsset;
                                        break;

                                    case ChangeEventResourceType.CampaignAssetSet:
                                        oldResourceEntity = oldResource.CampaignAssetSet;
                                        newResourceEntity = newResource.CampaignAssetSet;
                                        break;

                                    case ChangeEventResourceType.CampaignBudget:
                                        oldResourceEntity = oldResource.CampaignBudget;
                                        newResourceEntity = newResource.CampaignBudget;
                                        break;

                                    case ChangeEventResourceType.CampaignCriterion:
                                        oldResourceEntity = oldResource.CampaignCriterion;
                                        newResourceEntity = newResource.CampaignCriterion;
                                        break;

                                    case ChangeEventResourceType.CampaignFeed:
                                        oldResourceEntity = oldResource.CampaignFeed;
                                        newResourceEntity = newResource.CampaignFeed;
                                        break;

                                    case ChangeEventResourceType.CustomerAsset:
                                        oldResourceEntity = oldResource.CustomerAsset;
                                        newResourceEntity = newResource.CustomerAsset;
                                        break;

                                    case ChangeEventResourceType.Feed:
                                        oldResourceEntity = oldResource.Feed;
                                        newResourceEntity = newResource.Feed;
                                        break;

                                    case ChangeEventResourceType.FeedItem:
                                        oldResourceEntity = oldResource.FeedItem;
                                        newResourceEntity = newResource.FeedItem;
                                        break;

                                    default:
                                        knownResourceType = false;
                                        break;
                                }

                                message = "";
                                foreach (string fieldMaskPath in changeEvent.ChangedFields.Paths)
                                {
                                    if (changeEvent.ResourceChangeOperation ==
                                        ResourceChangeOperation.Create)
                                    {
                                        object newValue = FieldMasks.GetFieldValue(
                                            fieldMaskPath, newResourceEntity);

                                        if (changeEvent.ChangeResourceType == ChangeEventResourceType.CampaignBudget && IsNumber(newValue))
                                        {
                                            message += fieldMaskPath + " set to " + ((Convert.ToDouble(newValue) / 10000.00) / 100.00) + ". ";
                                        }
                                        else
                                        {
                                            message += fieldMaskPath + " set to " + newValue + ". ";
                                        }
                                    }
                                    else if (changeEvent.ResourceChangeOperation ==
                                        ResourceChangeOperation.Update)
                                    {
                                        object oldValue = FieldMasks.GetFieldValue(fieldMaskPath,
                                            oldResourceEntity);
                                        object newValue = FieldMasks.GetFieldValue(fieldMaskPath,
                                            newResourceEntity);

                                        if (changeEvent.ChangeResourceType == ChangeEventResourceType.CampaignBudget && IsNumber(newValue))
                                        {
                                            message += fieldMaskPath + " changed from " + ((Convert.ToDouble(oldValue.ToString()) / 10000.00) / 100.00) + " to " + ((Convert.ToDouble(newValue.ToString()) / 10000.00) / 100.00) + ". ";
                                        }
                                        else
                                        {
                                            message += fieldMaskPath + " changed from " + oldValue.ToString() + " to " + newValue.ToString() + ". ";
                                        }
                                    }
                                }

                                temp += ",('" + customerId + "','" + rowobj.ChangeEvent.ChangeDateTime + "','" + rowobj.ChangeEvent.UserEmail + "','" + rowobj.ChangeEvent.ChangeResourceType.ToString() + "','" + rowobj.ChangeEvent.ResourceChangeOperation.ToString() + "','" + (message).Replace("'", "''") + "','" + rowobj.Campaign.Id.ToString() + "','" + (rowobj.Campaign.Name).Replace("'", "''") + "','" + rowobj.AdGroup.Id.ToString() + "','" + (rowobj.AdGroup.Name).Replace("'", "''") + "')";
                            }

                            DBQuery = DBQuery + temp.Substring(1) + "; ";

                            DBQuery += "INSERT INTO tblchangehistory(acc_customer_id,change_date_time,user_email,change_resource_type,resource_change_operation,change_message,campaignid,campaign,ad_group_id,ad_group) "
                                    + " SELECT TMP.acc_customer_id,TMP.change_date_time,TMP.user_email,TMP.change_resource_type,TMP.resource_change_operation,TMP.change_message,TMP.campaignid,TMP.campaign,TMP.ad_group_id,TMP.ad_group FROM TMP_CHANGE_HISTORY TMP LEFT JOIN tblchangehistory CH ON TMP.change_date_time = CH.change_date_time AND TMP.acc_customer_id = CH.acc_customer_id WHERE CH.change_date_time IS NULL; "
                                    + " DROP TABLE TMP_CHANGE_HISTORY;";

                            MySqlConnection conn = null;

                            var ssh_host = "ec2-15-207-226-221.ap-south-1.compute.amazonaws.com";
                            int ssh_port = 22;
                            var ssh_user = "ubuntu";
                            var ssh_password = "Grow@123#";
                            var keyFile = new PrivateKeyFile(@"C:\Users\Admin\OneDrive\Desktop\grow-application.pem");
                            var keyFiles = new[] { keyFile };

                            var methods = new List<AuthenticationMethod>();
                            methods.Add(new PasswordAuthenticationMethod(ssh_user, ssh_password));
                            methods.Add(new PrivateKeyAuthenticationMethod(ssh_user, keyFiles));
                            ConnectionInfo conInfo = new ConnectionInfo(ssh_host, ssh_port, ssh_user, methods.ToArray());
                            conInfo.Timeout = TimeSpan.FromSeconds(1800);

                            // connect to the SSH server
                            var sshClient = new SshClient(conInfo);
                            sshClient.Connect();

                            // forward a local port to the database server and port, using the SSH server
                            var forwardedPort = new ForwardedPortLocal("127.0.0.1", ssh_host, Convert.ToUInt16(AS.Port));
                            sshClient.AddForwardedPort(forwardedPort);
                            forwardedPort.Start();

                            conn = new MySqlConnection(connectionString);
                            try
                            {
                                MySqlCommand cmd = new MySqlCommand(DBQuery, conn);
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandTimeout = 0;
                                conn.Open();
                                MySqlDataReader MyReader = cmd.ExecuteReader();     // Here our query will be executed and data saved into the database.

                                string DBResponse = string.Empty;
                                while (MyReader.Read())
                                {
                                    DBResponse = MyReader.GetString(0);
                                }
                                conn.Close();
                                sshClient.Disconnect();

                                //WriteLogToFile("Database Response: " + (string)cmd.Parameters["@Result"].Value);
                                WriteLogToFile(resp.Results.Count + " records are updated to database.");
                            }
                            catch (Exception ex)
                            {
                                WriteLogToFile("DB Error: " + ex.Message);
                            }
                            finally
                            {
                                if (conn != null)
                                {
                                    conn.Close();
                                    sshClient.Disconnect();
                                }
                            }
                        }
                    }
                );

            }
            catch (GoogleAdsException ex)
            {
                WriteLogToFile("Google API Failure:");
                WriteLogToFile("Message: " + ex.Message);
                WriteLogToFile("Failure: " + ex.Failure);
                WriteLogToFile("Request ID: " + ex.RequestId);
            }

        }
        /*---------------------Functions for get data from api and store to my sql database----------------------*/

        public static bool IsNumber(object value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }
    }

    public class GAPIParameters
    {
        public string ProxyServer { get; set; }
        public string ProxyUser { get; set; }
        public string ProxyPassword { get; set; }
        public string ProxyDomain { get; set; }
        public string DeveloperToken { get; set; }
        public string OAuth2ClientId { get; set; }
        public string OAuth2ClientSecret { get; set; }
        public string OAuth2Mode { get; set; }
        public string OAuth2RefreshToken { get; set; }
        public string LoginCustomerId { get; set; }
    }

    public class AppSettings
    {
        public string DBServerName { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
        public string AutoSyncTime { get; set; }
        public string GAdAccCustomerId { get; set; }
        public string PastRecordsDays { get; set; }
    }

}
