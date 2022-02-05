using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalInput
{
    private GameObject goalInputUI;

    public GoalInput(GameObject goalInputUI, bool showByDefault)
    {
        this.goalInputUI = goalInputUI;

        ShowInputFields(showByDefault);
    }

    public void ShowInputFields(bool displayInputFields)
    {
        goalInputUI.SetActive(displayInputFields);
    }

    public GameObject FindChild(string name)
    {
        GameObject childObj;

        childObj = goalInputUI.transform.Find(name).gameObject;

        if (childObj != null)
        {
            return childObj;
        }

        else
        {
            return null;
        }
    }
}
