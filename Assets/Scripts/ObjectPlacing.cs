using UnityEngine;

public class ObjectPlacing : MonoBehaviour
{
    [SerializeField]
    private float maxPlaceDistance;
    [SerializeField]
    private Vector3 extraFreeSpace = new Vector3(2, 0, 2);
    [SerializeField]
    private Vector3 placingOffset = new Vector3(0, 0.75f, 0);
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private LayerMask collisionLayerMask;
    [SerializeField]
    private string placeButton = "Interact";

    private bool holdingObject = false;
    private GameObject objectInstance;
    private BoxCollider objectCollider;
    private Vector3 offset;

    public delegate void ObjectPlacingEvent();
    public ObjectPlacingEvent onPlacingCancelled;

    public delegate void ObjectPlaced(GameObject isntance);
    public ObjectPlaced onObjectPlaced;


    void Update()
    {
        if(!holdingObject)
            return;

        if(Input.GetButtonDown("Cancel"))
        {
            Destroy(objectInstance);

            holdingObject = false;
            objectInstance = null;
            objectCollider = null;

            if(onPlacingCancelled != null)
                onPlacingCancelled();

            return;
        }

        bool canPlace = true;
        Vector3 objectPosition = offset + placingOffset;
        Vector3 objectRotation = new Vector3(0, transform.eulerAngles.y, 0);

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, maxPlaceDistance, groundLayer, QueryTriggerInteraction.Ignore))
        {
            objectPosition += hit.point;
            Collider[] colliders = Physics.OverlapBox(hit.point + offset + placingOffset, objectCollider.size / 2 + extraFreeSpace, Quaternion.Euler(objectRotation), collisionLayerMask, QueryTriggerInteraction.Ignore);
            if(colliders.Length > 0)
            {
                for(int i = 0; i < colliders.Length; i++)
                {
                    if(colliders[i].gameObject != objectInstance)
                    {
                        canPlace = false;
                        break;
                    }
                }
            }
        }
        else
            canPlace = false;

        objectInstance.SetActive(canPlace);
        objectInstance.transform.position = objectPosition;
        objectInstance.transform.rotation = Quaternion.Euler(objectRotation);

        if(canPlace && Input.GetButtonDown(placeButton))
        {
            if(onObjectPlaced != null)
                onObjectPlaced(objectInstance);

            holdingObject = false;
            objectInstance = null;
            objectCollider = null;
        }
    }

    public void HoldObject(GameObject gameObject)
    {
        holdingObject = true;
        objectInstance = Instantiate(gameObject);
        objectInstance.SetActive(false);
        objectCollider = objectInstance.GetComponent<BoxCollider>();
        offset = new Vector3(0, objectCollider.size.y / 2, 0);
    }
}
