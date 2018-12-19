using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace InfoCannon {
    public class Account {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Locale { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
    }

    public class UploadedPhoto {
        public string Id { get; set; }
    }

    //public class UploadedVideo
    //{
    //    public string Id 
    //}

    public class attached_media {
        public string media_fbid { get; set; }
    }

    public interface IFacebookClient {
        Task<T> GetAsync<T>(string accessToken, string endpoint, string args = null);
        Task<T> PostAsyncReturn<T>(string accessToken, string endpoint, object data, string args = null);
        Task PostAsync(string accessToken, string endpoint, object data, string args = null);
    }

    public class FacebookClient : IFacebookClient {
        private readonly HttpClient _httpClient;

        public FacebookClient() {
            _httpClient = new HttpClient {
                BaseAddress = new Uri("https://graph.facebook.com/v2.12/")
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

        private static StringContent GetPayload(object data) {
            var json = JsonConvert.SerializeObject(data);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public interface IFacebookService {
            Task<Account> GetAccountAsync(string accessToken);
            Task<UploadedPhoto> UploadPhotoOnWallAsync(string accessToken, string pageid = "me", string url = "", bool published = false, bool temporary = true);
            Task<UploadedPhoto> UploadVideoOnWallAsync(string accessToken, string pageid = "me", string file_url = "", string description = "");
            Task PostOnWallAsync(string accessToken, string pageid = "me", string message = "", string link = "", List<attached_media> attached_media = null, string PostedVideo = "");
        }

        public class FacebookService : IFacebookService {
            private readonly IFacebookClient _facebookClient;

            public FacebookService(IFacebookClient facebookClient) {
                _facebookClient = facebookClient;
            }

            public async Task PostOnWallAsync(string accessToken, string pageid = "me", string message = "", string link = "", List<attached_media> attached_media = null, string PostedVideo = "") {
                if (PostedVideo != "") {
                    var postVideo = UploadVideoOnWallAsync(accessToken, pageid, PostedVideo, message);
                    Task.WaitAll(postVideo);
                } else {
                    await _facebookClient.PostAsync(accessToken, pageid = "/feed", new { message, link, attached_media });
                }
            }

            public async Task<UploadedPhoto> UploadPhotoOnWallAsync(string accessToken, string pageid = "me", string url = "", bool published = false, bool temporary = true) {
                var result = await _facebookClient.PostAsyncReturn<dynamic>(accessToken, pageid + "/photos", new { url, published, temporary });

                if (result == null) {
                    return new UploadedPhoto();
                }

                var photo = new UploadedPhoto {
                    Id = result.id,
                };

                return photo;
            }

            public async Task<UploadedPhoto> UploadVideoOnWallAsync(string accessToken, string pageid = "me", string file_url = "", string description = "") {
                var result = await _facebookClient.PostAsyncReturn<dynamic>(accessToken, pageid + "/videos", new { file_url, description });
                if (result == null) {
                    return new UploadedPhoto();
                }

                var photo = new UploadedPhoto {
                    Id = result.id,
                };

                return photo;
            }

            public async Task PostOnPageAsync(string accessToken, string pageid, string message)
                => await _facebookClient.PostAsync(accessToken, pageid + "/feed", new { message });

            //public async Task PostOnGroup(string accessToken, string groupid, string message)
            //   => await _facebookClient.PostAsync(accessToken, groupid + "/feed", new { message });

            public async Task<Account> GetAccountAsync(string accessToken) {
                var result = await _facebookClient.GetAsync<dynamic>(accessToken, "me", "fields=id,name,email,first_name,last_name,age_range,birthday,gender,locale");

                if (result == null) {
                    return new Account();
                }

                var account = new Account {
                    Id = result.id,
                    Email = result.email,
                    Name = result.name,
                    UserName = result.username,
                    FirstName = result.first_name,
                    LastName = result.last_name,
                    Locale = result.locale
                };

                return account;
            }
        }
    }
}
