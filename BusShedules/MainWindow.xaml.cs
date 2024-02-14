using BusShedules.Pages;
using BusShedules.Pages.AdminPages;
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

namespace BusShedules
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new AuthorizationPage());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch ((MainFrame.Content as Page).Title)
            {
                case "Route":
                    MainFrame.Navigate(new MainPage());
                    break;
                case "Main":
                    MainFrame.Navigate(new AuthorizationPage());
                    break;
                case "BusNumbers":
                    MainFrame.Navigate(new MainAdminPage());
                    break;
                case "StopNames":
                    MainFrame.Navigate(new MainAdminPage());
                    break;
                case "MainAdmin":
                    MainFrame.Navigate(new AuthorizationPage());
                    break;
                case "AddRoute":
                    MainFrame.Navigate(new RoutesPage());
                    break;                   
               case "RouteDate":
                    MainFrame.Navigate(new MainAdminPage());
                    break;
                case "Routes":
                    MainFrame.Navigate(new MainAdminPage());
                    break;
                default:
                    break;

            }
        }
    }
}
