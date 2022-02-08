using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketInteractorFunctions : MonoBehaviour
{
    public static bool allowCollisionDetection;

    private bool allowEdit;

    public static int counter = 0;
    public static int counterExit = 0;

    public static GameObject buildingGameObject;
    public GameObject previousGameObj;

    public static float buildingIdToDelete;

    public GameObject confirmPlacementButton;

    public void AllowCollisionDetection()
    {
        allowCollisionDetection = true;
        // don't allow other things to go here.
    }

    public void DisallowCollisionDetection()
    {
        allowCollisionDetection = false;
        // allow other things to go here.
    }

    private void OnTriggerEnter(Collider collision)
    {
        // if (objectOfGame == collision.gameObject && !collision.gameObject.GetComponent<BuildingDescription>().cameFromDatabase)
        // {
        //     // This if statement resets counter to zero if a new object is put, which allows the new gameobject data to be sent to firebase.
        //     // Without this if statement, then adding a new gameobject will just add more to the counter and won't trigger the PostingBuildingData() function in PostingData.cs.
        //     counter = 0;
        // }
        // 
        // // This below is used to check whether the gameobject data is allowed to be sent to the database.
        // // The PostingBuildingData() function in the PostingData.cs script checks for this.
        // counter += 1;
        // 
        // counterExit = 0;

        // PostingData.allowPostingData = true;

        if (allowCollisionDetection && previousGameObj != collision.gameObject)
        {
            // Control.allowDisplayConfirmPlacementButton = true;
            buildingGameObject = collision.gameObject;
            previousGameObj = collision.gameObject;

            // buildingIdToDelete = 0;

            Debug.Log("Entered");
        }

        else if (allowCollisionDetection)
        {
            // Control.allowDisplayConfirmPlacementButton = true;
        }

        // activate confirmation options.
        // Don't allow other things to go here.
    }

    private void OnTriggerExit(Collider collision)
    {
        // if (objectOfGame2 == collision.gameObject)
        // {
        //     counterExit = 0;
        // }
        // 
        // collision.gameObject.GetComponent<BuildingDescription>().cameFromDatabase = false;
        // 
        // counterExit += 1;
        // 
        // counter = 0;

        if (!allowCollisionDetection)
        {
            Debug.Log("Exited");
            buildingIdToDelete = collision.gameObject.GetComponent<ObjectId>().objectId;
            Control.allowDisplayConfirmPlacementButton = true;
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
