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
//using static InfoCannon.Prompt;

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
            items.Add(new Item() { Text = "Alex Jones Show", Value = "https://api.infowarsmedia.com/api/channel/5b885d33e6646a0015a6fa2d/videos?limit=100&offset=0" });
            items.Add(new Item() { Text = "David Knight", Value = "https://api.infowarsmedia.com/api/channel/5b92d71e03deea35a4c6cdef/videos?limit=100&offset=0" });
            items.Add(new Item() { Text = "War Room", Value = "https://api.infowarsmedia.com/api/channel/5b9301172abf762e22bc22fd/videos?limit=100&offset=0" });
            items.Add(new Item() { Text = "Special Reports", Value = "https://api.infowarsmedia.com/api/channel/5b9429906a1af769bc31efeb/videos?limit=100&offset=0" });
            //items.Add(new Item() { Text = "Off Limits", Value = "https://api.infowarsmedia.com/api/channel/5cf1903bf4c60d001292685e/videos?limit=100&offset=0" });

            lstSource.DataSource = items;
            lstSource.DisplayMember = "Text";
            lstSource.ValueMember = "Value";

            ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.btnUncheckAll, "Uncheck All");
            ToolTip1.SetToolTip(this.btnCheckAll, "Check All");
            ToolTip1.SetToolTip(this.btnTest, "Send a test message to the selected page using this access key.");
            ToolTip1.SetToolTip(this.btnAddPageID, "Add a Page ID");
            ToolTip1.SetToolTip(this.btnRemovePageID, "Remove selected Page ID");
            ToolTip1.SetToolTip(this.btnProcess, "Download videos from selected VOD sources");
            ToolTip1.SetToolTip(this.btnPostVideos, "Uploaded selected videos to selected Page ID on Facebook");
            ToolTip1.SetToolTip(this.btnSaveSettings, "Save Page IDs and Access Key to Settings File");
            ToolTip1.SetToolTip(this.txtExtraText, "This text will be added to the end of each video description, must be set before gathering videos.");

            //Locate and parse the user's settings file
            UserSettings settings = UserSettings.Load();
            txtAccessKey.Text = settings.accessCode;
            txtExtraText.Text = settings.extratext;
            foreach (object obj in settings.pageId)
            {
                lstPageID.Items.Add(obj.ToString());
            }

            if (lstPageID.Items.Count > 0) {
                lstPageID.SelectedIndex = 0;
            }

            dpVideosFrom.Value = DateTime.Now;

            //Init Facebook Interface
            facebookClient = new FacebookClient();
            facebookService = new FacebookService(facebookClient);
        }

        public class VideoToPost {
            public string id {get; set; }
            public string title { get; set; }
            public string url { get; set; }
            public string summary { get; set; }
            public string channel { get; set; }
            public DateTime createdAt { get; set; }
            public string thumbnail { get; set; }
        }

        public class PostedVideo
        {
            public string VideoID { get; set; }
            public string PageID { get; set; }
        }

        public class VODResult
        {
            public string response { get; set; }
            public string channel { get; set; }
        }

        //Gathers Videos from Infowars Servers
        private async void btnProcess_Click(object sender, EventArgs e) {
            if (lstPageID.Items.Count == 0) {
                MessageBox.Show("You must select a Page ID from the list", "Error");
                return;
            }

            if (lstPageID.SelectedItem == null) {
                MessageBox.Show("You must select a Page ID from the list", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop );
                return;
            }
            string PageID = lstPageID.SelectedItem.ToString().Trim();
            string ExtraText = txtExtraText.Text;
            if (lstSource.SelectedItems == null) {
                MessageBox.Show("You must select at least one VOD Source to pull videos from", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            ((Button)sender).Enabled = false;
            lstSource.Enabled = false;
            var client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            QueuedVideos = new List<VideoToPost>();
            List<VODResult> VODResults = new List<VODResult>();
            lstVideos.Items.Clear();

            await Task.Run(() => { SetStatus("Connecting to Infowars Servers..."); });
            foreach (Item itm in lstSource.SelectedItems) {
                try
                {
                    string response = await client.DownloadStringTaskAsync(new Uri(itm.Value.ToString()));
                    VODResults.Add(new VODResult { response = response, channel = itm.Text });
                } catch
                {
                    await Task.Run(() => { SetStatus("Connection to Infowars Failed...Please try again"); });
                    ((Button)sender).Enabled = true;
                    lstSource.Enabled = true;
                    return;
                }
            }

            await Task.Run(() => { SetStatus("Gathering Videos..."); });
            await Task.Run(() => {

                foreach (VODResult result in VODResults) {
                    dynamic parsed = JsonConvert.DeserializeObject<dynamic>((string)result.response);
                    //Collect videos for date
                    foreach (var data in parsed.videos)
                    {
                        try
                        {
                            DateTime createdAt = data?.createdAt;
                            DateTime localCreatedAt = TimeZone.CurrentTimeZone.ToLocalTime((DateTime)createdAt);
                            if (localCreatedAt.Date == dpVideosFrom.Value.Date && createdAt.Date != null)
                            {
                                string title = data?.title;
                                string summary = data?.summary;
                                QueuedVideos.Add(new VideoToPost()
                                {
                                    id = data._id,
                                    title = title,
                                    url = data?.directUrl,
                                    summary = (title.Trim() + Environment.NewLine + Environment.NewLine + summary.Replace(title, "").Replace("<br>", Environment.NewLine).Replace(Environment.NewLine + Environment.NewLine, "").Trim() + " " + ExtraText),
                                    createdAt = createdAt,
                                    channel = result.channel,
                                    thumbnail = data?.posterLargeUrl
                                });
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            });

            QueuedVideos = QueuedVideos.OrderByDescending(x => x.createdAt).ToList();

            UserSettings settings = UserSettings.Load();
            int loopcount = 0;
            foreach (VideoToPost video in QueuedVideos) {
                //ListViewItem item = new ListViewItem(new[] { "1", "2" });
                //lstVideos.Items.Add(item);

                lstVideos.Items.Add(new ListViewItem { Text = video.title, ToolTipText = video.summary, Checked = (!settings.postedVideos.Any(x => x.PageID == PageID && x.VideoID == video.id)), SubItems = { video.channel } });
                loopcount++;
            }

            ((Button)sender).Enabled = true;
            lstSource.Enabled = true;

            if (QueuedVideos.Count == 0) {
                SetStatus("No videos found");
            } else {
                toolStripStatusLabel1.Text = "";
            }

        }

        private async void btnPostVideos_Click(object sender, EventArgs e) {
            if (lstPageID.Items.Count == 0) {
                MessageBox.Show("You must select a Page ID from the list", "Error");
                return;
            }

            if (lstPageID.SelectedItem == null) {
                MessageBox.Show("You must select a Page ID from the list", "Error");
                return;
            }
            string PageID = lstPageID.SelectedItem.ToString().Trim();
            string AccessKey = txtAccessKey.Text;

            ListView.CheckedIndexCollection CheckedIndices = lstVideos.CheckedIndices;
            List<int> SelectedIndices = new List<int>();
            foreach (int index in CheckedIndices) {
                SelectedIndices.Add(index);
            }

            btnProcess.Enabled = false;
            btnPostVideos.Enabled = false;
            UserSettings settings = UserSettings.Load();
            await Task.Run(() =>
            {
                int intCount = 0;
                foreach (int index in SelectedIndices) {
                    VideoToPost video = QueuedVideos[index];
                    intCount++;
                    SetStatus("Posting Video..." + intCount.ToString() + " of " + SelectedIndices.Count().ToString() + ": " + video.title);
                    List<attached_media> UploadedMedia = new List<attached_media>();
                    var postOnWallTask = facebookService.PostOnWallAsync(AccessKey, PageID, video.summary, "", UploadedMedia, video.url, video.title, video.thumbnail);
                    Task[] array = new Task[] { postOnWallTask };
                    try {
                        Task.WaitAll(array, -1);
                        if (!settings.postedVideos.Any(x => x.PageID == PageID && x.VideoID == video.id))
                        {
                            settings.postedVideos.Add(new PostedVideo { VideoID = video.id, PageID = PageID } );
                            settings.Save();
                        }
                    } catch {

                    }
                }
                SetStatus(SelectedIndices.Count().ToString() + " Videos Posted!");
            });

            //Uncheck processed items
            btnProcess.Enabled = true;
            btnPostVideos.Enabled = true;
            lstVideos.Items.OfType<ListViewItem>().Where(x=>SelectedIndices.Contains(x.Index)).ToList().ForEach(item => item.Checked = false);
        }

        private void btnSaveSettings_Click(object sender, EventArgs e) {
            UserSettings settings = UserSettings.Load();
            settings.pageId = new List<string>();
            foreach (object obj in lstPageID.Items) {
                settings.pageId.Add(obj.ToString());
            }

            settings.extratext = txtExtraText.Text;
            settings.accessCode = txtAccessKey.Text;
            settings.Save();
            SetStatus("Facebook Settings Saved");
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

            if (lstPageID.Items.Count == 0) {
                MessageBox.Show("You must select a Page ID from the list", "Error");
                return;
            }

            if (lstPageID.SelectedItem == null) {
                MessageBox.Show("You must select a Page ID from the list");
                return;
            }

            string PageID = lstPageID.SelectedItem.ToString();
            string AccessKey = txtAccessKey.Text;
            Task.Run(() =>
            {
                SetStatus("Sending test post to Facebook");
                var postOnWallTask1 = facebookService.PostOnWallAsync(AccessKey, PageID, "This is a test", "");
                Task.WaitAll(postOnWallTask1);
                SetStatus("Post Completed");
            });
        }

        private void lstVideos_SelectedIndexChanged(object sender, EventArgs e) {
            //ListView.SelectedListViewItemCollection selectedvideos = lstVideos.SelectedItems[0];
            //foreach (ListViewItem item in selectedvideos) {
            //}
        }

        private void lstPageID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.C)
            {
                string s = lstPageID.SelectedItem.ToString();
                Clipboard.SetData(DataFormats.StringFormat, s);
            }
        }

        private void btnAddPageID_Click(object sender, EventArgs e)
        {
            string PromptValue = Prompt.ShowDialog("The Page ID can be found at the bottom of the About section of the page.", "Enter Page ID");
            if (PromptValue.Trim() == "")
            {
                //MessageBox.Show("This Page ID is empty", "Error");
            } else if (lstPageID.Items.Contains(PromptValue)) {
                MessageBox.Show("This Page ID already exists", "Error");
            } else {
                lstPageID.Items.Add(PromptValue);
            }

        }

        private void btnRemovePageID_Click(object sender, EventArgs e)
        {
            if (lstPageID.SelectedItem == null) {
                MessageBox.Show("You must select a Page ID from the list");
                return;
            }
            lstPageID.Items.Remove(lstPageID.SelectedItem);
        }

        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            lstVideos.Items.OfType<ListViewItem>().ToList().ForEach(item => item.Checked = true);
        }

        private void btnUncheckAll_Click(object sender, EventArgs e)
        {
            lstVideos.Items.OfType<ListViewItem>().ToList().ForEach(item => item.Checked = false);
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
