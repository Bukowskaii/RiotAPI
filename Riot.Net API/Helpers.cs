using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using Newtonsoft.Json.Linq;

namespace Riot.Net_API
{
    internal class Helpers
    {
        internal static string apiKey = Riot.Net_API.Properties.Resources.BUKO_API_KEY;

        internal static string GetJSONString(string _url)
        {
            using (WebClient wc = new WebClient())
            {
                try
                {
                    return wc.DownloadString(_url);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message, e);
                    // What are possible Errors???
                }
            }
        }

        internal static dynamic GetJSONObject(string _url)
        {
            using(WebClient wc = new WebClient())
            try
            {
                string JSONString = wc.DownloadString(_url);
                if (JSONString == string.Empty)
                {
                    return null;
                }
                try
                {
                    return JObject.Parse(JSONString);
                }
                catch
                {
                    JSONString = "{\"champions\":" + JSONString + "}";
                    return JObject.Parse(JSONString);
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
    }


    public class Region
    {
        private ChampionAPI.ChampionRegion championRegion;
        private ChampionMasteryAPI.ChampionMasteryRegion championMasteryRegion;

        public Region(Regions _region)
        {
            switch (_region)
            {
                case Regions.BR:
                    this.championRegion = ChampionAPI.ChampionRegion.BR;
                    this.championMasteryRegion = ChampionMasteryAPI.ChampionMasteryRegion.BR1;
                    break;
                case Regions.EUN:
                    this.championRegion = ChampionAPI.ChampionRegion.EUNE;
                    this.championMasteryRegion = ChampionMasteryAPI.ChampionMasteryRegion.EUN1;
                    break;
                case Regions.EUW:
                    this.championRegion = ChampionAPI.ChampionRegion.EUW;
                    this.championMasteryRegion = ChampionMasteryAPI.ChampionMasteryRegion.EUW1;
                    break;
                case Regions.JP:
                    this.championRegion = ChampionAPI.ChampionRegion.JP;
                    this.championMasteryRegion = ChampionMasteryAPI.ChampionMasteryRegion.JP1;
                    break;
                case Regions.KR:
                    this.championRegion = ChampionAPI.ChampionRegion.KR;
                    this.championMasteryRegion = ChampionMasteryAPI.ChampionMasteryRegion.KR;
                    break;
                case Regions.LAN:
                    this.championRegion = ChampionAPI.ChampionRegion.LAN;
                    this.championMasteryRegion = ChampionMasteryAPI.ChampionMasteryRegion.LA1;
                    break;
                case Regions.LAS:
                    this.championRegion = ChampionAPI.ChampionRegion.LAS;
                    this.championMasteryRegion = ChampionMasteryAPI.ChampionMasteryRegion.LA2;
                    break;
                case Regions.NA:
                    this.championRegion = ChampionAPI.ChampionRegion.NA;
                    this.championMasteryRegion = ChampionMasteryAPI.ChampionMasteryRegion.NA1;
                    break;
                case Regions.OCE:
                    this.championRegion = ChampionAPI.ChampionRegion.OCE;
                    this.championMasteryRegion = ChampionMasteryAPI.ChampionMasteryRegion.OC1;
                    break;
                case Regions.RU:
                    this.championRegion = ChampionAPI.ChampionRegion.RU;
                    this.championMasteryRegion = ChampionMasteryAPI.ChampionMasteryRegion.RU;
                    break;
                case Regions.TR:
                    this.championRegion = ChampionAPI.ChampionRegion.TR;
                    this.championMasteryRegion = ChampionMasteryAPI.ChampionMasteryRegion.TR1;
                    break;
                default:
                    this.championRegion = ChampionAPI.ChampionRegion.None;
                    this.championMasteryRegion = ChampionMasteryAPI.ChampionMasteryRegion.None;
                    break;


            }

        }

        public ChampionAPI.ChampionRegion ChampionRegion
        {
            get { return this.championRegion; }
            set { this.championRegion = value; }
        }
        public ChampionMasteryAPI.ChampionMasteryRegion ChampionMasteryRegion
        {
            get { return this.championMasteryRegion; }
            set { this.championMasteryRegion = value; }
        }

        public enum Regions
        {
            BR,
            EUN,
            EUW,
            JP,
            KR,
            LAN,
            LAS,
            NA,
            OCE,
            RU,
            TR
        }

    }

}