using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectId : MonoBehaviour
{
    // This class holds the object id and other things used to identify the objects in the world.
    // I use this custom object id system instead of tags or layers because it is more robust and more detailed.
    // Also, having a custom object id will prevent conflicts with other coders who use tags or layers.

    public float objectId;
    public string objectType;

    public bool fromDatabase;

    public static List<float> allObjectIds = new List<float>();

    private void Start()
    {

    }

    public static float CreateObjectId(GameObject gameObj)
    {
        bool idExists = true;
        float id = 0;

        while (idExists)
        {
            id = Random.Range(1000000000f, 9999999999f);

            if (allObjectIds.Contains(id) || id == 0)
            {
                continue;
            }

            else
            {
                allObjectIds.Add(id);

                gameObj.AddComponent<ObjectId>();
                gameObj.GetComponent<ObjectId>().objectId = id;

                idExists = false;
            }
        }

        return id;
    }
}
