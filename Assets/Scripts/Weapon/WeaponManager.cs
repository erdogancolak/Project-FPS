using UnityEngine;
using TMPro;
public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    [Header("WeaponParent and Camera")]
    [SerializeField] Transform currentWeaponParent;
    [SerializeField] Transform camera;

    [Header("Availability")]
    [HideInInspector] public bool Availability;

    [Header("Animations")]
    [SerializeField] AnimationController animationController;

    public bool isFire;

    [SerializeField] string Fire_ID;
    [SerializeField] string Reload_ID;
    [SerializeField] string WeaponDown_ID;
    [SerializeField] string Aim_ID;

    [Header("Ammo")]
    [SerializeField] int currentAmmo;
    
    [SerializeField] float fireRate;
    float lastShotTime;

    [Header("Aim")]
    public bool isAim;

    [SerializeField] Vector3 originalPos;
    [SerializeField] Vector3 aimPos;

    [SerializeField] Quaternion originalRot;
    [SerializeField] Quaternion aimRot;

    [SerializeField] float aimSpeed;

    [SerializeField] float originalFOV;
    [SerializeField] float aimFOV;

    [SerializeField] GameObject crosshair;

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

    [Header("Bullet Scatters")]
    [SerializeField] Quaternion maxScatters;
    [SerializeField] Quaternion minScatters;

    Quaternion currentScatters;

    [Header("Recoil")]
    [SerializeField] Vector2 maxRecoil;
    [SerializeField] Vector2 minRecoil;

    [Header("BulletHoles")]
    [SerializeField] GameObject[] bulletHoles;

    [Header("Sound Effects")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip readySound;
    [SerializeField] AudioClip fireSound;
    [SerializeField] AudioClip reloadSound;

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
        if((Input.GetKeyDown(KeyCode.R) || currentAmmo <= 0) && currentAmmo != maxAmmo && totalAmmo > 0 &&  !isFire && !isReload)
        {
            StartReload();
        }
        if(Input.GetMouseButtonDown(1))
        {
            setAimBool();
        }
    }
    void StartFire()
    {
        isFire = true;
        animationController.setBool(Fire_ID, isFire);
        currentAmmo--;
        lastShotTime = Time.time;

        if (Physics.Raycast(CameraController.Instance.Camera.position, setScatter() * CameraController.Instance.Camera.forward, out hit, fireRange))
        {
            GameObject bulletHoleCopy = Instantiate(bulletHoles[Random.Range(0, bulletHoles.Length)], hit.point, Quaternion.LookRotation(hit.normal));
            bulletHoleCopy.transform.parent = hit.transform;
            Destroy(bulletHoleCopy, 10f);
        }

        CreateMuzzleFlash();
        setRecoil();
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

        bulletShellsEffect.Play();
    }

    Quaternion setScatter()
    {
        if(PlayerMovement.Instance.isWalking)
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
        float x = Random.Range(maxRecoil.x, minRecoil.x);
        float y = Random.Range(maxRecoil .y, minRecoil.y);

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
        if(isAim)
        {
            currentWeaponParent.localPosition = Vector3.Lerp(currentWeaponParent.localPosition, aimPos, aimSpeed * Time.deltaTime * 5f);
            currentWeaponParent.localRotation = Quaternion.Lerp(currentWeaponParent.localRotation, aimRot , aimSpeed * Time.deltaTime * 5f);
            camera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(camera.GetComponent<Camera>().fieldOfView, aimFOV, aimSpeed * Time.deltaTime * 5f);
            crosshair.SetActive(false);
        }
        else
        {
            currentWeaponParent.localPosition = Vector3.Lerp(currentWeaponParent.localPosition, originalPos, aimSpeed * Time.deltaTime * 5f);
            currentWeaponParent.localRotation = Quaternion.Lerp(currentWeaponParent.localRotation, originalRot, aimSpeed * Time.deltaTime * 5f);
            camera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(camera.GetComponent<Camera>().fieldOfView, originalFOV, aimSpeed * Time.deltaTime * 5f);
            crosshair.SetActive(true);
        }
        animationController.setBool(Aim_ID, isAim);
        
    }
    public void WeaponDown()
    {

    }

    public void setSoundEffects(AudioClip soundEffect)
    {
        audioSource.clip = soundEffect;
        audioSource.Play();
    }
}
