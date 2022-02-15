using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.XR.Interaction.Toolkit;

public class BuildingDescription : MonoBehaviour
{
    // The building description class contains information describing a building, but in Unity formats like Transform and Vector3.
    // This class is meant to be a component in its respective game object and stores unique information about it, which is why it derives from the MonoBehaviour class.

    public static List<BoxCollider> allBoxColliders = new List<BoxCollider>();

    private void Start()
    {

    }

    public BuildingDescription()
    {
        // This is the BuildingDescription object constructor.
        // Warning: Only create a BuildingDescription class if it is a component of an existing building game object.
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !Shop.mouseClicked)
        {
            foreach (BoxCollider boxColider in allBoxColliders)
            {
                boxColider.enabled = true;
            }

            Shop.mouseClicked = true;
        }
    }

    // The GenerateBuildingData function stores information about a building as C# built-in variable types so that it can be stored in a save system or Firebase.
    public static BuildingData GenerateBuildingData(GameObject gameObject)
    {
        Transform gameObjectTransform = gameObject.transform;

        string buildingType = gameObject.GetComponent<ObjectId>().objectType;
        float buildingId = gameObject.GetComponent<ObjectId>().objectId;
        string meshId = gameObject.GetComponent<MeshFilter>().mesh.name;
        int creditGeneration = gameObject.GetComponent<ObjectId>().creditGeneration;

        float[] transformPosition = new float[3];
        float[] transformRotation = new float[3];
        float[] transformScale = new float[3];


        // The below 9 statements convert transform information into float format so that it can be saved in a database.
        // The below 3 statements convert transform position of this object's respective building into float format and stores it in a list.
        transformPosition[0] = gameObjectTransform.position.x;
        transformPosition[1] = gameObjectTransform.position.y;
        transformPosition[2] = gameObjectTransform.position.z;

        // The below 3 statements convert transform rotation of this object's respective building into float format and stores it in a list.
        transformRotation[0] = gameObjectTransform.rotation.eulerAngles.x;
        transformRotation[1] = gameObjectTransform.rotation.eulerAngles.y;
        transformRotation[2] = gameObjectTransform.rotation.eulerAngles.z;

        // The below 3 statements convert transform scale of this object's respective building into float format and stores it in a list.
        transformScale[0] = gameObjectTransform.localScale.x;
        transformScale[1] = gameObjectTransform.localScale.y;
        transformScale[2] = gameObjectTransform.localScale.z;

        BuildingData buildingData = new BuildingData(transformPosition, transformRotation, transformScale, buildingType, meshId, buildingId, creditGeneration);

        buildingData.fromDatabase = true;

        Control.allBuildingData.Add(buildingData);

        return buildingData;
    }
}