using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Goals
{
    public string goalContent;
    public string howToAchieve;
    public long createdOn;
    public long updatedOn;

    public Goals()
    {

    }

    public Goals(string goalContent, string howToAchieve)
    {
        this.goalContent = goalContent;
        this.howToAchieve = howToAchieve;
        // timestamp properties
        var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        this.createdOn = timestamp;
        this.updatedOn = timestamp;
    }

    public string GoalsToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
