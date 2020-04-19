using UnityEngine;

public class BlowingDevice : MonoBehaviour
{
    [SerializeField]
    private float range = 3.5f;
    [SerializeField]
    private float minBurningSlowdown = 0.06f;
    [SerializeField]
    private float maxBurningSlowdown = 0.15f;
    [SerializeField]
    private LayerMask raycastMask;

    public float GetBurningSlowdown()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, range, raycastMask, QueryTriggerInteraction.Ignore))
        {
            Campfire camp = hit.collider.gameObject.GetComponent<Campfire>();
            if(camp == null)
                return 0f;

            float dist = Vector3.Distance(hit.point, transform.position);
            return Mathf.Lerp(maxBurningSlowdown, minBurningSlowdown, dist / range);
        }
        else 
            return 0f;
    }
}
