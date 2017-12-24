using Services.Models.HockeyFantasyService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MoreLinq;
using System.Net;
using Newtonsoft.Json;
using System.Globalization;
using MySportsFeedDriver;
using MySportsFeedDriver.Models;

namespace Services
{
    public class HockeyFantasyService
    {
        public List<LineUpData> GetLineUpData()
        {
            string fileLocation = ConfigurationManager.AppSettings["DraftKingsRoster"];

            List<LineUpData> lineup = new List<LineUpData>();
            using (var reader = new StreamReader(fileLocation))
            {
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var values = reader.ReadLine().Replace("\"", string.Empty).Split(',');

                    lineup.Add(new LineUpData()
                    {
                        Position = values[0],
                        Name = values[1],
                        Tier = values[2],
                        GameInfo = values[3],
                        AvgPointsPerGame = values[4],
                        Team = values[5]
                    });
                }
            }
            return lineup;
        }

        public void Test()
        {
            MySportsFeed driver = new MySportsFeed();

            List<PlayerGameLog> playerGameLogs = new List<PlayerGameLog>();

            List<LineUpData> lineUpData = GetLineUpData();
            foreach(var player in lineUpData)
            {
                playerGameLogs.AddRange(driver.GetPlayerGameLogs(player.Name.Replace(' ', '-')));
            }
        }
    }
}
