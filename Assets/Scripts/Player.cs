using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool lockInteractions = false;

    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private LayerMask interactablesMask;
    [SerializeField]
    private float interactionRangeUpdateRate = 4f;

    private PlayerMovement playerMovement;

    private Interactable currentInteraction;

    public delegate void PlayerAction();

    public PlayerAction interactionHighlight;
    public PlayerAction interactionDeselected;
    public PlayerAction interactionStarted;
    public PlayerAction interactionEnded;

    void Start()
    {
        StartCoroutine(CheckInteractions());
    }

    void Update()
    {
        if(currentInteraction != null)
        {
            if(Input.GetButtonDown(currentInteraction.GetInteractionButton()) && !currentInteraction.InProgress() && currentInteraction.IsInteractable())
            {
                currentInteraction.Interact(this, InteractionCompleted);
                if(interactionStarted != null)
                    interactionStarted();
            }

            if(Input.GetButtonUp(currentInteraction.GetInteractionButton()) && currentInteraction.InProgress())
            {
                currentInteraction.CancelInteraction();
                if(interactionEnded != null)
                    interactionEnded();
            }
        }
    }

    void InteractionCompleted()
    {
        EndInteraction();
    }

    void EndInteraction()
    {
        currentInteraction = null;
        if(interactionEnded != null)
            interactionEnded();
    }

    private IEnumerator CheckInteractions()
    {
        RaycastHit hit;
        if(!lockInteractions && Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Interactable.INTERACTION_MAX_DISTANCE, interactablesMask, QueryTriggerInteraction.Ignore))
        {
            Interactable i = hit.transform.gameObject.GetComponent<Interactable>();
            if(currentInteraction != null && currentInteraction != i && currentInteraction.InProgress())
                currentInteraction.CancelInteraction();

            currentInteraction = i;
            if(!currentInteraction.InProgress())
            {
                if(interactionHighlight != null)
                    interactionHighlight();
            }
            
            if(currentInteraction == null)
            {
                if(currentInteraction.InProgress())
                    currentInteraction.CancelInteraction();

                if(interactionDeselected != null)
                    interactionDeselected();
            }
        }
        else
        {
            if(currentInteraction != null && currentInteraction.InProgress())
                currentInteraction.CancelInteraction();
            
            if(interactionDeselected != null)
                interactionDeselected();
        }

        yield return new WaitForSeconds(1f / interactionRangeUpdateRate);

        StartCoroutine(CheckInteractions());
    }

    public Interactable GetCurrentInteractable()
    {
        return currentInteraction;
    }

    public void LockInteractions(bool lockInter)
    {
        lockInteractions = lockInter;
    }
}
