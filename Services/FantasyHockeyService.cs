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
using Services.Models.FantasyHockeyService;

namespace Services
{
    public class FantasyHockeyService
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
                        Name = HandleNamingExceptions(values[1]),
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

        }

        public List<DraftKingsPlayerSelection> GetDraftKingsPlayerSelections()
        {
            MySportsFeed driver = new MySportsFeed();

            List<LineUpData> lineUpData = GetLineUpData();
            string[] players = lineUpData.Select(x => x.Name.Replace(' ', '-')).ToArray();

            List<PlayerGameLog> playerGameLogs = driver.GetPlayerGameLogs(players);
            return GenerateDraftKingsPlayerSelections(playerGameLogs, lineUpData);
        }

        private List<DraftKingsPlayerSelection> GenerateDraftKingsPlayerSelections(List<PlayerGameLog> gameLogs, List<LineUpData> lineUpData)
        {
            var draftKingsPlayerSelections = new List<DraftKingsPlayerSelection>();

            var stats = gameLogs
                .GroupBy(x => (x.PlayerInfo.FirstName+x.PlayerInfo.LastName).Replace(" ",""))
                .ToDictionary(group => group.Key, group => group.ToList());

            foreach (var player in lineUpData)
            {
                var newEntry = new DraftKingsPlayerSelection(player.Tier, player.Name, stats[player.Name.Replace(" ","")]);

                draftKingsPlayerSelections.Add(newEntry);
            }

            return draftKingsPlayerSelections;
        }

        private string HandleNamingExceptions(string lineUpName)
        {
            if (lineUpName == "James van Riemsdyk")
            {
                return "James vanRiemsdyk";
            }

            return lineUpName;
        }
    }
}
