using System;
using System.Web.Mvc;
using TweetSharp;
using Newtonsoft.Json;
namespace MvcSample.Controllers
{
    public class TwitterController :
        Controller
    {
        public ActionResult Authorize()
        {
            // Step 1 - Retrieve an OAuth Request Token
            TwitterService service = new TwitterService("GXm5sizvtWrkZzGoC0SqKx5fz", "4m4IVNU5ImhtUDgp7lNtdIpAeWdealYD9HvfbiMnNdoneJu5Jm");

            // This is the registered callback URL
            OAuthRequestToken requestToken = service.GetRequestToken("http://aggregator.local:8081/Twitter/Callback");

            // Step 2 - Redirect to the OAuth Authorization URL
            Uri uri = service.GetAuthorizationUri(requestToken);
            return new RedirectResult(uri.ToString(), false /*permanent*/);
        }

        public ActionResult Callback(string oauth_token, string oauth_verifier)
        {
            var requestToken = new OAuthRequestToken { Token = oauth_token };

            // Step 3 - Exchange the Request Token for an Access Token
            TwitterService service = new TwitterService("GXm5sizvtWrkZzGoC0SqKx5fz", "4m4IVNU5ImhtUDgp7lNtdIpAeWdealYD9HvfbiMnNdoneJu5Jm");
            OAuthAccessToken accessToken = service.GetAccessToken(requestToken, oauth_verifier);

            // Step 4 - User authenticates using the Access Token
            service.AuthenticateWith(accessToken.Token, accessToken.TokenSecret);
            TwitterUser user = service.VerifyCredentials(new VerifyCredentialsOptions());
            Session["oauth_token"] = oauth_token;
            Session["access_token"] = accessToken.Token;
            Session["access_secret"] = accessToken.TokenSecret;
            return Content(string.Format("Your username is {0}", user.ScreenName));
        }

        public ActionResult TimeLine(string id)
        {
            TwitterService service = new TwitterService("GXm5sizvtWrkZzGoC0SqKx5fz", "4m4IVNU5ImhtUDgp7lNtdIpAeWdealYD9HvfbiMnNdoneJu5Jm");
            service.AuthenticateWith(Session["access_token"].ToString(), Session["access_secret"].ToString());
            var result = service.ListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions()
            {
                Count = 100,
            });
            string s = "";
            foreach (var r in result)
            {
                s += "<br>" +
                    "<img src=\"" + r.Author.ProfileImageUrl + "\" width=\"50\" height=\"50\">"
                    + r.Author.ScreenName + ":   " + r.TextAsHtml + "</br>";
                foreach (var m in r.Entities.Media)
                {
                    s += "<img src=\"" + m.MediaUrl + "\" >";
                }
            }

            return Content(s);//Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}