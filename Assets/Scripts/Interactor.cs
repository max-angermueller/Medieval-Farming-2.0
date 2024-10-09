
using UnityEngine;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{
    private RaycastHit hit;
    const float maxDistance = 10.0f;

    [SerializeField] 
    private Image[] canvasImages;

    [SerializeField] 
    private GameObject cam;

    [SerializeField]
    private Transform rightHand;  
    
    public LayerMask pickupLayer;  // layer for collectable objects

    private GameObject currentObject;
    private bool isHoldingObject = false;
    private Ray forwardRay;

    void Start()
    {
        forwardRay = new Ray(cam.transform.position, cam.transform.forward);
        Debug.Log(canvasImages.Length);
    }

    // Update is called once per frame
    void Update()
    {
        forwardRay = new Ray(cam.transform.position, cam.transform.forward);
        Physics.Raycast(forwardRay,out hit, maxDistance);
        //Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.blue, maxDistance);

        foreach (var  image in canvasImages)
        {
            image.enabled = false;
        }
        
        if (Input.GetKeyDown(KeyCode.F))  //Interagieren
        {
            Debug.Log(hit.collider.name);
        }


        if (hit.collider!= null)
        {
            if (hit.collider.GetComponent<Outlines>() != null)
            {
                Outlines outlineScr = hit.collider.GetComponent<Outlines>();
                outlineScr.OutlineActive = true;
                outlineScr.OnEnable();

                if (hit.collider.GetComponent<ActionOnClick>() != null)
                {
                    ActionOnClick hitAction = hit.collider.GetComponent<ActionOnClick>();
                    
                    int counter = 0;    
                    foreach (var image in canvasImages)
                    {
                        if (hitAction.getSprite(counter)!=null)
                        {
                            image.enabled = true;
                            image.sprite = hitAction.getSprite(counter);
                        }
                        else
                        {
                            image.sprite=null;
                        }
                        counter++;
                        
                    }
                    
                        

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                            if (hitAction.getRequirement() != null)
                            {
                                if (rightHand.childCount != 0)
                                {
                                    Item neededItem = hitAction.getRequirement();
                                    if (rightHand.GetComponentInChildren<Item>() != null)
                                    {
                                        Item itemInHands = rightHand.GetComponentInChildren<Item>();
                                        if (itemInHands.item == neededItem.item)
                                        {
                                            hitAction.ActivateEvents();
                                            Debug.Log(itemInHands.item.ItemType);
                                            if (itemInHands.item.ItemType == ItemType.Container)
                                            {
                                                itemInHands.UnfillContainer();
                                            }
                                            else DropObjectAndDestroy();

                                        }
                                        else Debug.Log("Wrong Item!");

                                    }

                                }

                            }else hitAction.ActivateEvents();

                    }
                }
            }
            
        }

       
        if (Input.GetKeyDown(KeyCode.E))  //Grab Item
        {
            if (isHoldingObject)
            {
               DropObject();
            }
            else if (hit.collider != null)  
            {
                
                if (hit.collider.gameObject.layer == Mathf.RoundToInt(Mathf.Log(pickupLayer.value, 2))) //check layer
                {           
                    PickupObjectToHand(hit.collider.gameObject);
                    //Debug.Log("Versuche Objekt aufzunehmen");
                }
                   
            }
        }
    }
  
 
    void PickupObjectToHand(GameObject obj)
    {
        // snap object to players hand
        obj.transform.SetParent(rightHand);
        obj.transform.localPosition = Vector3.zero;  
        obj.transform.localRotation = Quaternion.identity;  
        obj.GetComponent<Rigidbody>().isKinematic = true;  
        currentObject = obj;  
        isHoldingObject = true;
    }

    
    void DropObject()
    {
        if (currentObject != null)
        {
            currentObject.transform.SetParent(null);  // Objekt vom Spieler trennen
            Rigidbody rb = currentObject.GetComponent<Rigidbody>();
            currentObject.transform.position = currentObject.transform.position + currentObject.transform.forward*3;

            if (rb != null)
            {
                rb.isKinematic = false;  
            }

            currentObject = null;
            isHoldingObject = false;
        }
    }

    void DropObjectAndDestroy()
    {
        if (currentObject != null)
        {
            currentObject.transform.SetParent(null);  
            Destroy(currentObject);
            Debug.Log("Item destroyed.");
            currentObject = null;
            isHoldingObject = false;
        }
    }


}
