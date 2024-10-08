using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;

public class Smokehouse : MonoBehaviour
{
    [Header("--- Instances ---")]
    public List<GameObject> rawMeatPlaceholders;
    public List<GameObject> smokedMeatPlaceholders;
    private Dictionary<GameObject, List<GameObject>> placeholderPairs;
    public List<GameObject> woodlogPlaceholders;
    public GameObject meatToDropAfterSmoking;
    public AudioSource audioSource;
    public GameObject ReusableItems;
    public GameObject NewInstanciatedItems;
    public GameObject fireIconValueObject;
    [Header("--- Particles ---")]
    public List<GameObject> particles;
    public List<GameObject> placeholderParticles;


    public TextMeshProUGUI burnTimeText;

    private Coroutine updateUICoroutine;
    private Coroutine checkBurnTimeCoroutine;




    [Header("--- Settings ---")]
    public bool isBurning = false;
    [Range(1, 360)] [SerializeField] public int woodLogBurnTime = 3;
    public int woodLogAmount = 0;
    public int currentWoodLogs = 0;
    public int currentBurnTime = 0;
    public int remainingBurnTime = 0;
    private bool isBurnPaused = false;





    // Start is called before the first frame update
    void Start()
    {
        setUIFillAmount(0f);

        playParticles(false);
        woodLogAmount = woodlogPlaceholders.Count;

        if (ReusableItems == null)
            ReusableItems = GameObject.Find("/GameManager/ItemManager/ReusableItems");
        if (NewInstanciatedItems == null)
            NewInstanciatedItems = GameObject.Find("/GameManager/ItemManager/NewInstanciatedItems");

        placeholderPairs = createPlaceholderPairs(rawMeatPlaceholders, smokedMeatPlaceholders, placeholderParticles);
    }

    private Dictionary<GameObject, List<GameObject>> createPlaceholderPairs(List<GameObject> rawMeatPlaceholders, List<GameObject> smokedMeatPlaceholders, List<GameObject> placeholderParticles)
    {
        Dictionary<GameObject, List<GameObject>> placeHolderPairs = new Dictionary<GameObject, List<GameObject>>();
        for (int i = 0; i < rawMeatPlaceholders.Count; i++)
        {
            GameObject rawMeatPlaceholder = rawMeatPlaceholders[i];
            GameObject smokedMeatPlaceholder = smokedMeatPlaceholders[i];
            GameObject placeholderParticle = placeholderParticles[i];
            List<GameObject> data = new List<GameObject>();
            data.Add(smokedMeatPlaceholder);
            data.Add(placeholderParticle);
            if (rawMeatPlaceholder != null && smokedMeatPlaceholder != null && placeholderParticle != null)
            {
                placeHolderPairs.Add(rawMeatPlaceholder, data);
            }
        }
        return placeHolderPairs;
    }

    // Update is called once per frame
    void LateUpdate()
    {

    }

    private bool checkForWoodLogs()
    {
        this.currentWoodLogs = 0;
        foreach (GameObject woodLog in woodlogPlaceholders)
        {
            SnapToPlaceholder placeholderScript = woodLog.GetComponent<SnapToPlaceholder>();
            if (placeholderScript != null)
            {
                if (placeholderScript.isSnappedOn == true)
                {
                    this.currentWoodLogs++;
                }
            }
        }

        if (this.currentWoodLogs > 0)
        {
            return true;
        }

        return false;

    }

    public void addBurnTime()
    {
        currentBurnTime += woodLogBurnTime;
        StartCoroutine(updateUI(currentBurnTime));
    }


    public void startFire()
    {
        if (checkForWoodLogs())
        {
            isBurnPaused = false;
            if (currentBurnTime == 0)
            {
                currentBurnTime = currentWoodLogs * woodLogBurnTime;
            }

            //check the burnTime
            checkBurnTimeCoroutine = StartCoroutine(checkBurnTime());

            playSound(true);
            playParticles(true);
            isBurning = true;

            if (isBurning)
            {
                if (placeholderPairs != null)
                {
                    foreach (GameObject rawMeatPlaceholder in rawMeatPlaceholders)
                    {
                        StartCoroutine(smokeMeat(rawMeatPlaceholder));
                    }
                }
            }

        }
        else
        {
            isBurning = false;
        }
    }

    private IEnumerator checkBurnTime()
    {
        int remainBTime = 0;
        if (remainingBurnTime == 0 || remainingBurnTime == -1)
        {
            remainBTime = currentBurnTime - 1;
        }
        else
        {
            remainBTime = remainingBurnTime;
        }

        for (remainingBurnTime = remainBTime; remainingBurnTime >= 0; remainingBurnTime--)
        {
            updateUICoroutine = StartCoroutine(updateUI(remainingBurnTime));

            yield return new WaitForSeconds(1);
            if (remainingBurnTime % woodLogBurnTime == 0)
            {
                if (!isBurnPaused)
                {
                    removeWoodLogFromPlaceholder();
                }
                else
                {
                    currentBurnTime = remainingBurnTime;
                    yield break;
                }

            }
        }
        if (currentWoodLogs == 0)
        {
            yield return new WaitForSeconds(1);
            currentBurnTime = 0;
            stopFire();
        }


    }

    private IEnumerator updateUI(int remainingBurnTime)
    {
        //set text
        int minutes = remainingBurnTime / 60;
        int seconds = remainingBurnTime % 60;
        burnTimeText.text = $"{minutes:00}:{seconds:00}";

        //update filling amount bar
        int maxBurnTime = currentBurnTime;
        Debug.Log($"currentBurnTime: {currentBurnTime}");
        Debug.Log($"maxBurnTime: {maxBurnTime}");
        float fillAmount = (float)remainingBurnTime / (float)maxBurnTime;
        setUIFillAmount(fillAmount);

        yield return null;
    }

