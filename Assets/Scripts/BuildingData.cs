using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingData
{
    // This class contains all the data related to the building, but broken down into C#'s default variable types so that it can be stored in a database or on a save file.
    // This class has no methods because it is meant to only contain information about the Building class it relates to.

    public static int numAllBuildings;

    public static int numGoalAddingBuildings; // Useless
    public static int numMoneyGenBuildings; // Useful

    public readonly float[] transformPosition = new float[3];
    public readonly float[] transformRotation = new float[3];
    public readonly float[] transformScale = new float[3];

    public int[] cellLocation = new int[2];

    public string buildingType;
    public string meshId;

    public float buildingId;

    public BuildingData(float[] transformPosition, float[] transformRotation, float[] transformScale, string buildingType, string meshId, float buildingId, int[] cellLocation = null)
    {
        this.transformPosition = transformPosition;
        this.transformRotation = transformRotation;
        this.transformScale = transformScale;

        this.meshId = meshId;
        this.buildingId = buildingId;
        this.buildingType = buildingType;
        this.cellLocation = cellLocation;
    }

    public BuildingData()
    {

    }

    public string BuildingDataToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
