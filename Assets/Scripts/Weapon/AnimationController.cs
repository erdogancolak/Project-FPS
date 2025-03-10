using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    Animator animator;

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
}
