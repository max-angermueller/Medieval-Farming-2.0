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
    AnimalFood,
    Money,
    Container,
    FluidContainer,
    NPC,
    Interactable,
    Ressource,
    Default
}

public abstract class ItemObject : ScriptableObject //ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public GameObject itemModel;
    public ItemType itemType;
    public Vector3 buyPrice;  // x= Kupfer, y= Silber, z = Gold
    public Vector3 sellPrice;
    public int itemAmount = 1;
    public bool isPlaceableOnGround =true;
    public bool playSounds = false;
    public AudioClip[] dropSoundEffects;

    
}



