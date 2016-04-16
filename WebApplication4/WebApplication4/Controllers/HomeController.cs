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
        
        static void buildTable(ref string keyword, ref List<int> table)
        {
            int i = 2;
            int j = 0;
            table.Add(-1);
            table.Add(0);

            while (i < keyword.Length)
            {
                if (keyword[i-1] == keyword[j])
                {
                    table.Insert(i, j + 1);
                    i++;
                    j++;
                }
                else if (j > 0)
                {
                    j = table[j];
                }
                else
                {
                    table.Insert(i, 0);
                    i++;
                }
            }
        }

        static int KMP(string text, string keyword)
        // mengembalikan indeks posisi ditemukan, mengembalikan panjang text jika keyword tidak ditemukan 
        {
            int i = 0; // indeks untuk teks
            int j = 0; // indeks untuk keyword
            List<int> table = new List<int>(keyword.Length);

            buildTable(ref keyword, ref table);
            while (i + j < text.Length)
            {
                if (keyword[j] == text[i + j])
                {
                    j++;
                    if (j == keyword.Length)
                        return i;
                }
                else
                {
                    i += j - table[j];
                    if (j > 0)
                        j = table[j];
                }
            }
            return text.Length;
        }
        
        
        static void compute_last(ref string keyword, ref Dictionary<int, int> lastPosition)
        {
            for (int i = 0; i < keyword.Length; i++)
            {
                if (lastPosition.ContainsKey(keyword[i]))
                {
                    lastPosition[keyword[i]] = i + 1;
                }
                else
                {
                    lastPosition.Add(keyword[i], i + 1);
                }
            }
        }

        static int BM(string text, string keyword)
        // mengembalikan indeks posisi ditemukan, mengembalikan panjang text jika keyword tidak ditemukan 
        {
            Dictionary<int, int> lastPosition = new Dictionary<int, int>();
            compute_last(ref keyword, ref lastPosition);

            int textLength = text.Length;
            int keywordLength = keyword.Length;
            int i = 0;

            while (i <= textLength - keywordLength)
            {
                int j = keywordLength - 1;

                // mundurkan indeks j selama karakter sesuai
                while ((j >= 0) && keyword[j] == text[i + j])
                    j--;

                // keyword cocok
                if (j == -1)
                    return i;
                else // keyword tidak cocok
                {
                    int mismatch = (int)text[i + j];
                    if (lastPosition.ContainsKey(mismatch))
                    {
                        if (j < lastPosition[mismatch])
                            i++;
                        else
                            i = i + j - lastPosition[mismatch] + 1;
                    }
                    else
                    {
                        i = i + j + 1;
                    }
                }
            }
            return text.Length;
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
                    posisi = KMP(text, keyWord);
                    //posisi = BM(text, keyWord);
                    min = posisi;
                    minIdx = 1;


                    System.Diagnostics.Debug.WriteLine("tespos1 " + posisi);

                    keyWord = key2;
                    posisi = KMP(text, keyWord);
                    //posisi = BM(text, keyWord);
                    if (posisi < min)
                    {
                        min = posisi;
                        minIdx = 2;
                    }
                    System.Diagnostics.Debug.WriteLine("tespos2 " + posisi);


                    keyWord = key3;
                    posisi = KMP(text, keyWord);
                    //posisi = BM(text, keyWord);
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