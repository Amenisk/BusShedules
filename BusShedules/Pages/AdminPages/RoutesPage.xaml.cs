using BusShedules.ADOModel;
using BusShedules.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
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
    /// Логика взаимодействия для RoutesPage.xaml
    /// </summary>
    public partial class RoutesPage : Page
    {
        List<RouteInformation> routes = new List<RouteInformation>();

        public RoutesPage()
        {
            InitializeComponent();
            var routeList = App.Connection.Route.ToList();
            foreach (var r in routeList)
            {
                routes.Add(new RouteInformation(r));
            }

            dgRoutes.ItemsSource = routes;
        }

        private void AddRoute(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddRoutePage());
        }

        private void ChangeRoute(object sender, SelectionChangedEventArgs e)
        {
            NavigationService.Navigate(new AddRoutePage((RouteInformation)dgRoutes.SelectedItem));
        }
    }
}
