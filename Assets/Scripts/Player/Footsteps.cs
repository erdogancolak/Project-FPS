using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Footsteps: MonoBehaviour
{
    [Header("CharacterController")]
    [SerializeField] CharacterController charController;

    [Header("FootSteps")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip footstepClip;

    [Header("FootStep Settings")]
    [SerializeField] float footstepDelay;
    bool isWalking;
    
    void Update()
    {
        setFootStep();
    }

    void setFootStep()
    {
        //if (PlayerMovement.Instance.isWalking && charController.isGrounded)
        //{
        //    StartCoroutine(PlayFootsteps());
        //}
        //else if (!PlayerMovement.Instance.isWalking)
        //{
        //    StopCoroutine(PlayFootsteps());
        //}
        if (charController.isGrounded && charController.velocity.magnitude > 0.1f)
        {
            if (!isWalking)
            {
                isWalking = true;
                StartCoroutine(PlayFootsteps());
            }
        }
        else
        {
            isWalking = false;
            StopCoroutine(PlayFootsteps());
        }
    }

    IEnumerator PlayFootsteps()
    {

        if (footstepClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(footstepClip);
        }
        yield return new WaitForSeconds(footstepDelay);
    }
}
