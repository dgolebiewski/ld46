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
    [SerializeField]
    private GameObject rainEffect;
    [SerializeField]
    private Transform rainCheckPoint;
    [SerializeField]
    private float campfireMultiplierWhileRaining = 8f;
    [SerializeField]
    private float rainStartMinTime = 45;
    [SerializeField]
    private float rainStartMaxTime = 100;
    [SerializeField]
    private float rainTime = 210;
    [SerializeField]
    private LayerMask campfireCoverMask;
    [SerializeField]
    private float raycastChecksRate = 2f;
    [SerializeField]
    private float windStartMinTime = 60;
    [SerializeField]
    private float windStartMaxTime = 100;
    [SerializeField]
    private float windCampfireTimeMultiplier = 5f;
    [SerializeField]
    private Transform windDirectionPivot;
    [SerializeField]
    private Transform windCheckPoint;
    [SerializeField]
    private GameObject windDirectionUI;
    [SerializeField]
    private WindDirectionArrow windDirectionArrow;

    private float campfireTime;
    private float campfireFadeMultiplier = 1f;

    private bool isRaining = false;
    private bool isCovered = false;

    private bool isWindy = false;
    private bool coveredFromWind = false;

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
        Cursor.visible = false;

        infoPanel = InfoPanel.instance;

        ResetGameMaster();
        StartCoroutine(BonusesCheck());
        StartCoroutine(StartRain());
        StartCoroutine(StartWind());
    }

    void Update()
    {
        float campfireTimeDelta = Time.deltaTime * campfireFadeMultiplier;

        if(isRaining && !isCovered)
        {
            campfireTimeDelta *= campfireMultiplierWhileRaining;
        }

        campfireTime -= campfireTimeDelta;

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
        Cursor.visible = paused;
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

    private IEnumerator StartRain()
    {
        float timer = Random.Range(rainStartMinTime, rainStartMaxTime);

        yield return new WaitForSeconds(timer - 5f);

        infoPanel.DisplayInfo("It looks like it's going to rain.");

        yield return new WaitForSeconds(5f);

        rainEffect.SetActive(true);
        isRaining = true;

        StartCoroutine(RainCoverCheck());
        StartCoroutine(StopRain());
    }

    private IEnumerator StopRain()
    {
        yield return new WaitForSeconds(rainTime);

        rainEffect.SetActive(false);
        isRaining = false;

        StartCoroutine(StartRain());
    }

    private IEnumerator RainCoverCheck()
    {
        RaycastHit hit;
        isCovered = Physics.Raycast(rainCheckPoint.position, Vector3.down, out hit, 20, campfireCoverMask);

        if(hit.collider.gameObject.tag == "Campfire")
            isCovered = false;

        yield return new WaitForSeconds(1f / raycastChecksRate);

        if(isRaining)
            StartCoroutine(RainCoverCheck());
    }

    private IEnumerator StartWind()
    {
        yield return new WaitForSeconds(Random.Range(windStartMinTime, windStartMaxTime));

        if(!isWindy)
        {
            infoPanel.DisplayInfo("Wind is picking up.");
            windDirectionUI.SetActive(true);
        }
        else
            infoPanel.DisplayInfo("Wind is changing direction.");

        isWindy = true;
        windDirectionPivot.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        windDirectionArrow.SetDirection((windDirectionPivot.eulerAngles.y + 180) % 360);

        StartCoroutine(WindCoverCheck());
        StartCoroutine(StartWind());
    }

    private IEnumerator WindCoverCheck()
    {
        RaycastHit hit;
        coveredFromWind = Physics.Raycast(windCheckPoint.position, windCheckPoint.forward, out hit, 20, campfireCoverMask);

        if(hit.collider.gameObject.tag == "Campfire")
            coveredFromWind = false;

        yield return new WaitForSeconds(1f / raycastChecksRate);

        StartCoroutine(WindCoverCheck());
    }
}
