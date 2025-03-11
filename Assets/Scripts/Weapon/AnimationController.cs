using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    Animator animator;

    public AudioClip weaponReadySound;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void setBool(string AnimationID,bool AnimationBool)
    {
        animator.SetBool(AnimationID, AnimationBool);
    }
    public void setTrigger(string AnimationID)
    {
        animator.SetTrigger(AnimationID);
    }
    void EndFire()
    {
        WeaponManager.Instance.EndFire();
    }
    void EndReload()
    {
        WeaponManager.Instance.EndReload();
    }
    void WeaponDown()
    {
        WeaponManager.Instance.WeaponDown();
    }
    public void setAvailability(int index)
    {
        WeaponManager.Instance.Availability = index == 0 ? false : true;
    }

    void SetSound()
    {
        WeaponManager.Instance.setSoundEffects(weaponReadySound);
    }
}
