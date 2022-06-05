
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

        FullscreenWindow fullscreenWindow = new();
        public String newVersionUrl = "https://github.com/Decripter/Assistente-de-Apresentacoes/blob/master/Deploy/Assistente%20de%20Apresenta%C3%A7%C3%B5es%201.5.zip?raw=true";
        readonly String defaultMedia = "media.png";



        public MainWindow()
        {
            InitializeComponent();
        }
        public String Select_Video()
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new();
            dlg.DefaultExt = ".mp4"; // Default file extension
            dlg.Filter = "Vídeos|*.mp4;*.avi;*.wmv;*.webm;"; // Filter files by extension


            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;

                return filename;
            }
            else { return ""; }
        }
        
        public String Select_Image()
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new();
            dlg.DefaultExt = ".jpg"; // Default file extension
            dlg.Filter = "Imagens|*.jpg;*.jpeg;*.png;*.webp;*.bmp;*.gif"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;

                return filename;
            }
            else { return ""; }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetTelas();
            ChangeBorderColors();

            //fullscreenWindow.Show();

            mediaElement1.Source = new Uri(defaultMedia, UriKind.Relative);
            mediaElement2.Source = new Uri(defaultMedia, UriKind.Relative);
            mediaElement3.Source = new Uri(defaultMedia, UriKind.Relative);
            mediaElement4.Source = new Uri(defaultMedia, UriKind.Relative);
            mediaElement5.Source = new Uri(defaultMedia, UriKind.Relative);
            mediaElement6.Source = new Uri(defaultMedia, UriKind.Relative);
            mediaElement7.Source = new Uri(defaultMedia, UriKind.Relative);
            mediaElement8.Source = new Uri(defaultMedia, UriKind.Relative);

            videoElement1.Source = new Uri(defaultMedia, UriKind.Relative);
            videoElement2.Source = new Uri(defaultMedia, UriKind.Relative);
            videoElement3.Source = new Uri(defaultMedia, UriKind.Relative);
            videoElement4.Source = new Uri(defaultMedia, UriKind.Relative);
            videoElement5.Source = new Uri(defaultMedia, UriKind.Relative);
            videoElement6.Source = new Uri(defaultMedia, UriKind.Relative);
            videoElement7.Source = new Uri(defaultMedia, UriKind.Relative);
            videoElement8.Source = new Uri(defaultMedia, UriKind.Relative);
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

       

        private void Midia_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Select_Image();
        }

        private void PlayPause(object sender, RoutedEventArgs e)
        {
            if (isPlayng )
            {
                fullscreenWindow.videoPlayer.LoadedBehavior = MediaState.Pause;
                isPlayng = !isPlayng;
                PlayPauseButton.Content = "Play";
            }
            if(!isPlayng & fullscreenWindow.videoPlayer.Source != null)
            {
                fullscreenWindow.Focus();
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
            SelectMonitors.Items.Clear();
            if(Screen.AllScreens.Length > 1)
            {
                for (int i = 1; i < Screen.AllScreens.Length; i++)
                {
                    String name = Screen.AllScreens[i].DeviceName;
                    name = name.Substring(4, name.Length - 4);
                    SelectMonitors.Items.Add(name);
                }
                SelectMonitors.SelectedIndex = 0;

            }
            

        }

       

        private void SelectMonitors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = SelectMonitors.SelectedIndex;
            if (index > 0)
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
       
        

        private void atualizacaoDisponivel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("cmd", "/c start " + newVersionUrl);

        }

        
        private void MediaElementImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MediaElement media = (MediaElement)sender;

            if (ExibirEditarCheckbox.IsChecked == true & media.Source != new Uri(defaultMedia, UriKind.Relative))
            {
                fullscreenWindow.videoPlayer.Source = media.Source;
                fullscreenWindow.Focus();
                fullscreenWindow.videoPlayer.LoadedBehavior = MediaState.Play;

            }
            if (ExibirEditarCheckbox.IsChecked == false)
            {
                SetImages(media);

            }
        }
        
        private void SetImages(MediaElement mediaElement)
        {

            var media = Select_Image();
            if (media == "")
            {
                media = defaultMedia;
            }
            else
            {
                mediaElement.Source = new Uri(media, UriKind.Relative);
            }
            
        }
        private void videoElement_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MediaElement media = (MediaElement)sender;

            if (ExibirEditarCheckbox.IsChecked == true & media.Source != new Uri(defaultMedia, UriKind.Relative))
            {   
                fullscreenWindow.videoPlayer.LoadedBehavior = MediaState.Stop;
                fullscreenWindow.videoPlayer.Source = media.Source;
                AnimarMiniaturaVideos(media);
            }
            if (ExibirEditarCheckbox.IsChecked == false)
            {
                SetVideos(media);
                media.LoadedBehavior = MediaState.Stop;

            }
        }
        
        private void SetVideos(MediaElement mediaElement)
        {

            var media = Select_Video();
            if (media == "")
            {
                media = defaultMedia;

            }
            else
            {
                mediaElement.Source = new Uri(media, UriKind.Relative);
            }

        }

        private void ExibirEditarCheckbox_Click(object sender, RoutedEventArgs e)
        {
           ChangeBorderColors();
        }
        private void ChangeBorderColors()
        {
            if(ExibirEditarCheckbox.IsChecked == true)
            {

                BorderImg1.Background = new SolidColorBrush(Colors.Red);
                BorderImg2.Background = new SolidColorBrush(Colors.Red);
                BorderImg3.Background = new SolidColorBrush(Colors.Red);
                BorderImg4.Background = new SolidColorBrush(Colors.Red);
                BorderImg5.Background = new SolidColorBrush(Colors.Red);
                BorderImg6.Background = new SolidColorBrush(Colors.Red);
                BorderImg7.Background = new SolidColorBrush(Colors.Red);
                BorderImg8.Background = new SolidColorBrush(Colors.Red);

                BorderVid1.Background = new SolidColorBrush(Colors.Red);
                BorderVid2.Background = new SolidColorBrush(Colors.Red);
                BorderVid3.Background = new SolidColorBrush(Colors.Red);
                BorderVid4.Background = new SolidColorBrush(Colors.Red);
                BorderVid5.Background = new SolidColorBrush(Colors.Red);
                BorderVid6.Background = new SolidColorBrush(Colors.Red);
                BorderVid7.Background = new SolidColorBrush(Colors.Red);
                BorderVid8.Background = new SolidColorBrush(Colors.Red);

            }
            else
            {
                BorderImg1.Background = new SolidColorBrush(Colors.Green);
                BorderImg2.Background = new SolidColorBrush(Colors.Green);
                BorderImg3.Background = new SolidColorBrush(Colors.Green);
                BorderImg4.Background = new SolidColorBrush(Colors.Green);
                BorderImg5.Background = new SolidColorBrush(Colors.Green);
                BorderImg6.Background = new SolidColorBrush(Colors.Green);
                BorderImg7.Background = new SolidColorBrush(Colors.Green);
                BorderImg8.Background = new SolidColorBrush(Colors.Green);

                BorderVid1.Background = new SolidColorBrush(Colors.Green);
                BorderVid2.Background = new SolidColorBrush(Colors.Green);
                BorderVid3.Background = new SolidColorBrush(Colors.Green);
                BorderVid4.Background = new SolidColorBrush(Colors.Green);
                BorderVid5.Background = new SolidColorBrush(Colors.Green);
                BorderVid6.Background = new SolidColorBrush(Colors.Green);
                BorderVid7.Background = new SolidColorBrush(Colors.Green);
                BorderVid8.Background = new SolidColorBrush(Colors.Green);
            }
        }
        private void AnimarMiniaturaVideos(MediaElement mediaElement)
        {
            
                videoElement1.LoadedBehavior = MediaState.Stop;
                videoElement2.LoadedBehavior = MediaState.Stop;
                videoElement3.LoadedBehavior = MediaState.Stop;
                videoElement4.LoadedBehavior = MediaState.Stop;

                videoElement5.LoadedBehavior = MediaState.Stop;
                videoElement6.LoadedBehavior = MediaState.Stop;
                videoElement7.LoadedBehavior = MediaState.Stop;
                videoElement8.LoadedBehavior = MediaState.Stop;

                videoElement1.LoadedBehavior = MediaState.Play;
                videoElement2.LoadedBehavior = MediaState.Play;
                videoElement3.LoadedBehavior = MediaState.Play;
                videoElement4.LoadedBehavior = MediaState.Play;

                videoElement5.LoadedBehavior = MediaState.Play;
                videoElement6.LoadedBehavior = MediaState.Play;
                videoElement7.LoadedBehavior = MediaState.Play;
                videoElement8.LoadedBehavior = MediaState.Play;

                Thread.Sleep(100);

                videoElement1.LoadedBehavior = MediaState.Stop;
                videoElement2.LoadedBehavior = MediaState.Stop;
                videoElement3.LoadedBehavior = MediaState.Stop;
                videoElement4.LoadedBehavior = MediaState.Stop;

                videoElement5.LoadedBehavior = MediaState.Stop;
                videoElement6.LoadedBehavior = MediaState.Stop;
                videoElement7.LoadedBehavior = MediaState.Stop;
                videoElement8.LoadedBehavior = MediaState.Stop;

                mediaElement.LoadedBehavior = MediaState.Play;
            

        }

        private void ResetarImagem_Click(object sender, RoutedEventArgs e)
        {
            fullscreenWindow.videoPlayer.Source = null;
            if (isPlayng)
            {
                isPlayng = !isPlayng;
                PlayPauseButton.Content = "Play";
            }
        }
    }


}
