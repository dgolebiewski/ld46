using UnityEngine;

public class WindDirectionArrow : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private RectTransform arrow;
    [SerializeField]
    private float offset = 90;
    
    private float direction = 180;

    void Update()
    {
        arrow.localEulerAngles = new Vector3(0, 0, player.eulerAngles.y - direction + offset);
    }

    public void SetDirection(float dir)
    {
        direction = dir;
    }
}
