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

    public static List<BuildingData> allBuildingData = new List<BuildingData>();
    public List<Mesh> allMeshes = new List<Mesh>();

    public float moneyGenerationMultiplier;
    public int defaultNumGoals;

    public DatabaseReference dbReference;

    public static bool allowDisplayConfirmPlacementButton = false;


    private void Awake()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;

        confirmPlacementButton.SetActive(false);
    }

    private void Update()
    {
        // if (SocketInteractorFunctions.counterExit == 1) 
        // {
        //     DeleteBuildingData(SocketInteractorFunctions.buildingIdToDelete);
        //     SocketInteractorFunctions.counterExit = 2;
        // }

        ToggleButton();
    }

    public void ToggleButton()
    {
        if (allowDisplayConfirmPlacementButton && SocketInteractorFunctions.allowCollisionDetection)
        {
            confirmPlacementButton.SetActive(true);
            allowDisplayConfirmPlacementButton = false;
        }
    }

    public void GetBuildingStuff()
    {
        RetrieveBuildingData();
    }

    public void GenerateBuilding(BuildingData buildingData)
    {
        GameObject spawnedObj;

        spawnedObj = Instantiate(defaultBuildingGameObject);

        // spawnedObj.GetComponent<BuildingDescription>().cameFromDatabase = buildingData.storedInDatabase;

        MeshFilter meshFilter;

        SocketInteractorFunctions.counter = 2;
        SocketInteractorFunctions.counterExit = 420;

        Vector3 position = new Vector3(buildingData.transformPosition[0], buildingData.transformPosition[1], buildingData.transformPosition[2]);
        Vector3 rotation = new Vector3(buildingData.transformRotation[0], buildingData.transformRotation[1], buildingData.transformRotation[2]);
        Vector3 scale = new Vector3(buildingData.transformScale[0], buildingData.transformScale[1], buildingData.transformScale[2]);

        spawnedObj.transform.position = position;
        spawnedObj.transform.Rotate(rotation);
        spawnedObj.transform.localScale = scale;

        spawnedObj.GetComponent<ObjectId>().objectId = buildingData.buildingId;

        meshFilter = spawnedObj.GetComponent<MeshFilter>();

        foreach (Mesh mesh in allMeshes)
        {
            Debug.Log("buildingData.meshId: " + buildingData.meshId);
            Debug.Log("(buildingData.meshId).Substring(buildingData.meshId.Length - 9): " + (buildingData.meshId).Substring(0, buildingData.meshId.Length - 9));
            if (mesh.name == (buildingData.meshId).Substring(0, buildingData.meshId.Length - 9))
            {
                // Debug.Log("This runs.");
                meshFilter.mesh = mesh;
            }
        }

        // PostSomething(buildingData);

        spawnedObj.GetComponent<ObjectId>().objectType = buildingData.buildingType;
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

                    foreach(DataSnapshot d in snapshot.Children)
                    {
                        BuildingData buildingD = JsonUtility.FromJson<BuildingData>(d.GetRawJsonValue());

                        allBuildingData.Add(buildingD);
                    }

                    foreach(BuildingData buildingD in allBuildingData)
                    {
                        // Debug.Log("buildingD.buildingId: " + buildingD.buildingId);
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
}
