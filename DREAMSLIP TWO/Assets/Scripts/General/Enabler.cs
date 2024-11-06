using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

//This script enables gameobjects when certain criteria are met by the player


public class Enabler : MonoBehaviour
{
    //Gameobjects we want to enable after criteria is met
    public GameObject[] objectsToEnable;

    //Item we want to search for in the inventory
    public string itemName;

    //Number of items we need of that name to enable the object
    public int count;

    //Checks for criteria type
    //1: Count for a certain object in the player's inventory
    //2: Enable without checking inventory
	//3: Count for a certain object in the player's inventory and disable items
	//4: Disable without checking inventory
    public int operation;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Equals("Player")){CheckCriteria();}
    }


    public void CheckCriteria()
    {
        Debug.Log("Checking inventory");
        switch (operation)
        {
            case 1:
                if(CheckInventoryCount(itemName, count)){
					StartCoroutine(HintBox.ShowHint("You found all the required items."));
            		EnableItems();
				}
                break;
            case 2:
				EnableItems();
                break;
			case 3:
                
				break;
        }
    }

    private bool CheckInventoryCount(string name, int neededCount)
    {
        int count = 0;
        foreach (string i in Inventory.specialItems)
        {
            if (i.Equals(name))
            {
                count++;
            }
        }
        StartCoroutine(HintBox.ShowHint(String.Format("You need to find {0} more of [{1}].", neededCount - count, name)));
		return (count >= neededCount);
    }

	private void EnableItems(){
		foreach (GameObject g in objectsToEnable)
            {
                g.SetActive(true);
            }
        Destroy(gameObject);
	}

    private void DisableItems()
    {
        foreach (GameObject g in objectsToEnable)
        {
            g.SetActive(false);
        }
        Destroy(gameObject);
    }

}
