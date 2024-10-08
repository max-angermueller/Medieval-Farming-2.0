using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkToButter : MonoBehaviour
{
    public void addMilk()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3.0f/*, layersToIgnore*/))
        {
            GameObject hitGameObject = hit.transform.gameObject;
            if (hitGameObject.layer == 6)
            { // if InterActableObject was hit
                Buttermaker buttermakerScr = hitGameObject.transform.parent.GetComponent<Buttermaker>();
                if (buttermakerScr != null)
                {
                    if (GetComponent<Bucket>().FilledWithMilk)
                    {
                        buttermakerScr.makeButter();
                        GetComponent<Bucket>().UnfillBucket();
                    }
                    
                    
                }
            }
        }
    }
}
