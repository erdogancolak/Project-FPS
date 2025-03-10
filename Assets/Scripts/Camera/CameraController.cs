using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    private void Awake()
    {
        Instance = this;
    }

    [Header("References")]
    public Transform Camera;
    [SerializeField] Transform CameraParent;
}
