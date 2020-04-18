using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractableUI : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private TMP_Text interactionLabel;
    [SerializeField]
    private CanvasGroup interactionBar;
    [SerializeField]
    private Image interactionBarFill;

    private Interactable currentInteraction;
    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        player.interactionHighlight += DisplayInteractionHighlight;
        player.interactionDeselected += HideInteractionHighlight;
        player.interactionStarted += StartInteraction;
        player.interactionEnded += EndInteraction;
    }

    void Update()
    {
        if(currentInteraction != null && currentInteraction.GetInteractionMaxTime() > 0)
        {
            interactionBarFill.fillAmount = 1f - currentInteraction.GetInteractionTimer() / currentInteraction.GetInteractionMaxTime();
        }
    }

    void DisplayInteractionHighlight()
    {
        currentInteraction = player.GetCurrentInteractable();

        interactionBar.alpha = 0;
        canvasGroup.alpha = 1;
        interactionLabel.text = currentInteraction.GetInteractionLabel();
    }

    void HideInteractionHighlight()
    {
        currentInteraction = null;

        interactionBar.alpha = 0;
        canvasGroup.alpha = 0;
    }

    void StartInteraction()
    {
        currentInteraction = player.GetCurrentInteractable();
        
        if(currentInteraction.GetInteractionMaxTime() > 0)
        {
            interactionBarFill.color = currentInteraction.GetInteractionColor();
            interactionBar.alpha = 1;
            interactionBarFill.fillAmount = 0;
        }
    }

    void EndInteraction()
    {
        canvasGroup.alpha = 0;
        interactionBar.alpha = 0;
    }
}
