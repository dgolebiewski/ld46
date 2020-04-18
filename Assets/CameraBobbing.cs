using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    [SerializeField]
    private float bobbingSpeed = 0.18f;
    [SerializeField]
    private float bobbingSpeedSprint = 0.2f;
    [SerializeField]
    private float bobbingAmount = 0.2f;
    [SerializeField]
    private float midpoint = 2.0f;

    private float timer = 0f;
    
    void Update () 
    {
        float waveslice = 0.0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float currentBobbingSpeed = Input.GetButton("Sprint") ? bobbingSpeedSprint : bobbingSpeed;
    
        Vector3 newPos = transform.localPosition; 
    
        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
            timer = 0.0f;
        else 
        {
            waveslice = Mathf.Sin(timer);
            timer = timer + currentBobbingSpeed;
            if (timer > Mathf.PI * 2) 
                timer = timer - (Mathf.PI * 2);
        }
        
        if (waveslice != 0) 
        {
            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp (totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;
            newPos.y = midpoint + translateChange;
        }
        else 
            newPos.y = midpoint;
    
        transform.localPosition = newPos;
    }
}
