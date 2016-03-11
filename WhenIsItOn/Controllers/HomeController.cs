using DataManagement;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace WhenIsItOn.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
                return View("Index");

            ViewBag.SearchText = searchText;

            var listingSearchResults = new MongoDataManager().FindListing(searchText) as List<ListingSearchResult>;
            
            if (listingSearchResults == null || listingSearchResults.Count == 0)
            {
                return View("UserSearch");
            }

            return View(listingSearchResults);
        }

        [HttpPost]
        public ActionResult UserSearch(string title, string email)
        {
            ViewBag.SearchText = title;
            if (!Validate(title, email))
                return View();

            new MongoDataManager().SaveUserSearch(title, email);

            return View("Index");
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        private bool Validate(string title, string email)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(email))
                return false;

            if (!IsValidEmail(email))
                return false;

            return true;
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            return regex.IsMatch(email);
        }
    }
}