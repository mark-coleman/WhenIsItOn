using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ListingsWebJob;
using UserSearchWebJob;

namespace WhenIsItOn.Tests
{
    [TestClass]
    public class UserSearchManagerTests
    {
        [TestMethod]
        public void RunUserSearches_NotifiesUsers()
        {
            new UserSearchManager().RunUserSearches();
        }
    }
}
