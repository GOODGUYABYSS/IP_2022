using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitStaticVariables : MonoBehaviour
{
    // The below variables are used for the goals list.
    public RectTransform goalsListAnchor;
    public GameObject togglePrefab;

    public float distBetweenGoals;

    private void Awake()
    {
        if (distBetweenGoals == 0)
        {
            distBetweenGoals = 100;
        }

        GoalsList.CreateGoalListAnchor(goalsListAnchor);
        GoalsList.CreateTogglePrefab(togglePrefab);
        GoalsList.SetDistBetweenGoals(distBetweenGoals);
    }
}
