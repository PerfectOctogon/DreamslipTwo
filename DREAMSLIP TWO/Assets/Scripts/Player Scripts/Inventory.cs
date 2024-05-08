using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static ArrayList items = new ArrayList();
    public static ArrayList specialItems = new ArrayList();
    

    public static void AddItemToInventory(Item item)
    {
        if(item.isSpecialItem){
            AddSpecialItemToInventory(item);
            return;
        }
        items.Add(item.itemName);
    }

    public static void AddSpecialItemToInventory(Item item)
    {
        specialItems.Add(item.itemName);
    }
}
