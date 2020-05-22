using com.sun.org.apache.xml.@internal.dtm.@ref;
using InsuranceManagerApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace InsuranceManagerApp
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int progress = 0;
        ObservableCollection<CustomerDataViewModel> customerDatas = new ObservableCollection<CustomerDataViewModel>();
        public MainWindow()
        {
            InitializeComponent();
            DataGrid.ItemsSource = customerDatas;
        }

        private void ParseButton_Click(object sender, RoutedEventArgs e)
        {
            var parsePdf = new ParsePdf();
            LoadingDataProgressBar.Value = 0;
            LoadingDataProgressBar.Visibility = Visibility.Visible;
            ProgressLabel.Content = "0%";
            ProgressLabel.Visibility = Visibility.Visible;
            EstimatedTimeLabel.Content = "Przybliżony czas: 0s";
            EstimatedTimeLabel.Visibility = Visibility.Visible;
            ElementsCountLabel.Visibility = Visibility.Hidden;
            ParseButton.IsEnabled = false;
            DataGrid.IsEnabled = false;
            var th = new Thread(() =>
            {
                customerDatas = new ObservableCollection<CustomerDataViewModel>(parsePdf.ParseFiles().OrderBy(d => d.LastName));
                Dispatcher.Invoke( () => 
                { 
                    DataGrid.ItemsSource = customerDatas;
                    ParseButton.IsEnabled = true;
                    DataGrid.IsEnabled = true;
                    LoadingDataProgressBar.Visibility = Visibility.Hidden;
                    ProgressLabel.Visibility = Visibility.Hidden;
                    EstimatedTimeLabel.Visibility = Visibility.Hidden;
                    ElementsCountLabel.Visibility = Visibility.Visible;
                    ElementsCountLabel.Content = "Elementów: " + customerDatas.Count();
                });
            });
            parsePdf.ProgressChanged += ParsePdf_ProgressChanged;

            th.Start();
        }

        private void ParsePdf_ProgressChanged(object sender, ParsePdf.ProgressEventArgs e)
        {
            Dispatcher.BeginInvoke( new Action ( () => 
            {
                if (LoadingDataProgressBar.Value < 100)
                {
                    LoadingDataProgressBar.Value = e.Progress;
                    ProgressLabel.Content = e.Progress + "%";
                }
                if(e.TimeLeft >= 0)
                {
                    var time = TimeSpan.FromSeconds(e.TimeLeft);
                    EstimatedTimeLabel.Content = "Przybliżony czas: " + time.Duration();
                }
            }));
        }
    }
}
