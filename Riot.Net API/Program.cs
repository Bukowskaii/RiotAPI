using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

using Newtonsoft.Json;

using Google.GData.Client;
using Google.GData.Spreadsheets;
using Google.GData.Extensions;

namespace Riot.Net_API
{
    public static class Program
    {

        public static void Main(string[] args)
        {
            Console.WriteLine("Working...");

            long bukow = 50217665; // bukowskaii's player id
            Region bRegion = new Region(Region.Regions.NA);

            Console.WriteLine("Getting List of all champions...");
            List<ChampionDto> allChampions = ChampionAPI.RetrieveAllChampions(bRegion.ChampionRegion);


            Console.WriteLine("Getting masteries for Bukowskaii...");
            List<ChampionMasteryDto> myMasteries = ChampionMasteryAPI.GetAllChampionMastery(bRegion.ChampionMasteryRegion, bukow);


            Console.WriteLine("Getting Dictionary of champion Ids and champion metadata...");
            Dictionary<long, ChampionMetaData> championLookup = StaticDataAPI.GetChampionMetaData(bRegion.ChampionRegion);

            Console.WriteLine("Writing info to file...");
            List<long> loggedIds = new List<long>();
            StringBuilder sb = new StringBuilder();

            foreach (ChampionMasteryDto mastery in myMasteries)
            {
                string champName = championLookup[mastery.ChampionId].Name;
                string champTitle = championLookup[mastery.ChampionId].Title;

                sb.AppendLine(champName + " -- " + champTitle + "," +
                    mastery.ChampionId + "," +
                    mastery.ChestGranted + "," +
                    mastery.TokensEarned + "," +
                    UnixTimeStampToDateTime(mastery.LastPlayTime) + "," +
                    mastery.ChampionLevel + "," +
                    mastery.ChampionPoints);

                loggedIds.Add(mastery.ChampionId);
            }

            foreach (ChampionDto champion in allChampions)
            {

                if (!loggedIds.Contains(champion.Id))
                {
                    string champName = championLookup[champion.Id].Name;
                    string champTitle = championLookup[champion.Id].Title;

                    sb.AppendLine(champName + " - " + champTitle + "," +
                        champion.Id + "," +
                        "False," +
                        "0," +
                        "0," +
                        "0," +
                        "0");
                }

            }


            //File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "LeagueData.csv"), sb.ToString());

            //Console.WriteLine("Done! -- Press any key to exit.");
            //Thread.Sleep(100);
            //Console.ReadKey(false);

            //Thread.Sleep(100);



            const string ClientId = "376250057781-pud8thtu2qrku59p4jkusf5o97d0nc1e.apps.googleusercontent.com";
            string ClientSecret = "WAIKheO-M3YQ6RVlDrK0DC8i";
            string scope = "https://spreadsheets.google.com/feeds";
            string redirect = "urn:ietf:wg:oauth:2.0:oob";

            OAuth2Parameters parameters = new OAuth2Parameters();
            parameters.ClientId = ClientId;
            parameters.ClientSecret = ClientSecret;
            parameters.RedirectUri = redirect;
            parameters.Scope = scope;

            string authorizationUrl = OAuthUtil.CreateOAuth2AuthorizationUrl(parameters);
            
            Process.Start(authorizationUrl);
            Console.WriteLine("Paste your authorization code:");
            parameters.AccessCode = Console.ReadLine();
            //parameters.AccessCode = "4/6op1327wcvTW8eMwFm4D4ZETIS2JCDSas8oifd1OD9M";
            
            OAuthUtil.GetAccessToken(parameters);
            string accessToken = parameters.AccessToken;
            Console.WriteLine("OAuthAccessToken: " + accessToken);


            GOAuth2RequestFactory requestFactory =
                new GOAuth2RequestFactory(null, "LoL Crate Parse", parameters);
            SpreadsheetsService service = new SpreadsheetsService("LoL Crate Parse");
            service.RequestFactory = requestFactory;

            SpreadsheetQuery query = new SpreadsheetQuery();
            SpreadsheetFeed feed = service.Query(query);

            if (feed.Entries.Count > 0)
            {
                SpreadsheetEntry selectedEntry = new SpreadsheetEntry();
                foreach (SpreadsheetEntry entry in feed.Entries)
                {
                    if (entry.Title.Text == "LoL Crates")
                    {
                        selectedEntry = entry;
                    }
                }

                WorksheetFeed wFeed = selectedEntry.Worksheets;
                WorksheetEntry worksheet = (WorksheetEntry)wFeed.Entries[0];

                AtomLink listFeedLink = worksheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
                ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
                ListFeed listFeed = service.Query(listQuery);

                int entries = listFeed.Entries.Count;
                int remaining = 0;
                ListEntry existingRow;
                do
                {
                    existingRow = (ListEntry)listFeed.Entries[0];
                    existingRow.Delete();


                    listFeed = service.Query(listQuery);
                    remaining++;
                    Console.WriteLine(string.Format("Deleting {0} of {1} complete", remaining, entries));
                } while (listFeed.Entries.Count > 0);


                string[] rows = sb.ToString().Trim('\n').Split('\n');
                entries = rows.Length;
                remaining = 0;
                foreach (string row in rows)
                {
                    Console.WriteLine(string.Format("Inserting {0} of {1} complete", remaining, entries));
                    string[] elements = row.Split(',');
                    ListEntry toAdd = new ListEntry();

                    toAdd.Elements.Add(new ListEntry.Custom() { LocalName = "champion", Value = elements[0] });
                    toAdd.Elements.Add(new ListEntry.Custom() { LocalName = "id", Value = elements[1] });
                    toAdd.Elements.Add(new ListEntry.Custom() { LocalName = "chest", Value = elements[2] });
                    toAdd.Elements.Add(new ListEntry.Custom() { LocalName = "tokensearned", Value = elements[3] });
                    toAdd.Elements.Add(new ListEntry.Custom() { LocalName = "lastplayed", Value = elements[4] });
                    toAdd.Elements.Add(new ListEntry.Custom() { LocalName = "champlevel", Value = elements[5] });
                    toAdd.Elements.Add(new ListEntry.Custom() { LocalName = "masterypoints", Value = elements[6] });

                    service.Insert(listFeed, toAdd);
                    
                    remaining++;
                }
            }

            Console.WriteLine("Done! -- Press any key to exit.");
            Thread.Sleep(100);
            Console.ReadKey(false);
        }

        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }


    }

    

}
