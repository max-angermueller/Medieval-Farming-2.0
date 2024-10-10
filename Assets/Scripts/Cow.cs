using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Cow : Animal
{

    private bool hasMilk;
    
    float time;
    private bool bucketIsSnapped;
    public GameObject bucketPlaceholder;
    private SnapToPlaceholder placeholderScr;

    public bool HasMilk
    {
        get { return hasMilk; }
        set { hasMilk = value; }
    }
    public bool BucketIsSnapped
    {
        get { return bucketIsSnapped; }
        set { bucketIsSnapped = value; }
    }

    private void Start()
    {
        walkingScr = GetComponent<Walking>();
        hasMilk = true;
        bucketIsSnapped = false;
        time = TimeManager.hourAquivalence * 8f;
        //placeholderScr = placeholder.GetComponent<SnapToPlaceholder>();
    }
    private void FixedUpdate()
    {
        if (bucketPlaceholder != null)
        {
            if (placeholderScr.isSnappedOn)
            {
                GetComponent<Walking>().IdleWhileInteracting();
                placeholderScr.itemObject.GetComponent<MeshCollider>().isTrigger = true;
                bucketIsSnapped = true;

            }
            else
            {
                bucketIsSnapped = false;

            }
        }
    }

    public void startMilking()
    {

        if (bucketPlaceholder != null)
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
