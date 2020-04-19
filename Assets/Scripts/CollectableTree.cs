using System.Collections;
using UnityEngine;

public class CollectableTree : MonoBehaviour
{
    [SerializeField]
    private Vector3 forcePosition;
    [SerializeField]
    private float force;
    [SerializeField]
    private float fallThroughTimer = 1.25f;
    [SerializeField]
    private float destructionTimer = 2f;

    private Transform player;
    private Rigidbody rb;
    private CapsuleCollider col;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();

        GetComponent<Collectable>().onInteractionCompletePersistent += Fall;
    }

    void Fall()
    {
        Vector3 dir = player.position - transform.position;
        dir.Normalize();

        rb.isKinematic = false;
        rb.AddForceAtPosition(dir * force, forcePosition);
        StartCoroutine(FallThroughGround());
    }

    IEnumerator FallThroughGround()
    {
        yield return new WaitForSeconds(fallThroughTimer);

        col.isTrigger = true;

        Destroy(this.gameObject, destructionTimer);
    }
}
