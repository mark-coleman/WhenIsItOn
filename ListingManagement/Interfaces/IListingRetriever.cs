using Entities;
using System.Collections.Generic;

namespace ListingManagement.Interfaces
{
    public interface IListingRetriever
    {
        Listings GetListings();
        List<Channel> GetChannels();
        List<Programme> GetProgrammes(int ChannelId);
    }
}
