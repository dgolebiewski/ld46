using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool lockMovement = false;
    public bool lockCamera = false;
    public float rotationSensitivity = 400;
    [SerializeField]
    private float speed = 8f;
    [SerializeField]
    private float jumpSpeed = 8f;
    [SerializeField]
    private float gravity = 6.67f;
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private float cameraMaxRotation = 65;

    private CharacterController characterController;
    private Vector3 moveDirection;

    bool jump = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        
    }

    void Update()
    {
        if(!lockMovement)
        {
            if (characterController.isGrounded)
            {
                Vector3 forward = transform.forward * Input.GetAxis("Vertical");
                Vector3 sideways = transform.right * Input.GetAxis("Horizontal");

                moveDirection = forward + sideways;
                moveDirection.y = 0;
                moveDirection *= speed;

                if (Input.GetButton("Jump"))
                {
                    moveDirection.y = jumpSpeed;
                }
            }

            moveDirection.y -= gravity * Time.deltaTime;

            characterController.Move(moveDirection * Time.deltaTime);
        }

        if(!lockCamera)
        {
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + Input.GetAxisRaw("Mouse X") * rotationSensitivity * Time.deltaTime, 0);

            cameraTransform.localRotation = Quaternion.Euler(cameraTransform.eulerAngles.x - Input.GetAxisRaw("Mouse Y") * rotationSensitivity * Time.deltaTime, 0, 0);
        }
    }
}
