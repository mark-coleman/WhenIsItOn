using Common;
using DataManagement;
using DataManagement.Interfaces;
using Entities;
using ListingManagement;
using ListingManagement.Interfaces;
using System;
using System.Collections.Generic;

namespace UpdateListingsWebJob
{
    public class ListingManager
    {
        private IListingRetriever listingRetriever;
        private IDataManager dataStore;
        private INotifier notificationManager;

        public ListingManager()
        {
            listingRetriever = new XmlTvListingRetriever();
            dataStore = new MongoDataManager();
            //dataStore = new SqlDataManager();
            notificationManager = new EmailNotifier(); 
        }

        public ListingManager(IListingRetriever retriever, IDataManager dataManager, INotifier notifier)
        {
            if (retriever == null)
                throw new ArgumentNullException("retriever");
            if (dataManager == null)
                throw new ArgumentNullException("dataManager");
            if (notifier == null)
                throw new ArgumentNullException("notifier");

            listingRetriever = retriever;
            dataStore = dataManager;
            notificationManager = notifier;
        }

        public void UpdateListings()
        {
            try
            {
                using (new Tracer("UpdateListings"))
                {
                    dataStore.Reset();
                    Tracer.WriteLine("Reset data store");

                    Listings listings = listingRetriever.GetListings();
                    Tracer.WriteLine(string.Format("Retrieved listings"));

                    Tracer.WriteLine(string.Format("Saving listings for {0} channels...", listings.Channels.Count));
                    dataStore.SaveListings(listings);
                    Tracer.WriteLine(string.Format("Saved listings for {0} channels", listings.Channels.Count));

                    SendSuccessNotification();
                    Tracer.WriteLine("Sent success notification");
                }
            }
            catch (Exception ex)
            {
                SendErrorNotification(ex);
                throw;
            }
        }

        private void SendSuccessNotification()
        {
            try
            {
                SendNotification("Listing data has been successfully updated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void SendErrorNotification(Exception error)
        {
            try
            {
                SendNotification(string.Format("Listing data update failed: {0}.", error.ToString()));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void SendNotification(string text)
        {
            notificationManager.SendNotification("WhenIsItOn Listing Update Notification",
                                                 text,
                                                 new List<string>() { "markcoleman157@gmail.com" });
        }
    }
}