    private void setUIFillAmount(float fillAmount)
    {
        Renderer renderer = fireIconValueObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            Debug.Log($"fillAmount: {fillAmount}");
            renderer.sharedMaterial.SetFloat("_Health", fillAmount);
        }
    }

    private void removeWoodLogFromPlaceholder()
    {
        List<GameObject> woodLogPlaceholdersWithLogs = new List<GameObject>();
        foreach (GameObject woodlogPlaceholder in woodlogPlaceholders)
        {
            SnapToPlaceholder placeholderScript = woodlogPlaceholder.GetComponent<SnapToPlaceholder>();
            if (placeholderScript != null)
            {
                if (placeholderScript.isSnappedOn == true)
                {
                    woodLogPlaceholdersWithLogs.Add(woodlogPlaceholder);
                }
            }
        }

        int woodLogId = UnityEngine.Random.Range(0, woodLogPlaceholdersWithLogs.Count);
        removeWoodLog(woodLogPlaceholdersWithLogs[woodLogId]);
        currentWoodLogs--;
    }

    void removeWoodLog(GameObject woodLogPlaceholder)
    {
        SnapToPlaceholder placeholderScript = woodLogPlaceholder.GetComponent<SnapToPlaceholder>();
        if (placeholderScript != null)
        {
            GameObject woodLog = placeholderScript.itemObject;
            placeholderScript.snapOff(woodLog);
            woodLog.SetActive(false);
            woodLog.transform.parent = ReusableItems.transform;
        }

    }

    public void stopFire()
    {
        if (isBurning)
        {
            playSound(false);
            playParticles(false);
            //StopCoroutine(checkBurnTime());
            //StopCoroutine(updateUI(0));
            if (checkBurnTimeCoroutine != null) { StopCoroutine(checkBurnTimeCoroutine); }
            if (updateUICoroutine != null) { StopCoroutine(updateUICoroutine); }

            if (currentWoodLogs > 0)
            {
                isBurnPaused = true;
            }
            isBurning = false;
        }
    }

    private IEnumerator smokeMeat(GameObject rawMeatPlaceholder)
    {
        //get smokedMeatPlaceHolder + Script
        GameObject smokedMeatPlaceholder = placeholderPairs[rawMeatPlaceholder][0];
        if (smokedMeatPlaceholder != null)
        {
            SnapToPlaceholder smokedMeatPlaceholderScript = smokedMeatPlaceholder.GetComponent<SnapToPlaceholder>();

            //get rawMeatPlaceholderScript
            SnapToPlaceholder rawMeatPlaceholderScript = rawMeatPlaceholder.GetComponent<SnapToPlaceholder>();
            if (rawMeatPlaceholderScript != null)
            {
                if (rawMeatPlaceholderScript.isSnappedOn == true)
                {
                    GameObject rawMeat = rawMeatPlaceholderScript.itemObject;
                    GameObject smokedMeat = getItem(meatToDropAfterSmoking);
                    if (smokedMeat != null)
                    {
                        //get placeholderParticle + play
                        GameObject placeholderParticle = placeholderPairs[rawMeatPlaceholder][1];
                        if (placeholderParticle != null)
                        {
                            ParticleSystem particleSystem = placeholderParticle.GetComponent<ParticleSystem>();
                            if (particleSystem != null)
                            {
                                particleSystem.Play();
                            }
                        }
                        yield return new WaitForSeconds(0.25f);

                        //convert raw meat to smoked meat
                        rawMeatPlaceholderScript.itemObject = null;
                        rawMeatPlaceholderScript.isSnappedOn = false;
                        smokedMeat.transform.position = rawMeatPlaceholderScript.transform.position;
                        smokedMeat.transform.rotation = rawMeatPlaceholderScript.transform.rotation;

                        //snap smokedMeat to placeHolder
                        smokedMeatPlaceholderScript.snapOn(smokedMeat);

                        deactivateRawMeatPlaceholder(rawMeatPlaceholder);
                    }

                    //recycle raw meat GameObject
                    rawMeat.transform.parent = ReusableItems.transform;
                    rawMeat.SetActive(false);
                }
            }
        }
    }

    private void deactivateRawMeatPlaceholder(GameObject placeholder)
    {
        placeholder.SetActive(false);
    }

    public void activateRawMeatPlaceholder(GameObject placeholder)
    {
        placeholder.SetActive(true);
    }

    private void playSound(bool play)
    {
        if (audioSource != null)
        {
            if (play && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
            else if (!play)
            {
                audioSource.Stop();
            }

        }

    }

    private void playParticles(bool play)
    {
        foreach (GameObject particle in particles)
        {
            ParticleSystem ps = particle.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                if (play && !ps.isPlaying)
                {
                    ps.Play();
                }
                else if (!play && !ps.isStopped)
                {
                    ps.Stop();
                }

            }
        }
    }

    private GameObject getItem(GameObject itemToDrop)
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
                            item = reuseItem(reusableItem.gameObject, transform.position);
                            break;
                        }
                    }
                }
            }
        }
        if (item == null)
        {
            item = Instantiate(itemToDrop, transform.position, Quaternion.identity);
        }

        if (item != null)
        {
            item.transform.parent = NewInstanciatedItems.transform;
        }

        return item;
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
