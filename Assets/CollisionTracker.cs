using System.Collections.Generic;
using UnityEngine;

public class CollisionTracker : MonoBehaviour
{
    private List<GameObject> collisions = new List<GameObject>();

    void OnTriggerEnter(Collider col)
    {
        collisions.Add(col.gameObject);
    }

    void OnTriggerExit(Collider col)
    {
        collisions.Remove(col.gameObject);
    }

    public GameObject[] GetCollisions()
    {
        return collisions.ToArray();
    }

    public int GetCollisionCount()
    {
        return collisions.Count;
    }

    public bool IsColliding()
    {
        return collisions.Count > 0;
    }
}
