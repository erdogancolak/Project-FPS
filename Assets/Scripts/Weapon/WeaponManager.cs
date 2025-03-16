using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    [Header("WeaponParent and Camera")]
    [SerializeField] Transform currentWeaponParent;
    [SerializeField] Transform weaponCamera;

    [Header("Availability")]
    [HideInInspector] public bool Availability;

    [Header("WeaponSlots")]
    [SerializeField] WeaponVariables weaponSlot1;
    [SerializeField] WeaponVariables weaponSlot2;
    [SerializeField] WeaponVariables weaponSlot3;
    [SerializeField] WeaponVariables weaponSlot4;
    [SerializeField] WeaponVariables weaponSlot5;

    [Header("Animations")]
    [SerializeField] string Fire_ID;
    [SerializeField] string Reload_ID;
    [SerializeField] string WeaponDown_ID;
    [SerializeField] string Aim_ID;

    AnimationController animationController;

    [HideInInspector] public bool isFire;
    
    [Header("Ammo")]
    int currentAmmo;
    public int bulletsPerShot;
    
    float fireRate;
    float lastShotTime;

    [Header("Aim")]
    public bool isAim;

    public bool canAim;

    Vector3 originalPos;
    Vector3 aimPos;

    Quaternion originalRot;
    Quaternion aimRot;

    float aimSpeed;

    float originalFOV;
    float aimFOV;

    [SerializeField] GameObject crosshair;

    [Header("Reload")]
    [HideInInspector] public bool isReload;
    int maxAmmo;
    [SerializeField] int totalAmmo;
    float reloadTime;
    [SerializeField] TMP_Text ammoText;
    AmmoType Type;

    [Header("MuzzleFlash")]
    Transform WeaponTip;
    GameObject muzzleFlashEffect;

    [Header("BulletShells")]
    ParticleSystem bulletShellsEffect;
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

    [Header("Bullet Scatters")]
    Quaternion maxScatters;
    Quaternion minScatters;

    Quaternion currentScatters;

    [Header("Recoil")]
    Vector2 minRecoil;
    Vector2 maxRecoil;
    
    [SerializeField] CameraRecoil cameraRecoil;

    [Header("BulletHoles")]
    [SerializeField] GameObject[] bulletHoles;

    [Header("Crosshair")]
    Sprite crosshairSprite;

    [Header("Sound Effects")]
    [SerializeField] AudioSource audioSource;
    AudioClip fireSound;
    AudioClip reloadSound;
    private void Start()
    {
        currentAmmo = weaponSlot1.currentAmmo;
        ChangeWeapon(weaponSlot3);
    }
    private void Update()
    {
        Inputs();
        setTotalAmmo();
    }
    private void LateUpdate()
    {
        setAim();
    }
    void Inputs()
    {
        if(Input.GetMouseButton(0) && Availability && !isReload && currentAmmo > 0 && Time.time - lastShotTime >= fireRate)
        {
            StartFire();
        }
        if(Input.GetMouseButtonUp(0))
        {
            EndFire();
        }
        if((Input.GetKeyDown(KeyCode.R) || currentAmmo <= 0) && currentAmmo != maxAmmo && totalAmmo > 0 &&  !isFire && !isReload)
        {
            StartReload();
        }
        if(Input.GetMouseButtonDown(1))
        {
            setAimBool();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1) && weaponSlot1 != null && !isFire)
        {
            isAim = false;
            ChangeWeapon(weaponSlot1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && weaponSlot2 != null && !isFire)
        {
            isAim = false;
            ChangeWeapon(weaponSlot2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && weaponSlot3 != null && !isFire)
        {
            isAim = false;
            ChangeWeapon(weaponSlot3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && weaponSlot4 != null && !isFire)
        {
            isAim = false;
            ChangeWeapon(weaponSlot4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && weaponSlot5 != null && !isFire)
        {
            isAim = false;
            ChangeWeapon(weaponSlot5);
        }
    }
    void StartFire()
    {
        isFire = true;
        animationController.setBool(Fire_ID, isFire);
        currentAmmo--;
        lastShotTime = Time.time;
        for(int i = 0;i < bulletsPerShot;i++)
        {
            if (Physics.Raycast(CameraController.Instance.Camera.position, setScatter() * CameraController.Instance.Camera.forward, out hit, fireRange))
            {
                GameObject bulletHoleCopy = Instantiate(bulletHoles[Random.Range(0, bulletHoles.Length)], hit.point, Quaternion.LookRotation(hit.normal));
                bulletHoleCopy.transform.parent = hit.transform;
                Destroy(bulletHoleCopy, 10f);
            }
        }
        CreateMuzzleFlash();
        setRecoil();
        cameraRecoil.ApplyRecoil();
        setSoundEffects(fireSound);
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

        if (bulletShellsEffect != null)
            bulletShellsEffect.Play();
        else
            return;
    }

    Quaternion setScatter()
    {
        if (PlayerMovement.Instance.isWalking)
        {
            currentScatters = Quaternion.Euler(Random.Range(-maxScatters.eulerAngles.x, maxScatters.eulerAngles.x), Random.Range(-maxScatters.eulerAngles.y, maxScatters.eulerAngles.y), Random.Range(-maxScatters.eulerAngles.z, maxScatters.eulerAngles.z));
        }
        else if (currentWeaponParent.GetComponent<WeaponVariables>().TypeWeapon == WeaponVariables.weaponType.Shotgun)
        {
            currentScatters = Quaternion.Euler(Random.Range(-maxScatters.eulerAngles.x, maxScatters.eulerAngles.x), Random.Range(-maxScatters.eulerAngles.y, maxScatters.eulerAngles.y), Random.Range(-maxScatters.eulerAngles.z, maxScatters.eulerAngles.z));
        }
        else if(isAim || (!PlayerMovement.Instance.isRunning && !PlayerMovement.Instance.isWalking))
        {
            currentScatters = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            currentScatters = Quaternion.Euler(Random.Range(-minScatters.eulerAngles.x, minScatters.eulerAngles.x), Random.Range(-minScatters.eulerAngles.y, minScatters.eulerAngles.y), Random.Range(-minScatters.eulerAngles.z, minScatters.eulerAngles.z));
        }

        return currentScatters;
    }

    void setRecoil()
    {
        float x = Random.Range(minRecoil.x, maxRecoil.x);
        float y = Random.Range(minRecoil .y, maxRecoil.y);

        PlayerCamera.Instance.addRecoil(x, y);
    }
    void StartReload()
    {
        isReload = true;
        animationController.setBool(Reload_ID, isReload);
        setSoundEffects(reloadSound);
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

    void setAimBool()
    {
        isAim = !isAim;
    }

    void setAim()
    {
        if(!canAim)
        {
            return;
        }

        if(isAim && !isReload && Availability)
        {
            currentWeaponParent.localPosition = Vector3.Lerp(currentWeaponParent.localPosition, aimPos, aimSpeed * Time.deltaTime * 5f);
            currentWeaponParent.localRotation = Quaternion.Lerp(currentWeaponParent.localRotation, aimRot , aimSpeed * Time.deltaTime * 5f);
            weaponCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(weaponCamera.GetComponent<Camera>().fieldOfView, aimFOV, aimSpeed * Time.deltaTime * 5f);
            crosshair.SetActive(false);
        }
        else
        {
            currentWeaponParent.localPosition = Vector3.Lerp(currentWeaponParent.localPosition, originalPos, aimSpeed * Time.deltaTime * 5f);
            currentWeaponParent.localRotation = Quaternion.Lerp(currentWeaponParent.localRotation, originalRot, aimSpeed * Time.deltaTime * 5f);
            weaponCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(weaponCamera.GetComponent<Camera>().fieldOfView, originalFOV, aimSpeed * Time.deltaTime * 5f);
            crosshair.SetActive(true);
        }
        animationController.setBool(Aim_ID, isAim);
    }

    void ChangeWeapon(WeaponVariables Weapon)
    {
        if(Weapon.WeaponParent != currentWeaponParent)
        {
            currentWeaponParent.gameObject.SetActive(false);
            Weapon.WeaponParent.gameObject.SetActive(true);

            currentWeaponParent.GetComponent<WeaponVariables>().currentAmmo = currentAmmo;

            currentWeaponParent = Weapon.WeaponParent;
            bulletsPerShot = Weapon.bulletPerShot;

            animationController = Weapon.animationController;
            animationController.setAvailability(0);

            fireSound = Weapon.fireSound;
            reloadSound = Weapon.reloadSound;

            fireRate = Weapon.fireRate;

            currentAmmo = Weapon.currentAmmo;
            maxAmmo = Weapon.maxAmmo;
            Type = Weapon.Type;

            reloadTime = Weapon.reloadTime;

            WeaponTip = Weapon.WeaponTip;
            muzzleFlashEffect = Weapon.muzzleFlashEffect;
            bulletShellsEffect = Weapon.bulletShellsEffect;

            canAim = Weapon.canAim;

            originalPos = Weapon.originalPos;
            aimPos = Weapon.aimPos;

            originalRot = Weapon.originalRot;
            aimRot = Weapon.aimRot;

            aimSpeed = Weapon.aimSpeed;

            originalFOV = Weapon.originalFOV;
            aimFOV = Weapon.aimFOV;

            maxScatters = Weapon.maxScatters;
            minScatters = Weapon.minScatters;

            maxRecoil = Weapon.maxRecoil;
            minRecoil = Weapon.minRecoil;

            isReload = false;

            crosshairSprite = Weapon.Crosshair;

            crosshair.GetComponent<Image>().sprite = crosshairSprite;
        }
    }
    public void WeaponDown()
    {

    }

    public void setSoundEffects(AudioClip soundEffect)
    {
        audioSource.clip = soundEffect;
        audioSource.pitch = reloadTime;
        audioSource.Play();
        if(!audioSource.isPlaying)
            audioSource.pitch = 1;
    }
}
