using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;

public class PostingData : MonoBehaviour
{
    public DatabaseReference dbReference;

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
    }

    public void CreateNewGoal(string uuid, string goalContent, string howToAchieve)
    {
        Goals createGoals = new Goals(goalContent, howToAchieve);

        string key = dbReference.Child(uuid).Push().Key;

        dbReference.Child("currentGoals/" + uuid + "/" + key).SetRawJsonValueAsync(createGoals.GoalsToJson());
    }

    public void UpdateUsefulBuildingCount(int usefulBuildingCount, int totalBuildingCount)
    {
        dbReference.Child("playerStats/" + userId + "usefulBuildingCount").SetValueAsync(usefulBuildingCount);
        dbReference.Child("playerStats/" + userId + "totalBuildingCount").SetValueAsync(totalBuildingCount);
    }

    public void UpdateUselessBuildingCount(int uselessBuildingCount, int totalBuildingCount)
    {
        dbReference.Child("playerStats/" + userId + "uselessBuildingCount").SetValueAsync(uselessBuildingCount);
        dbReference.Child("playerStats/" + userId + "totalBuildingCount").SetValueAsync(totalBuildingCount);
    }

    public void UpdateCredits(int newCreditsValue)
    {
        // takes in new value of money and updates database
        dbReference.Child("playerStats/" + userId + "/credits").SetValueAsync(newCreditsValue);

    }

    public void UpdateMission1Completed()
    {
        dbReference.Child("playerStats/" + userId + "/mission1Completed").SetValueAsync(true);
    }

    public void UpdateMission2Completed()
    {
        dbReference.Child("playerStats/" + userId + "/mission2Completed").SetValueAsync(true);
    }
}
