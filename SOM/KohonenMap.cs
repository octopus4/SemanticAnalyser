using DataPreprocessing.Data;
using DataPreprocessing.Distance;
using DataPreprocessing.Solutions;

using SOM.Topologies;
using SOM.Neighborhoods;
using SOM.Learnings;

using System;
using System.Linq;
using System.Collections.Generic;
using DataPreprocessing.Data.Semantic;

namespace SOM
{
    /// <summary>
    /// Kohonen map for an abstract data
    /// </summary>
    public class KohonenMap : ISolver
    {
        /// <summary>
        /// Width of a map
        /// </summary>
        private int Width { get; }
        /// <summary>
        /// Height of a map
        /// </summary>
        private int Height { get; }

        /// <summary>
        /// Total number of epochs
        /// </summary>
        private int EpochCount { get; }
        /// <summary>
        /// Current learning epoch
        /// </summary>
        private int Epoch { get; set; }

        /// <summary>
        /// Source of data tokens
        /// </summary>
        private DataSource Source { get; }

        /// <summary>
        /// Topology
        /// </summary>
        private Topology Topology { get; }
        /// <summary>
        /// Neighborhood distance function
        /// </summary>
        private INeighborhoodFunction Neighborhood { get; }
        /// <summary>
        /// Learning diminishing function
        /// </summary>
        private ILearningFunction LearningRateFunction { get; }
        /// <summary>
        /// Weights distance function
        /// </summary>
        private DistanceFunction WeightsDistance { get; }

        /// <summary>
        /// Result of clusterization
        /// </summary>
        public ClusterizationResult Result { get; private set; }

        /// <summary>
        /// List of neurons
        /// </summary>
        public List<Neuron> Neurons { get; private set; }
        /// <summary>
        /// BMU for concrete epoch
        /// </summary>
        private Neuron BMU { get; set; }

        /// <summary>
        /// Is the map already learned
        /// </summary>
        public bool IsLearned
        {
            get { return Epoch >= EpochCount; }
        }

        /// <summary>
        /// Initializes an instance of <see cref="KohonenMap"/>
        /// </summary>
        /// <param name="width">Width of map</param>
        /// <param name="height">Height of map</param>
        /// <param name="epochCount">Total number of learning epochs</param>
        /// <param name="source">Source of data tokens</param>
        /// <param name="metric">Distance metric</param>
        /// <param name="neuronFactory">Neuron factory</param>
        /// <param name="distanceFactory">Distance function factory</param>
        public KohonenMap
            (int width, int height, int epochCount, DataSource source, Metric metric, INeuronCreator neuronFactory, IDistanceFunctionCreator distanceFactory)
        {
            Width = width;
            Height = height;

            EpochCount = epochCount;
            Epoch = 0;

            Source = source;
            Topology = new RectangleTopology(metric);

            LearningRateFunction = source.Length > 50 ?
                (ILearningFunction)new InverseLearningFunction(source.Length) :
                new LinearLearningFunction(EpochCount);
            Neighborhood = new GaussianNeighborhoodFunction();

            WeightsDistance = distanceFactory.Create(metric);

            InitNeurons(neuronFactory);
        }

