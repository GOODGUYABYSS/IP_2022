using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveGoal : MonoBehaviour
{
    public GameObject currentGameObj;

    private void Start()
    {
        Debug.Log("currentGameObj.GetComponent<ObjectId>().objectId: " + currentGameObj.GetComponent<ObjectId>().objectId);
    }

    public void RemoveGoalFunc()
    {
        Goal goalToRemove;

        float gameObjId = currentGameObj.GetComponent<ObjectId>().objectId;

        goalToRemove = GoalsList.FindGoalWithId(gameObjId);

        GoalsList.RemoveGoalAndReorganizeGoalOrder(goalToRemove);
    }
}
