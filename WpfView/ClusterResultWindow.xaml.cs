using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DataProcessing.Data;
using Visualisation;
using Visualisation.WPF;
using System.IO;
using Microsoft.Win32;
using System.ComponentModel;

namespace WpfView
{
    /// <summary>
    /// Логика взаимодействия для ClusterResultWindow.xaml
    /// </summary>
    public partial class ClusterResultWindow : Window
    {
        private VisualMap Map { get; }

        private int NeuronWidth { get; set; }
        private int NeuronHeight { get; set; }

        private int TokenClassIndex { get; set; }

        public ClusterResultWindow(VisualMap visualMap, DataFlow[] flowTypes)
        {
            InitializeComponent();

            Map = visualMap;
            Map.Index = 1;
            DrawMap();
        }

        private object[] FilterHeaders(string[] headers, DataFlow[] flowTypes)
        {
            List<object> result = new List<object>();
            for (int i = 0; i < flowTypes.Length; i++)
            {
                if (flowTypes[i] != DataFlow.NonUsed)
                {
                    result.Add(headers[i]);
                }
            }
            return result.ToArray();
        }

        private void ComboBoxMapsSelectedIndexChanged(object sender, EventArgs e)
        {
            Map.Index = 1;
            DrawMap();
        }

        private void DrawMap()
        {
            WPFCanvas canvas = (WPFCanvas)Map.Show();
            RenderTargetBitmap bmp = new RenderTargetBitmap(Map.Width, Map.Height, 96, 96, PixelFormats.Default);
            bmp.Render(canvas.Visual);
            pictureBoxClusters.Source = bmp;
            pictureBoxClusters.InvalidateVisual();
        }

        private void PictureBoxClustersMouseClick(object sender, MouseEventArgs e)
        {
            Point position = e.GetPosition(this);
            int x = (int)(position.X * Map.Width * 1.0 / pictureBoxClusters.Width);
            int y = (int)(position.Y * Map.Height * 1.0 / pictureBoxClusters.Height);

            sender = 1;
            Map.MouseClick(sender, x, y);
        }

        private void SaveAsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.FileOk += FileDialogOk;
            fileDialog.ShowDialog();
        }

        private void FileDialogOk(object sender, CancelEventArgs e)
        {
            SaveFileDialog fileDialog = (SaveFileDialog)sender;
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)pictureBoxClusters.Source));
            encoder.Save(fileDialog.OpenFile());
        }

        private void CloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {

        }
    }
}
