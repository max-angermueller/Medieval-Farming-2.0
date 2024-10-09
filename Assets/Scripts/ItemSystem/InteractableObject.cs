using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Placeable Object", menuName = "Scriptable Object System/Items/Interactable")]
public class InteractableObject : ItemObject
{
    public void Awake()
    {
        itemType = ItemType.Interactable;
    }
}
