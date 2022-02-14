using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketInteractorFunctions : MonoBehaviour
{
    public static GameObject buildingGameObject; // This variable sets the building game object to be sent to the database. Any new building that snapped to grid will be put here.
    private GameObject currentGameObject; // This variable sets the current selected game object of the respective socket interactor.

    public static float buildingIdToDelete; // This variable sets the id of the building to be deleted from the database.


    // The SelectGameObject() function controls what will occur when a game object is placed in the XRSocketInteractor. I.e. What will happen when a game object is snapped to grid.
    public void SelectGameObject()
    {

        currentGameObject = GetComponent<XRSocketInteractor>().interactablesSelected[0].transform.gameObject;
        Debug.Log("currentGameObject.GetComponent<ObjectId>().fromDatabase: " + currentGameObject.GetComponent<ObjectId>().fromDatabase);

        if (!currentGameObject.GetComponent<ObjectId>().fromDatabase)
        {
            Debug.Log("Nuuuts");
            buildingGameObject = currentGameObject; // The current game object information will be stored in buildingGameObject.

            /* Since buildingGameObject is a static variable, it will get overwritten every time an object enters a socket interactor.
               This ensures that a user has to press the confirmPlacementButton whenever he or she puts an object in the socket interactor to
               save the building position in the database. */

            Control.confirmPlacementButtonStatic.SetActive(true); // Displays the "Confirm Placement" button when an object enters a socket interactor.

            Debug.Log($"Entered SelectGameObject() function.\nGameobject {currentGameObject.name} entered.");
        }
    }

    // The UnselectGameObject() function controls what will occur when the object in the XRSocketInteractor is removed. I.e. What will happen when the current game object is removed.
    public void UnselectGameObject()
    {
        Debug.Log("Deeeeezzzz");
        buildingIdToDelete = currentGameObject.GetComponent<ObjectId>().objectId; // Sets the current gameobject's id to the buildingidto delete static function.
        currentGameObject.GetComponent<ObjectId>().fromDatabase = false;

        Control.confirmPlacementButtonStatic.SetActive(false); // Hides the "Confirm Placement" button when an object exits a socket interactor.

        Debug.Log($"Entered UnselectGameObject() function.\nGameobject {currentGameObject.name} exited.");
    }
}