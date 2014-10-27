using Entities;
using ListingManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ListingManagement
{

    public class XmlTvListingRetriever : IListingRetriever
    {
        private static string CHANNEL_LIST_URL = "http://xmltv.radiotimes.com/xmltv/channels.dat";
        private static string PROGRAMME_DATA_URL = "http://xmltv.radiotimes.com/xmltv/{0}.dat";
        private static char CHANNEL_DELIMITER = '|';
        private static char PROGRAMME_DELIMITER = '~';

        public Listings GetListings()
        {
            Listings listings = new Listings();
            listings.Channels = GetChannels();
            Parallel.ForEach(listings.Channels, channel =>
                {
                    channel.Programmes = GetProgrammes(channel.Number);
                });

            return listings;
        }

        public List<Channel> GetChannels()
        {
            string channelData = GetChannelListAsString();
            return DeserialiseChannelList(channelData);
        }

        public List<Programme> GetProgrammes(int ChannelId)
        {
            string programmeData = GetProgrammeDataAsString(ChannelId);
            return DeserialiseProgrammeData(programmeData, ChannelId);
        }

        private static List<Programme> DeserialiseProgrammeData(string programmeData, int channel)
        {
            List<Programme> listings = new List<Programme>();

            using (StringReader s = new StringReader(programmeData))
            {
                bool endOfFile = false;
                while (!endOfFile)
                {
                    string programmeLine = s.ReadLine();
                    if (programmeLine != null)
                    {
                        if (programmeLine.Contains(PROGRAMME_DELIMITER))
                        {
                            listings.Add(ParseProgramme(programmeLine, channel));
                        }
                    }
                    else
                    {
                        endOfFile = true;
                    }
                }
            }

            return listings;
        }

        private static Programme ParseProgramme(string programmeLine, int channel)
        {
            string[] c = programmeLine.Split(PROGRAMME_DELIMITER);
            return new Programme()
            {
                Title = c[0],
                Date = DateTime.Parse(c[19], CultureInfo.GetCultureInfo("en-GB")).Date,
                StartTime = c[20],
                EndTime = c[21],
                Duration = int.Parse(c[22]),
                Episode = c[2],
                Genre = c[16],
                IsFilm = bool.Parse(c[7]),
                Year = c[3]
            };
        }

        private static List<Channel> DeserialiseChannelList(string channelData)
        {
            List<Channel> channels = new List<Channel>();

            using (StringReader s = new StringReader(channelData))
            {
                bool endOfFile = false;
                while (!endOfFile)
                {
                    string channelLine = s.ReadLine();
                    if (channelLine != null)
                    {
                        if (channelLine.Contains(CHANNEL_DELIMITER))
                        {
                            channels.Add(ParseChannel(channelLine));
                        }
                    }
                    else
                    {
                        endOfFile = true;
                    }
                }
            }

            return channels;
        }

        private static Channel ParseChannel(string channelData)
        {
            string[] c = channelData.Split(CHANNEL_DELIMITER);
            return new Channel() { Number = int.Parse(c[0]), Name = c[1] };
        }

        private static string GetProgrammeDataAsString(int channelId)
        {
            return DownloadPageAsync(string.Format(PROGRAMME_DATA_URL, channelId)).Result;
        }

        private static string GetChannelListAsString()
        {
            return DownloadPageAsync(CHANNEL_LIST_URL).Result;
        }

        private static async Task<string> DownloadPageAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    using (HttpContent content = response.Content)
                    {
                        return await content.ReadAsStringAsync();
                    }
                }
            }
        }
    }
}
