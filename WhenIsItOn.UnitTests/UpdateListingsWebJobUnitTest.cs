using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UpdateListingsWebJob;
using ListingManagement;
using ListingManagement.Interfaces;
using DataManagement.Interfaces;
using Common;
using System.Collections.Generic;

namespace WhenIsItOn.UnitTests
{
    [TestClass]
    public class UpdateListingsWebJobUnitTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ListingManagerCtor_RetrieverIsNull_ThrowsArgumentNullException()
        {
            new ListingManager(null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ListingManagerCtor_DataManagerIsNull_ThrowsArgumentNullException()
        {
            Mock<IListingRetriever> retriever = new Mock<IListingRetriever>();
            retriever.Setup(r => r.GetListings());
            new ListingManager(retriever.Object, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ListingManagerCtor_NotifierIsNull_ThrowsArgumentNullException()
        {
            Mock<IListingRetriever> retriever = new Mock<IListingRetriever>();
            Mock<IDataManager> dataManager = new Mock<IDataManager>();
            new ListingManager(retriever.Object, dataManager.Object, null);
        }

        [TestMethod]
        public void ListingManagerCtor_ValidParams_CreatesInstance()
        {
            Mock<IListingRetriever> retriever = new Mock<IListingRetriever>();
            Mock<IDataManager> dataManager = new Mock<IDataManager>();
            Mock<INotifier> notifier = new Mock<INotifier>();
            
            ListingManager lm = new ListingManager(retriever.Object, dataManager.Object, notifier.Object);

            Assert.IsNotNull(lm);
        }

        [TestMethod]
        public void UpdateListings_ValidScenario_Succeeds()
        {
            // Arrange
            Mock<IListingRetriever> retriever = new Mock<IListingRetriever>();
            retriever.Setup(r => r.GetListings()).Returns(BuildValidListings());
            Mock<IDataManager> dataManager = new Mock<IDataManager>();
            dataManager.Setup(d => d.SaveListings(It.IsAny<Entities.Listings>()));
            Mock<INotifier> notifier = new Mock<INotifier>();
            notifier.Setup(n => n.SendNotification(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IList<string>>()));

            ListingManager lm = new ListingManager(retriever.Object, dataManager.Object, notifier.Object);

            // Act
            lm.UpdateListings();

            // Assert
            retriever.Verify(r => r.GetListings(), Times.Once());
            dataManager.Verify(d => d.SaveListings(It.IsAny<Entities.Listings>()), Times.Once());
            notifier.Verify(n => n.SendNotification(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IList<string>>()), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void UpdateListings_GetListingsFails_ThrowsException()
        {
            Mock<IListingRetriever> retriever = new Mock<IListingRetriever>();
            retriever.Setup(r => r.GetListings()).Throws(new ApplicationException("Test error"));

            Mock<IDataManager> dataManager = new Mock<IDataManager>();
            Mock<INotifier> notifier = new Mock<INotifier>();

            ListingManager lm = new ListingManager(retriever.Object, dataManager.Object, notifier.Object);
            lm.UpdateListings();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void UpdateListings_SaveListingsFails_ThrowsException()
        {
            Mock<IListingRetriever> retriever = new Mock<IListingRetriever>();
            retriever.Setup(r => r.GetListings()).Returns(BuildValidListings());

            Mock<IDataManager> dataManager = new Mock<IDataManager>();
            dataManager.Setup(d => d.SaveListings(It.IsAny<Entities.Listings>())).Throws(new ApplicationException());

            Mock<INotifier> notifier = new Mock<INotifier>();

            ListingManager lm = new ListingManager(retriever.Object, dataManager.Object, notifier.Object);
            lm.UpdateListings();
        }

        [TestMethod]
        public void UpdateListings_SendNotificationFails_Succeeds()
        {
            Mock<IListingRetriever> retriever = new Mock<IListingRetriever>();
            retriever.Setup(r => r.GetListings()).Returns(BuildValidListings());

            Mock<IDataManager> dataManager = new Mock<IDataManager>();
            dataManager.Setup(d => d.SaveListings(It.IsAny<Entities.Listings>()));

            Mock<INotifier> notifier = new Mock<INotifier>();
            notifier.Setup(n => n.SendNotification(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IList<string>>())).Throws(new ApplicationException());

            ListingManager lm = new ListingManager(retriever.Object, dataManager.Object, notifier.Object);
            lm.UpdateListings();
        }

        private static Entities.Listings BuildValidListings()
        {
            return new Entities.Listings()
            {
                Channels = new System.Collections.Generic.List<Entities.Channel>()
                {
                    new Entities.Channel() 
                    {
                        Name = "Channel 1",
                        Number = 1,
                        Programmes = new System.Collections.Generic.List<Entities.Programme>()
                        {
                            new Entities.Programme()
                            {
                                Date = DateTime.Now.AddDays(1),
                                Duration = 60,
                                StartTime = "18:00",
                                EndTime = "19:00",
                                IsFilm = false,
                                Title = "Programme 1"
                            }
                        }
                    }
                }
            };
        }
    }
}
