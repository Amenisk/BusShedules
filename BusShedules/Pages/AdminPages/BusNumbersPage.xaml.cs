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
    /// Логика взаимодействия для BusNumbersPage.xaml
    /// </summary>
    public partial class BusNumbersPage : Page
    {
        public BusNumbersPage()
        {
            InitializeComponent();
            ReloadBusNumberList();
        }

        private void DeleteBusNumber(object sender, RoutedEventArgs e)
        {
            if(lvBusNumbers.SelectedItem != null)
            {
                if(App.Connection.Route.Where(x => x.BusNumber.Number 
                    == ((BusNumber) lvBusNumbers.SelectedItem).Number).FirstOrDefault() == null) 
                { 
                    App.Connection.BusNumber.Remove((BusNumber) lvBusNumbers.SelectedItem);
                    App.Connection.SaveChanges();

                    MessageBox.Show("Номер автобуса успешно удален!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                    ReloadBusNumberList();
                }
                else
                {
                    MessageBox.Show("Невозможно удалить данный номер автобуса, " +
                        "так как с ним уже существует маршрут!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите номер автобуса!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddBusNumber(object sender, RoutedEventArgs e)
        {
            if (tbBusNumber.Text == "")
            {
                MessageBox.Show("Введите номер автобуса!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (int.TryParse(tbBusNumber.Text, out var busNumber) && tbBusNumber.Text.Length <= 3 && busNumber > 0)
            {
                if(App.Connection.BusNumber.Where(x => x.Number == busNumber).FirstOrDefault() != null)
                {
                    MessageBox.Show("Такой номер автобуса уже существует!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                BusNumber number = new BusNumber()
                {
                    Number = busNumber,
                };

                App.Connection.BusNumber.Add(number);
                App.Connection.SaveChanges();

                MessageBox.Show("Номер автобуса успешно добавлен!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                ReloadBusNumberList();
                tbBusNumber.Clear();
            }
            else
            {
                MessageBox.Show("Номер автобуса должен быть натуральным трехзначным числом!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ReloadBusNumberList()
        {
            lvBusNumbers.ItemsSource = App.Connection.BusNumber.ToList();
            lvBusNumbers.Items.SortDescriptions.Add(new SortDescription("Number", ListSortDirection.Ascending));
        }
    }
}
