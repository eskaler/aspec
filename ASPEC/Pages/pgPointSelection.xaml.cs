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
using System.Linq;
using System.IO;

namespace ASPEC.Pages
{
    /// <summary>
    /// Interaction logic for pgPointSelection.xaml
    /// </summary>
    public partial class pgPointSelection : Page
    {
        private Depart departSelected;
        private pgTaskCreation cameFrom;
        private Models.Point pointSelected;

        public pgPointSelection(pgTaskCreation _cameFrom)
        {
            InitializeComponent();
            cameFrom = _cameFrom;
            FillDepartsTree();
        }
        
        private void trvDeparts_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem trv = (TreeViewItem)trvDeparts.SelectedItem;
            string depPrjMark = trv.Header.ToString().Split(" ").Last();
            departSelected = Conn.Db.Depart.Where(d => d.PrjMark == depPrjMark).FirstOrDefault();

            
            if (departSelected.Scheme == null)
                return;
            string svgPath = $"{AppRoot.GetApplicationRoot()}SVG\\{departSelected.Scheme}.svg";
            if (File.Exists(svgPath))
            {
                
                lblDepartInfo.Content = $"{departSelected.Name} {departSelected.PrjMark}";
                svgViewBox.Source = new Uri(svgPath);
                Conn.Db.Entry(departSelected).Collection(u => u.Point).Load();
                svgCanvas.Children.Clear();
                svgCanvas.Height = svgViewBox.ActualHeight;
                foreach (Models.Point point in departSelected.Point)
                {
                    drawPoint((double)point.PosX, (double)point.PosY, point.Id, svgCanvas);
                }
            }
        }

        
        

        private void btnChoosePoint_Click(object sender, RoutedEventArgs e)
        {
            //SaveToPng(svgViewBox.DrawingCanvas);
            svgCanvas.Children.Clear();
            drawPoint((double)pointSelected.PosX, (double)pointSelected.PosY, pointSelected.Id, svgCanvas);
            (svgCanvas.Children[0] as Ellipse).Stroke = Brushes.Red;
            (svgCanvas.Children[0] as Ellipse).Fill = Brushes.Red;
            CreateSaveBitmap(svgViewBox.DrawingCanvas, svgCanvas, $"{AppRoot.GetApplicationRoot()}PNG\\{pointSelected.Id}.png");
            cameFrom.updatePoint(pointSelected);
            NavigationService.GoBack();
        }

