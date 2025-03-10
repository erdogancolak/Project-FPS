using UnityEngine;
using TMPro;
public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    [Header("Availability")]
    [HideInInspector] public bool Availability;

    [Header("Animations")]
    [SerializeField] AnimationController animationController;

    [SerializeField] bool isFire;

    [SerializeField] string Fire_ID;
    [SerializeField] string Reload_ID;
    [SerializeField] string WeaponDown_ID;

    [Header("Ammo")]
    [SerializeField] int currentAmmo;
    
    [SerializeField] float fireRate;
    float lastShotTime;

    [Header("Reload")]
    [SerializeField] bool isReload;
    [SerializeField] int maxAmmo;
    [SerializeField] int totalAmmo;
    [SerializeField] TMP_Text ammoText;
    [SerializeField] AmmoType Type;

    [Header("MuzzleFlash")]
    [SerializeField] Transform WeaponTip;
    [SerializeField] GameObject muzzleFlashEffect;

    [Header("BulletShells")]
    [SerializeField] ParticleSystem bulletShellsEffect;
    public enum AmmoType 
    { 
        _5_56,
        _7_62,
        _9mm,
        _45cal
    }

    [Header("BulletCount")]
    [SerializeField] int _5_56_Count;
    [SerializeField] int _7_62_Count;
    [SerializeField] int _9mm_Count;
    [SerializeField] int _45cal_Count;

    [Header("BulletRay")]
    RaycastHit hit;
    float fireRange = Mathf.Infinity;

    [Header("BulletHoles")]
    [SerializeField] GameObject[] bulletHoles;
    
    private void Update()
    {
        Inputs();
        setTotalAmmo();
    }
    void Inputs()
    {
        if(Input.GetMouseButton(0) && Availability && !isReload && currentAmmo > 0 && Time.time - lastShotTime >= fireRate)
        {
            StartFire();
        }
        if((Input.GetKeyDown(KeyCode.R) || currentAmmo <= 0) && currentAmmo != maxAmmo && totalAmmo > 0 &&  !isFire)
        {
            StartReload();
        }
    }
    void StartFire()
    {
        isFire = true;
        animationController.setBool(Fire_ID, isFire);
        currentAmmo--;
        lastShotTime = Time.time;

        if (Physics.Raycast(CameraController.Instance.Camera.position, CameraController.Instance.Camera.forward, out hit, fireRange))
        {
            GameObject bulletHoleCopy = Instantiate(bulletHoles[Random.Range(0, bulletHoles.Length)], hit.point, Quaternion.LookRotation(hit.normal));
            bulletHoleCopy.transform.parent = hit.transform;
            Destroy(bulletHoleCopy, 10f);
        }

        CreateMuzzleFlash();
    }
    public void EndFire()
    {
        isFire = false;
        animationController.setBool(Fire_ID, isFire);
    }

    void CreateMuzzleFlash()
    {
        GameObject muzzleFlashCopy = Instantiate(muzzleFlashEffect, WeaponTip.position,WeaponTip.rotation,WeaponTip);
        Destroy(muzzleFlashCopy , 5f);

        bulletShellsEffect.Play();
    }
    void StartReload()
    {
        isReload = true;
        animationController.setBool(Reload_ID, isReload);
    }
    public void EndReload()
    {
        isReload = false;
        animationController.setBool(Reload_ID, isReload);

        int amount = setReloadAmount(totalAmmo);
        currentAmmo += amount;

        switch (Type)
        {
            case AmmoType._5_56:
                _5_56_Count -= amount;
                break;
            case AmmoType._7_62:
                _7_62_Count -= amount;
                break;
            case AmmoType._9mm:
                _9mm_Count -= amount;
                break;
            case AmmoType._45cal:
                _45cal_Count -= amount;
                break;
        }
    }

    void setAmmoText(int Ammo , int TotalAmmo)
    {
        ammoText.text = (Ammo + " / " + TotalAmmo);
    }

    
    void setTotalAmmo()
    {
        switch(Type)
        {
            case AmmoType._5_56:
                totalAmmo = _5_56_Count;
                break;
            case AmmoType._7_62:
                totalAmmo = _7_62_Count;
                break;
            case AmmoType._9mm:
                totalAmmo = _9mm_Count;
                break;
            case AmmoType._45cal:
                totalAmmo = _45cal_Count;
                break;
        }

        ammoText.text = (currentAmmo + " / " + totalAmmo);
    }
    int setReloadAmount(int InventoryAmount)
    {
        int amountNeeded = maxAmmo - currentAmmo;

        if(amountNeeded <= InventoryAmount)
        {
            return amountNeeded;
        }
        else
        {
            return InventoryAmount;
        }
    }
    public void WeaponDown()
    {

    }
}
