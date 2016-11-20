using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riot.Net_API
{
    public static class ChampionAPI
    {
        public static List<ChampionDto> RetrieveAllChampions(ChampionRegion _region)
        {
            if (_region == ChampionRegion.None)
            {
                return null;
            }

            string url = string.Format(@"https://global.api.pvp.net/api/lol/{0}/v1.2/champion?api_key={1}", _region, Helpers.apiKey);

            dynamic champions = Helpers.GetJSONObject(url);

            List<ChampionDto> returnMe = new List<ChampionDto>();

            foreach (dynamic champion in champions["champions"])
            {
                returnMe.Add(new ChampionDto(champion));
            }

            return returnMe;
        }

        public static List<ChampionDto> RetrieveAllChampions(ChampionRegion _region, bool _freeToPlay)
        {
            if (_region == ChampionRegion.None)
            {
                return null;
            }

            string url = string.Format(@"https://global.api.pvp.net/api/lol/{0}/v1.2/champion?freeToPlay={1}&api_key={2}", _region, _freeToPlay, Helpers.apiKey);

            dynamic champions = Helpers.GetJSONObject(url);

            List<ChampionDto> returnMe = new List<ChampionDto>();

            foreach (dynamic champion in champions["champions"])
            {
                returnMe.Add(new ChampionDto(champion));
            }

            return returnMe;
        }

        public static ChampionDto RetrieveChampionById(ChampionRegion _region, int _id)
        {
            if (_region == ChampionRegion.None)
            {
                return null;
            }

            string url = string.Format(@"https://global.api.pvp.net/api/lol/{0}/v1.2/champion/{1}?api_key={2}", _region, _id, Helpers.apiKey);
            dynamic champion = Helpers.GetJSONObject(url);

            return new ChampionDto(champion);
        }

        public enum ChampionRegion
        {
            BR,
            EUNE,
            EUW,
            JP,
            KR,
            LAN,
            LAS,
            NA,
            OCE,
            RU,
            TR,
            None
        }
    }

    public class ChampionDto
    {
        #region Private Fields
        private bool active;
        private bool botEnabled;
        private bool botMmEnabled;
        private bool freeToPlay;
        private long id;
        private bool rankedPlayEnabled;
        #endregion

        public ChampionDto()
        {
            this.active = false;
            this.botEnabled = false;
            this.botMmEnabled = false;
            this.freeToPlay = false;
            this.id = 0;
            this.rankedPlayEnabled = false;
        }

        public ChampionDto(dynamic _champion)
        {
            this.active = (bool)_champion["active"];
            this.botEnabled = (bool)_champion["botEnabled"];
            this.botMmEnabled = (bool)_champion["botMmEnabled"];
            this.freeToPlay = (bool)_champion["freeToPlay"];
            this.id = (long)_champion["id"];
            this.rankedPlayEnabled = (bool)_champion["rankedPlayEnabled"];

        }

        #region Public Properties
        public bool Active
        {
            get { return this.active; }
            set { this.active = value; }
        }

        public bool BotEnabled
        {
            get { return this.botEnabled; }
            set { this.botEnabled = value; }
        }

        public bool BotMmEnabled
        {
            get { return this.botMmEnabled; }
            set { this.botMmEnabled = value; }
        }

        public bool FreeToPlay
        {
            get { return this.freeToPlay; }
            set { this.freeToPlay = value; }
        }

        public long Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public bool RankedPlayEnabled
        {
            get { return this.rankedPlayEnabled; }
            set { this.rankedPlayEnabled = value; }
        }
        #endregion
    }

}
