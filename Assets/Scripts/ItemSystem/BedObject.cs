using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bed Object", menuName = "Scriptable Object System/Misc/Bed")]
public class BedObject : ItemObject
{

    public float regainStaminaPercentage;

    public void Awake()
    {
        itemType = ItemType.Bed;
    }


}
