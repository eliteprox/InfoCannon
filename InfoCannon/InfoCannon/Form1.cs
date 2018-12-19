using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfoCannon;
using System.Web.Script.Serialization;
using System.Net;
using Newtonsoft.Json;
using static InfoCannon.FacebookClient;

namespace InfoCannon {


    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        public FacebookService facebookService { get; set; }
        public FacebookClient facebookClient { get; set; }

        private void Form1_Load(object sender, EventArgs e) {
            List<Item> items = new List<Item>();
            items.Add(new Item() { Text = "Alex Jones Show", Value = "https://vod-api.infowars.com/api/channel/5b885d33e6646a0015a6fa2d/videos?limit=3&offset=1" });
            items.Add(new Item() { Text = "David Knight", Value = "https://vod-api.infowars.com/api/channel/5b92d71e03deea35a4c6cdef/videos?limit=3&offset=1" });
            items.Add(new Item() { Text = "Special Reports", Value = "https://vod-api.infowars.com/api/channel/5b9301172abf762e22bc22fd/videos?limit=3&offset=1" });
            cmbSource.DataSource = items;
            cmbSource.DisplayMember = "Text";
            cmbSource.ValueMember = "Value";

            //Locate and parse the user's settings file
            UserSettings settings = UserSettings.Load();
            txtAccessKey.Text = settings.accessCode;
            txtPageID.Text = settings.pageId;

            //Post to Facebook
            facebookClient = new FacebookClient();
            facebookService = new FacebookService(facebookClient);
            //var getAccountTask = facebookService.GetAccountAsync(txtAccessKey.Text);
        }

        private async void btnProcess_Click(object sender, EventArgs e) {
            await Task.Run(() => { SetStatus("Connecting to Infowars Servers..."); });
            var client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            var response = await client.DownloadStringTaskAsync(new Uri(cmbSource.SelectedValue.ToString()));
            await Task.Run(() => {
                dynamic parsed = JsonConvert.DeserializeObject<dynamic>((string)response);
                SetStatus("Posting Videos...");
                int intCount = 0;
                foreach (var data in parsed.videos) {
                    intCount++;
                    string title = data?.title;
                    string URL = data?.directUrl;
                    string summary = data?.summary;
                    SetStatus("Posting Video..." + intCount.ToString() + " of " + parsed.videos.Count.ToString());
                    List<attached_media> UploadedMedia = new List<attached_media>();
                    var postOnWallTask = facebookService.PostOnWallAsync(txtAccessKey.Text, txtPageID.Text, summary, "", UploadedMedia, URL);
                    Task.WaitAll(postOnWallTask);
                }

                SetStatus(parsed.videos.Count.ToString() + " Videos Posted!");
            });

            //client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(loadHTMLCallback);
            //client.DownloadStringAsync(new Uri(cmbSource.SelectedValue.ToString()));
        }

        private void btnSaveSettings_Click(object sender, EventArgs e) {
            UserSettings settings = UserSettings.Load();
            settings.accessCode = txtAccessKey.Text;
            settings.pageId = txtPageID.Text;
            settings.Save();
            SetStatus("Facebook Login Settings Saved");
            clearStatus.Enabled = true;
        }

        public void SetStatus(string msg, bool setClear = true) {
            toolStripStatusLabel1.Text = msg;
            if (setClear) {
                clearStatus.Enabled = true;
            }
        }

        private void clearStatus_Tick(object sender, EventArgs e) {
            toolStripStatusLabel1.Text = "";
            clearStatus.Enabled = false;
        }

        private void btnTest_Click(object sender, EventArgs e) {
            Task.Run(() =>
            {
                SetStatus("Sending test post to Facebook");
                var postOnWallTask = facebookService.PostOnWallAsync(txtAccessKey.Text, txtPageID.Text, "This is a test", "");
                Task.WaitAll(postOnWallTask);
                SetStatus("Post Completed");
            });
        }
    }
}
