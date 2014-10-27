﻿using Common;
using DataManagement;
using DataManagement.Interfaces;
using Entities;
using ListingManagement;
using ListingManagement.Interfaces;
using System;
using System.Collections.Generic;

namespace ListingsWebJob
{
    public class ListingManager
    {
        private IListingRetriever listingRetriever;
        private IDataManager dataStore;

        public ListingManager()
        {
            listingRetriever = new XmlTvListingRetriever();
            dataStore = new MongoDataManager();
            //dataStore = new SqlDataManager();
        }

        public ListingManager(IListingRetriever retriever, IDataManager dataManager)
        {
            listingRetriever = retriever;
            dataStore = dataManager;
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

        private static void SendSuccessNotification()
        {
            try
            {
                SendEmail("Listing data has been successfully updated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void SendErrorNotification(Exception error)
        {
            try
            {
                SendEmail(string.Format("Listing data update failed: {0}.", error.ToString()));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void SendEmail(string text)
        {
            new EmailNotifier().SendNotification("WhenIsItOn Listing Update Notification",
                                                 text,
                                                 new List<string>() { "markcoleman157@gmail.com" });
        }
    }
}
