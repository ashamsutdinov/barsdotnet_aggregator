using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models
{
    public class TwitterUserModel
    {
        public string Name { get; set; }

        public class Tweet
        {
            public string AuthorScreenName;
            public string AuthorProfileImageUrl;
            public string TextAsHtml;
            public string Image;            
        }
        public List<Tweet> feed;
    }
}