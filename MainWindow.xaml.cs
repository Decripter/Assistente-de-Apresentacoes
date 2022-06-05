using System;
using System.Diagnostics;

using System.ComponentModel;
using System.Net.Http;
using System.Net;
using System.Threading;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfScreenHelper.Enum;


namespace Assistente_de_Apresentações
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Boolean isPlayng = false;

        FullscreenWindow fullscreenWindow = new FullscreenWindow();
        public String newVersionUrl = "https://github.com/Decripter/Assistente-de-Apresentacoes/blob/master/Deploy/Assistente%20de%20Apresenta%C3%A7%C3%B5es%201.1.zip?raw=true";





    public MainWindow()
        {
            InitializeComponent();
        }
        public void Select_Video()
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".mp4"; // Default file extension
            dlg.Filter = "Vídeos (.mp4)|*.mp4"; // Filter files by extension
            

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                fullscreenWindow.Play_Video(filename);
            }
        }

        public String Select_Image()
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".jpg"; // Default file extension
            dlg.Filter = "Imagens (.jpg)|*.jpg"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
               
                return filename;
            }
            else { return " "; }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetTelas();

            //Select_Video();

            
            GetUpdate();



            if (Screen.AllScreens.Length > 1)
            {

            fullscreenWindow.Left = Screen.AllScreens[1].WorkingArea.Left;
            fullscreenWindow.Top = Screen.AllScreens[1].WorkingArea.Top; 
            fullscreenWindow.Show();
            fullscreenWindow.WindowState = WindowState.Maximized;
            }
            
            
        }
        async void GetUpdate()
        {
              HttpClient client = new HttpClient();

            try
            {

                HttpResponseMessage response = await client.GetAsync(newVersionUrl);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                AtualizacaoDisponivel.Visibility = Visibility.Visible;


            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                
            }
        }
       
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            fullscreenWindow.Close();

        }

        private void Select_View_Image(Image imageBox)
        {
            BitmapImage tmpBm = new BitmapImage();
            tmpBm.BeginInit();
            tmpBm.UriSource = new Uri(Select_Image(), UriKind.Relative);
            tmpBm.EndInit();
            imageBox.Stretch = Stretch.Fill;
            imageBox.Source = tmpBm;
            //TODO trocar os imagebox por MediaElement
            //TODO deixar apenas uma guia para imagens e videos
            fullscreenWindow.Play_Video(tmpBm.UriSource.ToString());
            
        }

        private void Midia_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Select_Image();
        }

        private void PlayPause(object sender, RoutedEventArgs e)
        {
            if (isPlayng)
            {
                fullscreenWindow.videoPlayer.LoadedBehavior = MediaState.Pause;
                isPlayng = !isPlayng;
                PlayPauseButton.Content = "Play";
            }
            else
            {
                fullscreenWindow.videoPlayer.LoadedBehavior = MediaState.Play;
                isPlayng = !isPlayng;

                PlayPauseButton.Content = "Pause";
            }
        }

        private void Stop(object sender, RoutedEventArgs e)
        {   
            fullscreenWindow.videoPlayer.LoadedBehavior = MediaState.Stop;
            fullscreenWindow.videoPlayer.LoadedBehavior = MediaState.Play;
            Thread.Sleep(100);

            fullscreenWindow.videoPlayer.LoadedBehavior = MediaState.Stop;

            isPlayng = false;
            PlayPauseButton.Content = "Play";
        }

        private void AtualizarListaTelas_Click(object sender, RoutedEventArgs e)
        {
            GetTelas();
        }
        private void GetTelas()
        {
            selectMonitors.Items.Clear();
            for (int i = 0; i < Screen.AllScreens.Length; i++)
            {
                String name = Screen.AllScreens[i].DeviceName;
                name = name.Substring(4,name.Length-4);
                selectMonitors.Items.Add(name);
            }
            
        }

        private void selectMonitors_Selected(object sender, RoutedEventArgs e)
        {
        }

        private void selectMonitors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index  = selectMonitors.SelectedIndex;
            if(index > 1)
            {

                fullscreenWindow.Hide();
            fullscreenWindow.Left = 0;
            fullscreenWindow.Top = 0;
            fullscreenWindow.Left = Screen.AllScreens[index].WorkingArea.Left;
            fullscreenWindow.Top = Screen.AllScreens[index].WorkingArea.Top;
            fullscreenWindow.Show();
            fullscreenWindow.WindowState = WindowState.Maximized;
            }

        }
        private void Element_MediaOpened(object sender, EventArgs e)
        {
            timelineSlider.Maximum = fullscreenWindow.videoPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
        }

        private void MediaTimeChanged(object sender, EventArgs e)
        {
            timelineSlider.Value = fullscreenWindow.videoPlayer.Position.TotalMilliseconds;
        }

        private void atualizacaoDisponivel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("cmd", "/c start "+ newVersionUrl);

        }
    }
}
