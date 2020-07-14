using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ASPEC.Utilities;

namespace ASPEC.Pages
{
    /// <summary>
    /// Interaction logic for pgTaskCreation.xaml
    /// </summary>
    public partial class pgTaskCreation : Page
    {
        public Task task = new Task();
        private pgTasks cameFrom;
        private ObservableCollection<Measurement> measurements = new ObservableCollection<Measurement>();


        public pgTaskCreation(pgTasks _cameFrom)
        {
            InitializeComponent();
            cameFrom = _cameFrom;

            cmbExecutor.ItemsSource = Conn.Db.Shift.Where(s => s.Date == DateTime.Today).ToList();
            cmbRadiationType.ItemsSource = Conn.Db.RadiationType.ToList();
            cmbNewMeasurementUnit.ItemsSource = Conn.Db.Unit.ToList();
            cmbNewMeasurementDevice.ItemsSource = Conn.Db.Device.ToList();
            dtgMeasurements.ItemsSource = measurements;
            
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnSaveTask_Click(object sender, RoutedEventArgs e)
        {
            string error = string.Empty;
            if(task.Point == null)
                error = "Не выбрана точка измерения!";
            if (measurements.Count == 0)
                error += "\nОтсутствуют измерения!";
            if (error != string.Empty)
            {
                MessageBox.Show(error, "Внимание!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
                

            task.CreatedDate = DateTime.Now;
            task.CreatorId = Transfer.User.Id;
            task.ExecutorId = (int)cmbExecutor.SelectedValue;
            task.RadiationTypeId = (int)cmbRadiationType.SelectedValue;
            task.StatusId = 1;
            Conn.Db.Task.Add(task);
            Conn.Db.SaveChanges();

            foreach(Measurement measurement in measurements)
            {
                measurement.TaskId = task.Id;
                Conn.Db.Measurement.Add(measurement);
            }
            Conn.Db.SaveChanges();
            cameFrom.UpdateDtgTask();
            NavigationService.GoBack();
        }

        

        private void btnAddMeasurement_Click(object sender, RoutedEventArgs e)
        {
            measurements.Add(new Measurement { Unit = (Unit)cmbNewMeasurementUnit.SelectedItem, Device = (Device)cmbNewMeasurementDevice.SelectedItem });
        }

        private void btnRemoveMeasurement_Click(object sender, RoutedEventArgs e)
        {
            measurements.Remove((Measurement)dtgMeasurements.SelectedItem);   
        }

        private void btnPointSelection_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new pgPointSelection(this));
        }

        public void updatePoint(Models.Point point)
        {
            //string svgPath = $"{AppRoot.GetApplicationRoot()}{point.Scheme}.svg";
            //svgViewBox.Source = new Uri(svgPath);
            lblPointInfo.Content = point.Name;
            var uri = new Uri($"{AppRoot.GetApplicationRoot()}PNG\\{point.Id}.png");
            var bitmap = new BitmapImage(uri);
            imgPoint.Source = bitmap;
            task.Point = point;
        }

    }
}
