using UnityEngine;
using UnityEngine.UI;

public class InGameMenus : MonoBehaviour
{
    [System.Serializable]
    public class InGameMenusTab
    {
        public CanvasGroup canvasGroup;
        public Image tabButtonImage;

        public void Toggle(bool active)
        {
            canvasGroup.alpha = active ? 1 : 0;
            canvasGroup.interactable = active;
            canvasGroup.blocksRaycasts = active;
        }
    }

    [SerializeField]
    private InGameMenusTab[] tabs;
    [SerializeField]
    private Color activeTabColor = Color.cyan;
    [SerializeField]
    private Color inactiveTabColor = Color.blue;

    private CanvasGroup canvasGroup;

    private bool open = false;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if(tabs.Length > 1)
            for(int i = 1; i < tabs.Length; i++)
                Toggle(false);
    }

    public void Toggle()
    {
        Toggle(!open);
    }

    public void Toggle(bool _open)
    {
        open = _open;

        canvasGroup.alpha = open ? 1 : 0;
        canvasGroup.interactable = open;
        canvasGroup.blocksRaycasts = open;
    }

    public void SwitchTab(int tabIndex)
    {
        if(tabs.Length == 0)
            return;

        for(int i = 0; i < tabs.Length; i++)
        {
            if(i != tabIndex)
            {
                tabs[i].tabButtonImage.color = inactiveTabColor;
                tabs[i].Toggle(false);
            }
            else
            {
                tabs[i].tabButtonImage.color = activeTabColor;
                tabs[i].Toggle(true);
            }
        }
    }
}
