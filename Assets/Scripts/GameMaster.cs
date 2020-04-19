using System.Collections;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;

    [SerializeField]
    private float campfireMaxTime = 180;
    [SerializeField]
    private float extraTimePerFuelUnit = 10;
    [SerializeField]
    private float bonusesCheckRate = 1f;
    [SerializeField]
    private CollectableResources playerResources;
    [SerializeField]
    private CollectableResources refuelingCost;
    [SerializeField]
    private InGameMenus gameMenus;
    [SerializeField]
    private CraftingManager craftingManager;

    private float campfireTime;
    private float campfireFadeMultiplier = 1f;

    private bool paused = false;

    private InfoPanel infoPanel;

    public delegate void GameMasterEvent();
    public GameMasterEvent onResourcesUpdate;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Debug.Log("Multiple instances of GameMaster are not allowed!");
            this.enabled = false;
            return;
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        infoPanel = InfoPanel.instance;

        ResetGameMaster();
        StartCoroutine(BonusesCheck());
    }

    void Update()
    {
        campfireTime -= Time.deltaTime * campfireFadeMultiplier;

        if(Input.GetButtonDown("Cancel"))
            TogglePause();
    }

    public void TogglePause()
    {
        paused = !paused;
        TogglePause(paused);
    }

    public void TogglePause(bool pause)
    {
        paused = pause;
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Time.timeScale = paused ? 0 : 1;
        gameMenus.Toggle(paused);
    }

    public void AddFuel()
    {
        bool refuel = ModifyResources(refuelingCost);

        if(!refuel)
        {
            infoPanel.DisplayInfo("You don't have enough wood!");
            Debug.Log("You don't have enough wood!");
            return;
        }

        campfireTime += extraTimePerFuelUnit;
        if(campfireTime > campfireMaxTime)
            campfireTime = campfireMaxTime;
    }

    public bool ModifyResources(CollectableResources difference)
    {
        if(!CanAfford(difference))
            return false;

        playerResources.wood += difference.wood;
        playerResources.stone += difference.stone;

        if(onResourcesUpdate != null)
            onResourcesUpdate();

        return true;
    }

    public bool CanAfford(CollectableResources cost)
    {
        if(playerResources.wood + cost.wood < 0)
            return false;

        if(playerResources.stone + cost.stone < 0)
            return false;

        return true;
    }

    public void ResetGameMaster()
    {
        campfireTime = campfireMaxTime;

        if(onResourcesUpdate != null)
            onResourcesUpdate();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public CollectableResources GetCollectableResources()
    {
        return playerResources;
    }

    public float GetCampfireTime()
    {
        return campfireTime;
    }

    public float GetCampfireMaxTime()
    {
        return campfireMaxTime;
    }

    private IEnumerator BonusesCheck()
    {
        yield return new WaitForSeconds(1f / bonusesCheckRate);

        GameObject[] craftedObjects = craftingManager.GetGameObjects();

        campfireFadeMultiplier = 1f;

        foreach(GameObject g in craftedObjects)
        {
            BlowingDevice blower = g.GetComponent<BlowingDevice>();
            if(blower != null)
                campfireFadeMultiplier *= (1f - blower.GetBurningSlowdown());
        }

        StartCoroutine(BonusesCheck());
    }
}
