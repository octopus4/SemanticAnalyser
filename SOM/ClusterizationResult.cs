using System.Collections.Generic;
using DataProcessing.Data;

namespace SOM
{
    public class ClusterizationResult
    {
        private KohonenMap Map { get; set; }
        private DataSource Source { get; set; }

        private Dictionary<Neuron, List<DataToken>> _result;

        public Dictionary<Neuron, List<DataToken>> NeuronsToTokensMap
        {
            get
            {
                if (_result == null)
                {
                    _result = Calculate();
                }
                return _result;
            }
        }

        public ClusterizationResult(KohonenMap map, DataSource source)
        {
            Map = map;
            Source = source;
        }

        private Dictionary<Neuron, List<DataToken>> Calculate()
        {
            Dictionary<Neuron, List<DataToken>> result = new Dictionary<Neuron, List<DataToken>>();
            foreach (Neuron neuron in Map.Neurons)
            {
                result.Add(neuron, new List<DataToken>());
            }
            foreach (DataToken token in Source)
            {
                Neuron bmu = Map.FindBMU(token);
                result[bmu].Add(token);
            }
            return result;
        }
    }
}
