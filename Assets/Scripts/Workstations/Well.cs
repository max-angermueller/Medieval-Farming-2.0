using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Well : MonoBehaviour
{
    [SerializeField]
    private GameObject rightHand;
    
    private void Awake()
    {
        if (rightHand == null)
        {
            rightHand = GameObject.Find("Player").transform.GetChild(0).GetChild(0).gameObject; //Right Hand, child of camera, that is child of player
        }
    }

    public void FillBucket()
    {
        if (rightHand != null)
        {
            if (rightHand.GetComponentInChildren<Bucket>() != null)
            {
                Bucket bucketScr = rightHand.GetComponentInChildren<Bucket>();
                if (bucketScr.IsFilled == false)
                {
                    bucketScr.StartWaterAnimation();
                    
                }
                else Debug.Log("bucket full!");
            }
        }
    }
}
