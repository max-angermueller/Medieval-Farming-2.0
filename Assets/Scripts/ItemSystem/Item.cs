using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemObject item;

    public int ItemAmount = 1;

    public GameObject parent;

    private AudioSource audioSource;

    private AudioClip[] audioClips;
    private AudioClip[] dropSounds;
    private AudioClip[] eatSounds;

    public bool canBeEaten = false;


    //GameObject Components
    private MeshRenderer meshRenderer;
    private Material baseMaterial;
    //Scr_Interactable interactableScript;

    private void Awake()
    {
        this.ItemAmount = item.itemAmount;

        if (item.playSounds)
        {
            //Add audioSource
            audioSource = transform.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSetup();
            }

            //get Drop Sounds
            if (audioSource != null)
            {
                dropSounds = item.dropSoundEffects;
            }

            //get Eat Sounds
            FoodObject foodObject = this.item as FoodObject;
            if (item.itemType == ItemType.Food)
            {
                if (audioSource != null)
                {
                    if (foodObject != null)
                    {
                        eatSounds = foodObject.eatSoundEffects;
                    }

                }
            }

            if (foodObject != null)
            {
                canBeEaten = true;
            }
        }
    }

    private void Start()
    {
        //save baseMaterial;
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            baseMaterial = meshRenderer.sharedMaterial;
        }
    }

    public void Consume()
    {
        FoodObject foodObject = this.item as FoodObject;
        if (foodObject != null)
        {
            if (foodObject.itemType == ItemType.Food)
            {
                if (canBeEaten != false)
                {
                    if (eatSounds != null) { playSound(eatSounds); }
                    canBeEaten = false;
                   // foodObject.Consume();
                    StartCoroutine(deactivateGameObject()); //set to reusable items
                    
                }
            }
        }
    }

    private IEnumerator dissolveObject()
    {
        FoodObject foodObject = this.item as FoodObject;
        if (foodObject != null)
        {
            Material dissolveMaterial = foodObject.dissolveMaterial;
            if (dissolveMaterial != null)
            {
                if (meshRenderer != null)
                {
                    meshRenderer.sharedMaterial = dissolveMaterial;
                    var material = GetComponent<Renderer>().material;

                    float valueToChange = 1;
                    float time = 0;
                    float startValue = valueToChange;
                    float endValue = -1;
                    float duration = 1f; //1 second
                    while (time < duration)
                    {
                        valueToChange = Mathf.Lerp(startValue, endValue, time / duration);
                        time += Time.deltaTime;
                        material.SetFloat("_CutoffHeight", valueToChange);
                        yield return null;
                    }
                    valueToChange = endValue;
                    dissolveMaterial.SetFloat("_CutoffHeight", valueToChange);
                }
            }


        }
    }

    private IEnumerator deactivateGameObject()
    {
        StartCoroutine(dissolveObject());
       /* Scr_Interactable interactableScript = gameObject.GetComponent<Scr_Interactable>();
        if (interactableScript != null)
        {
            interactableScript.showOutlines = false;
            interactableScript.canBeHeld = false;
        }*/
        yield return new WaitForSeconds(0.5f);
        resetItem();
        gameObject.SetActive(false);

    }

    private void resetItem()
    {
       /* Scr_Interactable interactableScript = gameObject.GetComponent<Scr_Interactable>();
        if (interactableScript != null)
        {
            interactableScript.showOutlines = true;
            interactableScript.canBeHeld = true;
        }*/

        if (baseMaterial != null)
        {
            meshRenderer.sharedMaterial = baseMaterial;
        }

        FoodObject foodObject = this.item as FoodObject;
        if (foodObject != null)
        {
            canBeEaten = true;
        }

    }

    //play sound------------------------------------------------------------------------------------------
    private void OnCollisionEnter(Collision other)
    {
        if (item.playSounds)
        {
            if (other.relativeVelocity.magnitude > 4f)
            {
                playSound(dropSounds);
            }
        }
    }

    public void playSound(AudioClip[] audioClips)
    {
        if (item.playSounds)
        {
            if (audioSource != null)
            {
                if (audioClips != null && audioClips.Length > 0)
                {
                    int random = Random.Range(0, audioClips.Length);
                    audioSource.clip = audioClips[random];
                    audioSource.PlayOneShot(audioSource.clip); // Play();
                }
            }
        }
    }

    private void audioSetup()
    {
        if (item.playSounds)
        {
            transform.gameObject.AddComponent<AudioSource>();
            audioSource = transform.GetComponent<AudioSource>();
            audioSource.spatialBlend = 1.0f; //3d sound
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.minDistance = 3f;
            audioSource.maxDistance = 10f;
            audioSource.volume = 0.5f;
        }
    }
}
