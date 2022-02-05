using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDescription : MonoBehaviour
{
    public BuildingDescription()
    {

    }

    public BuildingData GenerateBuildingData()
    {
        Transform gameObjectTransform = gameObject.transform;

        string buildingType = gameObject.GetComponent<ObjectId>().objectType;
        float buildingId = gameObject.GetComponent<ObjectId>().objectId;
        string meshId = gameObject.GetComponent<MeshFilter>().mesh.name;

        float[] transformPosition = new float[3];
        float[] transformRotation = new float[3];
        float[] transformScale = new float[3];

        transformPosition[0] = gameObjectTransform.position.x;
        transformPosition[1] = gameObjectTransform.position.y;
        transformPosition[2] = gameObjectTransform.position.z;

        transformRotation[0] = gameObjectTransform.rotation.eulerAngles.x;
        transformRotation[1] = gameObjectTransform.rotation.eulerAngles.y;
        transformRotation[2] = gameObjectTransform.rotation.eulerAngles.z;

        transformScale[0] = gameObjectTransform.localScale.x;
        transformScale[1] = gameObjectTransform.localScale.y;
        transformScale[2] = gameObjectTransform.localScale.z;

        BuildingData buildingData = new BuildingData(transformPosition, transformRotation, transformScale, buildingType, meshId, buildingId);

        return buildingData;
    }

    public static void AddGoalBuilding()
    {
        BuildingData.numGoalAddingBuildings += 1;
    }

    public static void AddMoneyBuilding()
    {
        BuildingData.numMoneyGenBuildings += 1;
    }

    public static void CalculateTotalBuildings()
    {
        BuildingData.numAllBuildings = BuildingData.numMoneyGenBuildings + BuildingData.numGoalAddingBuildings;
    }
}
