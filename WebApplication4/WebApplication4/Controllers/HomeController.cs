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
        [HttpGet]
        public ActionResult About()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Result()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Form(string txtTwitterName, string txtboxPDAM, string txtboxPJU, string txtboxDinsos)
        {
            /*
            TempData["QueryPDAM"] = txtboxPDAM;
            TempData["QueryPJU"] = txtboxPJU;
            TempData["QueryDinsos"] = txtboxDinsos;
            TempData["Twittersearch"] = txtTwitterName;

            return RedirectToAction("Result");
            */
            string consumerKey = "DShcBlKUwZ3mUBRt3tjqJEDnd";
            string consumerSecret = "GxJYqCJTcJyteeqSDg0sXC3xRxe4ydbbSljbSxfUvv7Z9TiROq";
            string accessToken = "2535482743-roYx8Hbx0qqdluKQFeIQlVOKvmp5IVUXaO5z9b3";
            string accessTokenSecret = "OGuX0xXjB9BiEw1K3Htgpm2LbeRUsh2EqBRf3k7TXi4VT";

            string tweetSearchQuery = (string)TempData["Tweetsearch"];
            var service = new TwitterService(consumerKey, consumerSecret);
            service.AuthenticateWith(accessToken, accessTokenSecret);

            IEnumerable<TwitterStatus> tweets = service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions { ScreenName = "@PemkotBandung", Count = 5, });

            //TwitterSearchResult hasil = service.Search(new SearchOptions { Q = txtTwitterName, Count = 5 });
            //IEnumerable<TwitterStatus> tweets = hasil.Statuses;
            //TwitterStatus tweetStat = tweets.ElementAt(1);

            ViewBag.PDAM = (string)TempData["QueryPDAM"];
            ViewBag.PJU = (string)TempData["QueryPJU"];
            ViewBag.Dinsos = (string)TempData["QueryDinsos"];
            ViewBag.Tweets = tweets;

            return View();
        }
        [HttpPost]
        public ActionResult Result(string aaa)
        {
            string consumerKey = "DShcBlKUwZ3mUBRt3tjqJEDnd";
            string consumerSecret = "GxJYqCJTcJyteeqSDg0sXC3xRxe4ydbbSljbSxfUvv7Z9TiROq";
            string accessToken = "2535482743-roYx8Hbx0qqdluKQFeIQlVOKvmp5IVUXaO5z9b3";
            string accessTokenSecret = "OGuX0xXjB9BiEw1K3Htgpm2LbeRUsh2EqBRf3k7TXi4VT";

            string tweetSearchQuery = (string)TempData["Tweetsearch"];
            var service = new TwitterService(consumerKey, consumerSecret);
            service.AuthenticateWith(accessToken, accessTokenSecret);

            IEnumerable<TwitterStatus> tweets = service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions { ScreenName = "@MickyYuu96", Count = 5, });

            //TwitterSearchResult hasil = service.Search(new SearchOptions { Q = txtTwitterName, Count = 5 });
            //IEnumerable<TwitterStatus> tweets = hasil.Statuses;
            //TwitterStatus tweetStat = tweets.ElementAt(1);

            ViewBag.PDAM = (string)TempData["QueryPDAM"];
            ViewBag.PJU = (string)TempData["QueryPJU"];
            ViewBag.Dinsos = (string)TempData["QueryDinsos"];
            ViewBag.Tweets = tweets;

            return View();
        }
    }
}