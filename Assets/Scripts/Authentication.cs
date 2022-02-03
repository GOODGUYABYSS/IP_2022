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

    public static string userId;
    public static string email;

    public bool test;

    [SerializeField]
    GameObject usernameInputSignUp, emailInputSignUp, passwordInputSignUp, emailInputLogin, passwordInputLogin, emailInputForgetPassword, errorSignUp, errorLogin, errorForgetPassword;
    [SerializeField]
    string errorSignUpMessage, errorLoginMessage, errorForgetPasswordMessage;

    public static bool loggedIn;

    private void Awake()
    {
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
        errorLogin.GetComponent<TMP_Text>().text = errorLoginMessage;
        errorSignUp.GetComponent<TMP_Text>().text = errorSignUpMessage;
        errorForgetPassword.GetComponent<TMP_Text>().text = errorForgetPasswordMessage;

        // check if user is logged in
        if (loggedIn)
        {

        }
    }

    public void SigningUp()
    {
        string usernameSignUp = usernameInputSignUp.GetComponent<TMP_InputField>().text;
        string emailSignUp = emailInputSignUp.GetComponent<TMP_InputField>().text;
        string passwordSignUp = passwordInputSignUp.GetComponent<TMP_InputField>().text;

        if (usernameSignUp == "" || usernameSignUp == " ")
        {
            errorLoginMessage = "";
            errorSignUpMessage = "Please enter a valid username";
            return;
        }

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

                loggedIn = true;

                CreateNewPlayer(userId, usernameSignUp, email, true);
                CreatePlayerStats(userId, usernameSignUp, 0, 0, 0, 100, 0, false, false);
            }
        });
    }

    void CreateNewPlayer(string uuid, string username, string email, bool active)
    {
        NewPlayer createNewPlayer = new NewPlayer(username, email, active);

        dbReference.Child("players/" + uuid).SetRawJsonValueAsync(createNewPlayer.NewPlayerToJson());
    }

    void CreatePlayerStats(string uuid, string username, int usefulBuildingCount, int uselessBuildingCount, int totalBuildingCount, int credits, int numberOfGoalsCompleted, bool mission1Completed, bool mission2Completed)
    {
        PlayerStats createPlayerStats = new PlayerStats(username, uselessBuildingCount, usefulBuildingCount, totalBuildingCount, credits, numberOfGoalsCompleted, mission1Completed, mission2Completed);

        dbReference.Child("playerStats/" + uuid).SetRawJsonValueAsync(createPlayerStats.PlayerStatsToJson());
    }

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
            RetrievingData.mission1Completed = false;
            RetrievingData.mission2Completed = false;
            RetrievingData.totalBuildingCount = 0;
            RetrievingData.usefulBuildingCount = 0;
            RetrievingData.uselessBuildingCount = 0;

            Debug.Log("User has been logged out");
        }
    }
}
