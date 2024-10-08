using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Animalfood Object", menuName = "Scriptable Object System/AnimalFood")]

public class AnimalFood : ItemObject
{
    public void Awake()
    {
        itemType = ItemType.AnimalFood;
    }
}
