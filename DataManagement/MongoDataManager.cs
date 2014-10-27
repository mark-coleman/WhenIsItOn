using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using DataManagement.Interfaces;
using System.Configuration;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Linq;
using Common.Extensions;

namespace DataManagement
{
    public class MongoDataManager : IDataManager
    {
        #region IDataManager Implementation

        public void Reset()
        {
            var progs = GetProgrammeCollection();
            progs.RemoveAll();
        }

        public void SaveListings(Entities.Listings listings)
        {
            Parallel.ForEach(listings.Channels, channel =>
                {
                    Parallel.ForEach(channel.Programmes, programme =>
                    {
                        SaveProgramme(programme, channel);
                    });
                });
        }

        public IList<Entities.ListingSearchResult> FindListing(string listingName)
        {
            listingName = listingName.ToLower();

            var progs = GetProgrammeCollection();
            
            return progs.AsQueryable().Where(p => p.SearchTitle.StartsWith(listingName))
                .OrderBy(p => p.Date)
                .ThenBy(p => p.StartTime)
                .Select(p => new Entities.ListingSearchResult() 
                            { 
                                Title = p.Title,
                                Date = p.Date,
                                StartTime = p.StartTime,
                                ChannelName = p.Channel.Name,
                                DisplayStartDateTime = string.Format("{0} {1}", p.Date.ToDayAndDate(), p.StartTime)  
                            }).ToList<Entities.ListingSearchResult>();
        }

        public void SaveUserSearch(string listingName, string emailAddress)
        {
            var searches = GetUserSearchCollection();
            searches.Insert(new UserSearch(listingName, emailAddress));
        }

        public IList<string> GetTitles()
        {
            var progs = GetProgrammeCollection();
            var distinct = progs.AsQueryable().Select(p => p.Title).Distinct().ToList();
            return distinct.OrderBy(s => s).ToList();
        }

        public IList<Entities.UserSearch> GetUserSearches()
        {
            var searches = GetUserSearchCollection();
            return searches.FindAll().Select(u => new Entities.UserSearch()
                                            {
                                                ProgrammeName = u.ProgrammeName,
                                                Email = u.Email
                                            }).ToList<Entities.UserSearch>();
        }

        #endregion

        #region Private Functions

        private void SaveProgramme(Entities.Programme programme, Entities.Channel channel)
        {
            var progs = GetProgrammeCollection();
            progs.Insert(new Programme(programme, channel));
        }

        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["WhenIsItOn"].ConnectionString;
        }

        private static MongoCollection<Programme> GetProgrammeCollection()
        {
            return GetCollection<Programme>("programme");
        }

        private static MongoCollection<UserSearch> GetUserSearchCollection()
        {
            return GetCollection<UserSearch>("usersearch");
        }

        private static MongoCollection<T> GetCollection<T>(string collectionName)
        {
            var db = GetDatabase("WhenIsItOn");
            return db.GetCollection<T>(collectionName);
        }

        private static MongoDatabase GetDatabase(string databaseName)
        {
            string connString = GetConnectionString();
            var client = new MongoClient(connString);
            var server = client.GetServer();
            return server.GetDatabase(databaseName);
        }

        #endregion

        #region Private Classes

        private class Channel : Entities.Channel
        {
            public Channel(Entities.Channel channel)
            {
                this.Name = channel.Name;
                this.Number = channel.Number;
                this.Programmes = channel.Programmes;
            }
        }

        private class Programme : Entities.Programme
        {
            public Programme(Entities.Programme programme, Entities.Channel channel)
            {
                channel.Programmes = null;
                this.Channel = channel;
                this.Date = programme.Date;
                this.Duration = programme.Duration;
                this.EndTime = programme.EndTime;
                this.Episode = programme.Episode;
                this.Genre = programme.Genre;
                this.IsFilm = programme.IsFilm;
                this.StartTime = programme.StartTime;
                this.Title = programme.Title;
                this.Year = programme.Year;
                this.SearchTitle = programme.Title.ToLower();
            }

            [BsonId]
            public ObjectId Id { get; set; }
            public string SearchTitle { get; set; }
            public Entities.Channel Channel { get; set; }
        }

        private class UserSearch : Entities.UserSearch
        {
            public UserSearch(string programmeName, string email)
            {
                this.ProgrammeName = programmeName;
                this.Email = email;
            }

            [BsonId]
            public ObjectId Id { get; set; }
        }

        #endregion
    }
}
