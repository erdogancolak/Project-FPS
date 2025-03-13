using Unity.Mathematics;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform WeaponSwayObject;

    [Header("Settings")]
    [SerializeField] float slerpSpeed;
    [SerializeField] float intensity;
    [SerializeField] float aimIntensity;
    void Update()
    {
        Sway();
    }

    void Sway()
    {
        float x = Input.GetAxisRaw("Mouse X") * TotalIntensity();
        float y = Input.GetAxisRaw("Mouse Y") * TotalIntensity();

        Quaternion XRot = Quaternion.AngleAxis(-y, Vector3.right);
        Quaternion YRot = Quaternion.AngleAxis(x, Vector3.up);

        Quaternion Rot = XRot * YRot;

        WeaponSwayObject.localRotation = Quaternion.Slerp(WeaponSwayObject.localRotation, Rot, slerpSpeed * Time.deltaTime);
    }

    float TotalIntensity()
    {
        if (WeaponManager.Instance.isAim)
            return aimIntensity;
        else
            return intensity;
           
    }
}
