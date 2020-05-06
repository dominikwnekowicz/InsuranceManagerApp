using InsuranceManagerApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace InsuranceManagerApp
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<CustomerDataViewModel> customerDatas = new ObservableCollection<CustomerDataViewModel>();
        public MainWindow()
        {
            InitializeComponent();
            DataGrid.ItemsSource = customerDatas;
        }
        private void ParseButton_Click(object sender, RoutedEventArgs e)
        {
            var parsePdf = new ParsePdf();
            customerDatas = parsePdf.ParseFiles();
            DataGrid.ItemsSource = customerDatas;
        }
    }
}
