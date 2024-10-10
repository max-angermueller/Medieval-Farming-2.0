using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Object", menuName = "Scriptable Object System/NPC")]
public class NPCObject : ItemObject{

    public bool isTrader = false;

    public void Awake(){
        itemType = ItemType.NPC;
    }

    
}
