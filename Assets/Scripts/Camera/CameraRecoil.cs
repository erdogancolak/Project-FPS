using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float recoilAmount;
    [SerializeField] float recoilSpeed;
    [SerializeField] float returnSpeed;

    [Header("Recoil")]
    Vector3 originalRotation;
    Vector3 currentRecoil;

    void Start()
    {
        originalRotation = transform.localEulerAngles;
    }

    void Update()
    {
        currentRecoil = Vector3.Lerp(currentRecoil, Vector3.zero, returnSpeed * Time.deltaTime);
        transform.localEulerAngles = originalRotation + currentRecoil;
    }

    public void ApplyRecoil()
    {
        float recoilX = Random.Range(-0.5f, 0.5f);
        float recoilY = Random.Range(0.5f, 1f) * recoilAmount;
        currentRecoil += new Vector3(-recoilY, recoilX, 0);

        transform.localEulerAngles = originalRotation + currentRecoil;
    }
}

