using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingData
{
    // This class contains all the data related to the building, but broken down into C#'s default variable types so that it can be stored in a database or on a save file.
    // This class has no methods because it is meant to only contain information about the Building class it relates to.

    // The below 3 variables contain transform information about a building game object.
    public float[] transformPosition = new float[3];
    public float[] transformRotation = new float[3];
    public float[] transformScale = new float[3];

    // The below variable contains the location that the building is stored in the cell.
    public int[] cellLocation = new int[2];

    // The below variable contains the building type. Each building type describes what the building can do. For example, "MoneyGeneratingBuilding" will generate money over time.
    public string buildingType;

    // The below variable contains the mesh that the building will be using once loaded. The mesh is based on mesh name.
    public string meshId;

    public float buildingId;

    public bool fromDatabase = false;

    public int creditGeneration;

    // public bool storedInDatabase;

    public BuildingData(float[] transformPosition, float[] transformRotation, float[] transformScale, string buildingType, string meshId, float buildingId, int creditGeneration, int[] cellLocation = null)
    {
        // This is the BuildingData object constructor.

        this.transformPosition = transformPosition;
        this.transformRotation = transformRotation;
        this.transformScale = transformScale;

        this.meshId = meshId;
        this.buildingId = buildingId;
        this.buildingType = buildingType;
        this.cellLocation = cellLocation;

        this.creditGeneration = creditGeneration;
    }

    public BuildingData()
    {
        // This is the BuildingData object constructor overload in case a programmer wants to create an empty BuildingData class and set the variables manually over time.
    }

    public string BuildingDataToJson()
    {
        // This function converts this class to a Json format.
        return JsonUtility.ToJson(this);
    }
}
