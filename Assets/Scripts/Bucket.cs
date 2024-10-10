using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    private Animator anim;
    
    [SerializeField] private FluidType fluidType;
    [SerializeField] private bool filledWithMilk = false;
    [SerializeField] private bool filledWithWater = false;

    private bool isfilled = false;
    public bool FilledWithMilk { 
        get{ return filledWithMilk;}
        set{
            filledWithMilk = value;
        }
    }
    public bool FilledWithWater
    {
        get { return filledWithWater; }
        set
        {
            filledWithWater = value;
        }
    }
    public bool IsFilled
    {
        get { return isfilled; }
        set
        {
            isfilled = value;
        }
    }
    public FluidType FluidType
    {
        get { return fluidType; }
        set
        {
            fluidType = value;
        }
    }


    private Material water;
    private Material milk;

    void Start()
    {
        water = new Material(Shader.Find("Standard"));
        water.color = Color.cyan;

        milk = new Material(Shader.Find("Standard"));
        milk.color = Color.white;
        
        
        anim = GetComponent<Animator>();
       
        if (filledWithWater== true)
        {
            StartWaterAnimation();
        }
        else if (filledWithMilk == true)
        {
            StartMilkAnimation();          
        }
        else
        {
            isfilled= false;
            fluidType = FluidType.None;
        }
    }

    

    public void StartMilkAnimation()
    {
        anim.SetTrigger("Fill");
        transform.GetChild(1).GetComponent<MeshRenderer>().material = milk;
        filledWithMilk = true;
        filledWithWater = false;
        fluidType = FluidType.MilkContainer;
        isfilled = true;
       
    }

    public void StartWaterAnimation()
    {
        anim.SetTrigger("Fill");
        transform.GetChild(1).GetComponent<MeshRenderer>().material = water;
        filledWithWater = true;
        filledWithMilk = false;
        fluidType = FluidType.WaterContainer;
        isfilled = true;
       
    }

    public void UnfillBucket()
    {
        anim.SetTrigger("Unfill");
        filledWithWater = false;
        filledWithMilk = false;
        fluidType = FluidType.None;
        isfilled = false;
      
    }
}

