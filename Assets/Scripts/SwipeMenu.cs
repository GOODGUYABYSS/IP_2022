using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    public GameObject ShopScrollBar;
    public float scrollPos;
    public Transform ShopContent;
    public List<Transform> onlyShopItems = new List<Transform>();
    public int ItemCount = 1;


    public void GetOnlyStoreItems()
    {
        var ScrollItems = ShopContent.GetComponentsInChildren<Transform>();

        if (onlyShopItems.Count == 0)
        {
            foreach (Transform child in ScrollItems)
            {
                if (child.parent.name == "Content")
                {
                    onlyShopItems.Add(child);

                    
                }

                
               
            }

            foreach (Transform child in onlyShopItems)
            {
                child.localScale = Vector3.Lerp(child.localScale, new Vector3(.8f, .8f, .8f), 1f);
                child.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.4f);


            }

        }
        else
        {
            Debug.Log("No spawn anymore");
        }

        //Get only the child and not the grandchild of the shopContent
       
        


    }

    //attach this to the button 
    public void SelectNextShopItem()
    {
        //Distroy any null items in the list
        for (var i = onlyShopItems.Count - 1; i > -1; i--)
        {
            if (onlyShopItems[i] == null)
                onlyShopItems.RemoveAt(i);


        }

        foreach (Transform child in onlyShopItems)
        {
            child.localScale = Vector3.Lerp(child.localScale, new Vector3(.8f, .8f, .8f), 1f);
            child.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.4f);
           

        }

         if (ItemCount < onlyShopItems.Count)
        {
            onlyShopItems[ItemCount].localScale = Vector3.Lerp(onlyShopItems[ItemCount].localScale, new Vector3(1f, 1f, .8f), 1f);
            onlyShopItems[ItemCount].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.9f);

            Debug.Log(ItemCount);
            ItemCount += 1;
       

        //Part 2 
        //Scroll the scrollbar to the currently selected item
        //Amount of scroll = 1 divide by the (number of children) and add to the scrollbar.value     
            var ScrollDistance = 1f / onlyShopItems.Count;
        Debug.Log(scrollPos);
        scrollPos += ScrollDistance;
        ShopScrollBar.GetComponent<Scrollbar>().value = scrollPos;

        }
        else
        {
            Debug.Log("There are no more items");//Notification that there are no more items
            ItemCount = 0;
            scrollPos = 0;
            onlyShopItems[ItemCount].localScale = Vector3.Lerp(onlyShopItems[ItemCount].localScale, new Vector3(1f, 1f, .8f), 1f);
            onlyShopItems[ItemCount].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.9f);
            ShopScrollBar.GetComponent<Scrollbar>().value = scrollPos;
        }
        //Part 1
        //Make the selected item bigger and the previous and next item fade in alittle bitw
        //itemCount = add one to item count
        //will select the child item of the (shop transform area) index(itemCount) and make bigger

        //If itemcount is lesser then onlyShopItems.length, then the enlarge scroll thing works, else give notification that there is no more items
        //go back button that -Item count and scrolls backwards, if itemCount is lesser then 0, another notification that there is no more items
       


        
        

    }

    public void SelectPreviousShopItem()
    {
        //Distroy any null items in the list
        for (var i = onlyShopItems.Count - 1; i > -1; i--)
        {
            if (onlyShopItems[i] == null)
                onlyShopItems.RemoveAt(i);


        }

        foreach (Transform child in onlyShopItems)
        {
            child.localScale = Vector3.Lerp(child.localScale, new Vector3(.8f, .8f, .8f), 1f);
            child.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.4f);


        }

        if (0 <= ItemCount && ItemCount < onlyShopItems.Count)
        {
            onlyShopItems[ItemCount].localScale = Vector3.Lerp(onlyShopItems[ItemCount].localScale, new Vector3(1f, 1f, .8f), 1f);
            onlyShopItems[ItemCount].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.9f);

            Debug.Log(ItemCount);
            ItemCount -= 1;


            //Part 2 
            //Scroll the scrollbar to the currently selected item
            //Amount of scroll = 1 divide by the (number of children) and add to the scrollbar.value     
            var ScrollDistance = 1f / onlyShopItems.Count;
            Debug.Log(scrollPos);
            scrollPos -= ScrollDistance;
            ShopScrollBar.GetComponent<Scrollbar>().value = scrollPos;

        }
        else
        {
            Debug.Log("There are no more items");//Notification that there are no more items
            ItemCount = onlyShopItems.Count;
            scrollPos = 1;
            onlyShopItems[ItemCount].localScale = Vector3.Lerp(onlyShopItems[ItemCount].localScale, new Vector3(1f, 1f, .8f), 1f);
            onlyShopItems[ItemCount].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.9f);
            ShopScrollBar.GetComponent<Scrollbar>().value = scrollPos;
        }
        //Part 1
        //Make the selected item bigger and the previous and next item fade in alittle bitw
        //itemCount = add one to item count
        //will select the child item of the (shop transform area) index(itemCount) and make bigger

        //If itemcount is lesser then onlyShopItems.length, then the enlarge scroll thing works, else give notification that there is no more items
        //go back button that -Item count and scrolls backwards, if itemCount is lesser then 0, another notification that there is no more items






    }

    //function to select the UI to make it bigger


}
