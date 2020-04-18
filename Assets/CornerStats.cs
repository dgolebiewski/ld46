using UnityEngine;
using UnityEngine.UI;

public class CornerStats : MonoBehaviour
{
    [SerializeField]
    private Image fuelFill;
    [SerializeField]
    private Image staminaFill;
    [SerializeField]
    private PlayerMovement playerMovement;

    private GameMaster gameMaster;

    void Start()
    {
        gameMaster = GameMaster.instance;
    }

    void Update()
    {
        fuelFill.fillAmount = gameMaster.GetCampfireTime() / gameMaster.GetCampfireMaxTime();
        staminaFill.fillAmount = playerMovement.GetStamina() / playerMovement.GetMaxStamina();
    }
}
