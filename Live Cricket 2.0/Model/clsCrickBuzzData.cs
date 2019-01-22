using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using HtmlAgilityPack;
using System.Data;
using System.Text.RegularExpressions;

namespace Live_Cricket_2._0.Model
{
    internal sealed class clsCrickBuzzData
    {
        private static clsCrickBuzzData _objCrickBuzz = null;
        private static string strLiveScoreLink = string.Empty;
        private enum DatableSelection { Batting, Bowling };

        private clsCrickBuzzData()
        {
            strLiveScoreLink = "https://www.cricbuzz.com/cricket-match/live-scores";
        }

        public static clsCrickBuzzData GetCrickbuzzIstance()
        {
            try
            {
                if (_objCrickBuzz == null)
                {
                    _objCrickBuzz = new clsCrickBuzzData();
                }
                return _objCrickBuzz;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<clsCricketMatches> GetLiveMatches()
        {
            //Local variables
            List<clsCricketMatches> objCricketMatches = new List<clsCricketMatches>();
            try
            {
                //return null;

                HtmlWeb objHtmlWeb = new HtmlWeb();
                HtmlDocument objHtmlDoc = objHtmlWeb.Load("https://www.cricbuzz.com/cricket-match/live-scores");
                HtmlNodeCollection objHtmlClassNodes = objHtmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'cb-col cb-col-100 cb-lv-main')]");
                List<string> objHtmlHrefNodes = objHtmlClassNodes.Select(x => x.SelectSingleNode(".//a").Attributes["href"].Value).ToList();
                var lstOfMatchNoSplits = objHtmlHrefNodes.Select(x => x.Split('/'));
                List<string> lstOfMatchNos = lstOfMatchNoSplits.Select(x => "https://www.cricbuzz.com/match-api/" + x[2] + "/commentary.json").ToList();
                List<string> objMatchTitles = objHtmlClassNodes.Select(x => x.SelectSingleNode(".//a").Attributes["title"].Value).ToList();
                objCricketMatches = objMatchTitles.Zip(lstOfMatchNos, (strMatchName, strMatchLink) => new clsCricketMatches { objMatchName = strMatchName, objMatchHyperLink = strMatchLink }).ToList();

                //Dictionary<string, string> objDicOfMatchDetails = objMatchTitles.Zip(lstOfMatchNos, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v);
                //return objDicOfMatchDetails;
                return objCricketMatches;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static clsMatchDetails GetMatchDetails(string strMatchLink)
        {
            try
            {
                using (WebClient objWebClient = new WebClient())
                {
                    var JSONMatchDetails = objWebClient.DownloadString(strMatchLink);
                    clsMatchDetails objMatchDetails = JsonConvert.DeserializeObject<clsMatchDetails>(JSONMatchDetails);
                    return objMatchDetails;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static clsPlayer GetPlayerCarrierDetails(dynamic objPlayer, Globals.enumCarrier i_enumCarrier, Globals.enumMatchType i_enumMatchType)
        {
            try
            {
                HtmlWeb objHtmlWeb = new HtmlWeb();
                HtmlDocument objHtmlDoc = objHtmlWeb.Load(objPlayer.FullProfileLink);
                HtmlNodeCollection objHtmlClassNodes = objHtmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'cb-plyr-tbl')]");

                if (i_enumCarrier == Globals.enumCarrier.Batting)
                {
                    DataTable objBattingCarrier = GetPlayerCarrierTable(objHtmlClassNodes, DatableSelection.Batting);
                    if (objBattingCarrier != null)
                    {
                        DataRow objCarrierRow = objBattingCarrier.Select("Column1 = '" + i_enumMatchType.ToString() + "'").FirstOrDefault();
                        if (objCarrierRow != null)
                        {
                            objPlayer.TotalMatches = objCarrierRow["M"].ToString();
                            objPlayer.TotalRuns = objCarrierRow["Runs"].ToString();
                            objPlayer.Average = objCarrierRow["Avg"].ToString();
                            objPlayer.HighScore = objCarrierRow["HS"].ToString();
                            objPlayer.StrikeRate = objCarrierRow["SR"].ToString();
                            objPlayer.Hundreds = objCarrierRow["100"].ToString();
                            objPlayer.DoubleHundreads = objCarrierRow["200"].ToString();
                            objPlayer.Fifties = objCarrierRow["50"].ToString();
                            objPlayer.Fours = objCarrierRow["4s"].ToString();
                            objPlayer.Sixes = objCarrierRow["6s"].ToString();
                        }
                    }
                }

                else if (i_enumCarrier == Globals.enumCarrier.Bowling)
                {
                    DataTable objBowlingCarrier = GetPlayerCarrierTable(objHtmlClassNodes, DatableSelection.Bowling);
                    if (objBowlingCarrier != null)
                    {
                        DataRow objCarrierRow = objBowlingCarrier.Select("Column1 = '" + i_enumMatchType.ToString() + "'").FirstOrDefault();
                        if (objCarrierRow != null)
                        {
                            objPlayer.TotalMatches = objCarrierRow["M"].ToString();
                            objPlayer.TotalWickets = objCarrierRow["Wkts"].ToString();
                            objPlayer.TotalRuns = objCarrierRow["Runs"].ToString();
                            objPlayer.Average = objCarrierRow["Avg"].ToString();
                            objPlayer.Economy = objCarrierRow["Econ"].ToString();
                            objPlayer.FiveWickets = objCarrierRow["5W"].ToString();
                            objPlayer.BBM = objCarrierRow["BBM"].ToString();
                            objPlayer.StrikeRate = objCarrierRow["SR"].ToString();
                        }
                    }
                }
                return objPlayer;
            }
            catch (Exception ex)
            {
                return objPlayer;
            }
        }

        private static DataTable GetPlayerCarrierTable(HtmlNodeCollection i_objHtmlTableClassNodes, DatableSelection i_DataTableRequest)
        {
            // Local variables
            DataTable objDataTable = null;
            HtmlNode objHtmlNode = null;

            try
            {
                if (i_DataTableRequest == DatableSelection.Batting)
                {
                    objHtmlNode = i_objHtmlTableClassNodes.Where(x => x.InnerText.Contains("Batting Career Summary")).First().SelectNodes(".//table").First();
                }
                else if (i_DataTableRequest == DatableSelection.Bowling)
                {
                    objHtmlNode = i_objHtmlTableClassNodes.Where(x => x.InnerText.Contains("Bowling Career Summary")).First().SelectNodes(".//table").First();
                }

                objDataTable = new DataTable();

                // Creating columns to datatable
                var headers = objHtmlNode.SelectNodes(".//tr").First().SelectNodes(".//th").Select(x => x.InnerText);
                foreach (var header in headers) objDataTable.Columns.Add(header);

                // Adding values to datatable
                var rows = objHtmlNode.SelectNodes(".//tr").Skip(1).Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToArray());
                foreach (var row in rows) objDataTable.Rows.Add(row);

                return objDataTable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static void GetNotification(clsMatchDetails objMatchDetails, out string o_strTitle, out string o_strText)
        {
            try
            {
                // Local variables
                string[] arrEvents = { "four", "six", "wicket", "out", "fifty", "hundred" };
                o_strTitle = string.Empty;
                o_strText = string.Empty;

                if (objMatchDetails.comm_lines != null)
                {
                    if (arrEvents.Contains(objMatchDetails.comm_lines[0].evt) && Globals.strLastNotificationBallNo != objMatchDetails.comm_lines[0].b_no)
                    {
                        Globals.strLastNotificationBallNo = objMatchDetails.comm_lines[0].b_no;
                        o_strTitle = UppercaseFirst(objMatchDetails.comm_lines[0].evt);
                        o_strText = StripHTML(objMatchDetails.comm_lines[0].comm);
                    }
                }
            }
            catch (Exception)
            {
                o_strTitle = string.Empty;
                o_strText = string.Empty;
            }
        }

        public static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
    }
}
