using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riot.Net_API
{
    static class StaticDataAPI
    {
        static public Dictionary<long, ChampionMetaData> GetChampionMetaData(
            ChampionAPI.ChampionRegion _region)
        {
            Dictionary<long, ChampionMetaData> returnMe = new Dictionary<long, ChampionMetaData>();

            string url = string.Format("https://global.api.pvp.net/api/lol/static-data/na/v1.2/champion?api_key={0}", Helpers.apiKey);

            dynamic metaData = Helpers.GetJSONObject(url);
            string type = (string)metaData["type"];
            string version = (string)metaData["version"];

            foreach (dynamic champion in metaData["data"])
            {
                ChampionMetaData champData = new ChampionMetaData(champion.Value);
                returnMe.Add(champData.Id, champData);
            }

            return returnMe;
        }


    }

    class ChampionMetaData
    {
        #region Private Fields
        private long id;
        private string title;
        private string name;
        private string key;
        #endregion

        public ChampionMetaData(dynamic _championMetaData)
        {
            this.id = (long)_championMetaData["id"];
            this.title = (string)_championMetaData["title"];
            this.name = (string)_championMetaData["name"];
            this.key = (string)_championMetaData["key"];
        }

        #region Public Properties
        public long Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        public string Key
        {
            get { return this.key; }
            set { this.key = value; }
        }
        #endregion
    }
}
