using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    private int width;
    private int height;
    public readonly float cellSize;

    float xAdjust;
    float yAdjust;
    float zAdjust;

    GameObject parent;

    public GridSystem(int width, int height, float cellSize, float xAdjust, float yAdjust, float zAdjust, GameObject gridObj, GameObject parent)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        this.xAdjust = xAdjust;
        this.yAdjust = yAdjust;
        this.zAdjust = zAdjust;

        this.parent = parent;

        InitGridCells(gridObj);
    }

    private void InitGridCells(GameObject gridObj)
    {
        Vector3 spawnPos;
        GameObject spawnedGridObj;

        int[] cellPosition = new int[2];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                spawnPos = new Vector3(cellSize * i + xAdjust - (width * cellSize / 2) + (cellSize / 2) + parent.transform.position.x, 0 + yAdjust + parent.transform.position.y, cellSize * j + zAdjust - (height * cellSize / 2) + (cellSize / 2) + parent.transform.position.z);

                spawnedGridObj = Object.Instantiate(gridObj, parent: parent.transform);

                spawnedGridObj.transform.position = spawnPos;

                cellPosition[0] = i;
                cellPosition[1] = j;
            }
        }
    }
}
