using System;
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
using System.Windows.Shapes;

namespace Assistente_de_Apresentações
{
    /// <summary>
    /// Lógica interna para Window1.xaml
    /// </summary>
    public partial class FullscreenWindow : Window
    {
        public FullscreenWindow()
        {
            InitializeComponent();

        }
        void MyMediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            System.Windows.MessageBox.Show(e.ErrorException.Message);
        }
        public void Play_Video(string url)
        {
        #pragma warning disable CS8622 // A nulidade de tipos de referência no tipo de parâmetro não corresponde ao delegado de destino (possivelmente devido a atributos de nulidade).
            videoPlayer.MediaFailed += MyMediaElement_MediaFailed;
#pragma warning restore CS8622 // A nulidade de tipos de referência no tipo de parâmetro não corresponde ao delegado de destino (possivelmente devido a atributos de nulidade).


            videoPlayer.Stretch = Stretch.UniformToFill;
            
            videoPlayer.Source =
                new Uri(@url, UriKind.Absolute);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            videoPlayer.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            videoPlayer.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
        }

        public static implicit operator Form(FullscreenWindow v)
        {
            throw new NotImplementedException();
        }
    }
}
