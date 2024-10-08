using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Placeable Object", menuName = "Scriptable Object System/Placeable")]
public class PlaceableObject : ItemObject{
    public bool isPlaceable = true;
    public void Awake(){
        itemType = ItemType.Placeable;
    }

    public bool IsPlacable { get; set; }

    
}
