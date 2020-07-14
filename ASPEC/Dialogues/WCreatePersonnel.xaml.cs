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
using System.Windows.Shapes;
using ASPEC.Models;
using ASPEC.Utilities;

namespace ASPEC.Dialogues
{
    /// <summary>
    /// Interaction logic for WCreatePersonnel.xaml
    /// </summary>
    public partial class WCreatePersonnel : Window
    {
        private User _user;
        public User Personnel { 
            get { return _user; } 
        }

        public WCreatePersonnel()
        {
            InitializeComponent();
            txtPassword.Text = AuthorizationInfoGenerator.GenerateRandomPassword();
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            
            bool noEmptyFields = true;
            if (txtSecondName.Text == string.Empty)
                noEmptyFields = false;
            if (txtFirstName.Text == string.Empty)
                noEmptyFields = false;
            if (txtPatronymic.Text == string.Empty)
                noEmptyFields = false;
            if (!dtpBirthDate.SelectedDate.HasValue)
                noEmptyFields = false;
            if (noEmptyFields)
            {
                _user = new User
                {
                    SecondName = txtSecondName.Text,
                    FirstName = txtFirstName.Text,
                    Patronymic = txtPatronymic.Text,
                    BirthDate = (DateTime)dtpBirthDate.SelectedDate,
                    Login = txtLogin.Text,
                    Password = txtPassword.Text,
                    RoleId = 2
                };
                DialogResult = true;
                Close();
                
            }

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool noEmptyFields = true;
            if (txtSecondName.Text == string.Empty)
                noEmptyFields = false;
            if (txtFirstName.Text == string.Empty)
                noEmptyFields = false;
            if (txtPatronymic.Text == string.Empty)
                noEmptyFields = false;
            if (!dtpBirthDate.SelectedDate.HasValue)
                noEmptyFields = false;
            if (noEmptyFields)
            {
                txtLogin.Text = AuthorizationInfoGenerator.GenerateLogin(txtSecondName.Text, txtFirstName.Text, txtPatronymic.Text, dtpBirthDate.SelectedDate.Value);
            }
        }



        private void dtpBirthDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            txt_TextChanged(sender, null);
        }
    }

  
}
