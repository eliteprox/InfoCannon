using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace InfoCannon {
    public class UploadedMedia {
        public string Id { get; set; }
    }

    public class attached_media {
        public string media_fbid { get; set; }
    }

    public class upload_video
    {
        public string file_url { get; set; }
        public string description { get; set; }
        public string title { get; set; }
    }

    public interface IFacebookClient {
        Task<T> GetAsync<T>(string accessToken, string endpoint, string args = null);
        Task<T> PostAsyncReturn<T>(string accessToken, string endpoint, object data, string args = null);
        Task PostAsync(string accessToken, string endpoint, object data, string args = null);
        Task<T> PostASyncReturn_UploadImage<T>(string accessToken, string endpoint, upload_video data, string img_url = "", string args = null);
    }

    public class FacebookClient : IFacebookClient {
        private readonly HttpClient _httpClient;

        public FacebookClient() {
            _httpClient = new HttpClient {
                BaseAddress = new Uri("https://graph.facebook.com/v3.2/")
            };
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<T> GetAsync<T>(string accessToken, string endpoint, string args = null) {
            var response = await _httpClient.GetAsync($"{endpoint}?access_token={accessToken}&{args}");
            if (!response.IsSuccessStatusCode)
                return default(T);

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task<T> PostAsyncReturn<T>(string accessToken, string endpoint, object data, string args = null) {
            var payload = GetPayload(data);
            var response = await _httpClient.PostAsync($"{endpoint}?access_token={accessToken}&{args}", payload);
            if (!response.IsSuccessStatusCode)
                return default(T);

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task PostAsync(string accessToken, string endpoint, object data, string args = null) {
            var payload = GetPayload(data);
            await _httpClient.PostAsync($"{endpoint}?access_token={accessToken}&{args}", payload);
        }

        public async Task<T> PostASyncReturn_UploadImage<T>(string accessToken, string endpoint, upload_video data, string img_url = "", string args = null)
        {
            byte[] imageBytes = null;
            //Download the image content to bytes
            Uri uri = new Uri(img_url);
            if (false == String.IsNullOrWhiteSpace(img_url)) {
                using (var webClient = new System.Net.WebClient()) {
                    imageBytes = webClient.DownloadData(uri);
                }
            }

            //Obtain Filename
            string filename = System.IO.Path.GetFileName(uri.LocalPath);

            MemoryStream imageMemoryStream = new MemoryStream();
            imageMemoryStream.Write(imageBytes, 0, imageBytes.Length);
            Image newImage = Image.FromStream(imageMemoryStream, true, true);
            ImageConverter _imageConverter = new ImageConverter();
            byte[] paramFileStream = (byte[])_imageConverter.ConvertTo(newImage, typeof(byte[]));


            //Formulate a Request
            var content = new MultipartFormDataContent
            {
                //send form text values here
                 {new StringContent(data.file_url),"file_url"},
                 {new StringContent(data.description), "description"},
                 {new StringContent(data.title), "title"},
            };

            //Send Image Here
            if (imageBytes != null) {
                content.Add(new StreamContent(new MemoryStream(paramFileStream)), "thumb", filename);
            }

            var response = await _httpClient.PostAsync($"{endpoint}?access_token={accessToken}&{args}", content);
            if (!response.IsSuccessStatusCode)
                return default(T);

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task<T> PostImageAsyncReturn<T>(string accessToken, string endpoint, object data, string args = null)
        {
            var payload = GetPayload(data);
            var response = await _httpClient.PostAsync($"{endpoint}?access_token={accessToken}&{args}", payload);
            if (!response.IsSuccessStatusCode)
                return default(T);

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }

        private static StringContent GetPayload(object data) {
            var json = JsonConvert.SerializeObject(data);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public interface IFacebookService {
            Task<UploadedMedia> UploadPhotoOnWallAsync(string accessToken, string pageid = "me", string url = "", bool published = false, bool temporary = true);
            Task<UploadedMedia> UploadVideoOnWallAsync(string accessToken, string pageid = "me", string file_url = "", string description = "", string title = "", string thumbnail = "");
            Task PostOnWallAsync(string accessToken, string pageid = "me", string message = "", string link = "", List<attached_media> attached_media = null, string PostedVideo = "", string title = "", string thumbnail = "");
        }

        public class FacebookService : IFacebookService {
            private readonly IFacebookClient _facebookClient;

            public FacebookService(IFacebookClient facebookClient) {
                _facebookClient = facebookClient;
            }

            public async Task<UploadedMedia> UploadVideoOnWallAsync(string accessToken, string pageid = "me", string file_url = "", string description = "", string title = "", string thumbnail = "") {
                //var result = await _facebookClient.PostAsyncReturn<dynamic>(accessToken, pageid + "/videos", new { file_url, description, title });
                var result = await _facebookClient.PostASyncReturn_UploadImage<dynamic>(accessToken, pageid + "/videos", new upload_video {description = description, file_url = file_url, title = title }, thumbnail);
                if (result == null) {
                    return new UploadedMedia();
                }

                var video = new UploadedMedia {
                    Id = result.id,
                };

                return video;
            }

            public async Task PostOnWallAsync(string accessToken, string pageid = "me", string message = "", string link = "", List<attached_media> attached_media = null, string PostedVideo = "", string title = "", string thumbnail = "") {
                if (PostedVideo != "") {
                    var postVideo = UploadVideoOnWallAsync(accessToken, pageid, PostedVideo, message, title, thumbnail);
                    try {
                        Task.WaitAll(postVideo);
                    } catch {

                    }

                } else {
                    await _facebookClient.PostAsync(accessToken, pageid = "/feed", new { message, link, attached_media });
                }
            }

            public async Task<UploadedMedia> UploadPhotoOnWallAsync(string accessToken, string pageid = "me", string url = "", bool published = false, bool temporary = true) {
                var result = await _facebookClient.PostAsyncReturn<dynamic>(accessToken, pageid + "/photos", new { url, published, temporary });
                if (result == null) {
                    return new UploadedMedia();
                }

                var photo = new UploadedMedia {
                    Id = result.id,
                };

                return photo;
            }

            public async Task PostOnPageAsync(string accessToken, string pageid, string message)
                => await _facebookClient.PostAsync(accessToken, pageid + "/feed", new { message });

            //public async Task PostOnGroup(string accessToken, string groupid, string message)
            //   => await _facebookClient.PostAsync(accessToken, groupid + "/feed", new { message });
        }
    }
}
