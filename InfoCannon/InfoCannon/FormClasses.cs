using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using static InfoCannon.Form1;

namespace InfoCannon {

    public class Item {
        public Item() { }
        public string Value { set; get; }
        public string Text { set; get; }
    }

    public class UserSettings : AppSettings<UserSettings> {
        public string accessCode = "";
        public List<string> pageId = new List<string>();
        public List<PostedVideo> postedVideos = new List<PostedVideo>();
    }

    public class AppSettings<T> where T : new() {
        private const string DEFAULT_FILENAME = "settings.json";

        public void Save(string fileName = DEFAULT_FILENAME) {
            File.WriteAllText(fileName, (new JavaScriptSerializer()).Serialize(this));
        }

        public static void Save(T pSettings, string fileName = DEFAULT_FILENAME) {
            File.WriteAllText(fileName, (new JavaScriptSerializer()).Serialize(pSettings));
        }

        public static T Load(string fileName = DEFAULT_FILENAME) {
            T t = new T();
            if (File.Exists(fileName))
                t = (new JavaScriptSerializer()).Deserialize<T>(File.ReadAllText(fileName));
            return t;
        }
    }
}