        /// <summary>
        /// Initializes neurons of the map
        /// </summary>
        /// <param name="factory">Abstract neuron factory</param>
        private void InitNeurons(INeuronCreator factory)
        {
            Neurons = new List<Neuron>();
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    DataToken token = Source.GetRandomToken();
                    Neuron neuron = factory.Produce(j, i);
                    neuron.InitWeights(token);
                    Neurons.Add(neuron);
                }
            }
        }

        /// <summary>
        /// Learns single epoch
        /// </summary>
        /// <exception cref="MapIsLearnedException">Is thrown if you try to learn epoch when map is already learned</exception>
        /// <returns>Average learning error</returns>
        public double LearnEpoch()
        {
            if (IsLearned)
            {
                throw new MapIsLearnedException("Map is already learned", Epoch, EpochCount);
            }

            double error = 0;
            Dictionary<double, double> speedFactorMap = CalculateSpeedFactorMap();
            foreach (DataToken token in Source)
            {
                error += Math.Abs(FindWinner(token));
                CorrectMap(token, speedFactorMap);
            }
            Epoch++;

            return error / Source.Length;
        }

        /// <summary>
        /// Ends the learning by creating an instance of <see cref="ClusterizationResult"/>
        /// </summary>
        public void EndLearning()
        {
            Result = new ClusterizationResult(this, Source);
        }

        /// <summary>
        /// Finds the best matching unit in neurons for this token
        /// </summary>
        /// <param name="token">Instance of <see cref="DataToken"/></param>
        /// <returns>Error as a distance between points in an n-dim space</returns>
        private double FindWinner(DataToken token)
        {
            BMU = Neurons[0];
            double distance = WeightsDistance.Calculate(BMU.Weights, token.Values);
            for (int i = 1; i < Neurons.Count; i++)
            {
                double newDistance = WeightsDistance.Calculate(Neurons[i].Weights, token.Values);
                if (Math.Abs(newDistance) < Math.Abs(distance))
                {
                    BMU = Neurons[i];
                    distance = newDistance;
                }
            }

            return distance;
            /* actually, this is an error - it's distance in n-dimensional space between
             * vector of neuron's weights and vector of token's weights (data in DataToken)
             * */
        }

        /// <summary>
        /// Finds the best matching unit in neurons of clustering results for this token
        /// </summary>
        /// <param name="token">Instance of <see cref="DataToken"/></param>
        /// <param name="clusteringResults">Results of clustering</param>
        /// <returns></returns>
        public Neuron FindBMU(DataToken token, Dictionary<Neuron, List<DataToken>> clusteringResults = null)
        {
            Neuron bmu = Neurons[0];
            int i = 1;
            Func<bool> isClusterNotEmpty = clusteringResults != null ?
                () => clusteringResults[Neurons[i]].Count > 0 : (Func<bool>)null;
            double distance = WeightsDistance.Calculate(bmu.Weights, token.Values);
            for (i = 1; i < Neurons.Count; i++)
            {
                double newDistance = WeightsDistance.Calculate(Neurons[i].Weights, token.Values);
                if (Math.Abs(newDistance) < Math.Abs(distance)
                   && (isClusterNotEmpty == null || isClusterNotEmpty.Invoke()))
                {
                    bmu = Neurons[i];
                    distance = newDistance;
                }
            }
            return bmu;
        }

        /// <summary>
        /// Calculates SpeedFactor Dictionary, which maps distances between neurons on learning diminishing factors
        /// </summary>
        /// <returns></returns>
        private Dictionary<double, double> CalculateSpeedFactorMap()
        {
            Dictionary<double, double> result = new Dictionary<double, double>();
            foreach (Neuron neuron1 in Neurons)
            {
                foreach (Neuron neuron2 in Neurons)
                {
                    double distance = Topology.Distance(neuron1.Position, neuron2.Position);
                    if (!result.ContainsKey(distance))
                    {
                        double diminishingFactor = LearningRateFunction.Calculate(Epoch) * Neighborhood.Calculate(distance);
                        result.Add(distance, diminishingFactor);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Corrects Kohonen Map by correcting weights of its neurons
        /// </summary>
        /// <param name="token">Instance of <see cref="DataToken"/></param>
        /// <param name="speedFactorMap">Map of learning diminishing factors</param>
        private void CorrectMap(DataToken token, Dictionary<double, double> speedFactorMap)
        {
            for (int i = 0; i < Neurons.Count; i++)
            {
                double distance = Topology.Distance(BMU.Position, Neurons[i].Position);
                double diminishingFactor = speedFactorMap[distance];
                Neurons[i].CorrectWeights(token, diminishingFactor);
            }
        }

        /// <summary>
        /// Tries to classificate some input data token
        /// </summary>
        /// <param name="token">Instance of <see cref="DataToken"/></param>
        /// <returns>Name of the data token class, if it exists</returns>
        public string Classify(DataToken token)
        {
            Neuron winner = FindBMU(token, Result.NeuronsToTokensMap);
            return GetClass(winner);
        }

        /// <summary>
        /// Tries to define class of this neuron
        /// </summary>
        /// <param name="neuron">Instance of <see cref="Neuron"/></param>
        /// <returns></returns>
        private string GetClass(Neuron neuron)
        {
            var records = Result.NeuronsToTokensMap[neuron];
            if (records.Count > 0)
            {
                int index = GetClassIndex(records[0]);
                List<string> distinctTypes = records.Select(token => token.Values[index].ToString()).Distinct().ToList();
                string result = string.Empty;
                int max = 0;
                foreach (string type in distinctTypes)
                {
                    int recordsCount = records.Count(token => token.Values[index].ToString() == type);
                    if (recordsCount > max)
                    {
                        result = type;
                        max = recordsCount;
                    }
                }
                return result;
            }
            throw new ArgumentException("No winner for the input data exists");
        }

        /// <summary>
        /// Tries to find class index of the data token
        /// </summary>
        /// <param name="dataToken">Instance of <see cref="DataToken"/></param>
        /// <returns>Class (output) value index</returns>
        private int GetClassIndex(DataToken dataToken)
        {
            for (int i = 0; i < dataToken.Types.Length; i++)
            {
                if (dataToken.Flows[i] == DataFlow.Output)
                {
                    return i;
                }
            }
            throw new ArgumentException("Token does not contain CLASS attribute");
        }
    }
}
