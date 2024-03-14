using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
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

namespace AudioPlaer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<FileInfo> files = new List<FileInfo>();
        bool Plaing = false;
        bool povtor = false;
        int SoundIndex = 0;
        public MainWindow()
        {
            InitializeComponent();
            music.DisplayMemberPath = "Name";
        }
        private void VibPapka(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { IsFolderPicker = true};
            var result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                files = Directory.GetFiles(dialog.FileName)
                    .Select(item => new FileInfo(item))
                    .Where(file => IsAudioFile(file.Extension.ToLower()))
                    .ToList();

                /*var displayNames = files.Select(file => file.Name);
                music.ItemsSource = displayNames;*/
                music.ItemsSource = files;
                
                if (files.Count > 0)
                {
                    SoundIndex = 0;
                }
            }
        }

        private void playSound(object sender, RoutedEventArgs e)
        {
           
        }

        private void Nazad(object sender, RoutedEventArgs e)
        {
            if (SoundIndex > 0)
            {
                SoundIndex--;
            }
            else
            {
                SoundIndex = files.Count - 1;
            }
        }

        private void Vpered(object sender, RoutedEventArgs e)
        {
            if (SoundIndex < files.Count - 1)
            {
                SoundIndex++;
            }
            else
            {
                SoundIndex = 0;
            }

        }

        private void Povtor(object sender, RoutedEventArgs e)
        {
            if (povtor == false)
            {
                povtor = true;
            }
            else
            {
                povtor = false;
            }
        }

        private void audioSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media.Position = new TimeSpan(Convert.ToInt64(audioSlider.Value));
        }
        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            audioSlider.Maximum = media.NaturalDuration.TimeSpan.Ticks;
        }
        private bool IsAudioFile(string extension)
        {
            return extension == ".mp3" || extension == ".m4a" || extension == ".wav";
        }
    }
}
//можно немного времени на доработку