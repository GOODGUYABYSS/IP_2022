using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditorInternal;

public class Shop : MonoBehaviour
{
    [Header("Scripts")]
    public PostingData postDataThings;

    [Header("Shop Items")]
    public GameObject ShopItems;//The shop prefabs spawned using the list
    public Transform ShopArea;//The gameobject that the shop prefabs spawn in


    [Header("Mission Things")]
    public GameObject Mission;//The Mission prefabs spawned using the list
    public Transform MissionArea;// The gameobject that the Mission prefabs spawn in

    public int Mission2BuildingCount;
    /// <summary>
    //[Header("Notification")]

    ///  public GameObject Notification;//The Notification
    //public Transform NotificationArea;// The area that notifications spawn in

    [Header("Buildings")]
    public GameObject building1;//The building prefab with the turn table script
    public GameObject building2;//the building prefab with the turn table script
    public GameObject building3;//the building prefab with the turn table script
    public GameObject building4;//the building prefab with the turn table script
    public GameObject building5;//the building prefab with the turn table script

    public GameObject rightHandController;

    public static bool mouseClicked = false;

    public static Dictionary<string, GameObject> buttonList = new Dictionary<string, GameObject>();

    public GameObject buildingSpawnArea;

    public static bool allowBuyBuilding;

    private void Start()
    {
        
    }

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
            if (item.name != "MissionPrefab")
            {
                Destroy(item.gameObject);
            }
        }

        // destroy the game object that are placeholders


        foreach (ShopItemsClass i in RetrievingData.storeList)
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

    //storage function
    //Stores i.name(Item), i.credit generation (credit generation), if  i.name= building 1, Missions logs M1 = RetrievingData.missionList[0] ;

    public void MissionPrefab(string Item, int CreditGeneration)
    {
        // set mission prefab to true so that it can be instantiated
        Mission.SetActive(true);

        //foreach (Transform item in MissionArea)
        //{
        //    if (item.name != "MissionPrefab")
        //    {
        //        Destroy(item.gameObject);
        //    }
        //}

        var Mission1Data = RetrievingData.missionList[0];
        var Mission2Data = RetrievingData.missionList[1];

        MissionLogs MissionData = Mission1Data;

        if (Item == "Building3")
        {
            MissionData = Mission2Data;
        }

        GameObject mission = Instantiate(Mission, MissionArea);
        TextMeshProUGUI[] MissionVar = mission.GetComponentsInChildren<TextMeshProUGUI>();
        MissionVar[0].text = MissionData.missionContent;
        MissionVar[1].text = "Complete to obtain " + MissionData.buildingName;
        var button = mission.GetComponentsInChildren<Button>();

        if (Item == RetrievingData.missionList[0].buildingName)
        {
            button[0].onClick.AddListener(() => DestroyMissionPrefab(mission));
            button[0].onClick.AddListener(() => postDataThings.UpdateMission1NoAttempt());
            button[0].onClick.AddListener(() => SpawnItem(building1, CreditGeneration));
            buttonList.Add("mission1", button[0].gameObject);
            button[0].gameObject.SetActive(false);

            if (MissionData.missionStatus == "completed")
            {
                button[0].gameObject.SetActive(true);
            }
        }

        else if (Item == RetrievingData.missionList[1].buildingName)
        {
            button[0].onClick.AddListener(() => DestroyMissionPrefab(mission));
            button[0].onClick.AddListener(() => postDataThings.UpdateMission2NoAttempt());
            button[0].onClick.AddListener(() => SpawnItem(building3, CreditGeneration));
            buttonList.Add("mission2", button[0].gameObject);
            button[0].gameObject.SetActive(false);

            if (MissionData.missionStatus == "completed")
            {
                button[0].gameObject.SetActive(true);
            }
        }


        //if (Mission1Data.missionStatus == "completed")
        //{
        //    button[0].gameObject.SetActive(true);
        //    ClaimMe.onClick.AddListener(() => SpawnItem(building1, CreditGeneration));


        //}

        //if (Mission2Data.missionStatus == "completed")
        //{
        //    button[0].gameObject.SetActive(true);
        //    ClaimMe.onClick.AddListener(() => SpawnItem(building3, CreditGeneration));
        //}

    }

    public void DestroyMissionPrefab(GameObject missionPrefab)
    {
        Destroy(missionPrefab);
        Debug.Log("Destroyed");
    }


    public void ClickToBuy(string Item, string tag, int Price, int CreditGeneration)
    {


        Debug.Log("Bought " + Item);

        if (tag == "Building1")
        {

            Debug.Log("Mission 1 Set a savings goal");
            //Some function to create mission and save mission in the database in the mission panel
            if (RetrievingData.mission1Status == "onGoing")
            {
                Debug.Log("Please complete your current mission");
                //send some notification that cannot access
            }
            else
            {//spawn m1 prefab
                MissionPrefab(Item, CreditGeneration);
                RetrievingData.mission1Status = "onGoing";

                postDataThings.UpdateMission1Ongoing();

            }

            //Add mission to mission list

            //Spawn the prefab for building 1
        }

        // function that retrieves the mission data at the beginning
        if (tag == "Building2")
        {
            Debug.Log("PaySomeMoney");
            if (RetrievingData.credits >= Price)
            {
                SpawnItem(building2);
                RetrievingData.credits = RetrievingData.credits - Price;
                Debug.Log("How much money you have left:" + RetrievingData.credits);
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
            if (RetrievingData.mission2Status == "onGoing")
            {
                Debug.Log("Please complete your current mission");
                //send some notification that cannot access
            }
            else
            {//spawn m2 prefab
                MissionPrefab(Item, CreditGeneration);
                RetrievingData.mission2Status = "onGoing";
                postDataThings.StartMission2();
                postDataThings.UpdateMission2Ongoing();
            }
        }

        if (tag == "Building4")
        {

            Debug.Log("PaySomeMoney");
            if (RetrievingData.credits >= Price)
            {
                SpawnItem(building4);
                RetrievingData.credits = RetrievingData.credits - Price;
                GoalsList.maxNumGoals += 1;
                Debug.Log("How much money you have left:" + RetrievingData.credits);
            }

            else
            {
                Debug.Log(" You do not have enough money to buy this item. Come back later");
            }
        }

        if (tag == "Building5")
        {
            Debug.Log("PaySomeMoney");
            if (RetrievingData.credits >= Price)
            {
                SpawnItem(building5);
                RetrievingData.credits = RetrievingData.credits - Price;
                GoalsList.maxNumGoals += 1;
                Debug.Log("How much money you have left:" + RetrievingData.credits);
            }

            else
            {
                Debug.Log(" You do not have enough money to buy this item. Come back later");
            }
        }


    }

    public void SpawnItem(GameObject thingToSpawn, int creditGeneration = 0)
    {
        mouseClicked = true;
        allowBuyBuilding = false;

        // GameObject entry = Instantiate(thingToSpawn, position: rightHandController.transform.position, rotation: thingToSpawn.transform.rotation, rightHandController.transform);
        GameObject entry = Instantiate(thingToSpawn, position: buildingSpawnArea.transform.position, rotation: buildingSpawnArea.transform.rotation);

        // entry.transform.localPosition = new Vector3(0, 0, 0);

        entry.AddComponent<ObjectId>();
        ObjectId.CreateObjectId(entry);

        entry.GetComponent<ObjectId>().creditGeneration = creditGeneration;
        ObjectId.creditGenerationSum += creditGeneration;

        BuildingDescription.allBoxColliders.Add(entry.GetComponent<BoxCollider>());

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

        entry.AddComponent<BuildingDescription>();

        StartCoroutine(WaitAfterButtonPress());

        // add one more building owned by the player to firebase
        RetrievingData.totalBuildingCount += 1;
        postDataThings.UpdateTotalBuildingCount();
    }

    IEnumerator WaitAfterButtonPress()
    {
        yield return new WaitForSeconds(0.2f);

        mouseClicked = false;
    }

    //in the missions prefab there will be a button that allows player to claim their building after they have completed the mission.
    //This button will spawn the building in the players hand,(needs to save the players creditsGeneration) only if there is no other building in their hand
}
