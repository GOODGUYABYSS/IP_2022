using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalInputSys : MonoBehaviour
{
    public InputField goalInputField;
    public InputField descriptionInputField;

    public float distBtweenGoals;
    public int defaultMaxNumGoals;

    public GameObject toggleDisplayGoalInputFields;
    public GameObject goalInputUI;

    private GoalInput goalInput;

    public void Awake()
    {
        goalInput = new GoalInput(goalInputUI, false);

        GoalsList.maxNumGoals = BuildingData.numGoalAddingBuildings + defaultMaxNumGoals;
    }

    private void Update()
    {
        if (GoalsList.allGoals.Count >= GoalsList.maxNumGoals)
        {
            toggleDisplayGoalInputFields.SetActive(false);
            goalInput.ShowInputFields(false);
        }

        else
        {
            toggleDisplayGoalInputFields.SetActive(true);
        }
    }

    public void AddGoal()
    {
        Goal goal = new Goal(goalInputField.text, descriptionInputField.text);

        goal.DisplayGoal(GoalsList.togglePrefab, GoalsList.anchor);
    }

    public void ToggleDisplayInputFields(bool showGoalInputUI)
    {
        goalInput.ShowInputFields(showGoalInputUI);

        toggleDisplayGoalInputFields.transform.Find("Display").gameObject.SetActive(!showGoalInputUI);
        toggleDisplayGoalInputFields.transform.Find("Hide").gameObject.SetActive(showGoalInputUI);

    }
}
