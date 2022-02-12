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
    public Shop shopScript;
    public SwipeMenu swipeMenu;
    public DatabaseReference dbReference;

    public static List<ShopItemsClass> storeList = new List<ShopItemsClass>();
    public static List<MissionLogs> missionList = new List<MissionLogs>();
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

    public static int totalBuildingCount;
    public static int usefulBuildingCount;
    public static int uselessBuildingCount;

    public int goalSlotsLeft;

    //Goals that already exist
    public GameObject rowPrefab;
    public Transform tableContent;

    public GameObject leaderboardPrefab;
    public Transform leaderboardTable;

    public GameObject goalSlotsText;
    public GameObject goalCompletedText;

    private void Awake()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        RetrieveStoreThings();

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

            RetrieveMissionLogs();

            GetLeaderboard();

            RetrieveGoals();

            // stop the loop
            anotherLoginRetrievingData = false;
        }
    }
    public void RetrieveStoreThings()
    {
        dbReference.Child("storeThings").GetValueAsync().ContinueWithOnMainThread(task =>
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
                    

                    foreach (DataSnapshot d in snapshot.Children)
                    {
                        ShopItemsClass store = JsonUtility.FromJson<ShopItemsClass>(d.GetRawJsonValue());
                        storeList.Add(store);
                    }

                    foreach (ShopItemsClass store in storeList)
                    {
                        Debug.LogFormat("Name: {0}, Description: {1}, powerUp: {2}, price {3}", store.name, store.description, store.powerUp, store.price);
                    }
                    shopScript.DisplayShopItems();
                    swipeMenu.GetOnlyStoreItems();
                }

                
            }
        });
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
                    numberOfGoalsCompleted = int.Parse(snapshot.Child("numberOfGoalsCompleted").Value.ToString());
                    totalBuildingCount = int.Parse(snapshot.Child("totalBuildingCount").Value.ToString());
                    usefulBuildingCount = int.Parse(snapshot.Child("usefulBuildingCount").Value.ToString());
                    uselessBuildingCount = int.Parse(snapshot.Child("uselessBuildingCount").Value.ToString());
                    goalSlotsLeft = int.Parse(snapshot.Child("goalSlotsLeft").Value.ToString());

                    // set the max number of goals for the player
                    GoalsList.maxNumGoals = goalSlotsLeft;
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

    public void UpdateGoalsList()
    {
        int goalSlotsLeft = GoalsList.maxNumGoals;
        goalSlotsText.GetComponent<TMP_Text>().text = "You can add " + goalSlotsLeft + " more goals.";
        goalCompletedText.GetComponent<TMP_Text>().text = numberOfGoalsCompleted + " goals completed";
        // show the original prefab so that it can be instantiated
        rowPrefab.SetActive(true);

        // clear the table
        foreach (Transform item in tableContent)
        {
            // DO NOT DELETE ORIGINAL PREFAB
            if (item.name != "GoalPrefab")
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
            buttonDetails[3].onClick.AddListener(delegate () { DeleteGoal(pushKey, buttonDetails[3].gameObject); });

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

        goalCompletedText.GetComponent<TMP_Text>().text = newNumberOfGoalsCompleted + " goals completed";

        // delete goalContent and howToAchieve from firebase
        dbReference.Child("currentGoals/" + userId + "/" + key).SetValueAsync(null);

        // post updated number of completed goals to firebase
        dbReference.Child("playerStats/" + userId + "/numberOfGoalsCompleted").SetValueAsync(newNumberOfGoalsCompleted);

        // timestamp properties
        var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        // update playerStats updatedOn timestamp
        dbReference.Child("players/" + userId + "/updatedOn").SetValueAsync(timestamp);
    }

    public void DeleteGoal(string key, GameObject deleteButton)
    {
        GameObject tempParent = deleteButton.transform.parent.gameObject;

        Destroy(tempParent);

        GoalsList.maxNumGoals += 1;
        dbReference.Child("playerStats/" + userId + "/goalSlotsLeft").SetValueAsync(GoalsList.maxNumGoals);

        goalSlotsText.GetComponent<TMP_Text>().text = "You can add " + GoalsList.maxNumGoals + " more goals.";

        dbReference.Child("currentGoals/" + userId + "/" + key).SetValueAsync(null);
    }

    public void GetLeaderboard()
    {
        // reset the variable before retrieving from firebase
        usernameList.Clear();
        totalBuildingCountList.Clear();

        dbReference.Child("playerStats").OrderByChild("totalBuildingCount").LimitToLast(3).GetValueAsync().ContinueWithOnMainThread(task =>
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

    public void RetrieveMissionLogs()
    {
        dbReference.Child("missionLogs/" + userId).GetValueAsync().ContinueWithOnMainThread(task =>
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
                    foreach (DataSnapshot d in snapshot.Children)
                    {
                        MissionLogs mission = JsonUtility.FromJson<MissionLogs>(d.GetRawJsonValue());
                        missionList.Add(mission);
                    }

                    foreach (MissionLogs mission in missionList)
                    {
                        Debug.LogFormat("content: {0}, status: {1}, name: {2}", mission.missionContent, mission.missionStatus, mission.buildingName);
                    }
                }

            }
        });
    }
}
