using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    public string username;
    public int usefulBuildingCount;
    public int uselessBuildingCount;
    public int totalBuildingCount;
    public int credits;
    public int numberOfGoalsCompleted;

    public PlayerStats()
    {

    }

    public PlayerStats(string username, int usefulBuildingCount, int uselessBuildingCount, int totalBuildingCount, int credits, int numberOfGoalsCompleted)
    {
        this.username = username;
        this.usefulBuildingCount = usefulBuildingCount;
        this.uselessBuildingCount = uselessBuildingCount;
        this.totalBuildingCount = totalBuildingCount;
        this.credits = credits;
        this.numberOfGoalsCompleted = numberOfGoalsCompleted;
    }

    public string PlayerStatsToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
