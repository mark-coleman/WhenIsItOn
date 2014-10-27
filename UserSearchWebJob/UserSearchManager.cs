using Common;
using Common.Extensions;
using DataManagement;
using DataManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSearchWebJob
{
    public class UserSearchManager
    {
        private IDataManager dataStore;
        private INotifier notifier;

        public UserSearchManager()
        {
            notifier = new EmailNotifier();
            dataStore = new MongoDataManager();
            //dataStore = new SqlDataManager();
        }

        public UserSearchManager(INotifier notificationManager, IDataManager dataManager)
        {
            notifier = notificationManager;
            dataStore = dataManager;
        }

        /// <summary>
        /// Searches listings for matches to user searches, 
        /// notifying the user if a match is found
        /// </summary>
        public void RunUserSearches()
        {
            var searches = dataStore.GetUserSearches();
            Parallel.ForEach(searches, u => RunUserSearch(u));
        }

        private void RunUserSearch(Entities.UserSearch userSearch)
        {
            var progs = dataStore.FindListing(userSearch.ProgrammeName);
            if (progs.Count > 0)
            {
                NotifyUser(userSearch, progs);
            }
        }

        private void NotifyUser(Entities.UserSearch userSearch, IList<Entities.ListingSearchResult> progs)
        {
            string notificationText = BuildNotificationContent(userSearch, progs);
            SendNotification(notificationText, userSearch.Email);
        }

        private string BuildNotificationContent(Entities.UserSearch userSearch, IList<Entities.ListingSearchResult> progs)
        {
            Entities.ListingSearchResult firstShowing = GetFirstShowingOfProgramme(progs);
 
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Hi,");
            sb.AppendLine();
            sb.AppendLineFormat("When Is It On? has identified that {0} is next on UK television {1} on channel {2}. Enjoy!", 
                                userSearch.ProgrammeName, firstShowing.Date.ToDateOnly(), firstShowing.ChannelName);
            if (progs.Count > 1)
            {
                sb.AppendLine();
                sb.AppendLine("The full list of showings in the next two weeks is:");
                foreach (var prog in progs)
                {
                    sb.AppendLineFormat("{0} at {1} {2} on {3}.", prog.Title, prog.Date.ToDateOnly(), prog.StartTime, prog.ChannelName);
                }
            }

            sb.AppendLine();
            sb.AppendLine();

            return sb.ToString();
        }

        private Entities.ListingSearchResult GetFirstShowingOfProgramme(IList<Entities.ListingSearchResult> progs)
        {
            return progs.First();
        }

        private void SendNotification(string text, string address)
        {
            notifier.SendNotification("WhenIsItOn Listing Update Notification",
                                                 text,
                                                 new List<string>() { address });
        }

    }
}
