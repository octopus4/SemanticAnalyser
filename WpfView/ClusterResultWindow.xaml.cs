using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using DataPreprocessing.Data;
using SOM;
using Visualisation;
using Visualisation.WPF;

namespace WpfView
{
    /// <summary>
    /// Логика взаимодействия для ClusterResultWindow.xaml
    /// </summary>
    public partial class ClusterResultWindow : Window, IView
    {
        /// <summary>
        /// Visual map image size (width or height)
        /// </summary>
        private static readonly int MapImageSize = 600;

        private MapPresenter Map { get; }

        public ClusterResultWindow(DataSource source, ClusterizationResult result, string[] headers)
        {
            InitializeComponent();
            CanvasCreator creator = new WpfCanvasCreator();
            Map = new MapPresenter(MapImageSize, MapImageSize, source, creator, headers, this)
            {
                Index = 1
            };
            Map.Init(result.NeuronsToTokensMap);
        }

        private void PictureBoxClustersMouseClick(object sender, MouseEventArgs e)
        {
            Point position = e.GetPosition(this);
            int x = (int)(position.X * Map.Width * 1.0 / pictureBoxClusters.Width);
            int y = (int)(position.Y * Map.Height * 1.0 / pictureBoxClusters.Height);

            Map.MouseDown(x, y);
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

        public void Update(Canvas canvas)
        {
            pictureBoxClusters.Source = (ImageSource)canvas.Render();
            pictureBoxClusters.InvalidateVisual();
        }
    }
}
