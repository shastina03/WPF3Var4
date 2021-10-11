using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Praktikum_3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, string> options = new Dictionary<string, string>
        {
            { "SystemTime", "now" },
            { "TokyoTime", "35.5062647,138.6458125" },
            { "ChicagoTime", "41.875732, -87.623766" },
            { "LondonTime", "51.528308,-0.3817765" }
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private dynamic getDataJson(string location)
        {
            WebClient client = new WebClient() { Encoding = System.Text.Encoding.UTF8 };
            long unixTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
            string urlMain = String.Format("https://maps.googleapis.com/maps/api/timezone/json?location={0}&timestamp={1}&key={2}", 
                location,
                unixTime,
                "AIzaSyDZXNyu8SBDGP8sTHAduhTYFidcsPjwHqs"
                );
            string text = client.DownloadString(urlMain);
            return JsonConvert.DeserializeObject(text);
        }

        private string getTime(string location)
        {
            dynamic obj = getDataJson(location);
            int dstOffset = obj["dstOffset"];
            int rawOffset = obj["rawOffset"];
            DateTime date = DateTime.Now.AddSeconds(-10800);
            DateTime currentDate = date.AddSeconds(dstOffset + rawOffset);
            return currentDate.ToString();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton pressed = (RadioButton)sender;
            TextBlock result = TextBlockResult;

            result.Text = options[pressed.Name] == "now" 
                ? DateTime.Now.ToString() 
                : getTime(options[pressed.Name]);

            TextBlockResult.Visibility = Visibility.Visible;
        }
    }
}
