using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        bool rand = false;
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

                music.ItemsSource = files;
                
                if (files.Count > 0)
                {
                    SoundIndex = 0;
                    PlaySelectedSong();
                }
            }
        }

        private void PlaySelectedSong()
        {
            string selectedAudioPath = files[SoundIndex].FullName;
            media.Source = new Uri(selectedAudioPath);
            media.Play();
            Plaing = true;
        }

        private void playSound(object sender, RoutedEventArgs e)
        {
            if (media.Source != null)
            {
                if (Plaing)
                {
                    media.Pause();
                }
                else
                {
                    media.Play();
                }

                Plaing = !Plaing;
            }
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

            PlaySelectedSong();
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

            PlaySelectedSong();
        }

        private void Povtor(object sender, RoutedEventArgs e)
        {
            povtor = !povtor;
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

        private void RandBt(object sender, RoutedEventArgs e)
        {
            if (!rand)
            {
                Random value = new Random();
                files = files.OrderBy(x => value.Next()).ToList();
                rand = true;
            }
            else
            {
                files = files.OrderBy(file => file.Name).ToList();
                rand = false;
            }

            SoundIndex = 0;
            PlaySelectedSong();
        }
    }
}