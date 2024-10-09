using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Placeable Object", menuName = "Scriptable Object System/Items/Ressource")]

public class RessourceObject : ItemObject
{
    public void Awake()
    {
        itemType = ItemType.Ressource;
        isPlaceableOnGround = true;
    }
}
