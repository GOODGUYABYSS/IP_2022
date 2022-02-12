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
    public string Mission1Status;
    public string Mission2Status;
    public int Mission2BuildingCount;
    /// <summary>
    //[Header("Notification")]

    ///  public GameObject Notification;//The Notification
    //public Transform NotificationArea;// The area that notifications spawn in

    [Header("Buildings")]
    public GameObject Building1;//The building prefab with the turn table script
    public GameObject building2;//the building prefab with the turn table script
    public GameObject building3;//the building prefab with the turn table script
    public GameObject building4;//the building prefab with the turn table script
    // public GameObject building5;//the building prefab with the turn table script

    public GameObject rightHandController;
    public GameObject xrObjects;


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


        foreach ( ShopItemsClass i in RetrievingData.storeList)
        {
            Debug.Log(i.name);
            //Spawn a card with the building
            GameObject entry = Instantiate(Prefab, PrefabPos);
            TextMeshProUGUI[] ShopItemsName = entry.GetComponentsInChildren<TextMeshProUGUI>();
            ShopItemsName[0].text = i.name;
            ShopItemsName[1].text = i.powerUp;
            ShopItemsName[2].text = i.description;

            //Create unity tag layers
            InternalEditorUtility.AddTag(i.name);

            //Assign unity tag layers
            var tag = entry.tag = i.name;

            //Assign function to check for missions/ price
            var button = entry.GetComponentsInChildren<Button>();
            var ClickMe = button[0].GetComponent<Button>();
            ClickMe.onClick.AddListener(() => ClickToBuy(i.name, tag, i.price, i.creditGeneration));

            entry.transform.localScale = Vector3.one;
        }
    }

    public void ClickToBuy(string Item, string tag, int Price, int CreditGeneration)
    {

        MissionLogs M1 =  RetrievingData.missionList[0];
        MissionLogs M2 = RetrievingData.missionList[1];


        Debug.Log("Bought " + Item);

        if (tag == "Building1")
        {   
            
            Debug.Log("Mission 1 Set a savings goal");
            //Some function to create mission and save mission in the database in the mission panel
            if(Mission1Status == "onGoing")
            {
                Debug.Log("Please complete your current mission");
                //send some notification that cannot access
            }
            else
            {//spawn m1 prefab
                GameObject mission = Instantiate(Mission, MissionArea);
                TextMeshProUGUI[] MissionVar = mission.GetComponentsInChildren<TextMeshProUGUI>();
                MissionVar[0].text = M1.missionContent;
                MissionVar[1].text = "Complete to obtain " + M1.buildingName;
                var button = mission.GetComponentsInChildren<Button>();
                var ClaimMe = button[0].GetComponent<Button>();
                Mission1Status = "onGoing";

            }

            //Add mission to mission list

            //Spawn the prefab for building 1
        }

        if (tag == "Building2")
        {
            Debug.Log("PaySomeMoney");
            if (credits >= Price)
            {
                SpawnItem(building2);
                credits = credits - Price;
                Debug.Log("How much money you have left:" + credits);
                GoalsList.maxNumGoals += 1;
            }
            else
            {
                Debug.Log(" You do not have enough money to buy this item. Come back later");
                //Spawn this in the notification area with text mesh pro
            }

            //Spawn building in the players hand
        }

        if (tag == "Building3")
        {
            Debug.Log("Mission 2 Buy 2 useless buildings");

            if (Mission2Status == "onGoing")
            {
                Debug.Log("Please complete your current mission");
                //send some notification that cannot access
            }
            else
            {//spawn m2 prefab
                GameObject mission = Instantiate(Mission, MissionArea);
                TextMeshProUGUI[] MissionVar = mission.GetComponentsInChildren<TextMeshProUGUI>();
                MissionVar[0].text = M2.missionContent;
                MissionVar[1].text = "Complete to obtain " + M2.buildingName;
                Mission2Status = "onGoing";

            }
        }

        if (tag == "Building4")
        {

            Debug.Log("PaySomeMoney");
            if (credits >= Price)
            {
                SpawnItem(building4);
                credits = credits - Price;
                GoalsList.maxNumGoals += 1;
                Debug.Log("How much money you have left:" + credits);
            }
            
            else
            {
                Debug.Log(" You do not have enough money to buy this item. Come back later");
            }
        }

        if (tag == "Building5")
        {
            Debug.Log("PaySomeMoney");
            if (credits >= Price)
            {
                credits = credits - Price;
                GoalsList.maxNumGoals += 1;
                Debug.Log("How much money you have left:" + credits);
            }

            else
            {
                Debug.Log(" You do not have enough money to buy this item. Come back later");
            }
        }


    }

    public void claimBuilding( string building)
    {
        
    }


   public void SpawnItem(GameObject thingToSpawn, int creditGeneration = 0)
    {
        // xrObjects.transform.position;

        GameObject entry = Instantiate(thingToSpawn, position: rightHandController.transform.position, rotation: thingToSpawn.transform.rotation, rightHandController.transform);

        entry.transform.localPosition = new Vector3(0, 0, 0);

        entry.AddComponent<ObjectId>();

        ObjectId.CreateObjectId(entry);

        if (creditGeneration > 0)
        {
            entry.GetComponent<ObjectId>().objectType = "MoneyGeneratingBuilding";
        }

        else if (creditGeneration == 0)
        {
            entry.GetComponent<ObjectId>().objectType = "GoalGeneratingBuilding";
        }

        else
        {
            throw new System.Exception("Building type does not exist.");
        }

        entry.GetComponent<BoxCollider>().enabled = false;
        entry.AddComponent<BuildingDescription>();
    }

    //in the missions prefab there will be a button that allows player to claim their building after they have completed the mission.
    //This button will spawn the building in the players hand,(needs to save the players creditsGeneration) only if there is no other building in their hand
}
