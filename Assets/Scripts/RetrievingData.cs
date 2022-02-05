using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using System;

public class RetrievingData : MonoBehaviour
{
    public DatabaseReference dbReference;

    public static bool anotherLoginRetrievingData = true;

    public string userId;

    public static int credits;

    public Dictionary<string, string> goalsAndKeys = new Dictionary<string, string>();
    public Dictionary<string, string> goalsAndHowToAchieve = new Dictionary<string, string>();

    public List<string> usernameList = new List<string>();
    public List<string> totalBuildingCountList = new List<string>();

    public static int numberOfGoalsCompleted;
    [SerializeField]
    private int newNumberOfGoalsCompleted;

    // see if it needs to be static ASK JASLYN
    public string mission1Content;
    public string mission2Content;

    public static bool mission1Completed;
    public static bool mission2Completed;

    public static int totalBuildingCount;
    public static int usefulBuildingCount;
    public static int uselessBuildingCount;

    public GameObject rowPrefab;
    public Transform tableContent;

    public GameObject leaderboardPrefab;
    public Transform leaderboardTable;

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
        if (Authentication.loggedIn && anotherLoginRetrievingData)
        {
            userId = Authentication.userId;

            // reset new number of goals completed
            newNumberOfGoalsCompleted = 0;

            // retrieve the player data
            RetrievePlayerStats();

            RetrieveMission1Content();
            RetrieveMission2Content();

            RetrieveGoals();

            GetLeaderboard();

            // stop the loop
            anotherLoginRetrievingData = false;
        }
    }

    public void RetrievePlayerStats()
    {
        dbReference.Child("playerStats/" + userId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Something went wrong when reading the data, ERROR: " + task.Exception);
                return;
            }

            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    credits = int.Parse(snapshot.Child("credits").Value.ToString());
                    mission1Completed = Convert.ToBoolean(snapshot.Child("mission1Completed").Value.ToString());
                    mission2Completed = Convert.ToBoolean(snapshot.Child("mission2Completed").Value.ToString());
                    numberOfGoalsCompleted = int.Parse(snapshot.Child("numberOfGoalsCompleted").Value.ToString());
                    totalBuildingCount = int.Parse(snapshot.Child("totalBuildingCount").Value.ToString());
                    usefulBuildingCount = int.Parse(snapshot.Child("usefulBuildingCount").Value.ToString());
                    uselessBuildingCount = int.Parse(snapshot.Child("uselessBuildingCount").Value.ToString());
                }
            }
        });
    }

    public void RetrieveGoals()
    {
        // clear the dictionaries that store the data from firebase
        goalsAndKeys.Clear();
        goalsAndHowToAchieve.Clear();

        // retrieve the pushKey, goalContent, and howToAchieve in the order which goal was created first
        dbReference.Child("currentGoals/" + userId).OrderByChild("createdOn").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Something went wrong when reading the data, ERROR: " + task.Exception);
                return;
            }

            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    foreach (var child in snapshot.Children)
                    {
                        string pushKey = child.Key;

                        string goal = child.Child("goalContent").Value.ToString();
                        // saves goalContent and push key in one dictionary
                        goalsAndKeys.Add(goal, pushKey);

                        string tempAchieve = child.Child("howToAchieve").Value.ToString();
                        // saves goalContent and howToAchieve in another dictionary
                        goalsAndHowToAchieve.Add(goal, tempAchieve);
                    }
                }

                // update the My Goals UI
                UpdateGoalsList();
            }
        });
    }

    public void RetrieveNumberOfGoalsCompleted()
    {
        dbReference.Child("playerStats/" + userId + "/numberOfGoalsCompleted").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Something went wrong when reading the data, ERROR: " + task.Exception);
                return;
            }

            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    numberOfGoalsCompleted = int.Parse(snapshot.Value.ToString());
                }
            }
        });
    }

    public void UpdateGoalsList()
    {
        // show the original prefab so that it can be instantiated
        rowPrefab.SetActive(true);

        // clear the table
        foreach (Transform item in tableContent)
        {
            // DO NOT DELETE ORIGINAL PREFAB
            if (item.name != "row_v2 (1)")
            {
                Destroy(item.gameObject);
            }
        }

        // display each set of key and value in a prefab, by changing the text content
        foreach (KeyValuePair<string, string> keyValue in goalsAndHowToAchieve)
        {
            string pushKey = "";

            // clones the prefab
            GameObject entry = Instantiate(rowPrefab, tableContent);

            // get the component of children of the prefab
            TextMeshProUGUI[] goalDetails = entry.GetComponentsInChildren<TextMeshProUGUI>();
            TMP_InputField[] inputFieldDetails = entry.GetComponentsInChildren<TMP_InputField>();
            Button[] buttonDetails = entry.GetComponentsInChildren<Button>();

            // edit the text content to display goalContent and howToAchieve
            goalDetails[0].text = keyValue.Key;
            goalDetails[1].text = keyValue.Value;

            // hide the input field gameobjects after retrieving the data
            inputFieldDetails[0].gameObject.SetActive(false);
            inputFieldDetails[1].gameObject.SetActive(false);

            // get the respective push key from the dictionary based on the goalContent
            pushKey = goalsAndKeys[keyValue.Key];

            // add functions to the buttons of the instantiated prefab
            buttonDetails[0].onClick.AddListener(delegate() { EditGoal(buttonDetails[0].gameObject); });
            buttonDetails[1].onClick.AddListener(delegate () { ConfirmEditGoal(pushKey, buttonDetails[0].gameObject, buttonDetails[1].gameObject); });
            buttonDetails[2].onClick.AddListener(delegate () { GoalCompleted(pushKey, buttonDetails[2].gameObject); });
        }

        // hide the original prefab so that the result will only display data from firebase
        rowPrefab.SetActive(false);
    }

    public void EditGoal(GameObject editButton)
    {
        string oldGoalContent, oldHowToAchieve;

        GameObject tempParent = editButton.transform.parent.gameObject;

        bool showText;

        // change text content of the button whether it is in edit mode or not
        if (editButton.GetComponentInChildren<TMP_Text>().text == "Edit")
        {
            editButton.GetComponentInChildren<TMP_Text>().text = "Cancel";
        }

        else if(editButton.GetComponentInChildren<TMP_Text>().text == "Cancel")
        {
            editButton.GetComponentInChildren<TMP_Text>().text = "Edit";
        }

        // retrieve the text content of the text gameobjects
        oldGoalContent = tempParent.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text;
        oldHowToAchieve = tempParent.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text;

        // set the text content of the input fields to be the same as the text content of the text gameobjects
        tempParent.transform.GetChild(2).gameObject.GetComponent<TMP_InputField>().text = oldGoalContent;
        tempParent.transform.GetChild(3).gameObject.GetComponent<TMP_InputField>().text = oldHowToAchieve;

        // check if the text gameobject is currently active
        showText = tempParent.transform.GetChild(0).gameObject.activeInHierarchy;

        // if else statement to toggle between showing text or input fields for the goals and howToAchieve
        if (showText)
        {
            tempParent.transform.GetChild(0).gameObject.SetActive(false);
            tempParent.transform.GetChild(1).gameObject.SetActive(false);

            tempParent.transform.GetChild(2).gameObject.SetActive(true);
            tempParent.transform.GetChild(3).gameObject.SetActive(true);
        }

        else if (!showText)
        {
            tempParent.transform.GetChild(0).gameObject.SetActive(true);
            tempParent.transform.GetChild(1).gameObject.SetActive(true);

            tempParent.transform.GetChild(2).gameObject.SetActive(false);
            tempParent.transform.GetChild(3).gameObject.SetActive(false);
        }

    }

    // BELOW FUNCTION TO POST DATA FOR EASIER RETRIEVAL
    public void ConfirmEditGoal(string key, GameObject editButton, GameObject confirmButton)
    {
        string newGoalContent, newHowToAchieve;

        // reference to the parent object to get data on sibling gameobjects
        GameObject tempParent = confirmButton.transform.parent.gameObject;

        // retrieve new goalContent and new howToAchieve
        newGoalContent = tempParent.transform.GetChild(2).gameObject.GetComponent<TMP_InputField>().text;
        newHowToAchieve = tempParent.transform.GetChild(3).gameObject.GetComponent<TMP_InputField>().text;

        // update data in firebase
        dbReference.Child("currentGoals/" + userId + "/" + key + "/goalContent").SetValueAsync(newGoalContent);
        dbReference.Child("currentGoals/" + userId + "/" + key + "/howToAchieve").SetValueAsync(newHowToAchieve);

        // timestamp properties
        var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        // update the timestamp that the goal has been updated on
        dbReference.Child("currentGoals/" + userId + "/" + key + "/updatedOn").SetValueAsync(timestamp);

        // change the text content within the game, without retrieving data from firebase even though the data has already been updated, this is to save performance
        tempParent.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = newGoalContent;
        tempParent.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = newHowToAchieve;

        // hide the input fields and display the updated goalContent and howToAchieve text gameobjects
        tempParent.transform.GetChild(0).gameObject.SetActive(true);
        tempParent.transform.GetChild(1).gameObject.SetActive(true);
        tempParent.transform.GetChild(2).gameObject.SetActive(false);
        tempParent.transform.GetChild(3).gameObject.SetActive(false);

        // change edit button text content to "Edit" from "Cancel"
        editButton.GetComponentInChildren<TMP_Text>().text = "Edit";
    }

    public void GoalCompleted(string key, GameObject doneButton)
    {
        // reference to the parent object to get data on sibling gameobjects
        GameObject tempParent = doneButton.transform.parent.gameObject;

        // increase the number of goals completed
        newNumberOfGoalsCompleted = numberOfGoalsCompleted + 1;
        numberOfGoalsCompleted = newNumberOfGoalsCompleted;

        // destroy gameobject in the game to update UI
        Destroy(tempParent);

        // delete goalContent and howToAchieve from firebase
        dbReference.Child("currentGoals/" + userId + "/" + key).SetValueAsync(null);

        // post updated number of completed goals to firebase
        dbReference.Child("playerStats/" + userId + "/numberOfGoalsCompleted").SetValueAsync(newNumberOfGoalsCompleted);

        // timestamp properties
        var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        // update playerStats updatedOn timestamp
        dbReference.Child("players/" + userId + "/updatedOn").SetValueAsync(timestamp);

        RetrieveNumberOfGoalsCompleted();
    }

    public void RetrieveMission1Content()
    {
        dbReference.Child("missionContent/mission1").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Something went wrong when reading the data, ERROR: " + task.Exception);
                return;
            }

            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    mission1Content = snapshot.Value.ToString();
                }
            }
        });
    }

    public void RetrieveMission2Content()
    {
        dbReference.Child("missionContent/mission2").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Something went wrong when reading the data, ERROR: " + task.Exception);
                return;
            }

            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    mission2Content = snapshot.Value.ToString();
                }
            }
        });
    }

    public void GetLeaderboard()
    {
        // reset the variable before retrieving from firebase
        usernameList.Clear();
        totalBuildingCountList.Clear();

        dbReference.Child("playerStats").OrderByChild("totalBuildingCount").LimitToLast(10).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Something went wrong when reading the data, ERROR: " + task.Exception);
                return;
            }

            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    foreach (var child in snapshot.Children)
                    {
                        string username = child.Child("username").Value.ToString();
                        usernameList.Add(username);

                        string tempScore = child.Child("totalBuildingCount").Value.ToString();
                        totalBuildingCountList.Add(tempScore);
                    }

                    usernameList.Reverse();
                    totalBuildingCountList.Reverse();

                    UpdateLeaderboard();
                }
            }
        });
    }

    public void UpdateLeaderboard()
    {
        leaderboardPrefab.SetActive(true);
        int rankCounter = 1;

        // clear the table
        foreach (Transform item in leaderboardTable)
        {
            // DO NOT DELETE ORIGINAL PREFAB
            if (item.name != "leaderboard")
            {
                Destroy(item.gameObject);
            }
        }

        for (int i = 0; i < usernameList.Count; i++)
        {
            // clone prefab
            GameObject entry = Instantiate(leaderboardPrefab, leaderboardTable);

            // get component of TEXTMESHPROGUI
            TextMeshProUGUI[] leaderboardDetails = entry.GetComponentsInChildren<TextMeshProUGUI>();

            leaderboardDetails[0].text = rankCounter.ToString();
            leaderboardDetails[1].text = usernameList[i];
            leaderboardDetails[2].text = totalBuildingCountList[i];

            rankCounter++;
        }

        leaderboardPrefab.SetActive(false);
    }
}
