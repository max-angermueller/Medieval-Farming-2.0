
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;

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
    
    private GameObject lastRaycastHit;

    void Start()
    {
        forwardRay = new Ray(cam.transform.position, cam.transform.forward);
    }

    void changeRaycastHit(GameObject obj)
    {
        if (obj == null)
        {
            if (lastRaycastHit!= null && lastRaycastHit.GetComponent<Outlines>())
            {
                lastRaycastHit.GetComponent<Outlines>().enabled = false;
            }
        }
        else if (obj != lastRaycastHit)  //Neues Objekt im Ray
        {
            if (lastRaycastHit != null && lastRaycastHit.GetComponent<Outlines>())
            {
                lastRaycastHit.GetComponent<Outlines>().enabled = false;
                          
            }
            else if (lastRaycastHit == null && obj.GetComponent<Outlines>())
            {
                obj.GetComponent<Outlines>().enabled = true;
            }
            
        }
        else if (obj == lastRaycastHit)
        {
            if (obj.GetComponent<Outlines>())
            {
                obj.GetComponent<Outlines>().enabled = true;
            }
        }
        lastRaycastHit = obj;
    }
    void Update()
    {
        forwardRay = new Ray(cam.transform.position, cam.transform.forward);
        Physics.Raycast(forwardRay,out hit, maxDistance);
        //Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.blue, maxDistance);

        foreach (var  image in canvasImages)
        {
            image.enabled = false;
        }

        if (hit.collider == null)
        {
            changeRaycastHit(null);
        }
        else
        {
            GameObject raycastHit = hit.collider.gameObject;
            changeRaycastHit(raycastHit);
        }
        

        if (hit.collider!= null)
        {
            
            if (hit.collider.GetComponent<Outlines>() != null)
            {
                
                
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
                                        //Debug.Log(itemInHands.item.ItemType);
                                            switch (hitAction.GetActionType())
                                            {
                                                case ActionType.ConsumeObject:
                                                    if (itemInHands.item.itemType == ItemType.FluidContainer) //For Buckets
                                                {
                                                        if (rightHand.GetComponentInChildren<Bucket>()!= null) 
                                                        {
                                                            if (hitAction.getSpecialRequirements() == rightHand.GetComponentInChildren<Bucket>().FluidType)
                                                            {
                                                                if (itemInHands.UnfillContainer())
                                                                {
                                                                    hitAction.ActivateEvents();
                                                                }
                                                            }                                                      
                                                        }                                                        
                                                    }
                                                    else
                                                    {       //For Ressources
                                                        hitAction.ActivateEvents();
                                                        DropObjectAndDestroy();
                                                    }
                                                    break;

                                                case ActionType.FillContainer:
                                                    if (itemInHands.item.itemType == ItemType.FluidContainer)
                                                    {
                                                        if (hit.collider.tag == "Watersource" || hit.collider.tag ==  "Milksource")
                                                        {
                                                            hitAction.ActivateEvents();
                                                        }
                                                        else
                                                        {
                                                            itemInHands.FillContainer();
                                                        }
                                                    }
                                                    break;

                                                case ActionType.Interact:

                                                    break;

                                            }
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
