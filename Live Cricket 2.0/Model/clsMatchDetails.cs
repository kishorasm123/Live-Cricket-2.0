using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Cricket_2._0.Model
{

    public class clsMatchDetails
    {
        public Settings settings { get; set; }
        public string id { get; set; }
        public Series series { get; set; }
        public string start_time { get; set; }
        public string timeForNextDay { get; set; }
        public string end_time { get; set; }
        public string exp_end_time { get; set; }
        public string state { get; set; }
        public bool dn { get; set; }
        public int winning_team_id { get; set; }
        public string winningmargin { get; set; }
        public string match_desc { get; set; }
        public int[] mom { get; set; }
        public string type { get; set; }
        public bool live_coverage { get; set; }
        public bool minor_series { get; set; }
        public string state_title { get; set; }
        public string status { get; set; }
        public Venue venue { get; set; }
        public Score score { get; set; }
        public string hys { get; set; }
        public Toss toss { get; set; }
        public Team1 team1 { get; set; }
        public Team2 team2 { get; set; }
        public Official official { get; set; }
        public Player[] players { get; set; }
        public string last_update_time { get; set; }
        public Comm_Lines[] comm_lines { get; set; }
    }

    public class Settings
    {
        public int refresh { get; set; }
        public int timeout { get; set; }
        public object[] pull_geo { get; set; }
        public bool force_refresh { get; set; }
        public Video_Geo video_geo { get; set; }
        public bool video_enabled { get; set; }
        public string mode { get; set; }
    }

    public class Video_Geo
    {
        public string[] country { get; set; }
    }

    public class Series
    {
        public string id { get; set; }
        public string name { get; set; }
        public string short_name { get; set; }
        public string type { get; set; }
        public bool tour { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string category { get; set; }
    }

    public class Venue
    {
        public string id { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string location { get; set; }
        public string timezone { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }

    public class Score
    {
        public Over_Summary over_summary { get; set; }
        public string prev_overs { get; set; }
        public Batting batting { get; set; }
        public Bowling bowling { get; set; }
        public string max_overs { get; set; }
        public string crr { get; set; }
        public string overs_left { get; set; }
        public string target { get; set; }
        public string prtshp { get; set; }
        public string last_wkt { get; set; }
        public string last_wkt_score { get; set; }
        public Batsman[] batsman { get; set; }
        public Bowler[] bowler { get; set; }
    }

    public class Over_Summary
    {
        public string over { get; set; }
        public string ball_def { get; set; }
        public string rem_over { get; set; }
        public string runs { get; set; }
        public string wickets { get; set; }
        public string fours { get; set; }
        public string sixes { get; set; }
    }

    public class Batting
    {
        public string id { get; set; }
        public string score { get; set; }
        public Innings[] innings { get; set; }
    }

    public class Innings
    {
        public string id { get; set; }
        public string score { get; set; }
        public string wkts { get; set; }
        public string overs { get; set; }
    }

    public class Bowling
    {
        public string id { get; set; }
        public string score { get; set; }
        public Innings1[] innings { get; set; }
    }

    public class Innings1
    {
        public string id { get; set; }
        public string score { get; set; }
        public string wkts { get; set; }
        public string overs { get; set; }
    }

    public class Batsman
    {
        public string id { get; set; }
        public string strike { get; set; }
        public string r { get; set; }
        public string b { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "4s")] // Because JSON property name is '4s' but our CLR property name cannot start with number, so mentioned explicitly
        public string _4s { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "6s")] // Because JSON property name is '6s' but our CLR property name cannot start with number, so mentioned explicitly
        public string _6s { get; set; }
    }

    public class Bowler
    {
        public string id { get; set; }
        public string o { get; set; }
        public string m { get; set; }
        public string r { get; set; }
        public string w { get; set; }
    }

    public class Toss
    {
        public string winner { get; set; }
        public string decision { get; set; }
    }

    public class Team1
    {
        public string id { get; set; }
        public string name { get; set; }
        public string s_name { get; set; }
        public string round_flag { get; set; }
        public string square_flag { get; set; }
        public int[] squad { get; set; }
        public int[] squad_bench { get; set; }
    }

    public class Team2
    {
        public string id { get; set; }
        public string name { get; set; }
        public string s_name { get; set; }
        public string round_flag { get; set; }
        public string square_flag { get; set; }
        public int[] squad { get; set; }
        public int[] squad_bench { get; set; }
    }

    public class Official
    {
        public Umpire1 umpire1 { get; set; }
        public Umpire2 umpire2 { get; set; }
        public Umpire3 umpire3 { get; set; }
        public Referee referee { get; set; }
    }

    public class Umpire1
    {
        public string id { get; set; }
        public string name { get; set; }
        public string country { get; set; }
    }

    public class Umpire2
    {
        public string id { get; set; }
        public string name { get; set; }
        public string country { get; set; }
    }

    public class Umpire3
    {
        public string id { get; set; }
        public string name { get; set; }
        public string country { get; set; }
    }

    public class Referee
    {
        public string id { get; set; }
        public string name { get; set; }
        public string country { get; set; }
    }

    public class Player
    {
        public string id { get; set; }
        public string f_name { get; set; }
        public string name { get; set; }
        public string bat_style { get; set; }
        public string speciality { get; set; }
        public string image { get; set; }
        public string bowl_style { get; set; }
        public string role { get; set; }
    }

    public class Comm_Lines
    {
        public string timestamp { get; set; }
        public string i_id { get; set; }
        public string evt { get; set; }
        public string[] geo { get; set; }
        public string videoId { get; set; }
        public string cbvideoId { get; set; }
        public string comm { get; set; }
        public string o_no { get; set; }
        public string b_no { get; set; }
        public string score { get; set; }
        public string wkts { get; set; }
        public Batsman1[] batsman { get; set; }
        public Bowler1[] bowler { get; set; }
        public string[] all_evt { get; set; }
        public string bt_tm_name { get; set; }
        public string o_summary { get; set; }
        public string o_runs { get; set; }
    }

    public class Batsman1
    {
        public string id { get; set; }
        public string strike { get; set; }
        public string _4s { get; set; }
        public string _6s { get; set; }
        public string r { get; set; }
        public string b { get; set; }
    }

    public class Bowler1
    {
        public string id { get; set; }
        public string o { get; set; }
        public string m { get; set; }
        public string r { get; set; }
        public string w { get; set; }
        public string n { get; set; }
        public string wd { get; set; }
    }
    
}
