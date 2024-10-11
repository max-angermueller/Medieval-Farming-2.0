using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingBoard : MonoBehaviour
{
    bool dropMeat = false;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f && dropMeat == true)
        {
            dropMeat = false;
            Destroy(GetComponentInChildren<SnapToPlaceholder>().itemObject);
            GetComponent<ItemDropManager>().dropItems(transform.position + new Vector3( 0f,2f,0f ));
        }

    }
    public void chopMeat()
    {
        if (GetComponentInChildren<SnapToPlaceholder>() != null)
        {
            if (GetComponentInChildren<SnapToPlaceholder>().isSnappedOn)
            {
                anim.SetTrigger("Use");
                dropMeat = true;
                Debug.Log(anim.GetCurrentAnimatorStateInfo(0));
            }
        }
    }
}
