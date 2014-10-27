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
    public class SqlDataManagerTests
    {
        [TestInitialize]
        public void TestInit()
        {
            ResetDB();
        }

        [ClassCleanup]
        public static void TestRunCleanup()
        {
            ResetDB();
        }

        [TestMethod]
        public void SaveListings_ValidListingList_SavesListings()
        {
            // Arrange
            Listings listings = new Listings();
            listings.Channels = new List<Channel>()
            {
                new Channel() { Number = 1, Name = "Channel 1",
                                Programmes = new List<Programme>()
                                {
                                    new Programme() { Date = DateTime.Now, Title = "Prog 1", StartTime = DateTime.Now.AddMinutes(1).ToShortTimeString(), EndTime = DateTime.Now.AddMinutes(61).ToShortTimeString(), Duration = 60, Episode = "1 of 5", IsFilm = false, Genre = "Action", Year = "2012" },
                                    new Programme() { Date = DateTime.Now, Title = "Prog 2", StartTime = DateTime.Now.AddMinutes(1).ToShortTimeString(), EndTime = DateTime.Now.AddMinutes(61).ToShortTimeString(), Duration = 60, Episode = "1 of 5", IsFilm = false, Genre = "Action", Year = "2010" },
                                }
                              },
                new Channel() { Number = 2, Name = "Channel 2",
                                Programmes = new List<Programme>()
                                {
                                    new Programme() { Date = DateTime.Now, Title = "Prog 3", StartTime = DateTime.Now.AddMinutes(1).ToShortTimeString(), EndTime = DateTime.Now.AddMinutes(61).ToShortTimeString(), Duration = 60, Episode = "1 of 5", IsFilm = false, Genre = "Action", Year = "2012" },
                                    new Programme() { Date = DateTime.Now, Title = "Prog 4", StartTime = DateTime.Now.AddMinutes(1).ToShortTimeString(), EndTime = DateTime.Now.AddMinutes(61).ToShortTimeString(), Duration = 60, Episode = "1 of 5", IsFilm = false, Genre = "Action", Year = "2010" },
                                }
                              }
            };

            // Act
            new SqlDataManager().SaveListings(listings);

            // Assert
            Assert.AreEqual(2, GetProgrammeCount());
        }

        private static void ResetDB()
        {
            const string SQL_CHANNEL = "DELETE FROM Channel";
            const string SQL_PROGRAMME = "DELETE FROM Programme";

            string connString = GetConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL_PROGRAMME, conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand(SQL_CHANNEL, conn);
                cmd.ExecuteNonQuery();
            }
        }

        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["WhenIsItOn"].ConnectionString;
        }

        private static int GetChannelCount()
        {
            return GetRowCountFromTable("Channel");
        }

        private static int GetProgrammeCount()
        {
            return GetRowCountFromTable("Programme");
        }

        private static int GetRowCountFromTable(string table)
        {
            const string SQL = "SELECT COUNT(*) FROM {0}";

            string commandSql = string.Format(SQL, table);
            string connString = GetConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(commandSql, conn);
                return (int)cmd.ExecuteScalar();
            }
        }
    }
}
