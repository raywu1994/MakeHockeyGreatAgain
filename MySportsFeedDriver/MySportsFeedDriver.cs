using MySportsFeedDriver.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MySportsFeedDriver
{
    public class MySportsFeed
    {
        public MySportsFeed()
        {

        }

        public List<PlayerGameLog> GetPlayerGameLogs(string[] playerNames)
        {
            DateTime now = DateTime.Now;

            string currentSeasonJson = string.Empty;
            string lastSeasonJson = string.Empty;

            //check if the file is already saved for today 
            string fileFormat = "/jsonExports/" + now.ToString("yyyy-MM-dd") + "-{0}.json";
            if (File.Exists(string.Format(fileFormat,"A")) && File.Exists(string.Format(fileFormat,"B")))
            {
                using (StreamReader reader = new StreamReader(string.Format(fileFormat, "A")))
                {
                    currentSeasonJson = reader.ReadToEnd();
                }

                using (StreamReader reader = new StreamReader(string.Format(fileFormat, "B")))
                {
                    lastSeasonJson = reader.ReadToEnd();
                }
            }
            else
            {
                //retrieve this season's and last
                string currentSeason = now.Month > 8 ?
                    now.Year.ToString() + "-" + (now.Year + 1).ToString() :
                    (now.Year - 1).ToString() + "-" + now.Year.ToString();

                string lastSeason = now.Month > 8 ?
                    (now.Year - 1).ToString() + "-" + now.Year.ToString() :
                    (now.Year - 2).ToString() + "-" + (now.Year - 1).ToString();

                string urlFormat = "https://api.mysportsfeeds.com/v1.1/pull/nhl/{0}-regular/player_gamelogs.json?player={1}";
                string currentSeasonUrl = string.Format(urlFormat, currentSeason, string.Join(",", playerNames));
                string lastSeasonUrl = string.Format(urlFormat, lastSeason, string.Join(",", playerNames));

                currentSeasonJson = GetJsonResponse(currentSeasonUrl);
                lastSeasonJson = GetJsonResponse(lastSeasonUrl);

                File.WriteAllText(string.Format(fileFormat,"A"), currentSeasonJson);
                File.WriteAllText(string.Format(fileFormat,"B"), lastSeasonJson);
            }

            List<PlayerGameLog> gameLog = MapToPlayerGameLog(currentSeasonJson);
            if (!string.IsNullOrEmpty(lastSeasonJson))
            {
                gameLog.AddRange(MapToPlayerGameLog(lastSeasonJson));
            }
            return gameLog;
        }

        #region MappingJsonToModel
        private List<PlayerGameLog> MapToPlayerGameLog(string json)
        {
            List<PlayerGameLog> gameLogs = new List<PlayerGameLog>();

            dynamic array = JsonConvert.DeserializeObject(json);
            foreach (var line in array.playergamelogs.gamelogs)
            {
                var game = new Game
                {
                    Id = line.game.id,
                    Date = DateTime.ParseExact((string)line.game.date + " " + (string)line.game.time, "yyyy-MM-dd h:mmtt", CultureInfo.InvariantCulture),
                    AwayTeam = new Team
                    {
                        Id = line.game.awayTeam.ID,
                        City = line.game.awayTeam.City,
                        Name = line.game.awayTeam.Name,
                        Abbreviation = line.game.awayTeam.Abbreviation
                    },
                    HomeTeam = new Team
                    {
                        Id = line.game.homeTeam.ID,
                        City = line.game.homeTeam.City,
                        Name = line.game.homeTeam.Name,
                        Abbreviation = line.game.homeTeam.Abbreviation
                    },
                    Location = line.game.location
                };

                var player = new Player
                {
                    Id = line.player.ID,
                    LastName = line.player.LastName,
                    FirstName = line.player.FirstName,
                    JerseyNumber = line.player.JerseyNumber,
                    Position = line.player.Position,
                    Team = new Team
                    {
                        Id = line.team.ID,
                        City = line.team.City,
                        Name = line.team.Name,
                        Abbreviation = line.team.Abbreviation
                    }
                };

                PlayerGameStats gameStats = new PlayerGameStats()
                {
                    PenaltyStats = new PenaltyStats
                    {
                        Penalties = line.stats.Penalties["#text"],
                        PIM = line.stats.PenaltyMinutes["#text"]
                    },
                    ScoringStats = new ScoringStats
                    {
                        Goals = line.stats.Goals["#text"],
                        Assists = line.stats.Assists["#text"],
                        Points = line.stats.Points["#text"],
                        HatTricks = line.stats.HatTricks["#text"],
                        PowerPlayGoals = line.stats.PowerplayGoals["#text"],
                        PowerPlayAssists = line.stats.PowerplayAssists["#text"],
                        PowerPlayPoints = line.stats.PowerplayPoints["#text"],
                        ShortHandedGoals = line.stats.ShorthandedGoals["#text"],
                        ShortHandedAssists = line.stats.ShorthandedAssists["#text"],
                        ShortHandedPoints = line.stats.ShorthandedPoints["#text"],
                        GameWinningGoals = line.stats.GameWinningGoals["#text"],
                        GameTyingGoals = line.stats.GameTyingGoals["#text"]
                    }
                };

                if (player.Position == "G")
                {
                    gameStats.GoalieScoringStats = new GoalieScoringStats
                    {
                        Wins = line.stats.Wins["#text"],
                        Losses = line.stats.Losses["#text"],
                        OvertimeWins = line.stats.OvertimeWins["#text"],
                        OvertimeLosses = line.stats.OvertimeLosses["#text"],
                        GoalsAgainst = line.stats.GoalsAgainst["#text"],
                        ShotsAgainst = line.stats.ShotsAgainst["#text"],
                        Saves = line.stats.Saves["#text"],
                        GoalsAgainstAverage = line.stats.GoalsAgainstAverage["#text"],
                        SavePercentage = line.stats.SavePercentage["#text"],
                        Shutouts = line.stats.Shutouts["#text"],
                        GamesStarted = line.stats.GamesStarted["#text"],
                        CreditForGame = line.stats.CreditForGame["#text"],
                        MinutesPlayed = line.stats.MinutesPlayed["#text"]
                    };
                }
                else
                {
                    gameStats.SkatingStats = new SkatingStats
                    {
                        PlusMinus = line.stats.PlusMinus["#text"],
                        Shots = line.stats.Shots["#text"],
                        ShotPercentage = line.stats.ShotPercentage["#text"],
                        BlockedShots = line.stats.BlockedShots == null ? null : line.stats.BlockedShots["#text"],
                        Hits = line.stats.Hits["#text"],
                        FaceoffsTaken = line.stats.Faceoffs["#text"],
                        FaceoffWins = line.stats.FaceoffWins["#text"],
                        FaceoffsLosses = line.stats.FaceoffLosses["#text"],
                        FaceoffPercentage = line.stats.FaceoffPercent["#text"]
                    };
                }

                gameLogs.Add(new PlayerGameLog
                {
                    GameInfo = game,
                    PlayerInfo = player,
                    PlayerGameStats = gameStats
                });
            }
            return gameLogs;
        }
        #endregion

        #region Helpers+
        /// <summary>
        /// Sends a request to MySportsFeed. Return Json (as a strong), or empty if the call failed.
        /// </summary>
        private string GetJsonResponse(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.Headers.Add("Authorization", "Basic " + GenerateMySportsFeedAuthString());
                request.Method = "GET";

                string results;
                using (WebResponse response = request.GetResponse())
                {
                    StreamReader sr = new StreamReader(response.GetResponseStream());
                    results = sr.ReadToEnd();
                }

                return results;
            }
            catch(Exception e)
            {
                return string.Empty;
            }

        }
        private string GenerateMySportsFeedAuthString()
        {
            var username = ConfigurationManager.AppSettings["MySportsFeed.User"];
            var password = ConfigurationManager.AppSettings["MySportsFeed.Password"];

            var bytes = Encoding.UTF8.GetBytes(username + ":" + password);
            return Convert.ToBase64String(bytes);
        }
        #endregion
    }
}
