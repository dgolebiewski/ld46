using System.Collections;
using System.Linq;
using UnityEngine;

public class ObjectCulling : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objects;
    [SerializeField]
    private float objectsCullingDistance = 45f;
    [SerializeField]
    private float cullingUpdateRate = 5f;

    private Transform targetCamera;

    void Awake()
    {
        targetCamera = Camera.main.transform;
        if(objects == null || objects.Length == 0)
        {
            Transform[] children = transform.Cast<Transform>().ToArray();
            objects = new GameObject[children.Length];
            for(int i = 0; i < children.Length; i++)
                objects[i] = children[i].gameObject;
        }

        StartCoroutine(UpdateMesh());
    }

    IEnumerator UpdateMesh()
    {
        foreach(GameObject o in objects)
        {
            if(Vector3.Distance(o.transform.position, targetCamera.transform.position) > objectsCullingDistance)
                o.SetActive(false);
            else
                o.SetActive(true);
        }

        yield return new WaitForSeconds(1f / cullingUpdateRate);

        StartCoroutine(UpdateMesh());
    }
}
