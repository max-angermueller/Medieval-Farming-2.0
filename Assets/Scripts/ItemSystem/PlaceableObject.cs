using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Placeable Object", menuName = "Scriptable Object System/Items/Placeable")]
public class PlaceableObject : ItemObject{
    
    public void Awake(){
        itemType = ItemType.Placeable;
        isPlaceableOnGround = true;
    }
   
}
