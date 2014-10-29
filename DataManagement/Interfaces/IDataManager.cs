using Entities;
using System.Collections.Generic;

namespace DataManagement.Interfaces
{
    public interface IDataManager
    {
        void Reset();
        void SaveListings(Listings listings);
    	IList<ListingSearchResult> FindListing(string listingName);
        IList<string> GetTitles();
        void SaveUserSearch(string listingName, string emailAddress);
        IList<UserSearch> GetUserSearches();
        void DeleteUserSearch(UserSearch userSearch);
    }
}
