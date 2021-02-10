using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class MathUtility
    {
        
        /// <summary>
        /// This methods converts matrix coordinates to array index.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="rowCount"></param>
        /// <param name="columnCount"></param>
        /// <returns></returns>
        public static int MatrixCoordsToArrayIndex(int row, int column, int columns)
        {
            int index = column + columns * row; 

            return index;
        }

        /// <summary>
        /// Converts an array index to matrix coordinates.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rowCount"></param>
        /// <param name="columnCount"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public static void ArrayIndexToMatrixCoords(int index, int columns, out int row, out int column)
        {
            
            row = index / columns;
            column = index % columns;
        }

    }

}
