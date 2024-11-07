using System;

namespace MyThings.Extension
{
    /// <summary>
    /// All the Layers related things
    /// </summary>
    public static class ExtensionLayers
    {
        /// <summary>
        /// Get Int Containing Data of Layers Stored
        /// </summary>
        /// <param name="layers">The Layer(s)</param>
        /// <returns>The Int Value</returns>
        public static int GetIntLayer(this int layers)
        {
            return 1 << layers;
        }
        /// <summary>
        /// Get Int Containing Data of Layers Stored
        /// </summary>
        /// <typeparam name="t">The Enum</typeparam>
        /// <param name="layers">The Layer(s)</param>
        /// <returns>The Int Value</returns>
        public static int GetIntLayer<t>(this t layers) where t : Enum
        {
            return 1 << (int)(object)layers;
        }
        /// <summary>
        /// Get Int Containing Data of Layers Stored
        /// </summary>
        /// <param name="layers">The Layer(s)</param>
        /// <returns>The Int Value</returns>
        public static int GetIntLayer(params int[] layers)
        {
            int layer = 0;
            for (int i = 0; i < layers.Length; i++)
            {
                layer |= 1 << layers[i];
            }
            return layer;
        }

        /// <summary>
        /// Get Int Containing Data of Layers Stored
        /// </summary>
        /// <param name="layers">The Layer(s)</param>
        /// <returns>The Int Value</returns>
        public static int GetIntLayer<t>( params t[] layers) where t : Enum
        {
            int layer = 0;
            for (int i = 0; i < layers.Length; i++)
            {
                layer |= 1 << (int)(object)layers[i];
            }
            return layer;
        }

        /// <summary>
        /// it checks if the layer is equal to another layer(s)
        /// </summary>
        /// <param name="MainLayer">layer to check </param>
        /// <param name="LayersToCheckTo"> layer(s) to check from</param>
        /// <returns>Main Layer is in Layer(s) To Check Form</returns>
        public static bool LayerChecker(int MainLayer, int LayersToCheckTo)
        {
            return (LayersToCheckTo & MainLayer) == MainLayer;
        }
    }
}