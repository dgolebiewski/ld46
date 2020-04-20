using UnityEngine;

public class Interactable : MonoBehaviour
{
    public const float INTERACTION_MAX_DISTANCE = 3f;

    [SerializeField]
    private bool oneTimeInteraction = false;
    [SerializeField]
    private bool destroyOnInteraction = false;
    [SerializeField]
    private string interationButton = "Interact";
    [SerializeField]
    private Color interactionColor = Color.cyan;
    [SerializeField]
    private float interactionTime = 2f;
    [SerializeField]
    private string interactionLabel;
    [SerializeField]
    private AudioSource interactionSound;
    [SerializeField]
    private AudioSource interactionCompleteSound;

    private float currentInteractionTime;

    private bool interacted = false;

    public delegate void InteractableCallback();
    private InteractableCallback onInteractionComplete;

    public InteractableCallback onInteractionCompletePersistent;

    void Start()
    {
        currentInteractionTime = 0f;

        OnInit();
    }

    protected virtual void OnInit(){}

    void Update()
    {
        if(currentInteractionTime > 0)
        {
            currentInteractionTime -= Time.deltaTime;

            if(currentInteractionTime <= 0)
            {
                interacted = true;

                if(onInteractionComplete != null)
                    onInteractionComplete();
                onInteractionComplete = null;

                if(onInteractionCompletePersistent != null)
                    onInteractionCompletePersistent();

                if(interactionSound != null)
                    interactionSound.Stop();

                if(interactionCompleteSound != null)
                    interactionCompleteSound.Play();

                if(destroyOnInteraction)
                    Destroy(this.gameObject);
            }
        }
    }

    public virtual void Interact(Player interactant, InteractableCallback completionCallback)
    {
        if(oneTimeInteraction && interacted)
            return;

        currentInteractionTime = interactionTime;

        onInteractionComplete += completionCallback;

        if(interactionSound != null)
            interactionSound.Play();
    }

    public virtual void CancelInteraction()
    {
        currentInteractionTime = 0f;
        onInteractionComplete = null;

        if(interactionSound != null)
            interactionSound.Stop();
    }

    public string GetInteractionLabel()
    {
        return interactionLabel;
    }

    public Color GetInteractionColor()
    {
        return interactionColor;
    }

    public string GetInteractionButton()
    {
        return interationButton;
    }

    public bool IsInteractable()
    {
        return (oneTimeInteraction && !interacted) || !oneTimeInteraction;
    }

    public bool InProgress()
    {
        return currentInteractionTime > 0;
    }

    public float GetInteractionMaxTime()
    {
        return interactionTime;
    }

    public float GetInteractionTimer()
    {
        return currentInteractionTime;
    }
}
