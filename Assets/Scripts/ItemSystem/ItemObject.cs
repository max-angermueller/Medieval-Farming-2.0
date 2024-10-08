using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Food,
    Equipment,
    Plant,
    Placeable,
    Animal,
    Money,
    Storage,
    NPC,
    Workstation,
    Bed,
    Ressource,
    Default
}

public abstract class ItemObject : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public GameObject itemModel;
    public ItemType itemType = ItemType.Default;
    public Vector3 buyPrice;
    public Vector3 sellPrice;
    public int itemAmount;
    public bool isPlaceableOnGround;
    public bool playSounds = true;
    public AudioClip[] dropSoundEffects;
}


