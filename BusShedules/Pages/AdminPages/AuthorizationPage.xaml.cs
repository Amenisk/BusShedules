using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
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
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            InitializeComponent();
        }

        private void Authorizate(object sender, RoutedEventArgs e)
        {
            if (tbEmail.Text != "" && pbPassword.Password != "")
            {
                var admin = App.Connection.Admin.Where(x => x.Login == tbEmail.Text 
                    && x.Password == pbPassword.Password);
                if (admin != null)
                {
                    NavigationService.Navigate(new MainAdminPage());
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля для авторизации!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void NavigateToMainPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }
    }
}
