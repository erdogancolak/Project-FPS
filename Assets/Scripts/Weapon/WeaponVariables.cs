using UnityEngine;
using static WeaponManager;

public class WeaponVariables : MonoBehaviour
{
    [Header("ID")]
    public string weapon_ID;
    public bool isRifle;
    public Transform WeaponParent;

    [Header("Animations")]
    public AnimationController animationController;

    [Header("Sound Effects")]
    public AudioClip fireSound;
    public AudioClip reloadSound;

    [Header("Ammo")]
    public int currentAmmo;
    public float fireRate;

    [Header("Reload")]
    public int bulletPerShot;
    public int maxAmmo;
    public WeaponManager.AmmoType Type;

    [Header("MuzzleFlash")]
    public Transform WeaponTip;
    public GameObject muzzleFlashEffect;

    [Header("BulletShells")]
    public ParticleSystem bulletShellsEffect;

    [Header("Aim")]
    public bool canAim;

    public Vector3 originalPos;
    public Vector3 aimPos;

    public Quaternion originalRot;
    public Quaternion aimRot;

    public float aimSpeed;

    public float originalFOV;
    public float aimFOV;

    [Header("Bullet Scatters")]
    public Quaternion maxScatters;
    public Quaternion minScatters;

    [Header("Recoil")]
    public Vector2 minRecoil;
    public Vector2 maxRecoil;
}
