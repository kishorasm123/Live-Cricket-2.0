using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Cricket_2._0.Model
{
    public class clsMatchDetailsForUI
    {
        private clsMatchDetails _objMatchDetails = null;

        public string SeriesName { get; set; }
        private string _BattingTeam = string.Empty;
        public string BattingTeam { get; set; }
        public string TeamScore { get; set; }
        public clsBatsMan Batsman1 { get; set; }
        public clsBatsMan Batsman2 { get; set; }
        public clsBowler Bowler { get; set; }
        public string RunRate { get; set; }
        public string Recent { get; set; }
        public string ManOfMatch { get; set; }
        public string MatchType { get; set; }
        public string MatchCarrier { get; set; }
        public string MatchDescription { get; set; }
        public string Target { get; set; }
        public string PreviousOvers { get; set; }
        public string Status { get; set; }
        public string Patnership { get; set; }
        public string Toss { get; set; }
        public string Ground { get; set; }
        public string LastWicket { get; set; }
        public string StartTime { get; set; }
        public string Venue { get; set; }
        public string TeamNames { get; set; }

        public clsMatchDetailsForUI(clsMatchDetails i_objMatchDetails)
        {
            try
            {
                _objMatchDetails = i_objMatchDetails;
                Status = i_objMatchDetails.status;
                Ground = i_objMatchDetails.venue.location;
                MatchType = i_objMatchDetails.type;
                MatchCarrier = MatchType + " Carrier";
                MatchDescription = i_objMatchDetails.match_desc;
                StartTime = i_objMatchDetails.start_time;
                Venue = i_objMatchDetails.venue.name + ", "+ i_objMatchDetails.venue.location;
                TeamNames = i_objMatchDetails.team1.s_name + " Vs " + i_objMatchDetails.team2.s_name;
                if (IsNull(i_objMatchDetails.mom) == false)
                {
                    ManOfMatch = "Player of the Match : " + GetMOMPlayerDetails(i_objMatchDetails.mom[0].ToString()).Name;
                }

                if (IsNull(i_objMatchDetails.toss) == false) Toss = "Toss won by " + i_objMatchDetails.toss.winner + " & choosen " + i_objMatchDetails.toss.decision;
                if (IsNull(i_objMatchDetails.series) == false) SeriesName = i_objMatchDetails.series.short_name;
                if (IsNull(i_objMatchDetails.score) == false)
                {
                    TeamScore = "-" + i_objMatchDetails.score.batting.score;
                    RunRate = "CRR - " + i_objMatchDetails.score.crr;
                    Recent = i_objMatchDetails.score.prev_overs;
                    Patnership = "Partnership " + i_objMatchDetails.score.prtshp;
                    Target = i_objMatchDetails.score.target;
                    Batsman1 = GetBatsmanDetails(0);
                    Batsman2 = GetBatsmanDetails(1);
                    Bowler = GetBowlerDetails();
                    BattingTeam = GetBattingTeamName(_objMatchDetails.score.batting.id);
                    LastWicket = "Last WK " + GetLastWicket();
                }
            }
            catch (Exception)
            {
            }
        }

        private bool IsNull(object objToCheck = null)
        {
            try
            {
                if (object.ReferenceEquals(objToCheck, null) == true) return true;
                else return false;
            }
            catch (Exception)
            {
                return true;
            }
        }

        private clsPlayer GetMOMPlayerDetails(string i_PLayerID)
        {
            // Local variable
            clsPlayer objPlayerDetails = null;
            try
            {
                Player objPlayer = (Player)_objMatchDetails.players.Where(x => x.id == i_PLayerID).FirstOrDefault();
                objPlayerDetails = new clsPlayer();
                objPlayerDetails.Id = objPlayer.id;
                objPlayerDetails.Name = objPlayer.name;
                objPlayerDetails.FullName = objPlayer.f_name;
                objPlayerDetails.Speciality = objPlayer.speciality;
                objPlayerDetails.FullProfileLink = "https://www.cricbuzz.com/profiles/" + objPlayer.id;
                objPlayerDetails.Image = "https://www.cricbuzz.com/stats/img/faceImages/" + objPlayer.id + ".jpg";
                return objPlayerDetails;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private clsBatsMan GetBatsmanDetails(int i_BatsmanNo)
        {
            // Local variable
            clsBatsMan objBatsman = null;
            clsPlayer objPlayer = null;
            string playerID = string.Empty;
            try
            {
                playerID = _objMatchDetails.score.batsman[i_BatsmanNo].id;
                //Getting player details
                objPlayer = GetMOMPlayerDetails(playerID);
                objBatsman = new clsBatsMan();
                objBatsman.Id = objPlayer.Id;
                objBatsman.Name = objPlayer.Name;
                objBatsman.FullName = objPlayer.FullName;
                objBatsman.Speciality = objPlayer.Speciality;
                objBatsman.FullProfileLink = objPlayer.FullProfileLink;
                objBatsman.Image = objPlayer.Image;
                objBatsman.Fours = _objMatchDetails.score.batsman[i_BatsmanNo]._4s;
                objBatsman.Sixes = _objMatchDetails.score.batsman[i_BatsmanNo]._6s;
                objBatsman.Balls = _objMatchDetails.score.batsman[i_BatsmanNo].b;
                objBatsman.Runs = _objMatchDetails.score.batsman[i_BatsmanNo].r;
                objBatsman.Strike = _objMatchDetails.score.batsman[i_BatsmanNo].strike;
                if (objBatsman.Strike == "1")
                {
                    objBatsman.Name = objBatsman.Name + "*";
                }
                return objBatsman;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private clsBowler GetBowlerDetails()
        {
            // Local variable
            clsBowler objBowler = null;
            clsPlayer objPlayer = null;
            string playerID = string.Empty;
            try
            {
                playerID = _objMatchDetails.score.bowler[0].id;
                //Getting player details
                objPlayer = GetMOMPlayerDetails(playerID);
                objBowler = new clsBowler();
                objBowler.Id = objPlayer.Id;
                objBowler.Name = objPlayer.Name;
                objBowler.FullName = objPlayer.FullName;
                objBowler.Speciality = objPlayer.Speciality;
                objBowler.FullProfileLink = objPlayer.FullProfileLink;
                objBowler.Image = objPlayer.Image;
                objBowler.Maiden = _objMatchDetails.score.bowler[0].m;
                objBowler.Overs = _objMatchDetails.score.bowler[0].o;
                objBowler.Runs = _objMatchDetails.score.bowler[0].r;
                objBowler.Wickets = _objMatchDetails.score.bowler[0].w;
                return objBowler;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GetBattingTeamName(string i_BattingTeamId)
        {
            try
            {
                if (_objMatchDetails.team1.id == i_BattingTeamId) return _objMatchDetails.team1.s_name;
                else return _objMatchDetails.team2.s_name;
            }

            catch (Exception)
            {
                return string.Empty;
            }
        }

        private string GetLastWicket()
        {
            try
            {
                string strLastOutBatsManDetails = string.Empty;
                clsPlayer objLastBatsMan = GetMOMPlayerDetails(_objMatchDetails.score.last_wkt);
                return objLastBatsMan.Name + " " + _objMatchDetails.score.last_wkt_score;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

       
        #region OnProperty change implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
        #endregion
    }
    public class clsPlayer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string FullProfileLink { get; set; }
        public string Image { get; set; }
        public string Speciality { get; set; }
        public string TotalMatches { get; set; }
        public string TotalRuns { get; set; }
        public string Average { get; set; }
        public string StrikeRate { get; set; }
    }

    public class clsBatsMan : clsPlayer
    {
        public string HighScore { get; set; }
        public string DoubleHundreads { get; set; }
        public string Hundreds { get; set; }
        public string Fifties { get; set; }
        public string Balls { get; set; }
        public string Fours { get; set; }
        public string Sixes { get; set; }
        public string Runs { get; set; }
        public string Strike { get; set; }
    }

    public class clsBowler : clsPlayer
    {
        public string TotalWickets { get; set; }
        public string Economy { get; set; }
        public string Maiden { get; set; }
        public string Overs { get; set; }
        public string Runs { get; set; }
        public string Wickets { get; set; }
        public string FiveWickets { get; set; }
        public string BBM { get; set; }
    }
}