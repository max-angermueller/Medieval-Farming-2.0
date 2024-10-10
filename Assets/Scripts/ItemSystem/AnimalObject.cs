using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Animal Object", menuName = "Scriptable Object System/Animal")]
public class AnimalObject : ItemObject{
    public void Awake(){
        itemType = ItemType.Animal;
    }
    
}
