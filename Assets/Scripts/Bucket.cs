using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    private Animator anim;
    public bool startAnim;

    private bool filledWithMilk = false;
    private bool filledWithWater = false;

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

    private Material water;
    private Material milk;

    // Start is called before the first frame update
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
        startAnim = false;
    }

    public void StartWaterAnimation()
    {
        anim.SetTrigger("Fill");
        transform.GetChild(1).GetComponent<MeshRenderer>().material = water;
        filledWithWater = true;
        filledWithMilk = false;
        startAnim = false;
    }

    public void UnfillBucket()
    {
        anim.SetTrigger("Unfill");
        filledWithWater = false;
        filledWithMilk = false;
        startAnim = false;
    }
}

