using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.VFX;

public class ItemDropManager : MonoBehaviour
{
    public GameObject itemToDrop;
    public int itemAmountToDrop = 3;
    //private ChangeGrowthStageModel changeGrowthStageModelScript;
    private Animator animator;
    public float spawnDelay = 1.0f;
    public bool destroyParent = true;

    private VisualEffect particleEffect;

    public GameObject ReusableItems;
    public GameObject NewInstanciatedItems;

    // Start is called before the first frame update
    void Start()
    {
        /*this.changeGrowthStageModelScript = this.gameObject.GetComponent<ChangeGrowthStageModel>();
        if (this.changeGrowthStageModelScript != null)
        {
            this.particleEffect = this.changeGrowthStageModelScript.particleEffect;
            this.animator = this.changeGrowthStageModelScript.GetComponentInChildren<Animator>();
        }*/

        if (ReusableItems == null)
            ReusableItems = GameObject.Find("/GameManager/ItemManager/ReusableItems");
        if (NewInstanciatedItems == null)
            NewInstanciatedItems = GameObject.Find("/GameManager/ItemManager/NewInstanciatedItems");


    }

    // Update is called once per frame
    void Update()
    {
        // if(isHarvestable()){
        //     dropItems();
        // }
    }

    public bool isHarvestable()
    {
        /*if (changeGrowthStageModelScript != null)
        {
            return changeGrowthStageModelScript.currentGrowthStage == changeGrowthStageModelScript.models.Length - 1;
        }*/
        return false;
    }

    public void dropPlantItems()
    {
        if (isHarvestable())
        {
            Vector3 position = transform.position;
            playParticleEffect();
            StartCoroutine(spawnItems(position, spawnDelay, itemAmountToDrop, itemToDrop));
        }
    }

    public void dropItems(int itemAmountToDrop, GameObject itemToDrop)
    {
        Vector3 position = transform.position;
        Renderer renderer = transform.GetComponent<Renderer>();
        if (renderer != null)
        {
            position = renderer.bounds.center;
        }

        playParticleEffect();
        StartCoroutine(spawnItems(position, spawnDelay, itemAmountToDrop, itemToDrop));
    }

    public void dropItems()
    {
        Vector3 position = transform.position;
        Renderer renderer = transform.GetComponent<Renderer>();
        if (renderer != null)
        {
            position = renderer.bounds.center;
        }

        playParticleEffect();
        StartCoroutine(spawnItems(position, spawnDelay, itemAmountToDrop, itemToDrop));
    }

    public void dropItems(Vector3 position)
    {
        playParticleEffect();
        StartCoroutine(spawnItems(position, spawnDelay, itemAmountToDrop, itemToDrop));
    }

    IEnumerator spawnItems(Vector3 position, float delay, int itemAmountToDrop, GameObject itemToDrop)
    {
        Collider collider = transform.gameObject.GetComponentInChildren<Collider>();
        /*if (collider != null)
        {
            collider.enabled = false;
        }*/

        Debug.Log("itemAmountToDrop: " + itemAmountToDrop);

        for (int i = 0; i < itemAmountToDrop; i++)
        {

            GameObject item = null;

            //check if there are selled items that can be reused before instantiating new items
            if (ReusableItems != null)
            {
                foreach (Transform reusableItem in ReusableItems.transform)
                {
                    Item selledItemScript = reusableItem.gameObject.GetComponent<Item>();
                    if (selledItemScript != null)
                    {
                        Item itemToDropItemScript = itemToDrop.GetComponent<Item>();
                        if (itemToDropItemScript != null)
                        {
                            if (selledItemScript.item.itemName == itemToDropItemScript.item.itemName)
                            {
                                item = reuseItem(reusableItem.gameObject, position);
                                break;
                            }
                        }
                    }
                }
            }
            if (item == null)
            {
                item = Instantiate(itemToDrop, position, Quaternion.identity);
            }


            if (item != null)
            {
                item.transform.parent = NewInstanciatedItems.transform;
                Rigidbody rb = item.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(new Vector3(0, 200f, 0));
                }

            }
            Debug.Log("I DROP ITEMS");
            yield return new WaitForSeconds(delay);
        }

        if (destroyParent)
        {
            Destroy(gameObject);
        }

    }

    private void playParticleEffect()
    {
        if (particleEffect != null)
        {
            animator.Play("FadeOut");
            particleEffect.Play();
        }
    }




    private GameObject reuseItem(GameObject item, Vector3 position)
    {
        item.transform.position = position;
        item.transform.rotation = Quaternion.identity;
        item.SetActive(true);
        Rigidbody rigidbody = item.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
        }
        return item;

    }

}
