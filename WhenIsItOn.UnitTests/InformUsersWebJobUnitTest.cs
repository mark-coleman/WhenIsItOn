using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ListingManagement;
using ListingManagement.Interfaces;
using DataManagement.Interfaces;
using Common;
using System.Collections.Generic;
using InformUsersWebJob;

namespace WhenIsItOn.UnitTests
{
    [TestClass]
    public class InformUsersWebJobUnitTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserSearchManagerCtor_NotifierIsNull_ThrowsArgumentNullException()
        {
            new UserSearchManager(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserSearchManagerCtor_DataManagerIsNull_ThrowsArgumentNullException()
        {
            Mock<INotifier> notifier = new Mock<INotifier>();
            new UserSearchManager(notifier.Object, null);
        }

        [TestMethod]
        public void UserSearchManagerCtor_ValidParameters_CreatesInstance()
        {
            Mock<INotifier> notifier = new Mock<INotifier>();
            Mock<IDataManager> dataStore = new Mock<IDataManager>();
            UserSearchManager usm = new UserSearchManager(notifier.Object, dataStore.Object);

            Assert.IsNotNull(usm);
        }
    }
}
