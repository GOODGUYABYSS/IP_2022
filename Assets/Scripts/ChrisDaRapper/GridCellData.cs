using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridCellData
{
    public int[] cellPosition = new int[2];

    public float[] buildingTransformPosition = new float[3];
    public float[] buildingTransformRotation = new float[3];
    public float[] buildingTransformScale = new float[3];

    public string buildingType;

    public GridCellData(int[] cellPosition, float[] buildingTransformPosition, float[] buildingTransformRotation, float[] buildingTransformScale, 
                             string buildingType)
    {
        this.cellPosition = cellPosition;

        this.buildingTransformPosition = buildingTransformPosition;
        this.buildingTransformRotation = buildingTransformRotation;
        this.buildingTransformScale = buildingTransformScale;

        this.buildingType = buildingType;
    }
}
