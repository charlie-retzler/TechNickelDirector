using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace TechNickelDirector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PresentationWindow _presentationWindow;
        private WindowState _previousWindowState;
        private ResizeMode _resizeMode;
        private List<Cue> _availableFiles;
        private ObservableCollection<Cue> _actualCues = new ObservableCollection<Cue>();
        private Cue _currentCue;

        public MainWindow()
        {
            InitializeComponent();
            _presentationWindow = new PresentationWindow();
            _presentationWindow.Show();
            _availableFiles = LoadFolder(@"Z:\Moron Report Stuff\2017 Summer - Chaos\");           

            AvailableCues.ItemsSource = _availableFiles;
            ActualCues.ItemsSource = _actualCues;
        }

        private List<Cue> LoadFolder(string path)
        {
            var files = Directory.GetFiles(path);
            var cues = new List<Cue>();
            foreach (var file in files)
            {
                cues.Add(new Cue {FullFilename = file});
            }

            return cues;
        }

        private void FullScreenPlayer_Click(object sender, RoutedEventArgs e)
        {
            if (_presentationWindow.WindowStyle != WindowStyle.None)
            {
                _previousWindowState = _presentationWindow.WindowState;
                _resizeMode = _presentationWindow.ResizeMode;
            }

            _presentationWindow.Visibility = Visibility.Collapsed;
            _presentationWindow.ResizeMode = ResizeMode.NoResize;
            _presentationWindow.WindowState = WindowState.Maximized;
            _presentationWindow.WindowStyle = WindowStyle.None;
            _presentationWindow.Visibility = Visibility.Visible;
        }

        private void WindowedPlayer_Click(object sender, RoutedEventArgs e)
        {
            _presentationWindow.WindowState = _previousWindowState;
            _presentationWindow.ResizeMode = _resizeMode;
            _presentationWindow.WindowStyle = WindowStyle.SingleBorderWindow;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var media = _presentationWindow.Media;
            _presentationWindow.Media.Play();
        }

        private void PlayNextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_actualCues.Count <= 0)
            {
                return;
            }
                        
            _currentCue = _actualCues[0];            
            _actualCues.RemoveAt(0);

            CurrentCue.Text = _currentCue.FileName;

            var media = _presentationWindow.Media;
            media.Source = new Uri(_currentCue.FullFilename, UriKind.RelativeOrAbsolute);
            media.Play();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            var media = _presentationWindow.Media;
            _presentationWindow.Media.Pause();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            var media = _presentationWindow.Media;
            _presentationWindow.Media.Stop();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to close the application", "Close Application?", MessageBoxButton.YesNo);

            // show the message box here and collect the result
            _presentationWindow.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (AvailableCues.SelectedItem != null)
            {
               _actualCues.Add((Cue) AvailableCues.SelectedItem);
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ActualCues.SelectedIndex != -1)
            {
                _actualCues.RemoveAt(ActualCues.SelectedIndex);
            }
        }

        private void MoveUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (ActualCues.SelectedIndex > 0)
            {
                var index = ActualCues.SelectedIndex;
                _actualCues.Move(index, index-1);                
            }
        }

        private void MoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (ActualCues.SelectedIndex == -1)
            {
                return;
            }

            if (ActualCues.SelectedIndex < _actualCues.Count - 1){
                var index = ActualCues.SelectedIndex;
                _actualCues.Move(index, index + 1);
            }
        }
    }
}
