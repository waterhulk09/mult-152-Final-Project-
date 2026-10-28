using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerGrab : MonoBehaviour
{
   public InputActionReference grabAction;

   public Transform holdPoint;
   
    [HideInInspector]
   public GameObject heldObject;

public string carriedItemName = "Nothing";
public float carriedWeight = 0f;
 
 [HideInInspector]

public GameObject HeldObject
    {
        get { return heldObject; }
    }
    

    void Update()
    {
        if (grabAction.action.triggered)
        {
            if (heldObject == null)
            {
                TryGrab();
            }
            else
            {
                DropObject();
            }
        }
    }

    void TryGrab()
    {
        Collider[] nearbyObject = Physics.OverlapSphere(transform.position, 2f);

        foreach (Collider col in nearbyObject)
        {
            GrabObject grabObject = col.GetComponent<GrabObject>();

            if (grabObject != null && grabObject.playerInRange)
            {
                heldObject = col.gameObject;

               GrabObject grabData = heldObject.GetComponent<GrabObject>();
               carriedItemName = grabObject.itemName;
               carriedWeight = grabObject.weight;

                Rigidbody rb = heldObject.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.isKinematic = true;
                }
        
                  heldObject.transform.SetParent(holdPoint);
                  heldObject.transform.localPosition = Vector3.zero;

         return;
            }
        }
    }

void DropObject()

    {
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();

        heldObject.transform.SetParent(null);

        if(rb != null)
        {
            rb.isKinematic = false;
        
        }
     
     
        heldObject = null;
        carriedItemName = "Nothing";
         carriedWeight = 0f;
    }
   
    public void ReleaseHeldObject()
    {
        if (heldObject == null) return;

        heldObject.transform.SetParent(null);

        carriedItemName = "Nothing";
        carriedWeight = 0;

        heldObject = null;
    }

}
