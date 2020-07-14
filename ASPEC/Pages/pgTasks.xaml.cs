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
using System.Linq;
using ASPEC.Models;
using System.Collections.ObjectModel;

namespace ASPEC.Pages
{
    /// <summary>
    /// Interaction logic for pgTasks.xaml
    /// </summary>
    public partial class pgTasks : Page
    {
        public pgTasks()
        {
            InitializeComponent();
            var statuses = new ObservableCollection<Status>(Conn.Db.Status.ToList());
            var creators = new ObservableCollection<User>(Conn.Db.User.Where(u => u.RoleId == 1).ToList());
            var executors = new ObservableCollection<User>(Conn.Db.User.Where(u => u.RoleId == 2).ToList());

            cmbStatus.ItemsSource = statuses;
            cmbCreator.ItemsSource = creators;
            cmbExecutor.ItemsSource = executors;
            
            statuses.Insert(0, new Status { Id = -1, Name = "Любой" });
            creators.Insert(0, new User { Id = -1, SecondName = "Любой", FirstName = "", Patronymic = "" });
            executors.Insert(0, new User { Id = -1, SecondName = "Любой", FirstName = "", Patronymic = "" });

            SetDefaultFlter();

            cmbStatus.SelectionChanged += cmb_SelectionChanged;
            cmbCreator.SelectionChanged += cmb_SelectionChanged;
            cmbExecutor.SelectionChanged += cmb_SelectionChanged;

            dtpCreatedAfter.SelectedDateChanged += dtp_SelectedDateChanged;
            dtpCreatedUntil.SelectedDateChanged += dtp_SelectedDateChanged;

            UpdateDtgTask();
        }

        public void UpdateDtgTask()
        {
            var tasks = new ObservableCollection<Task>(Conn.Db.Task.Where(t =>
                t.CreatedDate >= dtpCreatedAfter.SelectedDate.Value &&
                t.CreatedDate <= dtpCreatedUntil.SelectedDate.Value).ToList());
            
            var idCreator = (cmbCreator.SelectedItem as User).Id;
            var idExecutor = (cmbExecutor.SelectedItem as User).Id;
            var idStatus = (cmbStatus.SelectedItem as Status).Id;


            if (idCreator > -1)
                tasks = new ObservableCollection<Task>(tasks.Where(t => t.CreatorId == idCreator));
            if (idExecutor > -1)
                tasks = new ObservableCollection<Task>(tasks.Where(t => t.ExecutorId == idExecutor));
            if (idStatus > -1)
                tasks = new ObservableCollection<Task>(tasks.Where(t => t.StatusId == idStatus));
            
            dtgTask.ItemsSource = tasks;
        }

        private void cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDtgTask();
        }
        private void dtp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDtgTask();
        }

        private void btnDropFilter_Click(object sender, RoutedEventArgs e)
        {
            SetDefaultFlter();
        }

        private void SetDefaultFlter()
        {
            cmbStatus.SelectedIndex = 0;
            cmbExecutor.SelectedIndex = 0;
            cmbCreator.SelectedIndex = cmbCreator.Items.IndexOf(Transfer.User);
            dtpCreatedAfter.SelectedDate = DateTime.Today;
            dtpCreatedUntil.SelectedDate = DateTime.Today;
        }

        private void btnCreateTask_Click(object sender, RoutedEventArgs e)
        {
            if (Conn.Db.Shift.Where(s => s.Date == DateTime.Today).ToList().Count > 0)
                NavigationService.Navigate(new pgTaskCreation(this));
            else
                MessageBox.Show("Добавьте сотрудников на смену", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);

        }

        private void btnViewTask_Click(object sender, RoutedEventArgs e)
        {
            if(dtgTask.SelectedIndex > -1)
                NavigationService.Navigate(new pgTaskView((Task)dtgTask.SelectedItem, this));
        }

        private void btnRemoveTask_Click(object sender, RoutedEventArgs e)
        {

        }

        

        
    }
}
