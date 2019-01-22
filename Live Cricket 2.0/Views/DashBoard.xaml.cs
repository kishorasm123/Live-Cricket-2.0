using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro;
using MahApps.Metro.Controls;

namespace Live_Cricket_2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DashBoard : MetroWindow
    {
        public DashBoard()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.Dashboard_VM(this);
        }

        private void ColorsSelectorOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedColor = this.ColorsSelector.SelectedItem as KeyValuePair<string, Color>?;
            if (selectedColor.HasValue)
            {
                var theme = MahApps.Metro.ThemeManager.DetectAppStyle(Application.Current);
                MahAppsMetroThemesSample.ThemeManagerHelper.CreateAppStyleBy(selectedColor.Value.Value, true);

                //Saving color to settings variable
                Properties.Settings.Default.intSelectedColorIndex = this.ColorsSelector.SelectedIndex;
                Properties.Settings.Default.Save();
                //Saving color to settings variable
            }
            Application.Current.MainWindow.Activate();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

       
    }
}
