using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Scriptable Object System/Items/Equipment")]
public class EquipmentObject : ItemObject{
    public int wear;
    public void Awake(){
        ItemType = ItemType.Equipment;   
    }
}