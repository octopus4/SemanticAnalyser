using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using Microsoft.Win32;
using DataProcessing.Data;
using DataProcessing.Data.Semantic;
using DataProcessing.Distance;
using DataProcessing.Distance.Semantic;
using SOM;
using SOM.Semantics;
using Visualisation;
using Visualisation.WPF;
using Visualisation.Graph;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Input;

namespace WpfView
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Visual map image size (width or height)
        /// </summary>
        private static readonly int MapImageSize = 600;
        /// <summary>
        /// Maximal width of a context in percents from a dictionary
        /// </summary>
        private static readonly int MaxContextWidth = 50;
        /// <summary>
        /// Maximal depth of a context (number of words on left or right side)
        /// </summary>
        private static readonly int MaxContextDepth = 3;

        /// <summary>
        /// Message shown if a user tries to get cluster results while the learning process is not ended yet
        /// </summary>
        private static readonly string ResultsExceptionMessage = "Текущий процесс обучения ещё не закончен!";

        /// <summary>
        /// Title of the chart's X axis
        /// </summary>
        private static readonly string EpochChartTitle = "Эпоха обучения";
        /// <summary>
        /// Title of the chart's Y axis
        /// </summary>
        private static readonly string ErrorChartTitle = "Ошибка обучения";

        /// <summary>
        /// Message shown if a user tries to put incorrect value in context width text field
        /// </summary>
        private static readonly string ContextWidthMessage = "Ширина контекста должна быть положительной и не превышать половины размера словаря";
        /// <summary>
        /// Message shown if a user tries to put incorrect value in context depth text field
        /// </summary>
        private static readonly string ContextDepthMessage = "Глубина контекста должна быть положительной и не превышать трёх";

        /// <summary>
        /// Message shown if a user tries to put incorrect value in map width text field
        /// </summary>
        private static readonly string WidthValidationMessage = "Ширина карты должна быть положительным числом";
        /// <summary>
        /// Message shown if a user tries to put incorrect value in map height text field
        /// </summary>
        private static readonly string HeightValidationMessage = "Высота карты должна быть положительным числом";
        /// <summary>
        /// Message shown if a user tries to put incorrect value in epochs count text field
        /// </summary>
        private static readonly string EpochValidationMessage = "Число эпох должно быть положительным числом";

        /// <summary>
        /// Filter for an open file dialog window
        /// </summary>
        private static readonly string SourceFileFilter = "Datasource text file (*.txt)|*.txt";
        /// <summary>
        /// Headers for a semantic data source
        /// </summary>
        private static readonly string[] SemanticHeaders = new string[] { "Слово", "Левый контекст", "Правый контекст" };

        /// <summary>
        /// Current headers
        /// </summary>
        private string[] Headers { get; set; }
        /// <summary>
        /// Semantic data source
        /// </summary>
        private DataSource Source { get; set; }
        /// <summary>
        /// Factory, that creates neurons
        /// </summary>
        private INeuronFactory NeuronFactory { get; set; }
        /// <summary>
        /// Factory, that creates distance functions
        /// </summary>
        private IDistanceFunctionFactory DistanceFactory { get; set; }
        /// <summary>
        /// Som Trainer, that trains <see cref="KohonenMap"/> in another thread
        /// </summary>
        private SomTrainer Trainer { get; set; }
        /// <summary>
        /// Instance of the <see cref="KohonenMap"/>
        /// </summary>
        private KohonenMap Map { get; set; }
        /// <summary>
        /// Data about errors
        /// </summary>
        private ObservableCollection<KeyValuePair<int, double>> Data { get; set; }

        private VisualGraph Graph { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Data = new ObservableCollection<KeyValuePair<int, double>>();
            SetupMapParams();
            SetupTrainer();
            SetupChart();
        }

        private void SetupMapParams()
        {
            NeuronFactory = new SemanticNeuronFactory();
            DistanceFactory = new SemanticDistanceFunctionFactory();
        }

        private void SetupTrainer()
        {
            Trainer = SomTrainer.GetInstance();
            Trainer.ThreadTick += TrainerThreadTick;
            Trainer.ThreadDone += TrainerThreadDone;
        }

        private void SetupChart()
        {
            errorChart.Axes.Add(new LinearAxis() { Title = EpochChartTitle, Orientation = AxisOrientation.X, Minimum = 1 });
            errorChart.Axes.Add(new LinearAxis() { Title = ErrorChartTitle, Orientation = AxisOrientation.Y, Minimum = 0});
            errorChart.Series[0].LegendItems.Clear();
            errorChart.DataContext = Data;
        }

        private void TrainerThreadDone(object sender, EventArgs e)
        {
            Dispatcher.Invoke
            (
                () => { ButtonGetClusterResultClick(sender, new RoutedEventArgs()); }
            );
        }

        private void TrainerThreadTick(object sender, EventArgs e)
        {
            Dispatcher.Invoke
            (
                () => { Data.Add(new KeyValuePair<int, double>(Trainer.Epoch, Trainer.Error)); }
            );
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog() { Filter = SourceFileFilter };
            fileDialog.FileOk += FileDialogFileOk;
            fileDialog.ShowDialog();
        }

        private void FileDialogFileOk(object sender, CancelEventArgs e)
        {
            OpenFileDialog fileDialog = (OpenFileDialog)sender;
            StringBuilder textBuilder = new StringBuilder();
            using (StreamReader reader = new StreamReader(fileDialog.OpenFile(), Encoding.Default))
            {
                while (!reader.EndOfStream)
                {
                    textBuilder.Append(reader.ReadLine());
                }
            }
            textBoxSource.Text = textBuilder.ToString();
        }

        private void ButtonCreateLearnClick(object sender, RoutedEventArgs e)
        {
            int? width = ValidateTextBox(textBoxWidth, WidthValidationMessage);
            int? height = ValidateTextBox(textBoxHeight, HeightValidationMessage);
            int? epochsCount = ValidateTextBox(textBoxEpochs, EpochValidationMessage);

            int? contextWidth = ValidateTextBox(textBoxContextWidth, ContextWidthMessage, upper: MaxContextWidth);
            int? contextDepth = ValidateTextBox(textBoxContextDepth, ContextDepthMessage, upper: MaxContextDepth);

            if (!width.HasValue || !height.HasValue || !epochsCount.HasValue ||
                !contextWidth.HasValue || !contextDepth.HasValue)
            {
                return;
            }

            Data.Clear();
            Headers = SemanticHeaders;
            Source = new SemanticDataSource(textBoxSource.Text, contextWidth.Value, contextDepth.Value);
            ((NumericAxis)errorChart.Axes[0]).Maximum = epochsCount.Value;
            Metric metric = Metric.Manhattan;

            Map = new KohonenMap(width.Value, height.Value, epochsCount.Value, Source, metric, NeuronFactory, DistanceFactory);

            Trainer.Start(Map);
        }

        private int? ValidateTextBox(TextBox textBox, string message, int lower = 0, int? upper = null)
        {
            if (int.TryParse(textBox.Text, out int result) && result > lower)
            {
                if (!upper.HasValue || result <= upper.Value)
                {
                    return result;
                }
            }

            MessageBox.Show(message);
            return null;
        }

        private void ButtonGetClusterResultClick(object sender, RoutedEventArgs e)
        {
            if (!Trainer.IsLearningStopped)
            {
                MessageBox.Show(ResultsExceptionMessage);
                return;
            }

            CanvasCreator creator = new WPFCanvasCreator();
            VisualMap visualMap = new VisualMap(MapImageSize, MapImageSize, Source, creator, Headers);

            visualMap.Init(Map.Result.NeuronsToTokensMap);

            CreateGraph();
            DrawGraph();

            Window clusterResultForm = new ClusterResultWindow(visualMap, Source.FlowTypes);
            clusterResultForm.ShowDialog();
        }

        private void ButtonStopLearningClick(object sender, RoutedEventArgs e)
        {
            if (!Trainer.IsLearningStopped)
            {
                Trainer.Stop();
            }
        }

        private void PictureBoxGraphMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                pictureBoxGraph.Width *= 1.25;
                pictureBoxGraph.Height *= 1.25;
            }
            else
            {
                pictureBoxGraph.Width /= 1.25;
                pictureBoxGraph.Height /= 1.25;
            }
            CreateGraph();
            DrawGraph();
        }

        private void PictureBoxGraphMouseDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(pictureBoxGraph);
            Graph.MouseClick((int)(Math.Round(point.X)), (int)(Math.Round(point.Y)));
            DrawGraph();
        }

        private void CreateGraph()
        {
            CanvasCreator creator = new WPFCanvasCreator();

#warning optimize memory
            Graph = new VisualGraph((int)pictureBoxGraph.Width, (int)pictureBoxGraph.Height, Source, creator);
            Graph.Init(Map.Result.NeuronsToTokensMap);
        }

        private void DrawGraph()
        {
            WPFCanvas canvas = (WPFCanvas)Graph.Image;
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)pictureBoxGraph.Width, (int)pictureBoxGraph.Height, 96, 96, PixelFormats.Default);

            bmp.Render(canvas.Visual);
            pictureBoxGraph.Source = bmp;
            pictureBoxGraph.InvalidateVisual();
        }
    }
}
