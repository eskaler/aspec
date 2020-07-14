using System;
using System.Collections.Generic;
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

namespace ASPEC.Pages
{
    /// <summary>
    /// Interaction logic for pgMain.xaml
    /// </summary>
    public partial class pgMain : Page
    {
        public pgMain()
        {
            InitializeComponent();
            lblAuthorizedUser.Content = $"Добро пожаловать, {Transfer.User.Fio}!";
            frPersonnel.Navigate(new pgPersonnel());
            frTasks.Navigate(new pgTasks());
        }

        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
