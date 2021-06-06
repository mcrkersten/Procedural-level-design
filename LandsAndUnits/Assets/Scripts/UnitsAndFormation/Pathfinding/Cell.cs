using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitsAndFormation
{
    public class Cell
    {
        public Vector3 _worldPosition;
        public Vector2Int _gridIndex;
        public byte _cost { get; private set; }
        public CellType _type { get; private set; }
        public CellType _lastType { get; private set; }

        public List<ushort> _intergrationLayers = new List<ushort>();
        public ushort _unitWeight = 0;

        public List<Cell> cardinalNeighbours;
        public List<Cell> allNeighbours;

        public Cell(Vector3 worldPosition, Vector2Int gridIndex)
        {
            _worldPosition = worldPosition;
            _gridIndex = gridIndex;
            _intergrationLayers.Add(0);
        }

        /// <summary>
        /// Changes the celltype and updates the basecost.
        /// </summary>
        /// <param name="type"></param>
        public void ChangeCellType(CellType type)
        {
            _lastType = _type;
            _type = type;
            _cost = CellCalculation.GetCellTypeCost(_type);
        }

        public void OnUnitEnter(ushort weight)
        {
            _unitWeight += weight;
        }

        public void OnUnitExit(ushort weight)
        {
            _unitWeight -= weight;
        }

        public void GetNeighbours(Cell[,] grid)
        {
            cardinalNeighbours = HelperFunctions.GetNeighbourCells(_gridIndex, GridDirection.CardinalDirections, GridController.Instance.GenesisField._gridSize, grid);
            allNeighbours = HelperFunctions.GetNeighbourCells(_gridIndex, GridDirection.AllDirections, GridController.Instance.GenesisField._gridSize, grid);
        }
    }

}
