using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace UnitsAndFormation
{
    [System.Serializable]
    public class FlowField
    {
        //Public used variables
        public Cell[,] _grid { get; private set; }
        public List<Cell> _allCells { get; private set; }
        private int _newestIntergrationLayer = 0;
        public int _gridSize { get; private set; }
        public float _cellRadius { get; private set; }

        public List<UnitInteractable> _allUnitInteractables = new List<UnitInteractable>();

        public delegate void GenericFlowfieldCreation();
        public static event GenericFlowfieldCreation OnGenesisFieldCreated;

        public FlowField(float cellRadius)
        {
            _cellRadius = cellRadius;
            _gridSize = MapGenerator.mapChunkSize;
        }

        public void InitializeGenesis()
        {
            CreateGrid();
            CreateCostField();
            OnGenesisFieldCreated?.Invoke();
        }

        /// <summary>
        /// Creates a grid of cells the size of _gridSize * _gridSize
        /// </summary>
        private void CreateGrid()
        {
            _allCells = new List<Cell>();
            _grid = new Cell[_gridSize, _gridSize];
            float xx = (-_cellRadius * 2 * _gridSize) / 2;
            float yy = (-_cellRadius * 2 * _gridSize) / 2;

            for (int x = 0; x < _gridSize; x++)
            {
                for (int y = 0; y < _gridSize; y++)
                {
                    Vector3 worldPos = new Vector3(xx + (_cellRadius * 2) * x, 0, yy + (_cellRadius * 2) * y);
                    _grid[x, y] = new Cell(worldPos, new Vector2Int(x, y));
                    _allCells.Add(_grid[x, y]);
                }
            }

            for (int x = 0; x < _gridSize; x++)
            {
                for (int y = 0; y < _gridSize; y++)
                {
                    _grid[x, y].GetNeighbours(_grid);
                }
            }
        }

        /// <summary>
        /// Sets the base cost of all cell's based on the cost of Celltype
        /// </summary>
        private void CreateCostField()
        {
            for (int x = 0; x < _gridSize; x++)
            {
                for (int y = 0; y < _gridSize; y++)
                {
                    _grid[x, y].ChangeCellType(MapGenerator.Instance.cellTypes[x, y]);
                }
            }
        }
        public int AddNewIntergrationLayer()
        {
            foreach (Cell cell in _allCells)
                cell._intergrationLayers.Add(ushort.MaxValue);
            _newestIntergrationLayer++;
            return _newestIntergrationLayer;
        }
        private void ResetIntergrationLayer(int layerToReset)
        {
            foreach (Cell cell in _allCells)
                cell._intergrationLayers[layerToReset] = ushort.MaxValue;
        }

        public void UpdateSignleIntergrationLayer(Vector3 destinationPosition, int requestedLayer)
        {
            ResetIntergrationLayer(requestedLayer);
            JobHandle job = UpdateIntergrationLayerJob(destinationPosition, requestedLayer);
            job.Complete();
        }

        public void UpdateSignleIntergrationLayer(UnitInteractable destinationPosition, int requestedLayer)
        {
            ResetIntergrationLayer(requestedLayer);
            JobHandle job = UpdateIntergrationLayerJob(destinationPosition._targetTransform.position, requestedLayer);
            job.Complete();
        }

        public void UpdateAllUnitInteractableIntergrationLayers()
        {
            NativeList<JobHandle> jobHandleList = new NativeList<JobHandle>(Allocator.Temp);
            foreach (UnitInteractable interactable in _allUnitInteractables)
            {
                if(interactable._intergrationLayer == 0)
                {
                    interactable._intergrationLayer = AddNewIntergrationLayer();
                }
                JobHandle job = UpdateIntergrationLayerJob(interactable._targetTransform.position, interactable._intergrationLayer);
                jobHandleList.Add(job);
            }
            JobHandle.CompleteAll(jobHandleList);
            jobHandleList.Dispose();
        }

        private JobHandle UpdateIntergrationLayerJob(Vector3 destinationPosition, int requestedLayer)
        {
            UpdateIntergrationLayer job = new UpdateIntergrationLayer(destinationPosition, requestedLayer);
            return job.Schedule();
        }

        /// <summary>
        /// Set and get the best direction of the given cell, based on all direct neighbours.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns>The direction to the cheapest neighbour.</returns>
        public GridDirection GetBestDirectionOfCell(Cell cell, int intergrationLayer)
        {
            List<Cell> currentNeighbours = cell.allNeighbours;
            int bestCost = int.MaxValue;

            //Find cheapest cell
            Cell bestCell = null;
            foreach (Cell currentNeighbour in currentNeighbours)
            {
                if (currentNeighbour._intergrationLayers[intergrationLayer] + currentNeighbour._unitWeight < bestCost)
                {
                    bestCost = currentNeighbour._intergrationLayers[intergrationLayer] + currentNeighbour._unitWeight;
                    bestCell = currentNeighbour;
                }
            }

            GridDirection direction = GridDirection.GetDirectionFromV2I(bestCell._gridIndex - cell._gridIndex);
            if (direction != null)
                return direction;
            else
                return GridDirection.None;
        }

        public Cell GetCellFromWorldPos(Vector3 worldPos)
        {
            float xx = ((_cellRadius * 2) * _gridSize);
            float yy = ((_cellRadius * 2) * _gridSize);

            Vector3 center = new Vector3(xx / 2, 0, yy / 2);
            Vector3 clickPoint = center + worldPos;


            float percentX = clickPoint.x / (_gridSize * (_cellRadius * 2));
            float percentY = clickPoint.z / (_gridSize * (_cellRadius * 2));

            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.Clamp(Mathf.RoundToInt((_gridSize) * percentX), 0, _gridSize);
            int y = Mathf.Clamp(Mathf.RoundToInt((_gridSize) * percentY), 0, _gridSize);
            return _grid[x, y];
        }

        public void UpdateCells(List<Cell> cellsToUpdate, CellType type)
        {
            foreach (Cell cell in cellsToUpdate)
            {
                cell.ChangeCellType(type);
            }
            GridController.Instance._lateUpdateFlowfield = true;
        }
        public void ResetCells(List<Cell> cellsToReset)
        {
            foreach (Cell cell in cellsToReset)
            {
                cell.ChangeCellType(cell._lastType);
            }
            GridController.Instance._lateUpdateFlowfield = true;
        }
    }

    [BurstCompatible]
    public struct UpdateIntergrationLayer : IJob
    {
        Vector3 _destination;
        int _intergrationLayer;
        public UpdateIntergrationLayer(Vector3 destination, int intergrationLayer)
        {
            _destination = destination;
            _intergrationLayer = intergrationLayer;
        }

        public void Execute()
        {
            Cell destinationCell = GridController.Instance.GenesisField.GetCellFromWorldPos(_destination);
            destinationCell._intergrationLayers[_intergrationLayer] = 0;

            Queue<Cell> cellsToCheck = new Queue<Cell>();
            cellsToCheck.Enqueue(destinationCell);

            while (cellsToCheck.Count > 0)
            {
                Cell currentCell = cellsToCheck.Dequeue();

                List<Cell> currentNeighbours = currentCell.cardinalNeighbours;
                foreach (Cell currentNeighbour in currentNeighbours)
                {
                    //Cell's turn did not yet come. So always further away than current cell. 
                    if (currentNeighbour._cost == byte.MaxValue) { continue; }

                    if (currentNeighbour._cost + currentCell._intergrationLayers[_intergrationLayer] < currentNeighbour._intergrationLayers[_intergrationLayer])
                    {
                        currentNeighbour._intergrationLayers[_intergrationLayer] = (ushort)(currentNeighbour._cost + currentCell._intergrationLayers[_intergrationLayer]);
                        cellsToCheck.Enqueue(currentNeighbour);
                    }
                }
            }
        }
    }
}

