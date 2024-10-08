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
        anim.Play("Base Layer.Make", 0);
        dropButter = true;
        //GetComponent<ItemDropManager>().dropItems(transform.position + new Vector3(0.1f, 2.2f, 0f));
        //Instantiate(prefab, transform.position + new Vector3(0f, 3f, 0f), Quaternion.identity);
    }

    //rb.AddRelativeForce(new Vector3(0, 3f, 0f), ForceMode.Impulse);
}
