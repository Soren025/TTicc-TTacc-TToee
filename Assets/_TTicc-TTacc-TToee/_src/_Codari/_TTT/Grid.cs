using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Codari.TTT
{
    public sealed class Grid : NetworkBehaviour
    {
        //[SerializeField]
        //private Coordinate coordinate;

        //[SerializeField]
        //private Text gridWinnerIcon;

        //[SyncVar]
        //private Selection gridWinner = Selection.None;

        //private Cell[,] cells = new Cell[3, 3];

        //private Image gridHighlight;

        //public Coordinate Coordinate => coordinate;

        //public Selection GridWinner => gridWinner;

        //public bool IsCurrent => TTTApplication.Instance.CurrentGrid == coordinate;

        //public bool IsFull => this.All(cell => cell.Selection.IsSelected());

        //public Cell this[Coordinate coordinate] => cells[coordinate.X, coordinate.Y];

        //[Server]
        //public void SetGridWinner(Selection gridWinner)
        //{
        //    this.gridWinner = gridWinner;
        //}

        //#region Rpcs

        //private void Rpc_SetHighlighted(bool highlighted)
        //{
        //    gridHighlight.enabled = highlighted;
        //}

        //#endregion

        //#region Unity Callbacks

        //void Awake()
        //{
        //    foreach (Cell cell in GetComponentsInChildren<Cell>(true))
        //    {
        //        cells[cell.Coordinate.X, cell.Coordinate.Y] = cell;
        //    }

        //    gridHighlight = GetComponent<Image>();
        //}

        //void LateUpdate()
        //{
        //    gridHighlight.enabled = !IsFull
        //        && (IsCurrent
        //            || (TTTApplication.Instance.CurrentGrid.IsValid ? TTTApplication.Instance[TTTApplication.Instance.CurrentGrid].IsFull : false));

        //    if (gridHighlight.enabled)
        //    {
        //        gridHighlight.color = TTTPlayer.Me.IsMyTurn ? TTTApplication.Instance.MeColor : TTTApplication.Instance.NotMeColor;
        //    }

        //    gridWinnerIcon.enabled = gridWinner.IsSelected();
        //    if (gridWinnerIcon.enabled)
        //    {
        //        gridWinnerIcon.text = gridWinner.ToString();

        //        Color gridWinnerIconColor = TTTPlayer.Me.Team == gridWinner ? TTTPlayer.Me.SelectionColor : TTTPlayer.NotMe.SelectionColor;
        //        gridWinnerIconColor.a = 0.4f;

        //        gridWinnerIcon.color = gridWinnerIconColor;
        //    }
        //}

        //#endregion

        //public IEnumerator<Cell> GetEnumerator()
        //{
        //    foreach (Cell cell in cells)
        //    {
        //        yield return cell;
        //    }
        //}

        //IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}