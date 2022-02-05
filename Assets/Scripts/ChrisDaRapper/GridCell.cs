using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    public readonly GameObject gridCellObj;

    Building building;

    public readonly int[] cellPosition = new int[2];

    public GridCell(GameObject gridCellObj, int[] cellPosition)
    {
        this.gridCellObj = gridCellObj;
        this.cellPosition = cellPosition;

        ObjectId.CreateObjectId(gridCellObj);
    }

    public void StoreBuildingInCell<TBuilding>(TBuilding building) where TBuilding : Building
    {
        this.building = building;
    }

    public bool ContainsBuilding()
    {
        if (building != null)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}
