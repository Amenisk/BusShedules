using BusShedules.ADOModel;
using BusShedules.Classes;
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
    /// Логика взаимодействия для RouteDatePage.xaml
    /// </summary>
    public partial class RouteDatePage : Page
    {
        List<RouteInformation> _routeInformationList = new List<RouteInformation>();
        public RouteDatePage()
        {
            InitializeComponent();
            dpRoute.DisplayDateStart = DateTime.Now;
            cbRoutes.ItemsSource = App.Connection.Route.ToList();
            var routeShedule = App.Connection.DateRoute.ToList();
            
            foreach (var route in routeShedule)
            {
                _routeInformationList.Add(new RouteInformation(route.Route, route.DateTimeBeginning));
            }  
            
            dgRoutes.ItemsSource = _routeInformationList;
        }

        private void AddRouteInShedule(object sender, RoutedEventArgs e)
        {
            if(cbRoutes.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите маршрут!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (dpRoute.SelectedDate.Value == DateTime.MinValue)
            {
                MessageBox.Show("Выберите дату!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (int.TryParse(tbHours.Text, out int hours) && int.TryParse(tbMinutes.Text, out int minutes))
            {
                if(!(hours >= 0 && hours <= 23 && minutes >= 0 && minutes <= 59))
                {
                    MessageBox.Show("Некорректный формат времени!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }


                var dateTime = dpRoute.SelectedDate.Value.AddHours(hours).AddMinutes(minutes);

                if(dateTime < DateTime.Now)
                {
                    MessageBox.Show($"Время должно быть больше: {DateTime.Now.Hour}:{DateTime.Now.Minute}!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                DateRoute dateRoute = new DateRoute()
                { 
                    RouteId = ((Route)cbRoutes.SelectedItem).RouteId,
                    Route = (Route)cbRoutes.SelectedItem,
                    DateTimeBeginning = dateTime
                };

                App.Connection.DateRoute.Add(dateRoute);
                App.Connection.SaveChanges();
                _routeInformationList.Add(new RouteInformation(dateRoute.Route, dateRoute.DateTimeBeginning));
                MessageBox.Show("Маршрут добавлен в расписание!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                dgRoutes.Items.Refresh();
                ClearPage();
            }
            else
            {
                MessageBox.Show("Введите время!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void DeleteRouteFromShedule(object sender, RoutedEventArgs e)
        {
            if(dgRoutes.SelectedIndex != -1)
            {
                var dateRoute = (RouteInformation)dgRoutes.SelectedItem;
                App.Connection.DateRoute.Remove(App.Connection.DateRoute.Where(x => x.RouteId == dateRoute.RouteId && x.DateTimeBeginning == dateRoute.BeginningDateTime).FirstOrDefault());
                _routeInformationList.Remove(dateRoute);                
                dgRoutes.Items.Refresh();
                App.Connection.SaveChanges();
            }
            else
            {
                MessageBox.Show("Выберите маршрут из расписания!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearPage()
        {
            cbRoutes.SelectedIndex = -1;
            dpRoute.SelectedDate = null;
            tbHours.Text = string.Empty;
            tbMinutes.Text = string.Empty;  
        }
    }
}
