using System.ComponentModel.DataAnnotations;

namespace MySportsFeedDriver.Models
{
    public class PlayerGameStats
    {
        public ScoringStats ScoringStats { get; set; }
        public PenaltyStats PenaltyStats { get; set; }
        public SkatingStats SkatingStats { get; set; }
        public GoalieScoringStats GoalieScoringStats { get; set; }
    }
    public class ScoringStats
    {
        public int Id { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int Points { get; set; }
        public int HatTricks { get; set; }
        public int PowerPlayGoals { get; set; }
        public int PowerPlayAssists { get; set; }
        public int PowerPlayPoints { get; set; }
        public int ShortHandedGoals { get; set; }
        public int ShortHandedAssists { get; set; }
        public int ShortHandedPoints { get; set; }
        public int GameWinningGoals { get; set; }
        public int GameTyingGoals { get; set; }
    }
    public class PenaltyStats
    {
        public int Id { get; set; }
        public int Penalties { get; set; }
        public int PIM { get; set; }
    }
    public class SkatingStats
    {
        public int Id { get; set; }
        public int PlusMinus { get; set; }
        public int Shots { get; set; }
        public double ShotPercentage { get; set; }
        public int? BlockedShots { get; set; }
        public int Hits { get; set; }
        public int FaceoffsTaken { get; set; }
        public int FaceoffWins { get; set; }
        public int FaceoffsLosses { get; set; }
        public double FaceoffPercentage { get; set; }
    }
    public class GoalieScoringStats
    {
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int OvertimeWins { get; set; }
        public int OvertimeLosses { get; set; }
        public int GoalsAgainst { get; set; }
        public int ShotsAgainst { get; set; }
        public int Saves { get; set; }
        public double GoalsAgainstAverage { get; set; }
        public double SavePercentage { get; set; }
        public int Shutouts { get; set; }
        public int GamesStarted { get; set; }
        public int CreditForGame { get; set; }
        public int MinutesPlayed { get; set; }
    }
}
