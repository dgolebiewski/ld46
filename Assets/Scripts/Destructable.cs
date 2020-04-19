using UnityEngine;

public class Destructable : Interactable
{
    protected override void OnInit()
    {
        onInteractionCompletePersistent += Destruction;
    }

    private void Destruction()
    {
        Destroy(this.gameObject);
    }
}
