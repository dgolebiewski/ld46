using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    [SerializeField]
    private TMP_Text tooltipContent;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetTooltipContent(string content)
    {
        tooltipContent.text = content;
    }

    public void Show()
    {
        animator.SetBool("show", true);
    }

    public void Hide()
    {
        animator.SetBool("show", false);
    }
}
