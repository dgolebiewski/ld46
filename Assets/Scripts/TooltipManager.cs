using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    [SerializeField]
    private bool followMouse = true;
    [SerializeField]
    private Vector2 mouseOffset = new Vector2(215, 165);

    [Space]

    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private TMP_Text tooltipHeader;
    [SerializeField]
    private TMP_Text tooltipContent;
    [SerializeField]
    private float textMargin = 15f;

    private RectTransform myTransform;
    private Animator animator;

    void Start()
    {
        myTransform = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(followMouse)
        {
            Vector2 offset = new Vector2(myTransform.sizeDelta.x / 2 + mouseOffset.x, -myTransform.sizeDelta.y / 2 - mouseOffset.y);

            if(Screen.width - Input.mousePosition.x < (myTransform.sizeDelta.x + mouseOffset.x * 2) * canvas.scaleFactor)
                offset = new Vector2(-offset.x, offset.y);

            if(Input.mousePosition.y * (1f / canvas.scaleFactor) < myTransform.sizeDelta.y + mouseOffset.y * 2)
                offset = new Vector2(offset.x, -offset.y);
                
            myTransform.anchoredPosition = (Vector2)Input.mousePosition * (1f / canvas.scaleFactor) + offset;
        }
    }

    void UpdateSize()
    {
        float height = tooltipHeader.GetPreferredValues().y + tooltipContent.GetPreferredValues().y + (textMargin * 2);

        myTransform.sizeDelta = new Vector2(myTransform.sizeDelta.x, height);
    }

    public void SetTooltip(string header, string content)
    {
        tooltipHeader.text = header;
        tooltipContent.text = content;

        UpdateSize();
    }

    public void SetHeader(string header)
    {
        tooltipHeader.text = header;

        UpdateSize();
    }

    public void SetContent(string content)
    {
        tooltipContent.text = content;
        
        UpdateSize();
    }

    public void ShowTooltip()
    {
        animator.SetBool("show", true);
    }

    public void HideTooltip()
    {
        animator.SetBool("show", false);
    }
}
