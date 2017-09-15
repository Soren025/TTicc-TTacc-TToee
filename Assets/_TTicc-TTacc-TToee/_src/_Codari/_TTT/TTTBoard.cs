using UnityEngine;
using UnityEngine.Networking;

using System.Collections;
using System.Collections.Generic;

using Sirenix.OdinInspector;

namespace Codari.TTT
{
    public class TTTBoard : SerializedNetworkBehaviour, IEnumerable<TTTGrid>
    {
        [ReadOnly] // Comment this attribute out if editing is required (basicly if you fuck up the scene)
        [SerializeField]
        [TableMatrix(HorizontalTitle = "X axis", VerticalTitle = "Y axis", IsReadOnly = true, ResizableColumns = false)]
        private TTTGrid[,] grids = new TTTGrid[3, 3];

        public TTTGrid this[Coordinate coordinate] => grids[coordinate.X, coordinate.Y];

        public TTTCell this[Coordinate gridCoordinate, Coordinate cellCoordinate] => this[gridCoordinate][cellCoordinate];

        public IEnumerator<TTTGrid> GetEnumerator()
        {
            foreach (TTTGrid grid in grids)
            {
                yield return grid;
            }
        }

        #region Explicit Definitions

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
