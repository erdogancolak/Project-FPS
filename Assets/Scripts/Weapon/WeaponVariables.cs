using UnityEngine;
using static WeaponManager;

public class WeaponVariables : MonoBehaviour
{
    public static WeaponVariables Instance;
    private void Awake()
    {
        Instance = this;
    }

    [Header("ID")]
    public string weapon_ID;
    public Transform WeaponParent;

    [Header("Weapon Type")]
    public weaponType TypeWeapon;
    public enum weaponType
    {
        Pistol,
        Shotgun,
        Smg,
        Rifle,
        Heavy,
    }

    [Header("Speed")]
    public float walkSpeed;
    public float runSpeed;

    [Header("Animations")]
    public AnimationController animationController;

    [Header("Sound Effects")]
    public AudioClip fireSound;
    public AudioClip reloadSound;

    [Header("Reload")]
    public float reloadTime;

    [Header("Ammo")]
    public int currentAmmo;
    public float fireRate;

    [Header("Reload")]
    public int bulletPerShot;
    public int maxAmmo;
    public WeaponManager.AmmoType Type;

    [Header("MuzzleFlash")]
    public Transform WeaponTip;
    public Transform WeaponTip2;
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

    [Header("Crosshair")]
    public Sprite Crosshair;
}
