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
        /*
        static void buildTable(ref string w, ref List<int> t)
        {
            int i = 2;
            int j = 0;
            int panjang = t.Capacity;
            t.Add(-1);
            t.Add(0);

            while (i < w.Length)
            {
                if (w[i - 1] == w[j])
                {
                    t.Insert(i, j + 1);
                    i++;
                    j++;
                }
                else if (j > 0)
                {
                    j = t[j];
                }
                else
                {
                    t.Insert(i, 0);
                    i++;
                }
            }
        }

        static int KMP(ref string s, ref string w)
        {
            int m = 0;
            int i = 0;
            List<int> t = new List<int>(w.Length);

            buildTable(ref w, ref t);
            while (m + i < s.Length)
            {
                if (w[i] == s[m + i])
                {
                    i++;
                    if (i == w.Length)
                        return m;
                }
                else
                {
                    m += i - t[i];
                    if (i > 0)
                        i = t[i];
                }
            }
            return s.Length;
        }
        */
        static void compute_last(ref string w, ref List<int> t)
        {
            for (int i = 0; i < 128; i++)
                t.Add(0);
            for (int i = 0; i < w.Length; i++)
                t[w[i]] = i + 1;
        }
        static int BM(ref string s, ref string w)
        {
            List<int> t = new List<int>(128);
            compute_last(ref w, ref t);

            int n = s.Length;
            int m = w.Length;
            int i = 0;
            while (i <= n - m)
            {
                int j = m - 1;
                while ((j >= 0) && w[j] == s[i + j])
                    j--;
                if (j == -1)
                    return i;
                else
                {
                    int mismatch = (int)s[i + j];
                    System.Diagnostics.Debug.WriteLine("tesmis " + mismatch);
                    if (j < t[mismatch])
                        i++;
                    else
                        i = i + j - t[mismatch] + 1;
                }
            }
            return s.Length;
        }
        
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
        
        [HttpPost]
        public ActionResult Form(string txtTwitterName, string txtboxPDAM, string txtboxPJU, string txtboxDinsos)
        {
            TempData["QueryPDAM"] = txtboxPDAM;
            TempData["QueryPJU"] = txtboxPJU;
            TempData["QueryDinsos"] = txtboxDinsos;
            TempData["Tweetsearch"] = txtTwitterName;

            return RedirectToAction("Result");
        }
        
        public ActionResult Result()
        {
            string consumerKey = "DShcBlKUwZ3mUBRt3tjqJEDnd";
            string consumerSecret = "GxJYqCJTcJyteeqSDg0sXC3xRxe4ydbbSljbSxfUvv7Z9TiROq";
            string accessToken = "2535482743-roYx8Hbx0qqdluKQFeIQlVOKvmp5IVUXaO5z9b3";
            string accessTokenSecret = "OGuX0xXjB9BiEw1K3Htgpm2LbeRUsh2EqBRf3k7TXi4VT";

            string tweetSearchQuery = (string)TempData["Tweetsearch"];
            string key1 = (string)TempData["QueryPDAM"];
            string key2 = (string)TempData["QueryPJU"];
            string key3 = (string)TempData["QueryDinsos"];
            if (TempData["Tweetsearch"] != null)
            {
                var service = new TwitterService(consumerKey, consumerSecret);
                service.AuthenticateWith(accessToken, accessTokenSecret);

                // IEnumerable<TwitterStatus> tweets = service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions { ScreenName = tweetSearchQuery, Count = 100, });

                TwitterSearchResult hasil = service.Search(new SearchOptions { Q = tweetSearchQuery, Count = 10 });
                IEnumerable<TwitterStatus> tweets = hasil.Statuses;
           //     TwitterStatus tweetStat = tweets.ElementAt(1);

                ViewBag.Tweets = tweets;
                
                List<TwitterStatus> tweets1 = new List<TwitterStatus>();
                List<TwitterStatus> tweets2 = new List<TwitterStatus>();
                List<TwitterStatus> tweets3 = new List<TwitterStatus>();
                int posisi = 0;
                int min, minIdx;
                string text;
                string keyWord;
                List<TwitterStatus> ltweet = tweets.ToList();
                foreach (var tweet in ltweet)
                {
                    text = tweet.Text;
                    keyWord = key1;
                    System.Diagnostics.Debug.WriteLine("tes " + text);
                    System.Diagnostics.Debug.WriteLine("tes " + keyWord);
                    //posisi = KMP(ref text, ref keyWord);
                    posisi = BM(ref text, ref keyWord);
                    min = posisi;
                    minIdx = 1;


                    System.Diagnostics.Debug.WriteLine("tespos1 " + posisi);

                    keyWord = key2;
                    //posisi = KMP(ref text, ref keyWord);
                    posisi = BM(ref text, ref keyWord);
                    if (posisi < min)
                    {
                        min = posisi;
                        minIdx = 2;
                    }
                    System.Diagnostics.Debug.WriteLine("tespos2 " + posisi);


                    keyWord = key3;
                    //posisi = KMP(ref text, ref keyWord);
                    posisi = BM(ref text, ref keyWord);
                    if (posisi < min)
                    {
                        min = posisi;
                        minIdx = 3;
                    }
                    System.Diagnostics.Debug.WriteLine("tespos3 " + posisi);

                    if (minIdx == 1)
                    {
                        tweets1.Add(tweet);
                        //TwitterStatus tweetStatusss = tweet;
                        //tweets1 = hasil.Statuses;
                        //TwitterStatus tweetStat1 = tweets.ElementAt(1);
                    }
                    else if (minIdx == 2)
                    {
                        tweets2.Add(tweet);
                        //tweets2 = hasil.Statuses;
                        //TwitterStatus tweetStat2 = tweets2.ElementAt(1);
                    }
                    else if (minIdx == 3)
                    {
                        tweets3.Add(tweet);
                        //tweets3 = hasil.Statuses;
                        //TwitterStatus tweetStat3 = tweets3.ElementAt(1);
                    }
                }

                ViewBag.Tweets1 = tweets1;
                ViewBag.Tweets2 = tweets2;
                ViewBag.Tweets3 = tweets3;

            }

            ViewBag.PDAM = (string)TempData["QueryPDAM"];
            ViewBag.PJU = (string)TempData["QueryPJU"];
            ViewBag.Dinsos = (string)TempData["QueryDinsos"];

            return View();
        }
    }
}