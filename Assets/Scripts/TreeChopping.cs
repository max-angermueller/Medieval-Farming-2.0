using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TreeChopping : MonoBehaviour
{

    public GameObject TreeFullModel;
    public GameObject TreeLogModel;
    public GameObject TreeStumpModel;
    public VisualEffect particleEffect_chopping;
    public VisualEffect particleEffect_leaves;

    public int maxHitsToFell = 3;
    private int currentHits = 0;
    public float secondsUntilNextHit = 1f;
    public bool canBeChopped = true;
    public string treeType;
    private ItemDropManager itemDropManager;

    private AudioSource audioSource;
    public AudioClip[] audioClips;


    IEnumerator startChopping()
    {
        canBeChopped = false;
        if (currentHits <= maxHitsToFell)
        {
            currentHits++;
            playParticleEffect();
        }

        if (currentHits == maxHitsToFell)
        {
            changeModels();
        }

        yield return new WaitForSeconds(secondsUntilNextHit);
        canBeChopped = true;

    }

    private void initializeTree()
    {
        TreeLogModel.SetActive(false);
        TreeStumpModel.SetActive(false);
    }

    public void chopTree()
    {
        if (canBeChopped)
        {
            StartCoroutine(startChopping());
        }



    }

    private void playParticleEffect()
    {
        if (particleEffect_chopping != null)
        {
            particleEffect_chopping.Play();
            if (currentHits == maxHitsToFell)
            {
                if (particleEffect_leaves != null)
                {
                    particleEffect_leaves.Play();
                }

            }
        }

    }

    private void changeModels()
    {

        switch (treeType)
        {
            case "Tree":
                {
                    playSound();
                    TreeFullModel.SetActive(false);
                    TreeLogModel.SetActive(true);
                    TreeStumpModel.SetActive(true);

                    MeshCollider collider = transform.GetComponent<MeshCollider>();
                    if (collider != null)
                    {
                        collider.enabled = false;
                    }

                    break;
                }
            case "Log":
                {
                    makeModelInvisible(TreeLogModel);
                    break;
                }
            case "Stump":
                {
                    makeModelInvisible(TreeStumpModel);
                    break;
                }

            default: break;
        }


    }

    void makeModelInvisible(GameObject model)
    {
        //make the tree log model invisible
        MeshRenderer meshRenderer = model.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }
        //disable the tree log model collider 
        MeshCollider meshCollider = model.GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.enabled = false;
        }

        Rigidbody rigidbody = model.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
        }

        if (itemDropManager != null)
        {
            itemDropManager.dropItems();
        }

        resetTree();
    }


    void resetTree()
    {
        currentHits = 0;
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

    // Start is called before the first frame update
    void Start()
    {
        switch (treeType)
        {
            case "Tree": { initializeTree(); break; }
            case "Log":
            case "Stump": { itemDropManager = transform.GetComponent<ItemDropManager>(); break; }
        }

        this.audioSource = GetComponent<AudioSource>();


    }

    // Update is called once per frame
    void Update()
    {

    }
}
