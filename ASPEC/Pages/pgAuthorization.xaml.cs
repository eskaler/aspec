using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ASPEC.Models;

namespace ASPEC.Pages
{
    /// <summary>
    /// Interaction logic for pgAuthorization.xaml
    /// </summary>
    public partial class pgAuthorization : Page
    {

        public pgAuthorization()
        {
            InitializeComponent();
            txtLogin.Focus();
            //cbFuck.ItemsSource = Conn.Db.Role.ToList();
        }

        private void btnAuthorize_Click(object sender, RoutedEventArgs e)
        {
            // ----- actual code ------
            var user = Conn.Db.User.Where(u => u.Login == txtLogin.Text && u.Password == pwdPassword.Password).FirstOrDefault();
            if (user == null)
            {
                MessageBox.Show("Неправильные логин или пароль", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Conn.Db.Entry(user).Reference(u => u.Role).Load();
            if (user.Role.Id == 1)
            {
                Transfer.User = user;
                this.NavigationService.Navigate(new pgMain());
                return;
            }
            MessageBox.Show("ПО планирования доступно только администраторам!");

            // ------ temporary code -------
            //var user = Conn.Db.User.Where(u => u.Login == "admin").FirstOrDefault();
            //Conn.Db.Entry(user).Reference(u => u.Role).Load();
            //Transfer.User = user;
            //this.NavigationService.Navigate(new pgMain());
        }

        //private void cbFuck_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    dtgTest.ItemsSource = Conn.Db.User.Where(u => u.Id == (int)cbFuck.SelectedValue).ToList();
        //}
    }
}
