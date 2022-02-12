using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionLogs
{
    public string missionContent;
    public string missionStatus;
    public string buildingName;

    public MissionLogs()
    {

    }

    public MissionLogs(string missionContent, string missionStatus, string buildingName)
    {
        this.missionContent = missionContent;
        this.missionStatus = missionStatus;
        this.buildingName = buildingName;
    }

    public string MissionLogsToJson()
    {
        return JsonUtility.ToJson(this);
    }
}

