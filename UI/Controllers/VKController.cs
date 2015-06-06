using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using UI.Models;
namespace UI.Controllers
{
    public class VKController : Controller
    {
        //
        // GET: /VK/
        private enum VkontakteScopeList
        {

            /// <summary>

            /// Пользователь разрешил отправлять ему уведомления.

            /// </summary>

            notify = 1,

            /// <summary>

            /// Доступ к друзьям.

            /// </summary>

            friends = 2,

            /// <summary>

            /// Доступ к фотографиям.

            /// </summary>

            photos = 4,

            /// <summary>

            /// Доступ к аудиозаписям.

            /// </summary>

            audio = 8,

            /// <summary>

            /// Доступ к видеозаписям.

            /// </summary>

            video = 16,

            /// <summary>

            /// Доступ к предложениям (устаревшие методы).

            /// </summary>

            offers = 32,

            /// <summary>

            /// Доступ к вопросам (устаревшие методы).

            /// </summary>

            questions = 64,

            /// <summary>

            /// Доступ к wiki-страницам.

            /// </summary>

            pages = 128,

            /// <summary>

            /// Добавление ссылки на приложение в меню слева.

            /// </summary>

            link = 256,

            /// <summary>

            /// Доступ заметкам пользователя.

            /// </summary>

            notes = 2048,

            /// <summary>

            /// (для Standalone-приложений) Доступ к расширенным методам работы с сообщениями.

            /// </summary>

            messages = 4096,

            /// <summary>

            /// Доступ к обычным и расширенным методам работы со стеной.

            /// </summary>

            wall = 8192,

            /// <summary>

            /// Доступ к документам пользователя.

            /// </summary>

            docs = 131072

        }
        public ActionResult Authorize()
        {
            int scope = (int)(VkontakteScopeList.audio | VkontakteScopeList.docs | VkontakteScopeList.friends | VkontakteScopeList.notes | VkontakteScopeList.notify | VkontakteScopeList.offers | VkontakteScopeList.pages | VkontakteScopeList.photos | VkontakteScopeList.questions | VkontakteScopeList.video | VkontakteScopeList.wall);

            return Redirect(String.Format("https://oauth.vk.com/authorize?client_id=4941969&scope={0}&redirect_uri=http://aggregator.local:8081/VK/Callback&response_type=code&v=5.33&display=page", scope));
        }
        public class VKServerResponse
        {
            public string access_token;
            private object expires_in;
            public string user_id;
        }

        public ActionResult Callback(string code)
        {
            WebRequest req = HttpWebRequest.Create(String.Format("https://oauth.vk.com/access_token?client_id=4941969&client_secret=9XgVFpm228Vw6JhtcUhP&code={0}&redirect_uri=http://aggregator.local:8081/VK/Callback", code));
            var response = req.GetResponse();
            string data;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                data = sr.ReadToEnd();
            }
            VKServerResponse result = JsonConvert.DeserializeObject<VKServerResponse>(data);

            Session["vk_access_token"] = result.access_token;

            return Redirect("/VK/Wall_information?id=" + result.user_id);
        }

        public class FriendsIdList
        {
            public List<Friend> response { get; set; }
        }

        public class Friend
        {
            public string uid;
            public string first_name;
            public string last_name;
            public string photo_50;
            public bool online;
            public string user_id;
        }

        public class VK_wall_response
        {
            public List<Record> response { get; set; }
        }
        public class Record
        {
            public string id { get; set; }
            public string from_id { get; set; }
            public string to_id { get; set; }
            public string date { get; set; }
            public string post_type { get; set; }
            public string text { get; set; }
            public Attachment attachment { get; set; }
            /* private object attachments;
             private List<Count> comments { get; set; }
             private List<Count> likes { get; set; }
             private List<Count> reposts { get; set; }*/
        }
        public class Attachment
        {
            public Photo photo;
        }
        public class Photo
        {
            public string src_big { get; set; }
        }
        public class Count
        {
            public int count;
        }
        public ActionResult Wall_information(string id)
        {
            //get all friends
            
            WebRequest req = HttpWebRequest.Create(String.Format("https://api.vk.com/method/friends.get?user_id={0}&fields=photo_50&access_token={1}", id, Session["vk_access_token"]));

            var response = req.GetResponse();
            string data;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                data = sr.ReadToEnd();
            }
            var friendsIdList = (FriendsIdList)JsonConvert.DeserializeObject(data, typeof(FriendsIdList));
            
            //get friends' wall

            VKUserModel model = new VKUserModel();
            model.Name = "Test";
            
            foreach (var friend in friendsIdList.response)
            {
                req = HttpWebRequest.Create(String.Format("https://api.vk.com/method/wall.get?owner_id={0}&filter=owner&order=hints&count=10&access_token={1}", friend.user_id, Session["vk_access_token"]));

                response = req.GetResponse();
                data = "";
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    data = sr.ReadToEnd();
                }
                int begin = data.IndexOf(',');
                data = data.Substring(begin + 1, data.Length - 3 - begin);
                //response==list of records
                data = "{response:[" + data + "]}";
                try
                {
                    VK_wall_response result = JsonConvert.DeserializeObject<VK_wall_response>(data);

                    foreach (var r in result.response)
                    {
                        Tweet tweet = new Tweet();
                        tweet.AuthorScreenName = friend.first_name+" "+friend.last_name;
                        tweet.AuthorProfileImageUrl = friend.photo_50;
                        tweet.dateUnix = r.date;
                        
                        tweet.TextAsHtml = "<br>" + r.text + "</br>";
                        string s = "";
                        if (r.attachment != null)
                            if (r.attachment.photo != null)
                                if (!r.attachment.photo.src_big.Equals(""))
                                    s = r.attachment.photo.src_big;
                        tweet.Image = s;

                        model.feed.Add(tweet.dateUnix,tweet);
                        if (model.feed.Count == 100)
                            break;
                    }

                    System.Threading.Thread.Sleep(50);
                }
                catch
                {

                }
            }

            return View("~/Views/VK/TimeLine.cshtml", model);
        }
        public ActionResult Audio(string id)
        {
            WebRequest req = HttpWebRequest.Create(String.Format("https://api.vk.com/method/audio.get?owner_id={0}&access_token={1}", id, Session["vk_access_token"]));
            
            var response = req.GetResponse();
            string data;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                data = sr.ReadToEnd();
            }
            return Content(data);
        }

    }
}