
using UnityEngine;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{
    private RaycastHit hit;
    const float maxDistance = 10.0f;

    [SerializeField] Image[] canvasImages;

    [SerializeField] 
    private GameObject cam;

    public Transform rightHand;  // Referenz zur rechten Hand des Spielers
    
    public LayerMask pickupLayer;  // Layer für aufhebbare Objekte (z.B. ein bestimmter Layer für Items)

    private GameObject currentObject;  // Das aktuell aufgenommene Objekt
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
                                        if (rightHand.GetComponentInChildren<Item>().item == neededItem.item)
                                        {
                                            hitAction.ActivateEvents();
                                            DropObjectAndDestroy();

                                    }
                                    else Debug.Log("Wrong Item!");

                                    }

                                }

                            }else hitAction.ActivateEvents();

                    }
                }
            }
            
        }

        // Checke, ob der Spieler das Aufnehmen-Objekt triggern möchte (z.B. per E-Taste)
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isHoldingObject)
            {
                // Wenn der Spieler ein Objekt hält, lasse es los
                DropObject();
            }
            else if (hit.collider != null)  
            {
                
                if (hit.collider.gameObject.layer == Mathf.RoundToInt(Mathf.Log(pickupLayer.value, 2))) //Checken, ob das Objekt im richtigen Layer liegt, mit Mathe lol
                {           
                    if (isHoldingObject)
                    {
                        // Wenn der Spieler ein Objekt hält, lasse es los
                        DropObject();
                    }
                    else
                    {
                        // Wenn kein Objekt gehalten wird, versuche eines aufzuheben
                        PickupObjectToHand(hit.collider.gameObject);
                        Debug.Log("Versuche Objekt aufzunehmen");
                    }
                }
                
            }
            
        }

    }
  
    // Methode zum Attach an die Hand
    void PickupObjectToHand(GameObject obj)
    {
        // Das Objekt an die Hand des Spielers anhängen
        obj.transform.SetParent(rightHand);
        obj.transform.localPosition = Vector3.zero;  // Setzt das Objekt auf die Handposition
        obj.transform.localRotation = Quaternion.identity;  // Setzt die Rotation des Objekts zurück
        obj.GetComponent<Rigidbody>().isKinematic = true;  
        currentObject = obj;  
        isHoldingObject = true;
    }

    // Methode zum Loslassen des Objekts
    void DropObject()
    {
        if (currentObject != null)
        {
            currentObject.transform.SetParent(null);  // Objekt vom Spieler trennen
            Rigidbody rb = currentObject.GetComponent<Rigidbody>();
            currentObject.transform.position = currentObject.transform.position + currentObject.transform.forward*3;

            if (rb != null)
            {
                rb.isKinematic = false;  // Physik wieder einschalten
            }

            currentObject = null;
            isHoldingObject = false;
        }
    }

    void DropObjectAndDestroy()
    {
        if (currentObject != null)
        {
            currentObject.transform.SetParent(null);  // Objekt vom Spieler trennen
            Destroy(currentObject);

            currentObject = null;
            isHoldingObject = false;
        }
    }


}
