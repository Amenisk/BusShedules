using BusShedules.ADOModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для StopNamesPage.xaml
    /// </summary>
    public partial class StopNamesPage : Page
    {
        public StopNamesPage()
        {
            InitializeComponent();
            ReloadBusNumberList();
        }

        private void DeleteStopName(object sender, RoutedEventArgs e)
        {
            if (lvStopNames.SelectedItem != null)
            {
                var routeStopList = App.Connection.RouteStop.ToList();

                foreach (var routeStop in routeStopList)
                {
                    if(routeStop.Stop.StopNameId == ((StopName)lvStopNames.SelectedItem).StopNameId)
                    {
                        MessageBox.Show("Невозможно удалить данное название остановки," +
                            " так как оно находится в созданном маршруте!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }   
                }

                
                App.Connection.StopName.Remove((StopName)lvStopNames.SelectedItem);
                App.Connection.SaveChanges();

                MessageBox.Show("Название остановки успешно удалено!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                ReloadBusNumberList();
            }
            else
            {
                MessageBox.Show("Выберите название остановки!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddStopName(object sender, RoutedEventArgs e)
        {
            if(tbStopName.Text == "")
            {
                MessageBox.Show("Введите название остановки!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (tbStopName.Text.Length <= 50)
            {
                if (App.Connection.StopName.Where(x => x.Name == tbStopName.Text).FirstOrDefault() != null)
                {
                    MessageBox.Show("Такое название остановки уже существует!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                StopName name = new StopName()
                {
                    Name = tbStopName.Text,
                };

                App.Connection.StopName.Add(name);
                App.Connection.SaveChanges();

                MessageBox.Show("Название остановки успешно добавлено!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                ReloadBusNumberList();
                tbStopName.Clear();
            }
            else
            {
                MessageBox.Show("Длина названия остановки должна быть меньше 50 символов!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ReloadBusNumberList()
        {
            lvStopNames.ItemsSource = App.Connection.StopName.ToList();
            lvStopNames.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }
    }
}
