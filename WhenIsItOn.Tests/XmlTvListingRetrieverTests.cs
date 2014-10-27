using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entities;
using ListingManagement;

namespace WhenIsItOn.Tests
{
    [TestClass]
    public class XmlTvListingRetrieverTests
    {
        [TestMethod]
        public void GetChannels_ReturnsListOfChannels()
        {
            List<Channel> channels = new XmlTvListingRetriever().GetChannels();
            Assert.IsTrue(channels.Count > 0);
        }

        [TestMethod]
        public void GetProgrammes_ReturnsListOfListings()
        {
            List<Programme> listings = new XmlTvListingRetriever().GetProgrammes(92);
            Assert.IsTrue(listings.Count > 0);
        }
    }
}
