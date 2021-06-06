using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UnitsAndFormation
{
    public static class HelperFunctions
    {

        //-----------------------------
        // CLICKER HELPERS
        //-----------------------------

        /// <summary>
        /// Convert the screenposition of the mouse to in scene 3D space
        /// </summary>
        /// <param name="mousePosition">Position of mouse on screen</param>
        /// <param name="layerMask">Only measure from objects with this layerMask</param>
        /// <returns>3D position of mouse on object with given LayerMask</returns>
        public static Vector3 GetMousePositionIn3D(Vector3 mousePosition, LayerMask layerMask)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 500f, layerMask))
                return hit.point;

            return -Vector3.zero;
        }

        //-----------------------------
        // CELL HELPERS
        //-----------------------------

        public static Cell GetCellAtRelativePos(Vector2Int orignPos, Vector2Int relativePos, int gridSize, Cell[,] grid)
        {
            Vector2Int finalPos = orignPos + relativePos;

            if (finalPos.x < 0 || finalPos.x >= gridSize || finalPos.y < 0 || finalPos.y >= gridSize)
            {
                return null;
            }

            else { return grid[finalPos.x, finalPos.y]; }
        }

        public static List<Cell> GetNeighbourCells(Vector2Int nodeIndex, List<GridDirection> directions, int gridSize, Cell[,] grid)
        {
            List<Cell> neighborCells = new List<Cell>();

            foreach (Vector2Int curDirection in directions)
            {
                Cell newNeighbor = HelperFunctions.GetCellAtRelativePos(nodeIndex, curDirection, gridSize, grid);
                if (newNeighbor != null)
                {
                    neighborCells.Add(newNeighbor);
                }
            }
            return neighborCells;
        }

        //-----------------------------
        // GROUP HELPERS
        //-----------------------------
        public static Vector3 GetCenterOfTransforms(List<Transform> transforms)
        {
            Vector3 sumPos = Vector3.zero;
            int x = 0;
            foreach (Transform t in transforms)
            {
                if(t != null)
                {
                    sumPos += t.position;
                    x++;
                }
            }
            return sumPos / transforms.Count;
        }


        //-----------------------------
        // UNITINTERACTABLE-SEARCH HELPERS
        //-----------------------------

        /// <summary>
        /// Find the object in the list that has the smallest distance to the given Vector3
        /// </summary>
        /// <param name="gameObjects">List of gameobjects</param>
        /// <param name="position">the vector3 to measure from</param>
        /// <returns>GameObject with smallest distance to the position Vector3</returns>
        public static GameObject GetClosestGameObjectInListToVector3(List<GameObject> gameObjects, Vector3 position)
        {
            float dist = float.MaxValue;
            GameObject closestInteractable = null;
            foreach (GameObject building in gameObjects)
            {
                float curDist = Vector3.Distance(building.transform.position, position);
                if (curDist < dist)
                {
                    dist = curDist;
                    closestInteractable = building;
                }
            }
            return closestInteractable;
        }

        /// <summary>
        /// </summary>
        /// <param name="buildingType">The type of Interactable that is wanted</param>
        /// <returns>List of gameObjects that all have a Interactable component of given buildingType</returns>
        public static List<GameObject> GetInteractableGameObjectList(InteractableType buildingType)
        {
            List<GameObject> buildings = new List<GameObject>();
            switch (buildingType)
            {
                case InteractableType.Construction:
                    List<Construction> a = BuildBuildingsLibrary.Instance._instantiatedConstructions;
                    foreach (Construction aa in a)
                        buildings.Add(aa.gameObject);
                    break;
                case InteractableType.Housing:
                    List<House> b = BuildBuildingsLibrary.Instance._instantiatedHouses;
                    foreach (House bb in b)
                        buildings.Add(bb.gameObject);
                    break;
                case InteractableType.Resource:
                    List<ResourceInteractable> c = BuildBuildingsLibrary.Instance._instantiatedResources;
                    foreach (ResourceInteractable cc in c)
                        buildings.Add(cc.gameObject);
                    break;
                case InteractableType.Workplace:
                    List<Workplace> d = BuildBuildingsLibrary.Instance._instantiatedWorkplaces;
                    foreach (Workplace dd in d)
                        buildings.Add(dd.gameObject);
                    break;
                case InteractableType.Storage:
                    List<Storage> e = BuildBuildingsLibrary.Instance._instantiatedStorages;
                    foreach (Storage ee in e)
                        buildings.Add(ee.gameObject);
                    break;
            }
            return buildings;
        }

        /// <summary>
        /// Locate the nearest gameObject with given peramiters
        /// </summary>
        /// <param name="position">position to measure from</param>
        /// <param name="b_type">Type of Interactable</param>
        /// <param name="r_type">Type of Resource it may hold.</param>
        /// <returns>Closest GameObject with given peramiterst</returns>
        public static GameObject LocateNearestInteractableOfType(Vector3 position, InteractableType b_type, ResourceType r_type = default)
        {
            List<GameObject> interactables = HelperFunctions.GetInteractableGameObjectList(b_type);
            List<GameObject> selectedInteractables = new List<GameObject>();

            if (r_type != ResourceType.None)
            {
                foreach (GameObject inter in interactables)
                {
                    Building storage = inter.GetComponent<Building>();
                    foreach (StockpileInformation stock in storage._stockpiles)
                    {
                        if (stock._resourceType == r_type)
                        {
                            selectedInteractables.Add(inter);
                        }
                    }
                }
            }
            else
            {
                selectedInteractables = interactables;
            }

            GameObject closestInteractable = HelperFunctions.GetClosestGameObjectInListToVector3(selectedInteractables, position);
            if (closestInteractable != null)
                Debug.Log("Found UnitInteractable: " + closestInteractable);

            return closestInteractable;
        }

        /// <summary>
        /// Locate the nearest harvestable Interactable of given type
        /// </summary>
        /// <param name="unit">The unit that wants to search</param>
        /// <param name="b_type">The type of UnitInteractable that we want to find</param>
        /// <param name="r_type">The type of resource the interactable has to be</param>
        /// <param name="fromSelf">Search from Unit position or from the position of the last interacted UnitInteractable</param>
        /// <returns> UnitInteractable that is closest to either the unit or last interacted UnitInteractable </returns>
        public static UnitInteractable LocateNextHarvestableResouce(Unit unit, InteractableType b_type, ResourceType r_type, bool fromSelf = default)
        {
            List<GameObject> resourceBuildings = HelperFunctions.GetInteractableGameObjectList(b_type);
            List<ResourceInteractable> selectedresources = new List<ResourceInteractable>();
            foreach (GameObject building in resourceBuildings)
            {
                ResourceInteractable r = building.GetComponent<ResourceInteractable>();
                if (r._resourceInformation._resourceType == r_type)
                    selectedresources.Add(r);
            }

            float dist = float.MaxValue;
            ResourceInteractable selectedResource = null;
            foreach (ResourceInteractable resource in selectedresources)
            {
                float curDist = 0;
                if (fromSelf)//If search for closest resource near the unit it self
                    curDist = Vector3.Distance(resource.transform.position, unit.transform.position);
                else //Search for closest resource near last interacted resource
                    curDist = Vector3.Distance(resource.transform.position, unit._unitBrain._memory._lastInteractedResource.transform.position);

                if (!resource._inCooldown)
                {
                    if (curDist < dist)
                    {
                        dist = curDist;
                        selectedResource = resource;
                    }
                }
            }

            return selectedResource;
        }

        /// <summary>
        /// Find all storages with Harvestable-resources that are filled below given percentage threshhold
        /// </summary>
        /// <param name="filledThreshhold">the percentage threshhold</param>
        /// <returns></returns>
        public static List<UnitInteractable> LocateUnFilledHarvestableStorages(float filledThreshhold)
        {
            List<UnitInteractable> storages = new List<UnitInteractable>();
            foreach (UnitInteractable storage in BuildBuildingsLibrary.Instance._instantiatedStorages)
            {
                foreach (StockpileInformation stock in ((Storage)storage)._stockpiles)
                {
                    if (ResourceDatabase.GetResourceInformation(stock._resourceType)._isHarvestable)
                    {
                        float filledPercentage = (stock._currentStockAmount / stock._max) * 100f;
                        if (filledPercentage < filledThreshhold)
                        {
                            storages.Add(storage);
                        }
                    }
                }
            }

            storages = storages.Distinct().ToList();
            return storages;
        }

        /// <summary>
        /// Find the UnitInteractable in the list that has the smallest distance to the given Vector3
        /// </summary>
        /// <param name="interactables">List of UnitInteractables</param>
        /// <param name="position">the vector3 to measure from</param>
        /// <returns></returns>
        public static UnitInteractable GetClosestUnitInteractableInListToVector3(List<UnitInteractable> interactables, Vector3 position)
        {
            float dist = float.MaxValue;
            UnitInteractable closestInteractable = null;
            foreach (UnitInteractable interactable in interactables)
            {
                float curDist = Vector3.Distance(interactable.transform.position, position);
                if (curDist < dist)
                {
                    dist = curDist;
                    closestInteractable = interactable;
                }
            }
            return closestInteractable;
        }
    }
}
