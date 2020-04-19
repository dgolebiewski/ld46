using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;

    [SerializeField]
    private float campfireMaxTime = 180;
    [SerializeField]
    private float extraTimePerFuelUnit = 10;
    [SerializeField]
    private CollectableResources playerResources;
    [SerializeField]
    private CollectableResources refuelingCost;
    [SerializeField]
    private InGameMenus gameMenus;

    private float campfireTime;

    private bool paused = false;

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

        ResetGameMaster();
    }

    void Update()
    {
        campfireTime -= Time.deltaTime;

        if(Input.GetButtonDown("Cancel"))
            TogglePause();
    }

    public void TogglePause()
    {
        paused = !paused;
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Time.timeScale = paused ? 0 : 1;
        gameMenus.Toggle(paused);
    }

    public void AddFuel()
    {
        ModifyResources(refuelingCost);

        campfireTime += extraTimePerFuelUnit;
        if(campfireTime > campfireMaxTime)
            campfireTime = campfireMaxTime;
    }

    public bool ModifyResources(CollectableResources difference)
    {
        playerResources.wood += difference.wood;
        if(playerResources.wood < 0)
        {
            playerResources.wood -= difference.wood;
            return false;
        }

        playerResources.stone += difference.stone;
        if(playerResources.stone < 0)
        {
            playerResources.stone -= difference.stone;
            return false;
        }

        if(onResourcesUpdate != null)
            onResourcesUpdate();

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
}
