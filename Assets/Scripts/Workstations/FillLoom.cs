using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillLoom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void fillLoom()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3.0f/*, layersToIgnore*/))
        {
            GameObject hitGameObject = hit.transform.gameObject;
            if (hitGameObject.layer == 6)
            { // if InterActableObject was hit
                loom loom = hitGameObject.transform.GetComponent<loom>();
                if (loom != null)
                {
                    loom.collectThread();
                    Destroy(gameObject);
                }
            }
        }
    }
}
