using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// UI script assigns variables to the buttons without the use of the unity inspector to reduce confussion when changing assets
/// </summary>
public class UIManager : MonoBehaviour
{
    public TeleportXRRig teleport;
    public SwipeMenu swipeNext;

    [Header("Main UI triggers")]
    public GameObject logIn;
    public GameObject signUp;
    public GameObject signOut;
    public GameObject volume;
   

    [Header("Main UI Interfaces")]    
    public GameObject startButton;
    public GameObject Lucre;
    public GameObject logInInterface;
    public GameObject signUpInterface;
    public GameObject volumeInterface;
    public GameObject sypnopsis;


    [Header("Shop UI")]
    public GameObject NextShopItemButton;


    public void Start()
    {
        AssignAllUI();
    }
    public void AssignAllUI()
    {

        //Hide the logIn and Signup UI First
        logInInterface.SetActive(false);
        signUpInterface.SetActive(false);

        var StartButton = startButton.GetComponent<Button>();
        StartButton.onClick.AddListener(() => StartGame());

        var ShowLogIn = logIn.GetComponent<Button>();
        ShowLogIn.onClick.AddListener(() => LogIn());

        var ShowSignUp = signUp.GetComponent<Button>();
        ShowSignUp.onClick.AddListener(() => SignUp());

        var SignOut = startButton.GetComponent<Button>();
        StartButton.onClick.AddListener(() => StartGame());

        var SwipeNext = NextShopItemButton.GetComponent<Button>();
        SwipeNext.onClick.AddListener(() => NextShopItem());

        var CloseTheSypnopsis = sypnopsis.GetComponent<Button>();
        CloseTheSypnopsis.onClick.AddListener(() => CloseSypnopsis(sypnopsis));




    }

    //Hides the start and the lucre logo
    public void StartGame()
    {
        logInInterface.SetActive(true);
        startButton.SetActive(false);
        Lucre.SetActive(false);
    }

    //Displayes the login interface when the player presses log in
    public void LogIn()
    {
        logInInterface.SetActive(true);
        signUpInterface.SetActive(false);
        startButton.SetActive(false);
    }


    //display the signUp interface when the player presses sign up
    public void SignUp()
    {
        signUpInterface.SetActive(true);
        logInInterface.SetActive(false);
        startButton.SetActive(false);
        
    }

    public void SignOut()
    {
        teleport.TeleportOut();
    }

    public void NextShopItem()
    {
        swipeNext.SelectNextShopItem();
        swipeNext.GetOnlyStoreItems();

    }

    public void ShowSynopsis()
    {
        sypnopsis.SetActive(true);

    }

    public void CloseSypnopsis(GameObject synopsisThing)
    {
        Debug.Log("Sypnopsis closing");
        synopsisThing.SetActive(false);
        Debug.Log("Sypnopsis closing");
        logInInterface.SetActive(true);
    }



}
