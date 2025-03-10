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

    [Header("Movement")]
    float Horizontal;
    float Vertical;

    [Header("Speed")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;

    [Header("Jump")]
    [SerializeField] float JumpForce;

    [Header("Gravity")]
    [SerializeField] LayerMask groundLayer;
    Vector3 gravityVector;
    float gravityScale = -9.81f;
    bool isGrounded;

    void Start()
    {
        
    }

    void Update()
    {
        Jump();
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
        if(Input.GetKey(KeyCode.LeftShift))
            return runSpeed;
        else
            return walkSpeed;
    }

    void Jump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            gravityVector.y = Mathf.Sqrt(JumpForce * -2f * gravityScale / 1000f);
    }

    void Gravity()
    {
        isGrounded = Physics.CheckSphere(GroundCheck.position, 0.4f, groundLayer);

        if (!isGrounded)
            gravityVector.y += gravityScale * Mathf.Pow(Time.deltaTime, 2);
        else if(gravityVector.y < 0)
            gravityVector.y = -0.15f;

        CharController.Move(gravityVector);
    }
}
