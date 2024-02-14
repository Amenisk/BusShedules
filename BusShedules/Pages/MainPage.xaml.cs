using BusShedules.ADOModel;
using BusShedules.Classes;
using BusShedules.Pages.AdminPages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BusShedules.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        List<RouteInformation> routes = new List<RouteInformation>();
        List<RouteInformation> sortRoutes = new List<RouteInformation>();

        public MainPage()
        {
            InitializeComponent();
            cbBusNumber.ItemsSource = App.Connection.BusNumber.ToList();
            cbBusNumber.Items.SortDescriptions.Add(new SortDescription("Number", ListSortDirection.Ascending));

            var dateList = App.Connection.DateRoute.ToList();

            foreach (var r in dateList)
            {
                var routeInformation = new RouteInformation(r.Route, r.DateTimeBeginning);
                if (routeInformation.EndingDateTime > DateTime.Now)
                {
                    routes.Add(routeInformation);
                }               
            }

            dgRoutes.ItemsSource = routes;
        }

        private void SortRoutes()
        {
            StopInformation fromStop = null;
            var findRoutes = new List<RouteInformation>();
            var removeRoutes = new List<RouteInformation>();

            sortRoutes = routes.OrderBy(x => x.BeginningDateTime).ToList();

            if (cbBusNumber.SelectedIndex != -1)
            {
                foreach (var r in sortRoutes)
                {
                    if (r.BusNumber != ((BusNumber)cbBusNumber.SelectedItem).Number.ToString())
                    {
                        removeRoutes.Add(r);
                    }
                }
            }

            foreach (var r in removeRoutes)
            {
                sortRoutes.Remove(r);
            }

            if (cmbSort.SelectedIndex == 1)
            {
                sortRoutes.Reverse();
                
            }

            dgRoutes.ItemsSource = sortRoutes;

            if (tbFrom.Text != "" && tbWhere.Text != "")
            {
                foreach (var r in sortRoutes)
                {
                    foreach (var s in r.StopsList)
                    {
                        if (fromStop == null && s.StopName.ToLower().Contains(tbFrom.Text.ToLower()))
                        {
                            fromStop = s;
                            continue;
                        }

                        if (fromStop != null && s.StopName.ToLower().Contains(tbWhere.Text.ToLower()))
                        {
                            fromStop.Highlighting = "Bold";
                            s.Highlighting = "Bold";
                            findRoutes.Add(r);
                            break;
                        }
                    }
                    fromStop = null;
                }
                sortRoutes = findRoutes;
                dgRoutes.ItemsSource = sortRoutes;
                return;
            }

            if (tbFrom.Text != "")
            {
                foreach (var r in sortRoutes)
                {
                    foreach (var s in r.StopsList)
                    {
                        if (fromStop == null && s.StopName.ToLower().Contains(tbFrom.Text.ToLower()))
                        {
                            s.Highlighting = "Bold";
                            findRoutes.Add(r);
                            break;
                        }
                    }
                }
                sortRoutes = findRoutes;
                dgRoutes.ItemsSource = sortRoutes;
                return;
            }

            if (tbWhere.Text != "")
            {
                foreach (var r in sortRoutes)
                {
                    foreach (var s in r.StopsList)
                    {
                        if (fromStop == null && s.StopName.ToLower().Contains(tbWhere.Text.ToLower()))
                        {
                            s.Highlighting = "Bold";
                            findRoutes.Add(r);
                            break;
                        }
                    }
                }

                sortRoutes = findRoutes;
                dgRoutes.ItemsSource = sortRoutes;
                return;
            }
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortRoutes();
            dgRoutes.Items.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SortRoutes();
            dgRoutes.Items.Refresh();
        }

        private void NavigateToRoutePage(object sender, SelectionChangedEventArgs e)
        {  
                NavigationService.Navigate(new RoutePage((RouteInformation)dgRoutes.SelectedItem));
        }

        private void cbBusNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortRoutes();
            dgRoutes.Items.Refresh();
        }
    }
}
