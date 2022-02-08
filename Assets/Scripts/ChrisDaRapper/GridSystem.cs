using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    private int width;
    private int height;
    public readonly float cellSize;

    public GridCell[,] gridArray;

    public GridSystem(int width, int height, float cellSize, GameObject gridObj)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new GridCell[width, height];
        InitGridCells(gridObj);
    }

    private void InitGridCells(GameObject gridObj)
    {
        Vector3 spawnPos;
        GameObject spawnedGridObj;

        int[] cellPosition = new int[2];

        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                spawnPos = new Vector3(cellSize * i, 0, cellSize * j);

                spawnedGridObj = Object.Instantiate(gridObj);

                spawnedGridObj.transform.position = spawnPos;

                cellPosition[0] = i;
                cellPosition[1] = j;

                gridArray[i, j] = new GridCell(spawnedGridObj, cellPosition);
            }
        }
    }
}
