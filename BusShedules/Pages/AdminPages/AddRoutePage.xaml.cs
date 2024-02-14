using BusShedules.ADOModel;
using BusShedules.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace BusShedules.Pages.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для AddRoutePage.xaml
    /// </summary>
    public partial class AddRoutePage : Page
    {
        private byte[] _map;
        private List<Stop> _stopList;
        private RouteInformation _routeInformation;

        public AddRoutePage()
        {
            InitializeComponent();
            LoadPage();
            _stopList = new List<Stop>();
            ReloadTable();
            if(_routeInformation == null)
            {
                btnDeleteRoute.Visibility = Visibility.Hidden;
            }
        }

        public AddRoutePage(RouteInformation route)
        {
            InitializeComponent();
            _routeInformation = route;
            LoadPage();          
            List<int> stopIdList = GetStopListId(App.Connection.RouteStop.Where(x => x.RouteId == _routeInformation.RouteId).ToList());
            _stopList = App.Connection.Stop.Where(x => stopIdList.Contains(x.StopId)).ToList();
            ReloadTable();
        }

        private void LoadRouteMap(object sender, RoutedEventArgs e)
        {
            try
            {
                var BtnSelect = sender as Button;
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() != null)
                {
                    _map = File.ReadAllBytes(dialog.FileName);
                    BtnSelect.Background = Brushes.Green;
                    MessageBox.Show("Карта маршрута успешно добавлена!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch
            {
                MessageBox.Show("Некорректный формат фото!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddStop(object sender, RoutedEventArgs e)
        {
            if (cbStopNames.SelectedIndex != -1)
            {
                if(int.TryParse(tbArrivalTime.Text, out var time))
                {
                    Stop stop = new Stop()
                    {
                        StopNameId = ((StopName) cbStopNames.SelectedItem).StopNameId,
                        StopName = (StopName)cbStopNames.SelectedItem,
                        ArrivalTime = time
                    };

                    if(!CheckStop(stop))
                    {
                        MessageBox.Show("Такая остановка уже есть в маршруте!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    _stopList.Add(stop);
                    cbStopNames.SelectedIndex = -1;
                    tbArrivalTime.Clear();
                    CreateRouteName();
                    ReloadTable();
                }
                else
                {
                    MessageBox.Show("Время прибытия должно быть натуральным числом!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите название остановки!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteStop(object sender, RoutedEventArgs e)
        {
            if(dgRoutes.SelectedIndex != -1)
            {
                _stopList.Remove((Stop) dgRoutes.SelectedItem);
            }
            else
            {
                MessageBox.Show("Выберите остановку из таблицы!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveRoute(object sender, RoutedEventArgs e)
        {
            if(_map == null)
            {
                MessageBox.Show("Загрузите карту маршрута!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (tbMapLink.Text == "")
            {
                MessageBox.Show("Запишите ссылку на карту!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (cbBusNumbers.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите номер автобуса!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(dgRoutes.Items.Count < 2)
            {
                MessageBox.Show("Минимальное кол-во остановок: 2!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int routeId = 0;

            if(_routeInformation == null)
            {
                Route route = new Route()
                {
                    Name = tbNameRoute.Text,
                    BusNumberId = ((BusNumber)cbBusNumbers.SelectedItem).BusNumberId,
                    Map = _map,
                    MapLink = tbMapLink.Text,
                };

                App.Connection.Route.Add(route);
                App.Connection.SaveChanges();
                routeId = App.Connection.Route.ToList().Last().RouteId;
            }
            else
            {
                var route = App.Connection.Route.Where(x => x.RouteId == _routeInformation.RouteId).FirstOrDefault();

                route.MapLink = tbMapLink.Text;
                route.Map = _map;
                route.Name = tbNameRoute.Text;
                route.BusNumberId = ((BusNumber)dgRoutes.SelectedItem).BusNumberId;

                ClearRouteStop(route.RouteId);
                App.Connection.SaveChanges();
                routeId = route.RouteId;
            }          

            foreach(var stop in _stopList)
            {
                App.Connection.Stop.Add(stop);
                App.Connection.SaveChanges();

                var stopId = App.Connection.Stop.ToList().Last().StopId;

                var routeStop = new RouteStop()
                { 
                    RouteId = routeId,
                    StopId = stopId
                };

                App.Connection.RouteStop.Add(routeStop);
                App.Connection.SaveChanges();
            }

            if (_routeInformation == null)
            {
                MessageBox.Show("Маршрут успешно добавлен!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Маршрут успешно изменен!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
            NavigationService.Navigate(new RoutesPage());
        }

        private void CreateRouteName()
        {
            if(dgRoutes.Items.Count > 1)
            {
                tbNameRoute.Text = $"{_stopList.First().StopName.Name} - {_stopList.Last().StopName.Name}";
            }
        }

        private void ReloadTable()
        {
            dgRoutes.ItemsSource = _stopList;
            dgRoutes.Items.Refresh();
            CreateRouteName();
        }

        private bool CheckStop(Stop stop)
        {
            foreach(var st in _stopList)
            {
                if(st.StopName.Name == stop.StopName.Name)
                {
                    return false;
                }
            }

            return true;
        }

        private void LoadPage()
        {
            cbBusNumbers.ItemsSource = App.Connection.BusNumber.ToList();
            cbStopNames.ItemsSource = App.Connection.StopName.ToList();
            cbBusNumbers.Items.SortDescriptions.Add(new SortDescription("Number", ListSortDirection.Ascending));
            cbStopNames.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }

        private List<int> GetStopListId(List<RouteStop> routeStopList)
        {
            var stopList = new List<int>();

            foreach(var rs in routeStopList)
            {
                stopList.Add(rs.StopId);
            }

            return stopList;
        }

        private void ClearRouteStop(int routeId)
        {
            var stopRouteList = App.Connection.RouteStop.Where(x => x.RouteId == routeId).ToList();

            var stopList = GetStopListId(stopRouteList);

            foreach(var rs in stopRouteList)
            {
                App.Connection.RouteStop.Remove(rs);
                App.Connection.SaveChanges();
            }

            foreach (var s in stopList)
            {
                App.Connection.Stop.Remove(App.Connection.Stop.Where(x => x.StopId == s).FirstOrDefault());
                App.Connection.SaveChanges();
            }
        }

        private void DeleteRoute(object sender, RoutedEventArgs e)
        {
            if(App.Connection.DateRoute.Where(x => x.DateRouteId == _routeInformation.RouteId).FirstOrDefault() != null)
            {
                ClearRouteStop(_routeInformation.RouteId);
                App.Connection.Route.Remove(App.Connection.Route.Where(x => x.RouteId == _routeInformation.RouteId).FirstOrDefault());
                App.Connection.SaveChanges();

                MessageBox.Show("Маршрут успешно удален!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new RoutesPage());
            }
            else
            {
                MessageBox.Show("Невозможно удалить маршрут, так как он добавлен в расписание!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
        }
    }
}
