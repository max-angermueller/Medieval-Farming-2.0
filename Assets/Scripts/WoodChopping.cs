using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WoodChopping : MonoBehaviour
{

    public List<GameObject> placeHolders;
    private List<SnapToPlaceholder> placeHolderScripts = new List<SnapToPlaceholder>();
    private SnapToPlaceholder currentPlaceholderScript;
    public GameObject ReusableItems;
    public GameObject NewInstanciatedItems;

    //woodLogHalfPresets -----------------------------------------------------------------
    public GameObject[] woodLogHalfPresets;
    public List<Vector3> woodLogHalfPresetsStartPositions = new List<Vector3>();
    public List<Quaternion> woodLogHalfPresetsStartRotations = new List<Quaternion>();
    public GameObject woodLogHalfContainer;

    //woodLogQuarterPresets -----------------------------------------------------------------
    public GameObject[] woodLogQuarterPresets;
    public List<Vector3> woodLogQuarterPresetsStartPositions = new List<Vector3>();
    public List<Quaternion> woodLogQuarterPresetsStartRotations = new List<Quaternion>();
    public GameObject woodLogQuarterContainer;

    //presets -----------------------------------------------------------------
    List<Vector3> currentPresetStartPositions;
    List<Quaternion> currentPresetStartRotations;


    public VisualEffect chopParticles;
    public string woodType = null;

    public Transform mainCamera;

    public AudioSource audioSource;
    public AudioClip[] audioClips;

    private void Start()
    {

        foreach (GameObject placeHolder in placeHolders)
        {
            SnapToPlaceholder snapToPlaceholderScript = placeHolder.GetComponent<SnapToPlaceholder>();
            if (snapToPlaceholderScript != null)
            {
                placeHolderScripts.Add(snapToPlaceholderScript);
            }
        }


        if (ReusableItems == null)
            ReusableItems = GameObject.Find("/GameManager/ItemManager/ReusableItems");
        if (NewInstanciatedItems == null)
            NewInstanciatedItems = GameObject.Find("/GameManager/ItemManager/NewInstanciatedItems");

        if (mainCamera == null)
            mainCamera = Camera.main.transform;

        setupPresets();
    }

    public bool isChoppable()
    {
        if (placeHolderScripts != null)
        {
            foreach (SnapToPlaceholder placeHolderScript in placeHolderScripts)
            {
                if (placeHolderScript.isSnappedOn)
                {
                    currentPlaceholderScript = placeHolderScript;
                    return true;
                }
            }
            return false;
        }
        return false;
    }

    public string getWoodType(SnapToPlaceholder placeHolderScript)
    {
        if (placeHolderScript != null)
        {
            Item itemScript = placeHolderScript.itemObject.GetComponent<Item>();
            if (itemScript != null)
            {
                string itemName = itemScript.item.itemName;
                if (itemName != null)
                {
                    if (itemName.Contains("Half"))
                    {
                        return "Half";
                    }
                    else if (itemName.Contains("Log"))
                    {
                        return "Log";
                    }
                }
            }
        }
        return null;
    }

    private void rotateTowardsPlayer(GameObject container)
    {
        Vector3 lookPosition = mainCamera.position - container.transform.position;
        lookPosition.y = 0;
        var rotation = Quaternion.LookRotation(lookPosition);
        container.transform.rotation = rotation;
    }

    void setupPresets()
    {
        //woodLogHalfPresets -----------------------------------------------------------------
        savePositions(woodLogHalfPresetsStartPositions, woodLogHalfPresets);
        saveRotations(woodLogHalfPresetsStartRotations, woodLogHalfPresets);
        foreach (GameObject preset in woodLogHalfPresets)
        {
            toggleVisibility(preset, false);
        }

        //woodLogQuarterPresets -----------------------------------------------------------------
        savePositions(woodLogQuarterPresetsStartPositions, woodLogQuarterPresets);
        saveRotations(woodLogQuarterPresetsStartRotations, woodLogQuarterPresets);

        foreach (GameObject preset in woodLogQuarterPresets)
        {
            toggleVisibility(preset, false);
        }
    }

    private void savePositions(List<Vector3> positionArray, GameObject[] presets)
    {
        foreach (GameObject preset in presets)
        {
            positionArray.Add(preset.transform.position);
        }
    }

    private void saveRotations(List<Quaternion> rotationArray, GameObject[] presets)
    {
        foreach (GameObject preset in presets)
        {
            rotationArray.Add(preset.transform.rotation);
        }
    }


    public void chopWood()
    {
        if (isChoppable())
        {
            woodType = getWoodType(currentPlaceholderScript);
            if (woodType != null)
            {
                StartCoroutine(startChoppingWood(woodType));
            }
        }
    }


    IEnumerator startChoppingWood(string woodType)
    {
        //setup -------------------------------------
        GameObject container = null;
        GameObject[] presets = null;

        playSound();    

        switch (woodType)
        {
            case "Log":
                {
                    container = woodLogHalfContainer;
                    presets = woodLogHalfPresets;
                    currentPresetStartPositions = woodLogHalfPresetsStartPositions;
                    currentPresetStartRotations = woodLogHalfPresetsStartRotations;
                    break;
                }
            case "Half":
                {
                    container = woodLogQuarterContainer;
                    presets = woodLogQuarterPresets;
                    currentPresetStartPositions = woodLogQuarterPresetsStartPositions;
                    currentPresetStartRotations = woodLogQuarterPresetsStartRotations;
                    break;
                }
        }

        //main --------------------------------------------
        yield return new WaitForSeconds(0.3f);
        chopParticles.Play();

        if (currentPlaceholderScript != null)
        {
            currentPlaceholderScript.itemObject.SetActive(false);
            currentPlaceholderScript.itemObject.transform.parent = ReusableItems.transform;
            currentPlaceholderScript.snapOff(currentPlaceholderScript.itemObject);
        }


        resetContainerAndPresets(container, presets);
        yield return new WaitForSeconds(0.01f);
        rotateTowardsPlayer(container);
        yield return new WaitForSeconds(0.01f);

        toggleAllPlaceholders(false);

        for (int i = 0; i < presets.Length; i++)
        {
            //make placeholder visible
            toggleVisibility(presets[i], true);
            //add Cut Force
            addForceToPreset(presets[i], i);
        }
        //show particles and spawn items
        StartCoroutine(showParticleEffects(presets));
        yield return new WaitForSeconds(1f);
        toggleAllPlaceholders(true);
    }


    private void addForceToPreset(GameObject preset, int i)
    {
        Rigidbody rb = preset.GetComponent<Rigidbody>();
        if (rb != null)
        {
            if (i % 2 == 0)
            {
                //add force to right half
                rb.AddRelativeForce(new Vector3(0, 3f, 0f), ForceMode.Impulse);
            }
            else
            {
                //add force to left half
                rb.AddRelativeForce(new Vector3(0, 3f, 0f), ForceMode.Impulse);
            }
        }
    }


    IEnumerator showParticleEffects(GameObject[] presets)
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < presets.Length; i++)
        {
            ParticleSystem particleEffect = presets[i].GetComponentInChildren<ParticleSystem>();
            if (particleEffect != null)
            {
                particleEffect.Play();
            }
            spawnItems(presets[i]);
            toggleVisibility(presets[i], false); // set presets "invisible"
        }
    }

    private void spawnItems(GameObject preset)
    {
        ItemDropManager itemDropManager = preset.GetComponent<ItemDropManager>();
        if (itemDropManager != null)
        {
            itemDropManager.dropItems();
        }
    }


    void resetContainerAndPresets(GameObject container, GameObject[] presets)
    {
        //reset Container
        container.transform.rotation = new Quaternion(0, 0, 0, 0);
        //reset Preset

        if (currentPresetStartPositions != null && currentPresetStartRotations != null)
        {
            for (int i = 0; i < presets.Length; i++)
            {
                presets[i].transform.position = currentPresetStartPositions.ElementAt(i);
                presets[i].transform.rotation = currentPresetStartRotations.ElementAt(i);
            }
        }


    }

    void toggleVisibility(GameObject preset, bool visibility)
    {
        Rigidbody rb = preset.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = !visibility;
            rb.useGravity = visibility;
        }

        MeshRenderer meshRenderer = preset.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = visibility;
        }

        Collider collider = preset.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = visibility;
        }
    }

    void toggleAllPlaceholders(bool visibility)
    {
        foreach (GameObject placeholder in placeHolders)
        {
            toggleVisibility(placeholder, visibility);
        }
    }


    private void playSound()
    {
        if (audioSource != null)
        {
            if (audioClips != null && audioClips.Length > 0)
            {
                int random = Random.Range(0, audioClips.Length - 1);
                audioSource.clip = audioClips[random];
                audioSource.Play();
            }
        }
    }
}
