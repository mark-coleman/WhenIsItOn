using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataManagement;
using Entities;
using DataManagement.Interfaces;
using System.Configuration;
using System.Data.SqlClient;

namespace WhenIsItOn.Tests
{
    [TestClass]
    public class MongoDataManagerTests
    {
        [TestInitialize]
        public void TestInit()
        {
            //ResetDB();
        }

        [ClassCleanup]
        public static void TestRunCleanup()
        {
            //ResetDB();
        }

        [TestMethod]
        public void SaveListings_ValidListingList_SavesListings()
        {
            // Arrange
            Listings listings = new Listings()
            {
                Channels = new List<Channel>()
                {
                    new Channel() 
                    { 
                        Number = 1, 
                        Name = "Channel 1", 
                        Programmes = new List<Programme>()
                        {
                            new Programme() { Date = DateTime.Now, Title = "Prog 1", StartTime = DateTime.Now.AddMinutes(1).ToShortTimeString(), EndTime = DateTime.Now.AddMinutes(61).ToShortTimeString(), Duration = 60, Episode = "1 of 5", IsFilm = false, Genre = "Action", Year = "2012" },
                            new Programme() { Date = DateTime.Now, Title = "Prog 2", StartTime = DateTime.Now.AddMinutes(1).ToShortTimeString(), EndTime = DateTime.Now.AddMinutes(61).ToShortTimeString(), Duration = 60, Episode = "1 of 5", IsFilm = false, Genre = "Action", Year = "2010" },
                        }
                    },
                    new Channel() 
                    { 
                        Number = 2, 
                        Name = "Channel 2",
                        Programmes = new List<Programme>()
                        {
                            new Programme() { Date = DateTime.Now, Title = "Prog 3", StartTime = DateTime.Now.AddMinutes(1).ToShortTimeString(), EndTime = DateTime.Now.AddMinutes(61).ToShortTimeString(), Duration = 60, Episode = "1 of 5", IsFilm = false, Genre = "Action", Year = "2012" },
                            new Programme() { Date = DateTime.Now, Title = "Prog 4", StartTime = DateTime.Now.AddMinutes(1).ToShortTimeString(), EndTime = DateTime.Now.AddMinutes(61).ToShortTimeString(), Duration = 60, Episode = "1 of 5", IsFilm = false, Genre = "Action", Year = "2010" },
                        }
                    }
                }
            };

            // Act
            new MongoDataManager().SaveListings(listings);

            // Assert
            //Assert.AreEqual(2, GetProgrammeCount());
        }

        [TestMethod]
        public void FindListing_ValidSearch_ReturnsList()
        {
            IList<ListingSearchResult> progs = new MongoDataManager().FindListing("The Blacklist");
            Assert.IsTrue(progs.Count > 0);
        }

        [TestMethod]
        public void GetTitles_ValidSearch_ReturnsOrderedDistinctList()
        {
            IList<string> progs = new MongoDataManager().GetTitles();
            Assert.IsTrue(progs.Count > 0);
        }

        [TestMethod]
        public void SaveUserSearch_ValidSearchDetails_SavesSearch()
        {
            const string PROG_NAME = "The Blacklist";

            new MongoDataManager().SaveUserSearch(PROG_NAME, "mark.coleman@outlook.com");
            IList<UserSearch> searches = new MongoDataManager().GetUserSearches()
                        .Where(u => u.ProgrammeName == PROG_NAME).ToList<UserSearch>();
            Assert.AreEqual(1, searches.Count, string.Format("There is not exactly 1 search for {0}", PROG_NAME));
        }

        private static void ResetDB()
        {
            new MongoDataManager().Reset();
        }
    }
}
