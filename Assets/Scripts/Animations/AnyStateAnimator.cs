using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;

public delegate void AnimationTriggerEvent(string animation);

public class AnyStateAnimator : MonoBehaviourPun
{
    private Animator animator;

    private Dictionary<string, AnyStateAnimation> animations = new Dictionary<string, AnyStateAnimation>();

    public AnimationTriggerEvent AnimationTriggerEvent { get; set; }

    private string currentAnimationLegs = string.Empty;

    private string currentAnimationBody = string.Empty;

    private void Awake()
    {
        this.animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        Animate();
    }

    public void AddAnimations(params AnyStateAnimation[] newAnimations)
    {
        for (int i = 0; i < newAnimations.Length; i++)
        {
            this.animations.Add(newAnimations[i].Name, newAnimations[i]);
        }
    }

    public void TryPlayAnimation(string newAnimation)
    {
        switch (animations[newAnimation].AnimationRig)
        {
            case RIG.BODY:
                PlayAnimation(ref currentAnimationBody);
                break;
            case RIG.LEGS:
                PlayAnimation(ref currentAnimationLegs);
                break;
        }

        void PlayAnimation(ref string currentAnimation)
        {
            if(currentAnimation == "")
            {
                animations[newAnimation].Active = true;
                currentAnimation = newAnimation;
            }
            else if(
                currentAnimation != newAnimation
                && !animations[newAnimation].HigherPriority.Contains(currentAnimation)
                || !animations[currentAnimation].Active
            )
            {
                animations[currentAnimation].Active = false;
                animations[newAnimation].Active = true;
                currentAnimation = newAnimation;
            }
        }
    }

    public void SetWeapon(float weapon)
    {
        animator.SetFloat("Weapon", weapon);
    }

    private void Animate()
    {
        foreach (string key in animations.Keys)
        {
            animator.SetBool(key, animations[key].Active);
        }
    }

    public void OnAnimationDone(string animation)
    {
        animations[animation].Active = false;
    }

    public void OnAnimationTrigger(string animation)
    {
        if(AnimationTriggerEvent != null)
        {
            AnimationTriggerEvent.Invoke(animation);
        }
    }
}
