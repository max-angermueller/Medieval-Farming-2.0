using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlintAndSteelBurning : MonoBehaviour
{
    public Animation anim;
    private AudioSource audioSource;
    public AudioClip[] audioClips;

    // Start is called before the first frame update
    void Start()
    {
        this.audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void startAnim()
    {
        if (anim != null)
        {
            if (anim)
            {
                anim.Play();
            }
        }
    }

    private void playSound()
    {
        if (audioSource != null)
        {
            int random = Random.Range(0, audioClips.Length);
            audioSource.clip = audioClips[random];
            audioSource.Play();
        }

    }

    public void burnCampfire()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3.0f, 1 << 6))
        {
            GameObject hitGameObject = hit.transform.gameObject;
            if (hitGameObject.layer == 6)
            { // if InterActableObject was hit
                Campfire campfireScript = hitGameObject.GetComponent<Campfire>();
                if (campfireScript != null && !campfireScript.isBurning)
                {
                    //startAnim();
                    playSound();
                    campfireScript.startFire();
                }
            }
        }
    }

    public void burnSmokehouse()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3.0f, 1 << 6))
        {
            GameObject hitGameObject = hit.transform.gameObject;
            if (hitGameObject.layer == 6)
            { // if InterActableObject was hit
                Smokehouse smokehouseScript = hitGameObject.GetComponent<Smokehouse>();
                if (smokehouseScript != null && !smokehouseScript.isBurning)
                {
                    //startAnim();
                    playSound();
                    smokehouseScript.startFire();
                }
            }
        }
    }
}

