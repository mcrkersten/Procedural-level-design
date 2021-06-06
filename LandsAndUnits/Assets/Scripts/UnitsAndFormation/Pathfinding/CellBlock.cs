using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnitsAndFormation
{
    public class CellBlock
    {
        public List<Cell> _cells = new List<Cell>();
        public int _gridSize;

        public Vector2Int _index;

        public CellBlock(Vector2Int index)
        {
            _index = index;
        }
    }
}
