using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditorInternal;

public class Shop : MonoBehaviour
{   public int credits = 100;

    [Header("Shop Items")]
    public GameObject ShopItems;//The shop prefabs spawned using the list
    public Transform ShopArea;//The gameobject that the shop prefabs spawn in

    [Header("Mission Things")]
    public GameObject Mission;//The Mission prefabs spawned using the list
    public Transform MissionArea;// The gameobject that the Mission prefabs spawn in
    public bool Mission1Status;
    public bool Mission2Status;
    public int Mission2BuildingCount;
    /// <summary>
    //[Header("Notification")]

    ///  public GameObject Notification;//The Notification
    //public Transform NotificationArea;// The area that notifications spawn in

    //[Header("Buildings")]
    //public GameObject Building1;//The building prefab with the turn table script
    //public GameObject Building2;//The building prefab with the turn table script
    //public GameObject Building3;//The building prefab with the turn table script
    //public GameObject Building4;//The building prefab with the turn table script
    //public GameObject Building5;//The building prefab with the turn table script
    /// </summary>


    public void DisplayShopItems()
    {

        var Prefab = ShopItems;
        var PrefabPos = ShopArea;

        //Bug that disables all the items that are spawned
        foreach (Transform item in ShopArea)
        {
            Destroy(item.gameObject);
        }

        foreach (Transform item in MissionArea)
        {
            Destroy(item.gameObject);
        }

        // destroy the game object that are placeholders


        ShopItemsClass fakeItem1 = new ShopItemsClass("Building 1","Generates 5 coins per minute", "Mission 1: Create a savings goal ", 0, false);
        ShopItemsClass fakeItem2 = new ShopItemsClass("Building 2", "Unlocks a Goal slot", "Cost 10 Credits ", 10, false);
        ShopItemsClass fakeItem3 = new ShopItemsClass("Building 3", "Generates 5 coins per minute", "Mission 1: Create a savings goal ", 0, false);
        ShopItemsClass fakeItem4 = new ShopItemsClass("Building 4", "Unlocks a Goal slot", "Cost 20 Credits ", 20, false);


        //list that contains all the shop items(Take from database if possible)

        //loop through the data snapshot,
        //Add the object things to the list

        List<ShopItemsClass> ShopItemsLists = new List<ShopItemsClass>() { fakeItem1, fakeItem2, fakeItem3, fakeItem4 };
       
       

        foreach ( ShopItemsClass i in ShopItemsLists)
        {
            //Spawn a card with the building
            GameObject entry = Instantiate(Prefab, PrefabPos);
            TextMeshProUGUI[] ShopItemsName = entry.GetComponentsInChildren<TextMeshProUGUI>();
            ShopItemsName[0].text = i.Name;
            ShopItemsName[1].text = i.PowerUp;
            ShopItemsName[2].text = i.Description;

            //Create unity tag layers
            InternalEditorUtility.AddTag(i.Name);

            //Assign unity tag layers
            var tag = entry.tag = i.Name;

            //Assign function to check for missions/ price
            var button = entry.GetComponentsInChildren<Button>();
            var ClickMe = button[0].GetComponent<Button>();
            ClickMe.onClick.AddListener(() => ClickToBuy(i.Name, tag, i.Price));


        }
    }

    public void ClickToBuy(string Item, string tag, int Price)
    {

        Mission M1 = new Mission("Create a savings goal", "Building 1", Mission1Status);
        Mission M2 = new Mission("Buy 2 Useless Buildings", "Building 3", Mission2Status);

        Debug.Log("Bought " + Item);

        if (tag == "Building 1")
        {   
            
            Debug.Log("Mission 1 Set a savings goal");
            //Some function to create mission and save mission in the database in the mission panel
            if(Mission1Status == true)
            {
                Debug.Log("Please complete your current mission");
                //send some notification that cannot access
            }
            else
            {//spawn m1 prefab
                GameObject mission = Instantiate(Mission, MissionArea);
                TextMeshProUGUI[] MissionVar = mission.GetComponentsInChildren<TextMeshProUGUI>();
                MissionVar[0].text = M1.Name;
                MissionVar[1].text = "Complete to obtain " + M1.ItemToObtain;
                Mission1Status = true;

            }
            

            //Add mission to mission list
        }

        if (tag == "Building 2")
        {
            Debug.Log("PaySomeMoney");
            if (credits >= Price)
            {
                credits = credits - Price;
                Debug.Log("How much money you have left:" + credits);
            }
            else
            {
                Debug.Log(" You do not have enough money to buy this item. Come back later");
                //Spawn this in the notification area with text mesh pro
            }

            //Spawn building in the players hand
        }

        if (tag == "Building 3")
        {
            Debug.Log("Mission 2 Buy 2 useless buildings");

            if (Mission2Status == true)
            {
                Debug.Log("Please complete your current mission");
                //send some notification that cannot access
            }
            else
            {//spawn m2 prefab
                GameObject mission = Instantiate(Mission, MissionArea);
                TextMeshProUGUI[] MissionVar = mission.GetComponentsInChildren<TextMeshProUGUI>();
                MissionVar[0].text = M2.Name;
                MissionVar[1].text = "Complete to obtain " + M2.ItemToObtain;
                Mission2Status = true;

            }
        }

        if (tag == "Building 4")
        {
            Debug.Log("PaySomeMoney");
            if (credits >= 20)
            {
                credits = credits - 20;
                Debug.Log("How much money you have left:" + credits);
            }
            
            else
            {
                Debug.Log(" You do not have enough money to buy this item. Come back later");
            }
        }
    }

    public void DisplayMission()
    {
        //loop through firebase to get mission details
        //Spawn mission according to 
        


    }

    private void Awake()
    {
        DisplayShopItems();
    }


    public void SpawnItemInShop(int BuildingName)
    {
        
    }

}
