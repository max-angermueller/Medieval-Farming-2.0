using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemObject item;

    public int ItemAmount = 1;

    private AudioSource audioSource;

    private AudioClip[] audioClips;
    private AudioClip[] dropSounds;
    private AudioClip[] eatSounds;

    [SerializeField] bool canBeEaten = false;

    private MeshRenderer meshRenderer;
    private Material baseMaterial;

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
            if (item.ItemType == ItemType.Food)
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

    public void UnfillContainer()
    {
        if (item.ItemType == ItemType.Container)
        {
            if (gameObject.GetComponent<Bucket>() != null)
            {
                gameObject.GetComponent<Bucket>().UnfillBucket();
            }
            
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
