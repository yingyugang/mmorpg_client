using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ps2D
{

    /// <summary>
    /// A layer.  Could be a sprite, could be a group.
    /// </summary>
    [System.Serializable]
    public class Layer
    {

        /// <summary>
        /// The photoshop layer id.
        /// </summary>
        public int photoshopLayerId { get; set; }

        /// <summary>
        /// The raw name of the photoshop layer.
        /// </summary>
        public string photoshopLayerName { get; set; }

        /// <summary>
        /// A unique string indicating hierarchy used for filtering.  I fear algorithms.
        /// </summary>
        public string hierarchyString { get; set; }

        /// <summary>
        /// The bounds of this layer.
        /// </summary>
        public PixelBounds bounds { get; set; }

        /// <summary>
        /// Is this visible in photoshop?
        /// </summary>
        public bool isVisible { get; set; }

        /// <summary>
        /// A list of children layers.
        /// </summary>
        public List<Layer> layers { get; set; }

        /// <summary>
        /// The order of layers.
        /// </summary>
        public int order { get; set; }

        /// <summary>
        /// The indent level.  How deeply nested is this?
        /// </summary>
        public int indentLevel { get; set; }

        /// <summary>
        /// The parent layer for this layer.
        /// </summary>
        public Layer parentLayer { get; set; }

        /// <summary>
        /// Get a list of guesses at to what the sprite name could be. 
        /// </summary>
        /// <returns>a list of guesses</returns>
        public List<string> GetGuessesForSpriteName()
        {
            List<string> guesses = new List<string>();

            // the delimiters in the layer name
            char[] delimiters = new char[] { ',' };

            // get a list of options inside the layer name
            var possibilities = photoshopLayerName.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            // which each csv separated
            foreach (string possibility in possibilities)
            {
                // for this attempt
                string attempt = possibility;

                // delete the file extensions
                attempt = Regex.Replace(attempt, @"\..*", "", RegexOptions.IgnoreCase);

                // delete resizing prefixes
                attempt = Regex.Replace(attempt, @"([0-9])*%", "");

                // clean it up
                attempt = attempt.Trim();

                guesses.Add(attempt);
            }

            return guesses;
        }


    }

}