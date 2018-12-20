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
using System.Threading;

namespace InfoCannon {


    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        public FacebookService facebookService { get; set; }
        public FacebookClient facebookClient { get; set; }
        public List<VideoToPost> QueuedVideos = new List<VideoToPost>();

        private void Form1_Load(object sender, EventArgs e) {
            List<Item> items = new List<Item>();
            items.Add(new Item() { Text = "Alex Jones Show", Value = "https://vod-api.infowars.com/api/channel/5b885d33e6646a0015a6fa2d/videos?limit=100&offset=0" });
            items.Add(new Item() { Text = "David Knight", Value = "https://vod-api.infowars.com/api/channel/5b92d71e03deea35a4c6cdef/videos?limit=100&offset=0" });
            items.Add(new Item() {Text = "War Room", Value = "https://vod-api.infowars.com/api/channel/5b9301172abf762e22bc22fd/videos?limit=100&offset=0" });
            items.Add(new Item() { Text = "Special Reports", Value = "https://vod-api.infowars.com/api/channel/5b9429906a1af769bc31efeb/videos?limit=100&offset=10" });

            cmbSource.DataSource = items;
            cmbSource.DisplayMember = "Text";
            cmbSource.ValueMember = "Value";

            //Locate and parse the user's settings file
            UserSettings settings = UserSettings.Load();
            txtAccessKey.Text = settings.accessCode;
            txtPageID.Text = settings.pageId;
            dpVideosFrom.Value = DateTime.Now;

            //Init Facebook Interface
            facebookClient = new FacebookClient();
            facebookService = new FacebookService(facebookClient);
        }

        public class VideoToPost {
            public string title { get; set; }
            public string url { get; set; }
            public string summary { get; set; }
            public DateTime createdAt { get; set; }
        }

        private async void btnProcess_Click(object sender, EventArgs e) {
            await Task.Run(() => { SetStatus("Connecting to Infowars Servers..."); });
            var client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            var response = await client.DownloadStringTaskAsync(new Uri(cmbSource.SelectedValue.ToString()));
            lstVideos.Items.Clear();
            await Task.Run(() => {
                dynamic parsed = JsonConvert.DeserializeObject<dynamic>((string)response);
                SetStatus("Gathering Videos...");
                
                //Collect videos for date
                QueuedVideos = new List<VideoToPost>();
                foreach (var data in parsed.videos) {
                    try {
                        DateTime createdAt = data?.createdAt;
                        if ((DateTime)createdAt.Date == dpVideosFrom.Value.Date && createdAt.Date != null) {
                            QueuedVideos.Add(new VideoToPost() {
                                title = data?.title,
                                url = data?.directUrl,
                                summary = data?.summary,
                                createdAt = createdAt
                            });
                        }
                    } catch {

                    }
                }

                QueuedVideos = QueuedVideos.OrderByDescending(x => x.createdAt).ToList();
            });

            int loopcount = 0;
            foreach (VideoToPost video in QueuedVideos) {
                lstVideos.Items.Add(new ListViewItem { Text = video.title, ToolTipText = video.summary, Checked = true } );
                loopcount++;
            }

            toolStripStatusLabel1.Text = "";
        }

        private async void btnPostVideos_Click(object sender, EventArgs e) {
            ListView.CheckedIndexCollection CheckedIndices = lstVideos.CheckedIndices;
            List<int> SelectedIndices = new List<int>();
            foreach (int index in CheckedIndices) {
                SelectedIndices.Add(index);
            }

            await Task.Run(() =>
            {
                int intCount = 0;
                foreach (int index in SelectedIndices) {
                    VideoToPost video = QueuedVideos[index];
                    intCount++;
                    SetStatus("Posting Video..." + intCount.ToString() + " of " + SelectedIndices.Count().ToString() + ": " + video.title);
                    List<attached_media> UploadedMedia = new List<attached_media>();
                    var postOnWallTask = facebookService.PostOnWallAsync(txtAccessKey.Text, txtPageID.Text, video.summary, "", UploadedMedia, video.url);
                    Task[] array = new Task[] { postOnWallTask };
                    try {
                        Task.WaitAll(array, -1);
                    } catch {

                    }
                }
                SetStatus(SelectedIndices.Count().ToString() + " Videos Posted!");
            });
        }

        private void btnSaveSettings_Click(object sender, EventArgs e) {
            UserSettings settings = UserSettings.Load();
            settings.accessCode = txtAccessKey.Text;
            settings.pageId = txtPageID.Text;
            settings.Save();
            SetStatus("Facebook Login Settings Saved");
        }

        public void SetStatus(string msg, bool setClear = true) {
            toolStripStatusLabel1.Text = msg;
            clearStatus.Enabled = setClear;
        }

        private void clearStatus_Tick(object sender, EventArgs e) {
            toolStripStatusLabel1.Text = "";
            clearStatus.Enabled = false;
        }

        private void btnTest_Click(object sender, EventArgs e) {
            Task.Run(() =>
            {
                SetStatus("Sending test post to Facebook");
                var postOnWallTask1 = facebookService.PostOnWallAsync(txtAccessKey.Text, txtPageID.Text, "This is a test", "");
                Task.WaitAll(postOnWallTask1);
                SetStatus("Post Completed");
            });
        }

        private void lstVideos_SelectedIndexChanged(object sender, EventArgs e) {
            //ListView.SelectedListViewItemCollection selectedvideos = lstVideos.SelectedItems[0];
            //foreach (ListViewItem item in selectedvideos) {

            //}
        }
    }
}
