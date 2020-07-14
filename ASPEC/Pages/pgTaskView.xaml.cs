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
using ASPEC.Models;
using ASPEC.Utilities;
using ASPEC.Pages;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

namespace ASPEC.Pages
{
    /// <summary>
    /// Interaction logic for pgTaskView.xaml
    /// </summary>
    public partial class pgTaskView : Page
    {
        private Task task;
        private pgTasks cameFrom;

        public pgTaskView(Task _task, pgTasks _cameFrom)
        {
            InitializeComponent();
            task = _task;
            cameFrom = _cameFrom;

            Conn.Db.Entry(task).Reference(t => t.Status).Load();
            Conn.Db.Entry(task).Reference(t => t.RadiationType).Load();
            Conn.Db.Entry(task).Reference(u => u.Point).Load();
            Conn.Db.Entry(task).Collection(t => t.Measurement).Load();

            foreach(Measurement measurement in task.Measurement)
            {
                Conn.Db.Entry(measurement).Reference(m => m.Unit).Load();
                Conn.Db.Entry(measurement).Reference(m => m.Device).Load();
            }
            
            txbTaskName.Text = $"Задание: {task.Id}";
            txbStatus.Text = task.Status.Name;
            txbCreatedDate.Text = task.CreatedDate.ToString();
            txbClosedDate.Text = task.ClosedDate.ToString();
            txbCreator.Text = task.Creator.Fio;
            txbExecutor.Text = task.Executor.Fio;
            tbcPointInfo.Text = task.Point.Name;            
            txbRadiationType.Text = task.RadiationType.Name;
            dtgMeasurements.ItemsSource = task.Measurement;

            var uri = new Uri($"{AppRoot.GetApplicationRoot()}PNG\\{task.Point.Id}.png");
            var bitmap = new BitmapImage(uri);
            imgPoint.Source = bitmap;

            if(task.Status.Id == 2)
            {
                btnLoadFromFile.IsEnabled = false;
                btnCloseTask.IsEnabled = false;
            }
        }

        private void btnLoadFromFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".json";
            dlg.Filter = "JSON документ (.json)|*.json";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                Task taskFile = JsonConvert.DeserializeObject<Task>(File.ReadAllText(filename));
                if(task.Id == taskFile.Id)
                {
                    foreach(var metf in taskFile.Measurement)
                    {
                        task.Measurement.First(m => m.Id == metf.Id).Value = metf.Value;
                        task.Measurement.First(m => m.Id == metf.Id).DateTime = metf.DateTime;
                    }
                    //foreach (var metf in task.Measurement)
                    //{
                    //    MessageBox.Show($"{metf.Id} {metf.Value} {metf.DateTime}");

                    //}

                    dtgMeasurements.Items.Refresh();

                    //dtgMeasurements.ItemsSource = null;
                    //dtgMeasurements.ItemsSource = task.Measurement;
                }
            }
        }

        private void btnCloseTask_Click(object sender, RoutedEventArgs e)
        {
            foreach(Measurement measurement in task.Measurement)
            {
                if (measurement.Value == null)
                {
                    MessageBox.Show("Остались незаполненные измерения!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                    
            }
            
            task.ClosedDate = DateTime.Now;
            task.Status = Conn.Db.Status.First(s => s.Id == 2);

            Conn.Db.SaveChanges();

            cameFrom.UpdateDtgTask();
            NavigationService.GoBack();
        }

        private void btnSaveToFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = $"task_{task.Id}_{task.CreatedDate.ToString("dd-MM-yyyy_HH-mm")}"; 
            dlg.DefaultExt = ".json"; 
            dlg.Filter = "JSON документ (.json)|*.json"; 

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {

                string json = JsonConvert.SerializeObject(task, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }); 
                string filename = dlg.FileName;
                string directoryPath = System.IO.Path.GetDirectoryName(filename);
                File.WriteAllText(filename, json);
                File.Copy($"{AppRoot.GetApplicationRoot()}PNG\\{task.Point.Id}.png", $"{filename.Replace(".json", ".png")}");
            }
            
        }

        

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        


    }
}
