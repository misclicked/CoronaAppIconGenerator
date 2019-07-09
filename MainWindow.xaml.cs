using System;
using System.Collections.Generic;
using System.IO;
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

namespace CoronaAppIconGenerator
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        private static List<Double> imageSize = new List<Double>() { 192, 144, 96, 72, 48, 36, 57 };
        private static List<string> fileName = new List<string>() { "Icon-xxxhdpi.png",
        "Icon-xxhdpi.png", "Icon-xhdpi.png", "Icon-hdpi.png", "Icon-mdpi.png", "Icon-ldpi.png", "icon.png"};

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConvertToAppIcon(string imagePath)
        {
            try
            {
                BitmapImage bmp = new BitmapImage(new Uri(imagePath));
                Directory.CreateDirectory("output");
                for (int i = 0; i < fileName.Count; i++)
                {
                    BitmapFrame bf = BitmapFrame.Create(new TransformedBitmap(bmp,
                        new ScaleTransform(
                            imageSize[i] / bmp.PixelWidth,
                            imageSize[i] / bmp.PixelHeight)));
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(bf);
                    using (var stream = File.Create(@".\output\" + fileName[i]))
                    {
                        encoder.Save(stream);
                    }
                }
                System.Diagnostics.Process.Start(@".\output");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please input a image file");
            }
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Count() > 1)
                {
                    MessageBox.Show("Please drop only 1 file at once");
                }
                else
                {
                    ConvertToAppIcon(files[0]);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        { 
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "Image Files (*.jpeg;*.png;*.jpg;*.gif)|*.jpeg;*.png;*.jpg;*.gif";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                ConvertToAppIcon(filename);
            }
        }
    }
}
