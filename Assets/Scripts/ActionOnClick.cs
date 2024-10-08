using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionOnClick : MonoBehaviour
{
    [SerializeField]    private Sprite[] sprites;
    [SerializeField]    private UnityEvent script;
    [SerializeField]    private Item requItem;
    void Start()
    {
        
    }

    public void ActivateEvents()
    {
        Debug.Log("Führe Events aus..");
        script?.Invoke();
    }

    public Sprite getSprite(int position)
    {
        if (position > sprites.Length - 1 || position < 0)
        {
            return null;
        }
        else return sprites[position];
      
    }

    public Item getRequirement()
    {
        return requItem;
    }

    public Sprite[] getSprites()
    {
        return sprites;
    }


}
