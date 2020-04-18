using UnityEngine;
using UnityEngine.UI;

public class CampfireTimer : MonoBehaviour
{
    [SerializeField]
    private GameMaster gameMaster;
    [SerializeField]
    private Image fill;

    void Update()
    {
        fill.fillAmount = gameMaster.GetCampfireTime() / gameMaster.GetCampfireMaxTime();
    }
}
