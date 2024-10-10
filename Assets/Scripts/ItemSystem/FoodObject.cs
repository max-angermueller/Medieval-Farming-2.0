using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food Object", menuName = "Scriptable Object System/Items/Food")]
public class FoodObject : ItemObject
{
    public int increaseStamina;

    public Material dissolveMaterial;

    public AudioClip[] eatSoundEffects;

    public void Awake()
    {
        itemType = ItemType.Food;
    }

    
}
