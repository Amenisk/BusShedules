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

namespace BusShedules.Pages.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для MainAdminPage.xaml
    /// </summary>
    public partial class MainAdminPage : Page
    {
        public MainAdminPage()
        {
            InitializeComponent();
        }

        private void NavigateToBusNumbersPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BusNumbersPage());
        }

        private void NavigateToStopNamesPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StopNamesPage());
        }

        private void NavigateToRoutesPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RoutesPage());
        }

        private void NavigateToRouteDatePage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RouteDatePage());
        }      
    }
}

