using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Control : MonoBehaviour
{
    // This script is the controller for generating objects and running methods.

    public GameObject defaultBuildingGameObject;

    public static List<BuildingData> allBuildingData = new List<BuildingData>();
    public static List<Mesh> allMeshes = new List<Mesh>();

    public float moneyGenerationMultiplier;
    public int defaultNumGoals;

    private void Awake()
    {
        
    }

    private void Update()
    {
 
    }

    public void GenerateBuilding(BuildingData buildingData)
    {
        MeshFilter meshFilter;

        Vector3 position = new Vector3(buildingData.transformPosition[0], buildingData.transformPosition[1], buildingData.transformPosition[2]);
        Vector3 rotation = new Vector3(buildingData.transformRotation[0], buildingData.transformRotation[1], buildingData.transformRotation[2]);
        Vector3 scale = new Vector3(buildingData.transformScale[0], buildingData.transformScale[1], buildingData.transformScale[2]);

        defaultBuildingGameObject.transform.position = position;
        defaultBuildingGameObject.transform.Rotate(rotation);
        defaultBuildingGameObject.transform.localScale = scale;

        meshFilter = defaultBuildingGameObject.GetComponent<MeshFilter>();

        foreach (Mesh mesh in allMeshes)
        {
            if (mesh.name == buildingData.meshId)
            {
                meshFilter.mesh = mesh;
            }
        }

        defaultBuildingGameObject.GetComponent<ObjectId>().objectType = buildingData.buildingType;
    }
}
