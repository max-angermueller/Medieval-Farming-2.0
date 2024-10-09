using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Container Object", menuName = "Scriptable Object System/Items/Container")]

public class ContainerObject : ItemObject
{
    private List<Item> storedItems;
    public void Awake()
    {
        ItemType = ItemType.Food;
    }

    public List<Item> StoredItems
    {
        get { return storedItems; }
        set { storedItems = value; }
    }

}
