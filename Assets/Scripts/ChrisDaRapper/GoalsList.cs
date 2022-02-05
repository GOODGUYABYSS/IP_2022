using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalsList
{
    // This class represents the list of every single goal that the user set

    public static List<Goal> allGoals = new List<Goal>();

    public static RectTransform anchor; // This variable sets the position of the first toggle gameobject, which future toggle gameobjects will be based on.
    public static GameObject togglePrefab;

    public static float distBetweenGoals;
    public static int maxNumGoals;

    public static void AddGoal(Goal goal)
    {
        if (allGoals.Count < maxNumGoals)
        {
            allGoals.Add(goal);
        }
    }

    public static void RemoveGoalAndReorganizeGoalOrder(Goal goal)
    {
        int removedGoalOrder;

        RectTransform goalTransform;

        if (goal != null)
        {
            removedGoalOrder = goal.RemoveGoalFromList();

            for (int i = removedGoalOrder; i < allGoals.Count; i++)
            {
                allGoals[i].goalOrder -= 1;

                goalTransform = allGoals[i].goalGameObj.GetComponent<RectTransform>();
                goalTransform.anchoredPosition = GenerateOrderedGoalPos(allGoals[i].goalOrder);
            }
        }
    }

    public static Goal FindGoalWithId(float id)
    {
        foreach (Goal goal in allGoals)
        {
            if (goal.goalGameObj.GetComponent<ObjectId>().objectId == id)
            {
                return goal;
            }
        }

        return null;
    }

    public static Vector2 GenerateOrderedGoalPos(int goalOrder)
    {
        Debug.Log("allGoals.Count: " + allGoals.Count);
        return new Vector2(0, -goalOrder * distBetweenGoals);
    }

    public static void CreateGoalListAnchor(RectTransform rectTransform)
    {
        anchor = rectTransform;
    }

    public static void CreateTogglePrefab(GameObject prefab)
    {
        togglePrefab = prefab;
    }

    public static void SetDistBetweenGoals(float distance)
    {
        distBetweenGoals = distance;
    }
}
