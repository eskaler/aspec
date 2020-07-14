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
using ASPEC.Dialogues;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;

namespace ASPEC.Pages
{
    /// <summary>
    /// Interaction logic for pgPersonnel.xaml
    /// </summary>
    public partial class pgPersonnel : Page
    {
        public pgPersonnel()
        {
            InitializeComponent();
            Conn.Db.Shift.RemoveRange(Conn.Db.Shift.Where(s => s.Date < DateTime.Today));
            Conn.Db.SaveChanges();
            UpdateDtgPersonnel();
            UpdateDtgShift();
            
        }

        private void btnAddToShift_Click(object sender, RoutedEventArgs e)
        {
            if(dtgPersonnel.SelectedItems.Count >0)
            {
                foreach (User user in dtgPersonnel.SelectedItems)
                {
                    Conn.Db.Shift.Add(new Shift { UserId = user.Id, Date = DateTime.Today });
                    Conn.Db.SaveChanges();
                }
                UpdateDtgPersonnel();
                UpdateDtgShift();
                
            }
        }

        private void btnRemoveFromShift_Click(object sender, RoutedEventArgs e)
        {
            if (dtgShift.SelectedItems.Count > 0)
            {
                foreach (Shift shift in dtgShift.SelectedItems)
                {
                    Conn.Db.Shift.Remove(Conn.Db.Shift.Where(s => s.Id == shift.Id && s.Date == DateTime.Today).FirstOrDefault());
                    Conn.Db.SaveChanges();
                }
                UpdateDtgPersonnel();
                UpdateDtgShift();
            }
        }

        private void btnCreatePersonnel_Click(object sender, RoutedEventArgs e)
        {
            var createPersonnelDialogue = new WCreatePersonnel();
            if (createPersonnelDialogue.ShowDialog() == true)
            {
                User newPersonnel = createPersonnelDialogue.Personnel;
                Conn.Db.User.Add(newPersonnel);
                Conn.Db.SaveChanges();
                UpdateDtgPersonnel();
            }
        }

        private void btnDeletePersonnel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            UpdateDtgPersonnel(txtSearch.Text);
        }

        private void UpdateDtgPersonnel(string search = null)
        {
            if(search == null)
            {
                dtgPersonnel.ItemsSource = Conn.Db.User.Where(u => u.Role.Id == 2 &&
                        Conn.Db.Shift.Where(s => s.UserId == u.Id).FirstOrDefault() == null).ToList();
                return;
            }
            search = search.ToLower();

            dtgPersonnel.ItemsSource = Conn.Db.User.
                Where(  u => u.Role.Id == 2 &&
                        Conn.Db.Shift.Where(s => s.UserId == u.Id).FirstOrDefault() == null && (
                        u.SecondName.ToLower().Contains(search) ||
                        u.FirstName.ToLower().Contains(search)  ||
                        u.Patronymic.ToLower().Contains(search) ||
                        u.BirthDate.ToString().Contains(search) ))
                .ToList();

        }
        
        private void UpdateDtgShift()
        {
            dtgShift.ItemsSource = Conn.Db.Shift.Where(s => s.Date == DateTime.Today).ToList();
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = $"login_information";
            dlg.DefaultExt = ".json";
            dlg.Filter = "JSON документ (.json)|*.json";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                List<LoginInformation> loginInformation = new List<LoginInformation>();
                using (MD5 md5Hash = MD5.Create())
                {

                    foreach (User user in Conn.Db.User.Where(u => u.Role.Id == 2).ToList()) { 
                        loginInformation.Add(new LoginInformation { login = GetMd5Hash(md5Hash, user.Login), password = GetMd5Hash(md5Hash, user.Password) });
                    };
                }
                string json = JsonConvert.SerializeObject(loginInformation, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                string filename = dlg.FileName;
                File.WriteAllText(filename, json);
            }
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        class LoginInformation
        {
            public string login { get; set; }
            public string password { get; set; }
        }
    }
}
