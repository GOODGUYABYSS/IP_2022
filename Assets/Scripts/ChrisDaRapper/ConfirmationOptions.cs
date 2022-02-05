using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConfirmationOptions
{
    private GameObject buttonGroup;

    private ObjectId buildingId;

    public ConfirmationOptions(GameObject buttonGroup)
    {
        this.buttonGroup = buttonGroup;
    }

    public void ShowBuildingPlacementOptions(GameObject currentBuilding)
    {
        if (!buttonGroup.activeSelf)
        {
            buttonGroup.SetActive(true);
        }
    }

    public void ConfirmBuildingPlacement(bool confirmation)
    {
        // This function should be placed on a button

        // if (confirmation)
        // {
        //     if (buildingId.objectType == "GoalAddingBuilding")
        //     {
        //         Building.AddGoalAddingBuilding();
        //     }
        // 
        //     else if (buildingId.objectType == "MoneyGeneratingBuilding")
        //     {
        //         Building.AddMoneyGenBuilding();
        //     }
        // 
        //     else
        //     {
        //         Debug.LogError("Object is not a building but ConfirmBuildingPlacement() is still being used.");
        //     }
        // }
        // 
        // else
        // {
        //     buttonGroup.SetActive(false);
        // }
    }

    public void SetButtonGroup(GameObject buttonGroup)
    {
        this.buttonGroup = buttonGroup;
    }
}
