﻿using UnityEngine;
using UnityEngine.Networking;

using System.Collections;
using System.Collections.Generic;

using Sirenix.OdinInspector;

namespace Codari.TTT
{
    public sealed class TTTGrid : SerializedNetworkBehaviour, IEnumerable<TTTCell>
    {
        [ReadOnly] // Comment this attribute out if editing is required (basicly if you fuck up the scene)
        [SerializeField]
        [TableMatrix(HorizontalTitle = "X axis", VerticalTitle = "Y axis", IsReadOnly = true, ResizableColumns = false)]
        private TTTCell[,] cells = new TTTCell[3, 3];

        public TTTCell this[Coordinate coordinate] => cells[coordinate.X, coordinate.Y];

        public IEnumerator<TTTCell> GetEnumerator()
        {
            foreach (TTTCell cell in cells)
            {
                yield return cell;
            }
        }

        #region Explicit Definitions

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}