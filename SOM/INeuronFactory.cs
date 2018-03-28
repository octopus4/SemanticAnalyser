﻿using DataProcessing.Data;

namespace SOM
{
    /// <summary>
    /// Abstract Kohonen Map neuron factory
    /// </summary>
    public interface INeuronFactory
    {
        /// <summary>
        /// Produces single neuron
        /// </summary>
        /// <param name="token">Basic data token</param>
        /// <param name="x">X coordinate on the map</param>
        /// <param name="y">Y coordinate on the map</param>
        /// <returns>Instance of <see cref="Neuron"/></returns>
        Neuron Produce(DataToken token, int x, int y);
    }
}
