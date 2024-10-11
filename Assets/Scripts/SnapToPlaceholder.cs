using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SnapToPlaceholder : MonoBehaviour
{
    [Header("--- Settings ---")]
    public GameObject objectToSnap;
    public GameObject player;
    public GameObject holdingItem;
    public Transform mainCamera;
    public bool canBeManuallySnappedOn = true;
    public bool objCanBeRemoved = true;
    public bool showAlways = true;
    public bool showAtDistance = false;
    public bool showAfterHandCheck = false;
    [Range(0.1f, 15f)] public float showRange = 3f;
    [Range(0.1f, 3f)] public float animationTime = 1f;

    public List<GameObject> placeholderSiblings;

    [Header("--- Functions ---")]
    public UnityEvent snapOnFunction;
    public UnityEvent snapOffFunction;

    [Header("--- Debug Values ---")]
    public GameObject itemObject;
    [SerializeField] private Vector3 itemObjectScale;
    public bool isSnappedOn = false;

    private Item itemScript;
    private string itemName;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
            if (player != null)
            {
                mainCamera = player.transform.Find("Camera");
            }
        }

        if (objectToSnap != null)
        {
            itemScript = objectToSnap.GetComponent<Item>();
            if (itemScript != null)
            {
                itemName = itemScript.item.itemName;
            }
        }


        if (holdingItem == null)
        {
            holdingItem = GameObject.Find("/GameManager/ItemManager/HoldingItem");
        }

        toggleVisibility(transform.gameObject, showAlways);

    }

    void LateUpdate()
    {
        checkVisibilityOptions();
    }


    //-----------------------------MAIN FUNCTIONS---------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (canBeManuallySnappedOn)
        {
            if (other.gameObject.layer == 7)
            {
                if (isSnappedOn == false)
                {
                    snapOn(other.gameObject);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (canBeManuallySnappedOn)
        {
            if (other.gameObject.layer == 6)
            {
                if (isSnappedOn == false)
                {
                    snapOn(other.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isSnappedOn == true)
        {
            if (other.transform.gameObject == itemObject && itemObject != null)
            {
                snapOff(itemObject);
            }

        }

    }

    public void snapOn(GameObject item)
    {
        if (isSnappedOn == false)
        {

            if (equalsObjectToSnap(item))
            {
                if (!objCanBeRemoved)
                {
                    Collider collider = item.GetComponent<Collider>();
                    if (collider != null)
                    {
                        collider.enabled = false;
                    }
                }

                isSnappedOn = true;

                itemObject = item;
                itemObjectScale = itemObject.transform.lossyScale;

                //disable rigidbody
                Rigidbody rigidbody = item.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    rigidbody.useGravity = false;
                    rigidbody.isKinematic = true;
                }

                setPosRotScale(item);

                //toggle visibility
                showPlaceHolder(false);

                //execute custom Function
                executeFunctionOnSnapOn();

                //disable other placeholders on same position
                foreach (GameObject placeholderSibling in placeholderSiblings)
                {
                    placeholderSibling.SetActive(false);
                }
            }
        }
    }

    public void snapOff(GameObject item)
    {
        if (isSnappedOn == true)
        {
            if (itemObject != null)
            {
                //rigidbody is beeing toggled by Src_Interactor

                //revert scale
                item.transform.localScale = itemObjectScale;
                item.GetComponent<Collider>().isTrigger = false;
                isSnappedOn = false;

                //toggle visibility
                showPlaceHolder(true);

                itemObject = null;

                //execute custom Function
                executeFunctionOnSnapOff();

                //enable other placeholders on same position
                foreach (GameObject placeholderSibling in placeholderSiblings)
                {
                    placeholderSibling.SetActive(true);
                }
            }
        }

    }

    //-----------------------------TRANSLATION FUNCTIONS---------------------------------------------------
    private void setPosRotScale(GameObject item)
    {
        //set position to placeholder position;
        StartCoroutine(lerpToPosition(item));
        //set rotation to placeholder position;
        StartCoroutine(lerpToRotation(item));
        //set scale to placeholder
        StartCoroutine(lerpScale(item));
    }

    IEnumerator lerpToPosition(GameObject item)
    {
        float elapsedTime = 0;
        Vector3 start = item.transform.position;
        while (elapsedTime < animationTime)
        {
            float time = elapsedTime / animationTime;
            item.transform.position = Vector3.Lerp(start, transform.position, time);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        item.transform.position = transform.position;
        yield return null;
    }

    IEnumerator lerpToRotation(GameObject item)
    {
        float elapsedTime = 0;
        Quaternion start = item.transform.rotation;
        while (elapsedTime < animationTime)
        {
            float time = elapsedTime / animationTime;
            item.transform.rotation = Quaternion.Slerp(start, transform.rotation, time);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        item.transform.rotation = transform.rotation;
        yield return null;
    }

    IEnumerator lerpScale(GameObject item)
    {
        float elapsedTime = 0;
        Vector3 start = item.transform.localScale;
        while (elapsedTime < animationTime)
        {
            float time = elapsedTime / animationTime;
            item.transform.localScale = Vector3.Lerp(start, transform.lossyScale, time);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        item.transform.localScale = transform.lossyScale;
        yield return null;
    }


    //-----------------------------UTILITIES FUNCTIONS---------------------------------------------------
    void showPlaceHolder(bool visibility)
    {
        if (showAlways || showAtDistance || showAfterHandCheck)
        {
            MeshRenderer meshRenderer = transform.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = visibility;
            }
        }

    }

    private bool equalsObjectToSnap(GameObject itemObject)
    {
        if (itemName != null)
        {
            Item itemScript = itemObject.GetComponent<Item>();
            if (itemScript != null)
            {
                if (itemScript.item.itemName == itemName)
                {
                    return true;
                }
            }
        }

        return false;
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
    }

    private void showPlaceholderAfterHandCheck()
    {
        if (itemInPlayerHandEqualsPlaceholderItem())
        {
            toggleVisibility(transform.gameObject, true);
        }
        else
        {
            toggleVisibility(transform.gameObject, false);
        }
    }

    private void showPlaceholderAtDistance()
    {
        float distance = Vector3.Distance(mainCamera.transform.position, transform.position);
        if (distance < showRange)
        {
            if (!isSnappedOn)
            {
                toggleVisibility(transform.gameObject, true);
            }
            else
            {
                toggleVisibility(transform.gameObject, false);
            }
        }
        else
        {
            toggleVisibility(transform.gameObject, false);
        }
    }


    private bool itemInPlayerHandEqualsPlaceholderItem()
    {
        if (player != null)
        {
            if (mainCamera != null)
            {
                GameObject objInHand = null;

                //check rightHand for Object
                Transform rightHand = mainCamera.Find("RightHand");
                if (rightHand != null)
                {
                    if (rightHand.childCount > 0)
                    {
                        objInHand = rightHand.GetChild(0).gameObject;
                    }
                }

                //check holdingItem for Object
                if (holdingItem != null)
                {
                    if (holdingItem.transform.childCount > 0)
                    {
                        objInHand = holdingItem.transform.GetChild(0).gameObject;
                    }
                }

                if (objInHand != null)
                {
                    if (equalsObjectToSnap(objInHand))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    void checkVisibilityOptions()
    {
        if (!showAlways)
        {
            if (showAtDistance)
            {
                if (showAfterHandCheck)
                {
                    if (itemInPlayerHandEqualsPlaceholderItem())
                    {
                        showPlaceholderAtDistance();
                    }
                    else
                    {
                        toggleVisibility(transform.gameObject, false);
                    }
                }
                else
                {
                    showPlaceholderAtDistance();
                }
            }
        }
        else
        {
            if (showAfterHandCheck)
            {
                showPlaceholderAfterHandCheck();
            }
        }
    }

    void executeFunctionOnSnapOn()
    {
        if (snapOnFunction != null)
        {
            snapOnFunction.Invoke();
        }
    }

    void executeFunctionOnSnapOff()
    {
        if (snapOffFunction != null)
        {
            snapOffFunction.Invoke();
        }
    }
}
