using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private GameObject buildingGameObject;

    private string buildingType;
    private int[] cellLocation;

    public Building(GameObject buildingGameObject, string buildingType, int[] cellLocation = null)
    {
        this.buildingGameObject = buildingGameObject;
        this.buildingType = buildingType;
        this.cellLocation = cellLocation;

        ObjectId.CreateObjectId(buildingGameObject);

        buildingGameObject.GetComponent<ObjectId>().objectType = buildingType;
    }

    public static void AddGoalAddingBuilding()
    {
        BuildingData.numGoalAddingBuildings += 1;
    }

    public static float EarnMoney(float multiplier)
    {
        return BuildingData.numMoneyGenBuildings * multiplier;
    }

    public static void CalculateTotalBuildings()
    {
        BuildingData.numAllBuildings = BuildingData.numMoneyGenBuildings + BuildingData.numGoalAddingBuildings;
    }
}
