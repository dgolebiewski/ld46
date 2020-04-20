using System.Collections;
using UnityEngine;
using TMPro;

public class InfoPanel : MonoBehaviour
{
    public static InfoPanel instance;

    [SerializeField]
    private TMP_Text panelContent;
    [SerializeField]
    private float infoPanelTime = 3.5f;

    private float hideTimer;

    private Animator animator;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Debug.LogError("Multiple instances of InfoPanel are not allowed!");
            this.enabled = false;
            return;
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(hideTimer > 0)
        {
            hideTimer -= Time.unscaledDeltaTime;
            if(hideTimer <= 0)
            {
                animator.SetBool("show", false);
            }
        }
    }

    public void DisplayInfo(string info, bool indefinite=false)
    {
        panelContent.text = info;
        animator.SetBool("show", true);
        if(!indefinite)
            hideTimer = infoPanelTime;
        else
            hideTimer = 0;
    }

    public void HideInfo()
    {
        animator.SetBool("show", false);
    }
}
