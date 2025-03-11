using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance;
    private void Awake()
    {
        Instance = this;
    }

    [Header("References")]

    [SerializeField] Transform Character;
    public Transform CameraParent;

    [Header("Settings")]

    [SerializeField][Range(0, 10f)] float Sensivity;

    float X;
    float Y;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        MouseLook();
    }

    private void MouseLook()
    {
        X += Input.GetAxisRaw("Mouse X") * Sensivity;
        Y += Input.GetAxisRaw("Mouse Y") * Sensivity;

        Y = Mathf.Clamp(Y, -80, 80);

        CameraParent.localRotation = Quaternion.Euler(-Y, 0f, 0f);
        Character.localRotation = Quaternion.Euler(0, X, 0);
    }

    public void addRecoil(float x, float y)
    {
        X += x;
        Y += y;
    }
}
