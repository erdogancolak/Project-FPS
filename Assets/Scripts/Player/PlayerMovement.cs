using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;
    private void Awake()
    {
        Instance = this;
    }

    [Header("References")]

    [SerializeField] CharacterController CharController;
    [SerializeField] Transform Character;
    [SerializeField] Transform GroundCheck;

    [SerializeField] WeaponManager weaponManager;
    [SerializeField] float currentWeaponSpeed;

    [Header("Movement")]
    float Horizontal;
    float Vertical;

    public bool isWalking;
    public bool isRunning;

    [Header("Speed")]
    public float walkSpeed;
    public float runSpeed;

    [Header("Jump")]
    [SerializeField] float JumpForce;

    [Header("Gravity")]
    [SerializeField] LayerMask groundLayer;
    Vector3 gravityVector;
    float gravityScale = -9.81f;
    [HideInInspector] public bool isGrounded;

    [Header("Crouch")]
    [SerializeField] float normalHeight;
    [SerializeField] float crouchHeight;
    [HideInInspector] public bool isCrouch;
    float crouchSpeed;

    void Start()
    {
        var currentWeapon = weaponManager.currentWeaponParent;
    }

    void Update()
    {
        Jump();
        CheckMovement();
        HandleCrouch();
    }
    private void FixedUpdate()
    {
        Movement();
        Gravity();
    }

    void Movement()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");

        Vector3 Move = Character.right * Horizontal + Character.forward * Vertical;
        CharController.Move(Move * TotalSpeed() * Time.deltaTime);
    }
    float TotalSpeed()
    {
        if(Input.GetKey(KeyCode.LeftShift) && !WeaponManager.Instance.isFire)
            return runSpeed;
        else
            return walkSpeed;
    }

    void CheckMovement()
    {
        if((Horizontal != 0 || Vertical != 0) || (Horizontal != 0 && Vertical != 0))
            {
            if(TotalSpeed() == runSpeed)
            {
                isWalking = false;
                isRunning = true;
            }
            if(TotalSpeed() == walkSpeed)
            {
                isWalking = true;
                isRunning = false;
            }
        }
        else
        {
            isWalking = false;
            isRunning = false;
        }
    }

    void Jump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            gravityVector.y = Mathf.Sqrt(JumpForce * -2f * gravityScale);
        }
    }

    void Gravity()
    {
        isGrounded = Physics.CheckSphere(GroundCheck.position, 0.4f, groundLayer);

        if (!isGrounded)
        {
            gravityVector.y += gravityScale * Time.deltaTime;
        }
        else if (gravityVector.y < 0)
        {
            gravityVector.y = -2f;
        }

        CharController.Move(gravityVector * Time.deltaTime);
    }

    void HandleCrouch()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (!isCrouch)
            {
                isCrouch = true;
                CharController.height = crouchHeight;
                crouchSpeed = walkSpeed * 0.3f;
                walkSpeed = crouchSpeed;
            }
        }
        else
        {
            if (isCrouch)
            {
                isCrouch = false;
                CharController.height = normalHeight;
                walkSpeed = WeaponManager.Instance.currentWeaponParent.GetComponent<WeaponVariables>().walkSpeed;
                runSpeed = WeaponManager.Instance.currentWeaponParent.GetComponent<WeaponVariables>().runSpeed;
            }
        }
    }
}
