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
        
    }

    public void SetButtonGroup(GameObject buttonGroup)
    {
        this.buttonGroup = buttonGroup;
    }
}
