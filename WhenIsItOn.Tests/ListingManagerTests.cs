using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ListingsWebJob;

namespace WhenIsItOn.Tests
{
    [TestClass]
    public class ListingManagerTests
    {
        [TestMethod]
        public void UpdateListings_SavesListings()
        {
            new ListingManager().UpdateListings();
        }
    }
}
