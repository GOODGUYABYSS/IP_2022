using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Database;
using System;
using TMPro;

public class Authentication : MonoBehaviour
{
    FirebaseAuth auth;
    public DatabaseReference dbReference;

    public TeleportXRRig teleport;
    public static string userId;
    public static string email;

    public bool test;

    public GameObject signUpUI;
    public GameObject loginUI;

    [SerializeField]
    GameObject usernameInputSignUp, emailInputSignUp, passwordInputSignUp, emailInputLogin, passwordInputLogin, emailInputForgetPassword, errorSignUp, errorLogin, errorForgetPassword;
    [SerializeField]
    string errorSignUpMessage, errorLoginMessage, errorForgetPasswordMessage;

    public static bool loggedIn;
    public static bool signedUp;

    private void Awake()
    {
        // initialise firebase
        auth = FirebaseAuth.DefaultInstance;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // update display error message
        errorLogin.GetComponent<TMP_Text>().text = errorLoginMessage;
        errorSignUp.GetComponent<TMP_Text>().text = errorSignUpMessage;
        errorForgetPassword.GetComponent<TMP_Text>().text = errorForgetPasswordMessage;

        if (signedUp)
        {
            signUpUI.SetActive(false);
            loginUI.SetActive(true);
        }

        // check if user is logged in
        if (loggedIn)
        {
            teleport.Teleport();
        }
    }

    public void SigningUp()
    {
        // get relevant data from input fields
        string usernameSignUp = usernameInputSignUp.GetComponent<TMP_InputField>().text;
        string emailSignUp = emailInputSignUp.GetComponent<TMP_InputField>().text;
        string passwordSignUp = passwordInputSignUp.GetComponent<TMP_InputField>().text;

        // make sure that user typed something in username
        if (usernameSignUp == "" || usernameSignUp == " ")
        {
            errorLoginMessage = "";
            errorSignUpMessage = "Please enter a valid username";
            return;
        }

        // create new account for users
        auth.CreateUserWithEmailAndPasswordAsync(emailSignUp, passwordSignUp).ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                // show error
                Debug.LogError("Sorry, there was an error creating your account, ERROR: " + task.Exception);
                test = true;
                errorLoginMessage = "";
                errorSignUpMessage = "Please enter a valid email and password.";
                return;
            }

            else if (task.IsCompleted)
            {
                FirebaseUser newPlayer = task.Result;
                Debug.LogFormat("Welcome to my new game, {0}", newPlayer.Email);
                userId = newPlayer.UserId;
                email = newPlayer.Email;

                signedUp = true;

                // create necessary data for player
                CreateNewPlayer(userId, usernameSignUp, email, true);
                CreatePlayerStats(userId, usernameSignUp, 0, 1, 100, 0);
                CreateMissionLogs(userId, "mission1", "Create a financial goal", "noAttempt", "Building1");
                CreateMissionLogs(userId, "mission2", "Complete 3 financial goals", "noAttempt", "Building3");
            }
        });
    }

    // create new classes object to run the function in sign up
    void CreateNewPlayer(string uuid, string username, string email, bool active)
    {
        NewPlayer createNewPlayer = new NewPlayer(username, email, active);

        dbReference.Child("players/" + uuid).SetRawJsonValueAsync(createNewPlayer.NewPlayerToJson());
    }

    // create new classes object to run the function in sign up
    void CreatePlayerStats(string uuid, string username, int totalBuildingCount, int goalSlotsLeft, int credits, int numberOfGoalsCompleted)
    {
        PlayerStats createPlayerStats = new PlayerStats(username, totalBuildingCount, goalSlotsLeft, credits, numberOfGoalsCompleted);

        dbReference.Child("playerStats/" + uuid).SetRawJsonValueAsync(createPlayerStats.PlayerStatsToJson());
    }

    // create new classes object to run the function in sign up
    void CreateMissionLogs(string uuid, string missionNumber, string missionContent, string missionStatus, string buildingName)
    {
        MissionLogs createMissionLogs = new MissionLogs(missionContent, missionStatus, buildingName);

        dbReference.Child("missionLogs/" + uuid + "/" + missionNumber).SetRawJsonValueAsync(createMissionLogs.MissionLogsToJson());
    }

    // create new classes object to run the function in sign up
    public void LoggingIn()
    {
        string emailLogin = emailInputLogin.GetComponent<TMP_InputField>().text;
        string passwordLogin = passwordInputLogin.GetComponent<TMP_InputField>().text;

        auth.SignInWithEmailAndPasswordAsync(emailLogin, passwordLogin).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                // show error
                Debug.LogError("Sorry, there was an error logging into your account, ERROR: " + task.Exception);
                errorSignUpMessage = "";
                errorLoginMessage = "Wrong email or password.";
                return;
            }

            else if (task.IsCompleted)
            {
                FirebaseUser User = task.Result;
                Debug.LogFormat("Welcome to my new game, {0}", User.Email);
                userId = User.UserId;
                email = User.Email;

                loggedIn = true;
                
                UpdateLoginTime();

            }
        });
    }

    // forget password
    public void ForgetPassword()
    {
        string emailForgetPassword = emailInputForgetPassword.GetComponent<TMP_InputField>().text;

        auth.SendPasswordResetEmailAsync(emailForgetPassword).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                // send an error
                Debug.LogError("SendPasswordResetEmailAsync was canceled");
                return;
            }

            else if (task.IsFaulted)
            {
                // send an error
                Debug.LogError("SendPasswordResetAsync encountered an error " + task.Exception);
                errorForgetPasswordMessage = "Please enter a valid email.";
                return;
            }

            else if (task.IsCompleted)
            {
                Debug.Log("Password reset email is sent successfully.");
            }
        });
    }

    // update the last logged in timestamp for the player
    void UpdateLoginTime()
    {
        // timestamp properties
        var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        // updates the last logged in timestamp for the player
        dbReference.Child("players/" + userId + "/lastLoggedIn").SetValueAsync(timestamp);
    }

    public void SigningOut()
    {
        if (auth.CurrentUser != null)
        {
            auth.SignOut();

            // reset all the static datas
            loggedIn = false;
            userId = "";
            PostingData.anotherLoginPostingData = true;
            RetrievingData.anotherLoginRetrievingData = true;
            RetrievingData.credits = 0;
            RetrievingData.numberOfGoalsCompleted = 0;
            RetrievingData.totalBuildingCount = 0;
            RetrievingData.storeList.Clear();
            RetrievingData.missionList.Clear();
            RetrievingData.mission1Status = "noAttempt";
            RetrievingData.mission2Status = "noAttempt";

            Shop.mouseClicked = true;
            Shop.buttonList.Clear();


            BuildingDescription.allBoxColliders.Clear();
            Control.allBuildingData.Clear();
            ObjectId.creditGenerationSum = 0;
            ObjectId.allObjectIds.Clear();

            GoalsList.maxNumGoals = 0;

            Debug.Log("User has been logged out");
        }
    }
}
