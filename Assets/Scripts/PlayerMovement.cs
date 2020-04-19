using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool lockMovement = false;
    public bool lockCamera = false;
    public float rotationSensitivity = 400;
    [SerializeField]
    private float speed = 8f;
    [SerializeField]
    private float sprintSpeed = 12f;
    [SerializeField]
    private float maxStamina = 14f;
    [SerializeField]
    private float staminaRegen = 0.6f;
    [SerializeField]
    private float sprintCooldown = 1.5f;
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

    private float stamina = 0f;
    private float sprintCooldownLeft = 0;
    bool sprint = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        stamina = maxStamina;
    }

    void Update()
    {
        sprint = Input.GetButton("Sprint") && stamina > 0 && sprintCooldownLeft <= 0;

        if(!lockMovement)
        {
            float currentSpeed = sprint ? sprintSpeed : speed;

            if (characterController.isGrounded)
            {
                Vector3 forward = transform.forward * Input.GetAxis("Vertical");
                Vector3 sideways = transform.right * Input.GetAxis("Horizontal");

                moveDirection = forward + sideways;
                moveDirection.y = 0;
                moveDirection *= currentSpeed;

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

        if(!sprint)
        {
            stamina += staminaRegen * Time.deltaTime;
            sprintCooldownLeft -= Time.deltaTime;

            if(stamina > maxStamina)
                stamina = maxStamina;
        }
        else
        {
            stamina -= Time.deltaTime;
            if(stamina <= 0)
                sprintCooldownLeft = sprintCooldown;
        }
    }

    public bool IsSprinting()
    {
        return sprint;
    }

    public float GetStamina()
    {
        return stamina;
    }

    public float GetMaxStamina()
    {
        return maxStamina;
    }
}
