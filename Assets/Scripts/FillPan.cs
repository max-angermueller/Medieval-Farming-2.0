using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillPan : MonoBehaviour
{
    private Animator anim;
    public bool startAnim;
    public MyEnum panType = new MyEnum();
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        startAnim = false;
    }

    public enum MyEnum
    {
        Water,
        Food
    };
    void TryToFillFood()
    {
        StartFillAnimation();
        //Destroy(handObject.gameObject);
    }

    void TryToFillWater()
    {
        StartFillAnimation();
        //handObject.GetComponent<FillBucket>().UnfillBucket();
    }

    public void TryToFill()
    {
        
        if (panType.ToString() == "Water")
        {
            TryToFillWater();
        }

        if (panType.ToString() == "Food")
        {
            TryToFillFood();
        }
    }




    public void StartFillAnimation()
    {
        Debug.Log("Start Filling");
        anim.SetTrigger("Fill");
        startAnim = false;
    }

    public void StartUnfillAnimation()
    {
        anim.SetTrigger("Unfill");
        startAnim = false;
    }
}
