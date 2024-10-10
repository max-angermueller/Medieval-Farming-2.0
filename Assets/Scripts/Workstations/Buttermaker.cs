using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttermaker : MonoBehaviour
{
    Animator anim;
    
    bool dropButter = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f && dropButter== true)
        {
            dropButter = false;
            GetComponent<ItemDropManager>().dropItems(transform.position + new Vector3(-0.05f, 2.5f, 0f));
        }
        
    }

    public void makeButter()
    {
        Debug.Log("Jetzt wird Butter gestampft");
        anim.SetTrigger("MakeButter");
        dropButter = true;
        
    }

}
