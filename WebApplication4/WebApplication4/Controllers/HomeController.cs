using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TweetSharp;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Result()
        {
            string consumerKey = "lfwH9auYQcCb3Ba0R752Yw";
            string consumerSecret = "EERQE8Ys3nvGThh9mTQ88w2bUIzoOLnRhvKsbOjhc";
            string accessToken = "50680924-yIdW5ULk98ThsDm0DTQlbRXREawVMcPytQ8WS04GW";
            string accessTokenSecret = "4fJYy6Yttdl314F5WeOTkuR8apkTFXj5ZK2MmvLtk";
            string tweetSearchQuery = (string)TempData["Tweetsearch"];
            var service = new TwitterService(consumerKey, consumerSecret);
            service.AuthenticateWith(accessToken, accessTokenSecret);

            IEnumerable<TwitterStatus> tweets = service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions { ScreenName = tweetSearchQuery, Count = 5 });

            //TwitterSearchResult hasil = service.Search(new SearchOptions{ Q = txtTwitterName, Count = 5 });
            //IEnumerable<TwitterStatus> tweets = hasil.Statuses;
            //TwitterStatus tweetStat = tweets.ElementAt(1);
            ViewBag.PDAM = (string)TempData["QueryPDAM"];
            ViewBag.PJU = (string)TempData["QueryPJU"];
            ViewBag.Dinsos = (string)TempData["QueryDinsos"];
            ViewBag.Tweets = tweets;
            return View();
        }

        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Form(string txtTwitterName, string txtboxPDAM, string txtboxPJU, string txtboxDinsos)
        {
            TempData["QueryPDAM"] = txtboxPDAM;
            TempData["QueryPJU"] = txtboxPJU;
            TempData["QueryDinsos"] = txtboxDinsos;
            TempData["Twittersearch"] = txtTwitterName;

            return RedirectToAction("Result");
        }
    }
}