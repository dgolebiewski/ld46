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
    private MeshRenderer[] objectRenderers;
    private BoxCollider objectCollider;
    private CollisionTracker objectCollisionTracker;
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
            objectInstance.transform.position = objectPosition;
            objectInstance.transform.rotation = Quaternion.Euler(objectRotation);

            if(objectCollisionTracker.IsColliding())
            {
                GameObject[] collidingObjects = objectCollisionTracker.GetCollisions();
                foreach(GameObject o in collidingObjects)
                {
                    if(o.tag != "Ground")
                    {
                        canPlace = false;
                        break;
                    }
                }
            }
        }
        else
            canPlace = false;

        foreach(MeshRenderer mr in objectRenderers)
            mr.enabled = canPlace;

        if(canPlace && Input.GetButtonDown(placeButton))
        {
            if(onObjectPlaced != null)
                onObjectPlaced(objectInstance);

            objectCollider.isTrigger = false;

            holdingObject = false;
            objectInstance = null;
            objectCollider = null;
        }
    }

    public void HoldObject(GameObject gameObject, Vector3 objectOffset)
    {
        holdingObject = true;
        objectInstance = Instantiate(gameObject);
        objectCollider = objectInstance.GetComponent<BoxCollider>();
        objectCollisionTracker = objectInstance.GetComponent<CollisionTracker>();
        objectRenderers = objectInstance.GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer mr in objectRenderers)
            mr.enabled = false;
        offset = objectOffset;

        objectCollider.isTrigger = true;
    }
}
