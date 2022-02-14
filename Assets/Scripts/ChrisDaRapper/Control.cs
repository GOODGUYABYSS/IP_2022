using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Firebase.Database;
using Firebase.Extensions;

public class Control : MonoBehaviour
{
    // This script is the controller for generating objects and running methods.

    public GameObject defaultBuildingGameObject;

    public GameObject confirmPlacementButton;
    public static GameObject confirmPlacementButtonStatic;

    public static List<BuildingData> allBuildingData = new List<BuildingData>();
    public List<MeshFilter> allMeshes = new List<MeshFilter>();

    public float moneyGenerationMultiplier;
    public int defaultNumGoals;

    public DatabaseReference dbReference;

    public static bool allowDisplayConfirmPlacementButton = false;

    private float totalCredits;
    private int numBuildings;
    private int multiplier;


    private void Awake()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;

        confirmPlacementButtonStatic = confirmPlacementButton;

        confirmPlacementButton.SetActive(false);

        StartCoroutine(EarnMoney());
    }

    public void GetBuildingStuff()
    {
        Debug.Log("Hello 10");
        RetrieveBuildingData();
        Debug.Log("Hello 11");
    }

    // What this function does is converts the information from firebase or any other save system into the original Unity game object that was saved.
    public void GenerateBuilding(BuildingData buildingData)
    {
        Debug.Log("Hello 7");
        GameObject spawnedObj; // Stores an instantiated game object for use by other processes.

        Debug.Log("Hello 6");

        spawnedObj = Instantiate(defaultBuildingGameObject); // Creates a base prefab that will be modified based on the information from the save system.

        Debug.Log("Hello 5");

        // The below 3 statements create a Vector3 for position, rotation, and scale respectively based on their respective bool arrays.
        // These Vector3s will ultimately be used to set the attributes of the Transform of the instantiated game object.
        Vector3 position = new Vector3(buildingData.transformPosition[0], buildingData.transformPosition[1], buildingData.transformPosition[2]);
        Vector3 rotation = new Vector3(buildingData.transformRotation[0], buildingData.transformRotation[1], buildingData.transformRotation[2]);
        Vector3 scale = new Vector3(buildingData.transformScale[0], buildingData.transformScale[1], buildingData.transformScale[2]);

        Debug.Log("Hello 4");

        // The below 3 statements set the transform attributes of the instantiated game object.
        spawnedObj.transform.position = position;
        spawnedObj.transform.Rotate(rotation);
        spawnedObj.transform.localScale = scale;

        Debug.Log("Hello 3");

        spawnedObj.GetComponent<ObjectId>().objectId = buildingData.buildingId; // This statement sets the building ID of the instantiated game object.
        spawnedObj.GetComponent<ObjectId>().fromDatabase = true;

        Debug.Log("spawnedObj.GetComponent<ObjectId>().fromDatabase: " + spawnedObj.GetComponent<ObjectId>().fromDatabase);

        // The below foreach loop loops through all the meshes to see what mesh name matches the mesh ID (stored mesh name) of the saved data and then sets the game object to have that mesh afterwards.
        foreach (MeshFilter mesh in allMeshes)
        {
            Debug.Log("meshfilter.name: " + mesh.sharedMesh.name);
            // The below if statement checks whether the mesh ID matches the mesh name.
            if (mesh.sharedMesh.name == (buildingData.meshId).Substring(0, buildingData.meshId.Length - 9))
            {
                Debug.Log("(buildingData.meshId).Substring(0, buildingData.meshId.Length - 9): " + (buildingData.meshId).Substring(0, buildingData.meshId.Length - 9));
                spawnedObj.GetComponent<MeshFilter>().sharedMesh = mesh.sharedMesh;
            }
        }

        spawnedObj.GetComponent<ObjectId>().objectType = buildingData.buildingType; // Sets the type of building the object will become. E.g. Money generating building or Goal adding building.

        spawnedObj.GetComponent<BoxCollider>().enabled = true;
    }

    private void RetrieveBuildingData()
    {
        dbReference.Child("buildingData/" + Authentication.userId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Something went wrong when reading the data, ERROR: " + task.Exception);
                return;
            }

            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    BuildingData bd = JsonUtility.FromJson<BuildingData>(snapshot.GetRawJsonValue());

                    Debug.Log("bd.meshId: " + bd.meshId);

                    foreach (DataSnapshot d in snapshot.Children)
                    {
                        BuildingData buildingD = JsonUtility.FromJson<BuildingData>(d.GetRawJsonValue());

                        allBuildingData.Add(buildingD);
                    }

                    foreach (BuildingData buildingD in allBuildingData)
                    {
                        Debug.Log("allBuildingData[0]: " + allBuildingData[0]);
                        GenerateBuilding(buildingD);
                    }
                }
            }
        });
    }

    // private void DeleteBuildingData(float buildingId)
    // {
    //     string parentName = "";
    // 
    //     dbReference.Child("buildingData/" + Authentication.userId).GetValueAsync().ContinueWithOnMainThread(task => 
    //     {
    //         if (task.IsCanceled || task.IsFaulted)
    //         {
    //             Debug.LogError("Something went wrong when reading the data, ERROR: " + task.Exception);
    //             return;
    //         }
    // 
    //         else if (task.IsCompleted)
    //         {
    //             DataSnapshot snapshot = task.Result;
    // 
    //             var reference = snapshot.Reference;
    //             string name = reference.Parent.ToString();
    // 
    //             if (snapshot.Exists)
    //             {
    //                 foreach (var child in snapshot.Children)
    //                 {
    //                     if (int.Parse(child.Child("buildingId").Value.ToString()) == buildingId)
    //                     {
    //                         parentName = child.Child("buildingId").Reference.Parent.Reference.Parent.ToString();
    //                     }
    // 
    //                     dbReference.Child("buildingData/" + Authentication.userId + "/" + parentName).SetValueAsync(null);
    //                 }
    //             }
    //         }
    //     });
    // }

    IEnumerator EarnMoney()
    {
        while (true)
        {
            yield return new WaitForSeconds(60f);

            totalCredits += multiplier * numBuildings;
        }
    }
}