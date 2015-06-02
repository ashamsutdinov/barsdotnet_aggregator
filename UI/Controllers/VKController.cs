using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using Newtonsoft.Json;
using System.IO;

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
            public int expires_in;
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
            public string photo_100;
            public bool online;
            public string user_id;
        }

        public ActionResult Wall_information(string id)
        {
            //get all friends
            WebRequest req = HttpWebRequest.Create(String.Format("https://api.vk.com/method/friends.get?user_id={0}&fields=photo_100&access_token={1}", id, Session["vk_access_token"]));

            var response = req.GetResponse();
            string data;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                data = sr.ReadToEnd();
            }
            var friendsIdList = (FriendsIdList)JsonConvert.DeserializeObject(data, typeof(FriendsIdList));
            List<String> friendsId = new List<string>();
            int i = 0;

            //get friends' wall
            data = "";
            foreach (var r in friendsIdList.response)
            {
                //        friendsId.Add(r.user_id);
                //   }

                req = HttpWebRequest.Create(String.Format("https://api.vk.com/method/wall.get?owner_id={0}&count=10&access_token={1}", r.user_id, Session["vk_access_token"]));

                response = req.GetResponse();

                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    data += sr.ReadToEnd();
                }

            }
            return Content(data);
        }
        public ActionResult Audio(string id)
        {
            WebRequest req = HttpWebRequest.Create(String.Format("https://api.vk.com/method/audio.get?owner_id={0}&access_token={1}", id, Session["vk_access_token"]));
            // WebRequest req = HttpWebRequest.Create(String.Format("https://api.vk.com/method/wall.get?access_token={0}", VK_access_token));

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