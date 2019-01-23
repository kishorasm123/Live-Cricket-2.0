using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Live_Cricket_2._0.Model;
using Newtonsoft.Json;
using System.Net;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro;
using Live_Cricket_2._0.Views;
using System.Windows.Input;
using System.Windows.Forms;
using System.IO;
using Live_Cricket_2._0.Properties;
using System.Resources;
namespace Live_Cricket_2._0.ViewModels
{
    public class Dashboard_VM : DependencyObject, INotifyPropertyChanged
    {
        #region Properties

        #region Changing color code
        public List<KeyValuePair<string, Color>> Colors
        {
            get { return (List<KeyValuePair<string, Color>>)GetValue(ColorsProperty); }
            set { SetValue(ColorsProperty, value); }
        }
        public static readonly DependencyProperty ColorsProperty = DependencyProperty.Register("Colors",
                                         typeof(List<KeyValuePair<string, Color>>),
                                         typeof(DashBoard),
                                         new PropertyMetadata(default(List<KeyValuePair<string, Color>>)));
        #endregion

        #region OnProperty change implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string info)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
        #endregion

        private enum enumMessageType { NoMatchesForToday, ErrorInFetchingMatchesForToday, WelcomeUser, MatchLoading, NoScoreDetails };
        private ScoreDetails _objScoreDetailsForUI = null;
        private string _strVerticalPinPath = Path.Combine(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "Resources"), "VerticalPin.png");
        private string _strHorizontalPinPath = Path.Combine(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "Resources"), "HorizontalPin.png");
        private CustomMessages objCustomMessage = new CustomMessages();
        private bool _isApplicationLoadedForFirstTime = true;
        private NotifyIcon objNotification = null;
        private DashBoard _objCurrentWindow = null;
        public DashBoard objCurrentWindow
        {
            set
            {
                _objCurrentWindow = value; OnPropertyChanged("objCurrentWindow");
            }
            get { return _objCurrentWindow; }
        }
        private ObservableCollection<clsCricketMatches> _objCollOfMatchDetails = null;
        public ObservableCollection<clsCricketMatches> objCollOfMatchDetails
        {
            set
            {
                if (value == null) DisplayCustomMessages(enumMessageType.ErrorInFetchingMatchesForToday);
                else if ((value != null) && (value.Count == 0)) DisplayCustomMessages(enumMessageType.NoMatchesForToday);
                _objCollOfMatchDetails = value; OnPropertyChanged("objCollOfMatchDetails");
            }
            get { return _objCollOfMatchDetails; }
        }
        private int _intSelectedColorIndex = 0;
        public int intSelectedColorIndex
        {
            set
            {
                _intSelectedColorIndex = value; ColorsSelectorOnSelectionChanged(); OnPropertyChanged("_intSelectedColorIndex");
            }
            get { return _intSelectedColorIndex; }
        }
        private FrameworkElement _objPlayerDetailsUI = null;
        public FrameworkElement objPlayerDetailsUI
        {
            set { _objPlayerDetailsUI = value; OnPropertyChanged("objPlayerDetailsUI"); }
            get { return _objPlayerDetailsUI; }
        }
        private FrameworkElement _objScoreDetailsUI = null;
        public FrameworkElement objScoreDetailsUI
        {
            set { _objScoreDetailsUI = value; OnPropertyChanged("objScoreDetailsUI"); }
            get { return _objScoreDetailsUI; }
        }
        private FrameworkElement _objStartUpMessaage = null;
        public FrameworkElement objStartUpMessaage
        {
            set { _objStartUpMessaage = value; OnPropertyChanged("objStartUpMessaage"); }
            get { return _objStartUpMessaage; }
        }
        private RelayCommand _objSelectedMatch = null;
        public RelayCommand objSelectedMatch
        {
            set { _objSelectedMatch = value; OnPropertyChanged("objSelectedMatch"); }
            get { return _objSelectedMatch; }
        }
        private RelayCommand _objSelectedPlayer = null;
        public RelayCommand objSelectedPlayer
        {
            set { _objSelectedPlayer = value; OnPropertyChanged("objSelectedPlayer"); }
            get { return _objSelectedPlayer; }
        }
        private RelayCommand _cmdAutoHide = null;
        public RelayCommand cmdAutoHide
        {
            set { _cmdAutoHide = value; OnPropertyChanged("cmdAutoHide"); }
            get { return _cmdAutoHide; }
        }
        private clsCricketMatches _objSelectedMatchItem = null;
        public clsCricketMatches objSelectedMatchItem
        {
            set { _objSelectedMatchItem = value; Globals.strLastNotificationBallNo = string.Empty; OnPropertyChanged("objSelectedMatchItem"); }
            get { return _objSelectedMatchItem; }
        }
        private clsMatchDetailsForUI _objMatchDetailsForUI = null;
        public clsMatchDetailsForUI objMatchDetailsForUI
        {
            set
            {
                _objMatchDetailsForUI = value; OnPropertyChanged("objMatchDetailsForUI");
            }
            get { return _objMatchDetailsForUI; }
        }
        private bool _isEnableFlyOut = false;
        public bool IsEnableFlyOut
        {
            set
            {
                _isEnableFlyOut = value; OnPropertyChanged("IsEnableFlyOut");
            }
            get { return _isEnableFlyOut; }
        }
        private bool _isMatchSelected = false;
        public bool isMatchSelected
        {
            set
            {
                _isMatchSelected = value; OnPropertyChanged("isMatchSelected");
            }
            get { return _isMatchSelected; }
        }
        private bool _isNotificationOn = true;
        public bool isNotificationOn
        {
            set
            {
                _isNotificationOn = value; OnPropertyChanged("isNotificationOn");
            }
            get { return _isNotificationOn; }
        }
        private clsPlayer _objSelectedPlayerForUC = null;
        public clsPlayer objSelectedPlayerForUC
        {
            set
            {
                _objSelectedPlayerForUC = value; OnPropertyChanged("objSelectedPlayerForUC");
            }
            get { return _objSelectedPlayerForUC; }
        }
        private string _strTodaysMatchesCustomMessage = null;
        public string StrTodaysMatchesCustomMessage
        {
            set
            {
                _strTodaysMatchesCustomMessage = value; OnPropertyChanged("StrTodaysMatchesCustomMessage");
            }
            get { return _strTodaysMatchesCustomMessage; }
        }
        private bool _isShowStartUpMessage = false;
        public bool isShowStartUpMessage
        {
            set
            {
                _isShowStartUpMessage = value; OnPropertyChanged("isShowStartUpMessage");
            }
            get { return _isShowStartUpMessage; }
        }
        private string _strAutoHidePath = string.Empty;
        public string StrAutoHidePath
        {
            set
            {
                _strAutoHidePath = value; OnPropertyChanged("StrAutoHidePath");
            }
            get { return _strAutoHidePath; }
        }
        #endregion

        #region Methods
        public Dashboard_VM(DashBoard i_objDashboard)
        {
            clsCrickBuzzData.GetCrickbuzzIstance();
            objCurrentWindow = i_objDashboard;
            SetColorToDashBoard(i_objDashboard);
            objNotification = new NotifyIcon();
            _objScoreDetailsForUI = new ScoreDetails();
            objPlayerDetailsUI = new PlayerDetails();
            objStartUpMessaage = objCustomMessage;
            StrAutoHidePath = _strHorizontalPinPath;
            DisplayCustomMessages(enumMessageType.WelcomeUser);
            TasksToRunOnBackGroundRecursively();
            objSelectedMatch = new RelayCommand(GetSelectedMatchScore, DefaultCanExecute);
            objSelectedPlayer = new RelayCommand(GetSelectedPlayerDetails, DefaultCanExecute);
            cmdAutoHide = new RelayCommand(SetAutoHide, DefaultCanExecute);
        }
        private void SetAutoHide(object parameter = null)
        {
            if (StrAutoHidePath == _strVerticalPinPath)
            {
                StrAutoHidePath = _strHorizontalPinPath;
                objCurrentWindow.Topmost = false;
            }
            else
            {
                StrAutoHidePath = _strVerticalPinPath;
                objCurrentWindow.Topmost = true;
            }
        }
        private void GetSelectedMatchScore(object parameter = null)
        {
            // Local variables
            clsMatchDetails objMatchDetails = null;
            try
            {
                if (parameter == null)
                {
                    //objMatchDetailsForUI = null;
                    isMatchSelected = true;
                    DisplayCustomMessages(enumMessageType.MatchLoading);
                }
                else
                {
                    objMatchDetails = clsCrickBuzzData.GetMatchDetails(objSelectedMatchItem.objMatchHyperLink);
                    objMatchDetailsForUI = new clsMatchDetailsForUI(objMatchDetails);
                    string strNotificationTitle = string.Empty;
                    string strNotificationText = string.Empty;
                    clsCrickBuzzData.GetNotification(objMatchDetails, out strNotificationTitle, out strNotificationText);
                    if (strNotificationTitle != string.Empty && isNotificationOn) DisplayNotification(strNotificationTitle, strNotificationText);
                }
            }
            catch (Exception)
            {
            }
        }
        private bool DefaultCanExecute(object parameter = null)
        {
            {
                return true;
            }
        }
        private void SetColorToDashBoard(DashBoard i_objDashboard)
        {
            try
            {
                // Adding colors to combobox
                Colors = typeof(Colors)
               .GetProperties()
               .Where(prop => typeof(Color).IsAssignableFrom(prop.PropertyType))
               .Select(prop => new KeyValuePair<String, Color>(prop.Name, (Color)prop.GetValue(null)))
               .ToList();
                var theme = ThemeManager.DetectAppStyle(System.Windows.Application.Current);
                ThemeManager.ChangeAppStyle(i_objDashboard, theme.Item2, theme.Item1);
                if (Properties.Settings.Default.intSelectedColorIndex != 0)
                {
                    intSelectedColorIndex = Properties.Settings.Default.intSelectedColorIndex;
                }
                // Adding colors to combobox
            }
            catch (Exception)
            {
            }
        }
        private void GetSelectedPlayerDetails(object parameter = null)
        {
            try
            {
                System.Threading.ThreadPool.QueueUserWorkItem(GetSelectedPlayerFullDetails, (parameter));
                IsEnableFlyOut = true;
            }
            catch (Exception)
            {
            }
        }
        private void GetSelectedPlayerFullDetails(object parameter = null)
        {
            // Local variables
            clsPlayer objSelectedPlayer = null;
            Globals.enumMatchType objEnumMatchType = Globals.enumMatchType.ODI;
            Globals.enumCarrier objEnumCarrier = Globals.enumCarrier.Batting;
            try
            {
                if ((string)parameter == "Batsman1") { objSelectedPlayer = objMatchDetailsForUI.Batsman1; objEnumCarrier = Globals.enumCarrier.Batting; } //objBatsmanCarrierVisibility = Visibility.Visible; }
                else if ((string)parameter == "Batsman2") { objSelectedPlayer = objMatchDetailsForUI.Batsman2; objEnumCarrier = Globals.enumCarrier.Batting; }// objBatsmanCarrierVisibility = Visibility.Visible; }
                else if ((string)parameter == "Bowler") { objSelectedPlayer = objMatchDetailsForUI.Bowler; objEnumCarrier = Globals.enumCarrier.Bowling; } //objBatsmanCarrierVisibility = Visibility.Hidden; }
                if (objMatchDetailsForUI.MatchType == Globals.enumMatchType.ODI.ToString().ToUpper()) objEnumMatchType = Globals.enumMatchType.ODI;
                else if (objMatchDetailsForUI.MatchType == Globals.enumMatchType.Test.ToString().ToUpper()) objEnumMatchType = Globals.enumMatchType.Test;
                else if (objMatchDetailsForUI.MatchType == Globals.enumMatchType.T20I.ToString().ToUpper()) objEnumMatchType = Globals.enumMatchType.T20I;
                else if (objMatchDetailsForUI.MatchType == Globals.enumMatchType.IPL.ToString().ToUpper()) objEnumMatchType = Globals.enumMatchType.IPL;
                objSelectedPlayerForUC = objSelectedPlayer;
                objSelectedPlayerForUC = clsCrickBuzzData.GetPlayerCarrierDetails(objSelectedPlayer, objEnumCarrier, objEnumMatchType);
                //IsEnableFlyOut = true;
            }
            catch (Exception)
            {
            }
        }
        private void GetAllMatches()
        {
            try
            {
                clsCrickBuzzData.GetCrickbuzzIstance();
                // If application is loaded for first time then, adding match deatils to collection
                if (_isApplicationLoadedForFirstTime)
                {
                    List<clsCricketMatches> objDicOfMatches = clsCrickBuzzData.GetLiveMatches();
                    _isApplicationLoadedForFirstTime = false;
                    objCollOfMatchDetails = new ObservableCollection<clsCricketMatches>(objDicOfMatches);
                }
                else
                {
                    System.Threading.Thread.Sleep(500);
                    List<clsCricketMatches> objDicOfMatches = clsCrickBuzzData.GetLiveMatches();
                    // Comparing whether old list of matches is equal to newly obtained list of matches
                    var difList = objCollOfMatchDetails.Where(a => !new ObservableCollection<clsCricketMatches>(objDicOfMatches).Any(a1 => a1.objMatchHyperLink == a.objMatchHyperLink))
                    .Union(new ObservableCollection<clsCricketMatches>(objDicOfMatches).Where(a => !objCollOfMatchDetails.Any(a1 => a1.objMatchName == a.objMatchName)));
                    if (difList.FirstOrDefault() != null && objDicOfMatches != null) objCollOfMatchDetails = new ObservableCollection<clsCricketMatches>(objDicOfMatches);
                }
                isShowStartUpMessage = false;
            }
            catch (Exception)
            {
                objCollOfMatchDetails = null;
            }
        }
        private async void TasksToRunOnBackGroundRecursively()
        {
            try
            {
                // System.Threading.ThreadPool.QueueUserWorkItem(GetSelectedMatchScore, false);
                await Task.Run(new Action(GetAllMatches));
                if (_objCollOfMatchDetails == null) DisplayCustomMessages(enumMessageType.ErrorInFetchingMatchesForToday);
                else if (_objCollOfMatchDetails.Count == 0) DisplayCustomMessages(enumMessageType.NoMatchesForToday);
                else
                {
                    // System.Threading.ThreadPool.QueueUserWorkItem(GetSelectedMatchScore, false);
                    await Task.Run(() => GetSelectedMatchScore(false));
                    // Modifying Score UI
                    if (_objMatchDetailsForUI == null) DisplayCustomMessages(enumMessageType.NoScoreDetails);
                    else
                    {
                        if (_objMatchDetailsForUI.TeamScore != null) objScoreDetailsUI = _objScoreDetailsForUI;
                        else
                        {
                            if (DateTime.Now < FromUnixTime(long.Parse(_objMatchDetailsForUI.StartTime))) DisplayCustomMessages(enumMessageType.NoScoreDetails, "Match Starts at " + FromUnixTime(long.Parse(objMatchDetailsForUI.StartTime)).ToString("MMM d, hh:mm tt"));
                            else DisplayCustomMessages(enumMessageType.NoScoreDetails);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                TasksToRunOnBackGroundRecursively();
            }
        }
        private void DisplayNotification(string i_strTitle, string i_strText)
        {
            try
            {
                string strToolRunningPath = AppDomain.CurrentDomain.BaseDirectory + "Resources\\icoCricketLogo.ico";
                objNotification.Icon = new System.Drawing.Icon(strToolRunningPath);
                objNotification.BalloonTipIcon = ToolTipIcon.Info;
                objNotification.BalloonTipTitle = i_strTitle;
                objNotification.BalloonTipText = i_strText;
                objNotification.Visible = true;
                objNotification.ShowBalloonTip(10000);
                // objNotification.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
        private void DisplayCustomMessages(enumMessageType i_enumMessageType, string i_strCustomMessages = "")
        {
            try
            {
                if (i_enumMessageType == enumMessageType.NoScoreDetails)
                {
                    objScoreDetailsUI = objCustomMessage;
                    StrTodaysMatchesCustomMessage = "No ScoreCard Found " + System.Environment.NewLine + i_strCustomMessages;
                }
                else if (i_enumMessageType == enumMessageType.ErrorInFetchingMatchesForToday)
                {
                    isShowStartUpMessage = true;
                    StrTodaysMatchesCustomMessage = "No/Poor Internet Connection " + System.Environment.NewLine + i_strCustomMessages;
                }
                else if (i_enumMessageType == enumMessageType.NoMatchesForToday)
                {
                    isShowStartUpMessage = true;
                    StrTodaysMatchesCustomMessage = "No matches for today" + System.Environment.NewLine + i_strCustomMessages;
                }
                else if (i_enumMessageType == enumMessageType.WelcomeUser)
                {
                    isShowStartUpMessage = true;
                    StrTodaysMatchesCustomMessage = "Welcome " + System.Environment.UserName + " !" + System.Environment.NewLine + i_strCustomMessages;
                }
                else if (i_enumMessageType == enumMessageType.MatchLoading)
                {
                   objScoreDetailsUI = objCustomMessage;
                   StrTodaysMatchesCustomMessage = "Loading the scorecard" + i_strCustomMessages;
                }
            }
            catch (Exception)
            {
            }
        }
        public static DateTime FromUnixTime(long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();
        private void ColorsSelectorOnSelectionChanged()
        {
            var selectedColor = Colors.ElementAt(intSelectedColorIndex) as KeyValuePair<string, Color>?;
            if (selectedColor.HasValue)
            {
                MahAppsMetroThemesSample.ThemeManagerHelper.CreateAppStyleBy(selectedColor.Value.Value, true);
                //Saving color to settings variable
                Properties.Settings.Default.intSelectedColorIndex = intSelectedColorIndex;
                Properties.Settings.Default.Save();
                //Saving color to settings variable
            }
        }
        #endregion
    }
}