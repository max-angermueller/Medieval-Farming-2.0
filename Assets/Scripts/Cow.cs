using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//importe mich!


public class Cow : MonoBehaviour
{
   
    public bool hasMilk;
    public GameObject placeholder;
    float time;
    public bool snapon;
    SnapToPlaceholder placeholderScr;

    private void FixedUpdate()
    {
        if (placeholder != null)
        {
            if (placeholderScr.isSnappedOn)
            {
                GetComponent<Walking>().IdleWhileMilking();
                placeholderScr.itemObject.GetComponent<MeshCollider>().isTrigger = true;
                snapon = true;

            }
            else
            {
                snapon = false;
                
            }
        }
    }

    private void Start()
    {
        hasMilk = true;
        snapon = false;
        time = TimeManager.hourAquivalence * 8f;
        //placeholderScr = placeholder.GetComponent<SnapToPlaceholder>();


    }


    public void startMilking()
    {

        if (placeholder != null)
        {
            if (placeholderScr.isSnappedOn == true)
            {
                if (!hasMilk) Debug.Log("Keine Milch");
                else
                {
                    if (placeholderScr.itemObject.GetComponent<Bucket>().FilledWithMilk != true &&
                        placeholderScr.itemObject.GetComponent<Bucket>().FilledWithWater != true)
                    {
                        placeholderScr.itemObject.GetComponent<Bucket>().StartMilkAnimation();
                        hasMilk = false;
                       // TimedAction.Create(restoreMilk, time, false, "Milk");
                    }

                }
            }
        }
        
    }

    void restoreMilk()
    {
        hasMilk = true;
    }



}
