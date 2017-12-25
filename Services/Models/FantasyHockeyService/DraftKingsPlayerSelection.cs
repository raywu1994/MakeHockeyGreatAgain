using MySportsFeedDriver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;

namespace Services.Models.FantasyHockeyService
{
    public class DraftKingsPlayerSelection
    {
        public DraftKingsPlayerSelection(string tier, string playerName, List<PlayerGameLog> gameLogs)
        {
            Tier = tier;
            PlayerName = playerName;
            GameLogs = gameLogs.OrderByDescending(x => x.GameInfo.Date).ToList();

            AvgFPLast10 = Statistics.Mean(GameLogs.Select(x => CalculateFanPoints(x)));
            VarFPLast10 = Statistics.Variance(GameLogs.Select(x => CalculateFanPoints(x)));
        }

        public string Tier { get; set; }
        public string PlayerName { get; set; }

        private List<PlayerGameLog> GameLogs { get; set; }

        /// <summary>
        /// Average fan points last 10 games
        /// </summary>
        public double AvgFPLast10 { get; }
        /// <summary>
        /// Variance of fan points last 10 games 
        /// </summary>
        public double VarFPLast10 { get; }


        private double CalculateFanPoints(PlayerGameLog gameLog)
        {
            double total = 0;
            if (gameLog.PlayerInfo.Position == "G")
            {
                var goalieStats = gameLog.PlayerGameStats.GoalieScoringStats;
                total += goalieStats.Wins * 3;
                total += goalieStats.Saves * 0.2;
                total += goalieStats.GoalsAgainst * -1;
                total += goalieStats.Shutouts * 2;
            }
            else
            {
                var skatingStats = gameLog.PlayerGameStats.SkatingStats;
                total += skatingStats.Shots * 0.5;
                total += skatingStats.BlockedShots.HasValue ? skatingStats.BlockedShots.Value * 0.5 : 0;
            }
            var scoringStats = gameLog.PlayerGameStats.ScoringStats;
            total += scoringStats.Goals * 3;
            total += scoringStats.Assists * 2;
            total += scoringStats.ShortHandedPoints;
            total += scoringStats.HatTricks * 1.5;

            return total;
        }
    }
}
