using DataManagement.Interfaces;
using Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataManagement
{
    public class SqlDataManager : IDataManager
    {
        #region IDataManager Implementation

        public void Reset()
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

        public void SaveListings(Listings listings)
        {
            foreach (var channel in listings.Channels)
            {
                SaveChannel(channel);
                Parallel.ForEach(channel.Programmes, programme =>
                    {
                        SaveProgramme(programme, channel.Number);
                    });
            }
        }

        public IList<ListingSearchResult> FindListing(string listingName)
        {
            throw new NotImplementedException();
        }

        public void SaveUserSearch(string listingName, string emailAddress)
        {
            throw new NotImplementedException();
        }

        public IList<UserSearch> GetUserSearches()
        {
            throw new NotImplementedException();
        }

        public IList<string> GetTitles()
        {
            throw new NotImplementedException();
        }

        public void DeleteUserSearch(UserSearch userSearch)
        {
            throw new NotImplementedException();
        }

        #endregion

        private static void SaveChannel(Channel channel)
        {
            const string SQL = "INSERT INTO Channel (Id, Name) VALUES (@Id, @Name)";
            string connString = GetConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL, conn);
                cmd.Parameters.Add("@Id", channel.Number);
                cmd.Parameters.Add("@Name", channel.Name);

                cmd.ExecuteNonQuery();
            }
        }

        private static void SaveProgramme(Programme programme, int channelId)
        {
            const string SQL = "INSERT INTO Programme (Title, Episode, Year, Film, Genre, StartTime, EndTime, Duration, Date, ChannelId) VALUES (@Title, @Episode, @Year, @Film, @Genre, @StartTime, @EndTime, @Duration, @Date, @ChannelId)";
            string connString = GetConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(SQL, conn);
                cmd.Parameters.Add("@Title", programme.Title);
                cmd.Parameters.Add("@Episode", programme.Episode);
                cmd.Parameters.Add("@Year", programme.Year);
                cmd.Parameters.Add("@Film", programme.IsFilm);
                cmd.Parameters.Add("@Genre", programme.Genre);
                cmd.Parameters.Add("@StartTime", programme.StartTime);
                cmd.Parameters.Add("@EndTime", programme.EndTime);
                cmd.Parameters.Add("@Duration", programme.Duration);
                cmd.Parameters.Add("@Date", programme.Date);
                cmd.Parameters.Add("@ChannelId", channelId);

                cmd.ExecuteNonQuery();
            }
        }

        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["WhenIsItOn"].ConnectionString;
        }

    }
}