        private void svgCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point p = e.GetPosition(this);
            MessageBox.Show("Координата x=" + p.X.ToString() + " y=" + p.Y.ToString());
        }

        public static void CreateSaveBitmap(Canvas canvas1, Canvas canvas2, string filename)
        {
            RenderTargetBitmap renderBitmap1 = new RenderTargetBitmap((int)canvas1.ActualWidth, (int)canvas1.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);

            canvas1.Measure(new Size((int)canvas1.ActualWidth, (int)canvas1.ActualHeight));
            canvas1.Arrange(new Rect(new Size((int)canvas1.ActualWidth, (int)canvas1.ActualHeight)));

            renderBitmap1.Render(canvas1);

            RenderTargetBitmap renderBitmap2 = new RenderTargetBitmap((int)canvas2.ActualWidth, (int)canvas2.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);

            canvas2.Measure(new Size((int)canvas2.ActualWidth, (int)canvas2.ActualHeight));
            canvas2.Arrange(new Rect(new Size((int)canvas2.ActualWidth, (int)canvas2.ActualHeight)));

            renderBitmap2.Render(canvas2);

            //Combine the images here
            var dg = new DrawingGroup();
            var id1 = new ImageDrawing(renderBitmap1,
                                       new Rect(0, 0, renderBitmap1.Width, renderBitmap1.Height));
            var id2 = new ImageDrawing(renderBitmap2,
                                       new Rect(0, 0, renderBitmap2.Width, renderBitmap2.Height));
            dg.Children.Add(id1);
            dg.Children.Add(id2);
            var combinedImg = new RenderTargetBitmap((int)(renderBitmap1.Width + 0.5),
                                  (int)(Math.Max(renderBitmap1.Height, renderBitmap2.Height) + 0.5), 96, 96, PixelFormats.Pbgra32);
            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                dc.DrawDrawing(dg);
            }
            combinedImg.Render(dv);

            //JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            PngBitmapEncoder encoder = new PngBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(combinedImg));

            using (FileStream file = File.Create(filename))
            {
                encoder.Save(file);
            }
        }

        public void drawPoint(double x, double y, Guid id, Canvas cv)
        {

            Ellipse circle = new Ellipse()
            {
                Width = 30,
                Height = 30,
                Stroke = Brushes.Blue,
                StrokeThickness = 6,
                Fill = Brushes.Blue,
                Tag = id.ToString()
            };


            circle.MouseLeftButtonDown += Circle_GotFocus;
            //circle.GotFocus += Circle_GotFocus;


            cv.Children.Add(circle);

            circle.SetValue(Canvas.LeftProperty, (double)x);
            circle.SetValue(Canvas.TopProperty, (double)y);

        }

        private void Circle_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as Ellipse).Stroke = Brushes.Red;
            (sender as Ellipse).Fill = Brushes.Red;
            pointSelected = departSelected.Point.Where(p => p.Id.ToString() == (string)(sender as Ellipse).Tag).FirstOrDefault();
            lblPointInfo.Content = $"Точка: {pointSelected.Name}";
            foreach (Ellipse ellispe in svgCanvas.Children)
            {
                if ((string)ellispe.Tag != pointSelected.Id.ToString())
                {
                    ellispe.Stroke = Brushes.Blue;
                    ellispe.Fill = Brushes.Blue;
                }
            }
        }

        private void FillDepartsTree()
        {
            foreach (Depart depart in Conn.Db.Depart.ToList())
            {
                if (depart.Parent == null)
                {
                    TreeViewItem treeViewItem = new TreeViewItem();
                    treeViewItem.Header = $"{depart.Name} {depart.PrjMark}";
                    //treeViewItem.ItemsSource = Conn.Db.Depart.Where(d => d.ParentId == depart.Id).ToList();
                    trvDeparts.Items.Add(treeViewItem);
                    GetTreeItems(depart, treeViewItem);
                }
            }
        }

        private void GetTreeItems(Depart depart, TreeViewItem treeViewItem)
        {
            foreach (Depart dep in Conn.Db.Depart.Where(dep => dep.ParentId == depart.Id).ToList())
            {
                TreeViewItem trv = new TreeViewItem();
                trv.Header = $"{dep.Name} {dep.PrjMark}";
                treeViewItem.Items.Add(trv);
                GetTreeItems(dep, trv);
            }
        }


        //private void SaveToPng(Canvas canvas)
        //{
        //    Rect bounds = VisualTreeHelper.GetDescendantBounds(canvas);
        //    double dpi = 96d;

        //    RenderTargetBitmap rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, dpi, dpi, System.Windows.Media.PixelFormats.Default);

        //    DrawingVisual dv = new DrawingVisual();
        //    using (DrawingContext dc = dv.RenderOpen())
        //    {
        //        VisualBrush vb = new VisualBrush(canvas);
        //        dc.DrawRectangle(vb, null, new Rect(new System.Windows.Point(), bounds.Size));
        //    }

        //    rtb.Render(dv);

        //    BitmapEncoder pngEncoder = new PngBitmapEncoder();
        //    pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

        //    try
        //    {
        //        System.IO.MemoryStream ms = new System.IO.MemoryStream();

        //        pngEncoder.Save(ms);
        //        ms.Close();

        //        System.IO.File.WriteAllBytes(pointSelected.Id.ToString()+".png", ms.ToArray());
        //    }
        //    catch (Exception err)
        //    {
        //        MessageBox.Show(err.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}



    }
}
