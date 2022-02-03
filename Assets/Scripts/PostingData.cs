using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using System;

public class PostingData : MonoBehaviour
{
    public DatabaseReference dbReference;
    public RetrievingData retrievingData;

    public static bool anotherLoginPostingData = true;
    public string userId;

    [SerializeField]
    GameObject goalContent, howToAchieve;

    private void Awake()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Authentication.loggedIn && anotherLoginPostingData)
        {
            userId = Authentication.userId;
            anotherLoginPostingData = false;
        }
    }

    public void CreateNewGoalButton()
    {
        string tempGoal, tempHow;

        tempGoal = goalContent.GetComponent<TMP_InputField>().text;
        tempHow = howToAchieve.GetComponent<TMP_InputField>().text;

        CreateNewGoal(userId, tempGoal, tempHow);

        goalContent.GetComponent<TMP_InputField>().placeholder.GetComponent<TMP_Text>().text = "Enter Goal";
        howToAchieve.GetComponent<TMP_InputField>().placeholder.GetComponent<TMP_Text>().text = "Enter How To Achieve";

        goalContent.GetComponent<TMP_InputField>().text = "";
        howToAchieve.GetComponent<TMP_InputField>().text = "";

        if (!RetrievingData.mission1Completed)
        {
            UpdateMission1Completed();
            RetrievingData.mission1Completed = true;
        }

        // retrieve goals again
        retrievingData.RetrieveGoals();
    }

    public void CreateNewGoal(string uuid, string goalContent, string howToAchieve)
    {
        Goals createGoals = new Goals(goalContent, howToAchieve);

        string key = dbReference.Child(uuid).Push().Key;

        dbReference.Child("currentGoals/" + uuid + "/" + key).SetRawJsonValueAsync(createGoals.GoalsToJson());
    }

    public void UpdateUsefulBuildingCount()
    {
        dbReference.Child("playerStats/" + userId + "/usefulBuildingCount").SetValueAsync(RetrievingData.usefulBuildingCount);
        dbReference.Child("playerStats/" + userId + "/totalBuildingCount").SetValueAsync(RetrievingData.totalBuildingCount);

        // update leaderboard
        retrievingData.GetLeaderboard();
        // update timestamp
        UpdatePlayerStatsTimestamp();
    }

    public void UpdateUselessBuildingCount()
    {
        dbReference.Child("playerStats/" + userId + "/uselessBuildingCount").SetValueAsync(RetrievingData.uselessBuildingCount);
        dbReference.Child("playerStats/" + userId + "/totalBuildingCount").SetValueAsync(RetrievingData.totalBuildingCount);

        // update leaderboard
        retrievingData.GetLeaderboard();
        // update timestamp
        UpdatePlayerStatsTimestamp();
    }

    public void UpdateCredits()
    {
        // takes in new value of money and updates database
        dbReference.Child("playerStats/" + userId + "/credits").SetValueAsync(RetrievingData.credits);

        // update timestamp
        UpdatePlayerStatsTimestamp();

    }

    public void UpdateMission1Completed()
    {
        dbReference.Child("playerStats/" + userId + "/mission1Completed").SetValueAsync(true);

        // update timestamp
        UpdatePlayerStatsTimestamp();
    }

    public void UpdateMission2Completed()
    {
        dbReference.Child("playerStats/" + userId + "/mission2Completed").SetValueAsync(true);

        // update timestamp
        UpdatePlayerStatsTimestamp();
    }

    public void UpdatePlayerStatsTimestamp()
    {
        // timestamp properties
        var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        dbReference.Child("players/" + userId + "/updatedOn").SetValueAsync(timestamp);
    }
}
