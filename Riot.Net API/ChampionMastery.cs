using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riot.Net_API
{
    public static class ChampionMasteryAPI
    {
        public static ChampionMasteryDto GetSpecificChampionMastery(ChampionMasteryRegion _region, long _playerId, long _championId)
        {
            if (_region == ChampionMasteryRegion.None)
            {
                return null;
            }

            string url = string.Format(@"https://global.api.pvp.net/championmastery/location/{0}/player/{1}/champion/{2}?api_key={3}", _region, _playerId, _championId, Helpers.apiKey);
            dynamic championMastery = Helpers.GetJSONObject(url);

            return new ChampionMasteryDto(championMastery);
        }

        public static List<ChampionMasteryDto> GetAllChampionMastery(ChampionMasteryRegion _region, long _playerId)
        {
            if (_region == ChampionMasteryRegion.None)
            {
                return null;
            }

            string url = string.Format(@"https://global.api.pvp.net/championmastery/location/{0}/player/{1}/champions?api_key={2}", _region, _playerId, Helpers.apiKey);
            dynamic championMasteries = Helpers.GetJSONObject(url);

            List<ChampionMasteryDto> returnMe = new List<ChampionMasteryDto>();

            foreach (dynamic championMastery in championMasteries["champions"])
            {
                returnMe.Add(new ChampionMasteryDto(championMastery));
            }

            return returnMe;
        }

        public static int GetTotalChampionScore(ChampionMasteryRegion _region, long _playerId)
        {
            if (_region == ChampionMasteryRegion.None)
            {
                return 0;
            }

            string url = string.Format(@"https://global.api.pvp.net/championmastery/location/{0}/player/{1}/score?api_key={2}", _region, _playerId, Helpers.apiKey);
            dynamic totalScore = Helpers.GetJSONObject(url);

            return (int)totalScore;
        }

        public static List<ChampionMasteryDto> GetTopNChampionMasteries(ChampionMasteryRegion _region, long _playerId, int _n = 3)
        {
            if (_region == ChampionMasteryRegion.None)
            {
                return null;
            }

            string url = string.Format(@"https://global.api.pvp.net/championmastery/location/{0}/player/{1}/topchampions?count={2}&api_key={3}", _region, _playerId, _n, Helpers.apiKey);

            dynamic championMasteries = Helpers.GetJSONObject(url);

            List<ChampionMasteryDto> returnMe = new List<ChampionMasteryDto>();

            foreach (dynamic championMastery in championMasteries)
            {
                returnMe.Add(new ChampionMasteryDto(championMastery));
            }

            return returnMe;

        }

        public enum ChampionMasteryRegion
        {
            BR1,
            EUN1,
            EUW1,
            JP1,
            KR,
            LA1,
            LA2,
            NA1,
            OC1,
            RU,
            TR1,
            None
        }
    }
    
    public class ChampionMasteryDto
    {
        #region Private Fields
        long championId;
        int championLevel;
        int championPoints;
        long championPointsSinceLastLevel;
        long championPointsUntilNextLevel;
        bool chestGranted;
        long lastPlayTime;
        long playerId;
        int tokensEarned;
        #endregion

        public ChampionMasteryDto(dynamic _championMastery)
        {
            if (_championMastery != null)
            {
                this.championId = (long)_championMastery["championId"];
                this.championLevel = (int)_championMastery["championLevel"];
                this.championPoints = (int)_championMastery["championPoints"];
                this.championPointsSinceLastLevel = (long)_championMastery["championPointsSinceLastLevel"];
                this.championPointsUntilNextLevel = (long)_championMastery["championPointsUntilNextLevel"];
                this.chestGranted = (bool)_championMastery["chestGranted"];
                this.lastPlayTime = (long)_championMastery["lastPlayTime"];
                this.playerId = (long)_championMastery["playerId"];
                this.tokensEarned = (int)_championMastery["tokensEarned"];
            }

        }

        #region Public Properties
        public long ChampionId
        {
            get { return this.championId; }
            set { this.championId = value; }
        }
        public int ChampionLevel
        {
            get { return this.championLevel; }
            set { this.championLevel = value; }
        }
        public int ChampionPoints
        {
            get { return this.championPoints; }
            set { this.championPoints = value; }
        }
        public long ChampionPointsSinceLastLevel
        {
            get { return championPointsSinceLastLevel; }
            set { this.championPointsSinceLastLevel = value; }
        }
        public long ChampionPointsUntilNextLevel
        {
            get { return this.championPointsUntilNextLevel; }
            set { this.championPointsUntilNextLevel = value; }
        }
        public bool ChestGranted
        {
            get { return this.chestGranted; }
            set { this.chestGranted = value; }
        }
        public long LastPlayTime
        {
            get { return this.lastPlayTime; }
            set { this.lastPlayTime = value; }
        }
        public long PlayerId
        {
            get { return this.playerId; }
            set { this.playerId = value; }
        }
        public int TokensEarned{
            get{return this.tokensEarned;}
            set{this.tokensEarned = value;}
        }
        #endregion

    }

}
