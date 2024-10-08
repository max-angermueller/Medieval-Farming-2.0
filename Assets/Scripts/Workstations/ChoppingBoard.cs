using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingBoard : MonoBehaviour
{
    bool dropMeat = false;
    Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f && dropMeat == true)
        {
            dropMeat = false;
            Destroy(transform.GetChild(0).GetComponent<SnapToPlaceholder>().itemObject);
            GetComponent<ItemDropManager>().dropItems(transform.position + new Vector3( 0f,2f,0f ));
        }

    }
    public void chopMeat()
    {
        if (transform.GetChild(0).GetComponent<SnapToPlaceholder>().isSnappedOn)
        {
            anim.Play("Base Layer.Make", 0);
            dropMeat = true;
        }
        
    }
}
