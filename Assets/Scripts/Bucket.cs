using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    private Animator anim;
    public bool startAnim;

    private bool filledWithMilk = false;
    private bool filledWithWater = false;

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

    private Material water;
    private Material milk;

    void Start()
    {
        water = new Material(Shader.Find("Standard"));
        water.color = Color.cyan;

        milk = new Material(Shader.Find("Standard"));
        milk.color = Color.white;

        anim = GetComponent<Animator>();
        startAnim = false;
        if (filledWithWater== true)
        {
            StartWaterAnimation();
        }
        else if (filledWithMilk == true)
        {
            StartMilkAnimation();
        }
    }

  

    public void StartMilkAnimation()
    {
        anim.SetTrigger("Fill");
        transform.GetChild(1).GetComponent<MeshRenderer>().material = milk;
        filledWithMilk = true;
        filledWithWater = false;
        isfilled = true;
        startAnim = false;
    }

    public void StartWaterAnimation()
    {
        anim.SetTrigger("Fill");
        transform.GetChild(1).GetComponent<MeshRenderer>().material = water;
        filledWithWater = true;
        filledWithMilk = false;
        isfilled = true;
        startAnim = false;
    }

    public void UnfillBucket()
    {
        anim.SetTrigger("Unfill");
        filledWithWater = false;
        filledWithMilk = false;
        isfilled = false;
        startAnim = false;
    }
}

