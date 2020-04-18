using UnityEngine;

public class Campfire : Interactable
{
    [SerializeField]
    private GameMaster gameMaster;

    protected override void OnInit()
    {
        onInteractionCompletePersistent += Refuel;
    }

    private void Refuel()
    {
        gameMaster.AddFuel();
    }
}
