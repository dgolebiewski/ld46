using UnityEngine;
using TMPro;

public class ResourcesUI : MonoBehaviour
{
    [SerializeField]
    private GameMaster gameMaster;
    [SerializeField]
    private TMP_Text resourcesText;

    void Start()
    {
        gameMaster.onResourcesUpdate += UpdateUI;
    }

    private void UpdateUI()
    {
        CollectableResources resources = gameMaster.GetCollectableResources();

        resourcesText.text = "Wood: " + resources.wood + "\nStone: " + resources.stone;
    }
}
