using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketInteractorFunctions : MonoBehaviour
{
    public bool allowCollisionDetection;

    public void AllowCollisionDetection()
    {
        allowCollisionDetection = true;
    }

    public void DisallowCollisionDetection()
    {
        allowCollisionDetection = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (allowCollisionDetection)
        {
            BuildingDescription buildingDescription = collision.gameObject.GetComponent<BuildingDescription>();

            BuildingData buildingData = buildingDescription.GenerateBuildingData();

            Control.allBuildingData.Add(buildingData);

            Debug.Log("Control.allBuildingData.Count: " + Control.allBuildingData.Count);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (allowCollisionDetection)
        {
            RemoveBuildingFromList(collision);

            Debug.Log("Exited");
        }
    }

    private void RemoveBuildingFromList(Collider collision)
    {
        // Removes a collided building from Control.allBuildingData.

        float id = collision.gameObject.GetComponent<ObjectId>().objectId;

        foreach (BuildingData data in Control.allBuildingData)
        {
            if (data.buildingId == id)
            {
                Control.allBuildingData.Remove(data);
                return;
            }
        }

        Debug.LogWarning("Current BuildingData object does not exist in Control.allBuildingData");
    }
}
