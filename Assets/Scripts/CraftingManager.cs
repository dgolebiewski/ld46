using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    [System.Serializable]
    public class CraftableObject
    {
        public GameObject prefab;
        public CollectableResources cost;
    }

    [SerializeField]
    private CraftableObject[] craftableObjects;
    [SerializeField]
    private ObjectPlacing objectPlacing;
    [SerializeField]
    private Player player;

    private GameMaster gameMaster;
    private InfoPanel infoPanel;

    private CraftableObject currentObject;

    private List<GameObject> placedObjects = new List<GameObject>();

    void Start()
    {
        gameMaster = GameMaster.instance;
        infoPanel = InfoPanel.instance;

        objectPlacing.onObjectPlaced += ObjectPlaced;
        objectPlacing.onPlacingCancelled += UnlockInteractions;
    }

    public void SelectObject(int index)
    {
        if(index >= craftableObjects.Length)
            return;

        if(!gameMaster.CanAfford(craftableObjects[index].cost))
        {
            infoPanel.DisplayInfo("You don't have enough resources!");
            return;
        }

        gameMaster.TogglePause(false);
        player.lockInteractions = true;

        objectPlacing.HoldObject(craftableObjects[index].prefab);
        currentObject = craftableObjects[index];
    }

    public GameObject[] GetGameObjects()
    {
        return placedObjects.ToArray();
    }

    private void ObjectPlaced(GameObject objectInstance)
    {
        placedObjects.Add(objectInstance);
        Destructable destructable = objectInstance.GetComponent<Destructable>();
        if(destructable != null)
            destructable.onInteractionCompletePersistent += delegate{ placedObjects.Remove(objectInstance); };

        gameMaster.ModifyResources(currentObject.cost);
        UnlockInteractions();
    }

    private void UnlockInteractions()
    {
        player.lockInteractions = false;
    }
}
