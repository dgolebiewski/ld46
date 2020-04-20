using System.Collections;
using UnityEngine;

public class TutorialFadeout : MonoBehaviour
{
    [SerializeField]
    private float delay = 20f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(delay);

        animator.SetTrigger("fade_out");
    }
}
