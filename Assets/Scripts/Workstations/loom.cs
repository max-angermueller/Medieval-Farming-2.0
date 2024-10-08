using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void collectThread()
    {
        GetComponent<ItemDropManager>().dropItems(transform.position + new Vector3(0f, 2f, 0f));
    }
}