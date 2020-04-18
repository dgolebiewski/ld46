using UnityEngine;

public class Collectable : Interactable
{
    [SerializeField]
    private CollectableResources resources;

    private GameMaster gameMaster;

    protected override void OnInit()
    {
        gameMaster = GameMaster.instance;
        onInteractionCompletePersistent += ChangeResourcesBalance;
    }

    private void ChangeResourcesBalance()
    {
        gameMaster.ModifyResources(resources);
    }
}
