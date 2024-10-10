using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum ActionType
{
    ConsumeObject,
    Interact,
    FillContainer
}
public enum FluidType
{
    None,
    MilkContainer,
    WaterContainer
}
public class ActionOnClick : MonoBehaviour
{


    [SerializeField]    private Sprite[] sprites;
    [SerializeField]    private UnityEvent script;
    [SerializeField]    private ActionType actionType = ActionType.ConsumeObject;
    [SerializeField]    private Item requItem;
    [SerializeField]    private FluidType specialRequirements = FluidType.None;


    public void ActivateEvents()
    {
        //Debug.Log("Führe Events aus..");
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
   

    public ActionType GetActionType() 
    { 
        return actionType; 
    
    }
    public Item getRequirement()
    {
        return requItem;
    }

    public Sprite[] getSprites()
    {
        return sprites;
    }
    public FluidType getSpecialRequirements()
    {
        return specialRequirements;
    }


}
