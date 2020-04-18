using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float rotationSensitivity = 400;
    [SerializeField]
    private float speed = 140;
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private float cameraMaxRotation = 65;

    private Rigidbody rb;

    float vertical = 0, horizontal = 0, rotationX = 0, rotationY;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");
        rotationX = Input.GetAxisRaw("Mouse X");
        rotationY = -Input.GetAxisRaw("Mouse Y");
    }

    void FixedUpdate()
    {
        float sum = vertical + horizontal;
        if(sum > 1)
        {
            vertical = vertical / sum;
            horizontal = horizontal / sum;
        }

        rb.velocity = transform.forward * vertical * speed * Time.fixedDeltaTime + transform.right * horizontal * speed * Time.fixedDeltaTime;
        rb.MoveRotation(Quaternion.Euler(0, transform.eulerAngles.y + rotationX * rotationSensitivity * Time.fixedDeltaTime, 0));

        cameraTransform.localRotation = Quaternion.Euler(cameraTransform.eulerAngles.x + rotationY * rotationSensitivity * Time.fixedDeltaTime, 0, 0);
    }
}
