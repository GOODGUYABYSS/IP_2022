using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal
{
    // This class represents each goal item in the checklist/goalslist and what you can do with them.

    public readonly string goal;
    public readonly string description;

    public int goalOrder;

    public GameObject goalGameObj;

    public Goal(string goal, string description)
    {
        this.goal = goal;
        this.description = description;

        goalOrder = GoalsList.allGoals.Count;

        GoalsList.AddGoal(this);
    }

    public void DisplayGoal(GameObject toggleObj, RectTransform transformAnchor)
    {
        // This function displays the goal in the goals list section of the game.

        GameObject toggleLabel;

        int numGoals;
        string goalString;

        numGoals = GoalsList.allGoals.Count;

        goalString = "Goal: " + goal + "\n" + "Description: " + description;

        goalGameObj = Object.Instantiate(toggleObj, transformAnchor);

        goalGameObj.GetComponent<RectTransform>().anchoredPosition = GoalsList.GenerateOrderedGoalPos(goalOrder);

        toggleLabel = goalGameObj.transform.Find("Label").gameObject;
        toggleLabel.GetComponent<Text>().text = goalString;

        ObjectId.CreateObjectId(goalGameObj);

        goalGameObj.GetComponent<ObjectId>().objectType = "GoalListItem";
    }

    public int RemoveGoalFromList()
    {
        GoalsList.allGoals.Remove(GoalsList.allGoals[goalOrder]);
        Object.Destroy(goalGameObj); // Destroys the gameobject associated with this instance of Goal.

        return goalOrder;
    }
}
