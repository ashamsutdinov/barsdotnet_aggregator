﻿using System.Web;
using System.Collections.Generic;
namespace UI.Models
{
    public class VKUserModel
    {
        public string Name { get; set; }//don't know what is it

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