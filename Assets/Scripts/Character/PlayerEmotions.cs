using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Emotion
{
    None,
    Sadness,
    Happiness,
    Anger,
    Fear
}
public class PlayerEmotions : MonoBehaviour
{
    [SerializeField]
    private Emotion currentEmotion = Emotion.None;

    private CharacterMovement cm;

    public Animator animator;
    private bool hasPunchPower = false;
    private bool canPunch = true;
    public SpriteRenderer punchSprite;

    public CharacterHeads heads;
    public SpriteRenderer headSprite;

    public float speedMultiplier = 1.5f;

    public Vector3 sphereOverlapOffset;
    public float sphereOverlapRadius = .8f;
    public LayerMask layerMask;

    private void Start()
    {
        cm = GetComponent<CharacterMovement>();
        SetPower();
        punchSprite.enabled = CanUsePunch();
    }

    public void UpdateWithInput(Inputs inputs)
    {
        if(inputs.Punch)
        {
            if (CanUsePunch())
            {
                animator.SetTrigger("Punch");
                Collider[] hitColliders = Physics.OverlapSphere(transform.position + sphereOverlapOffset, sphereOverlapRadius, layerMask);
                
                foreach (var hitCollider in hitColliders)
                {
                    ObstacleParent o = hitCollider.GetComponent<ObstacleParent>();
                    if (o)
                    {
                        o.Dispose(false);
                    }
                    //Debug.Log(hitCollider.transform.position);
                    break;
                }
                canPunch = false;
                punchSprite.enabled = CanUsePunch();
                _ = StartCoroutine(nameof(StartPunchCooldown));
            }
        }
       
    }

    IEnumerator StartPunchCooldown()
    {
        yield return new WaitForSeconds(5.0f);
        canPunch = true;
        punchSprite.enabled = CanUsePunch();
    }

    public Emotion GetEmotion()
    {
        return currentEmotion;
    }

    public void SetEmotion(Emotion emotion)
    {
        ResetEmotionPower();
        currentEmotion = emotion;
        SetPower();
    }

    private void SetPower()
    {
        switch (currentEmotion)
        {
            case Emotion.Happiness:
                cm.AllowDoubleJump(true);
                headSprite.sprite = heads.happiness;
                headSprite.color = Color.green;
                break;
            case Emotion.Anger:
                hasPunchPower = true;
                headSprite.sprite = heads.anger;
                headSprite.color = Color.red;
                break;
            case Emotion.Fear:          //Move faster
                cm.SetSpeed(cm.GetSpeed() * speedMultiplier);
                headSprite.sprite = heads.fear;
                headSprite.color = Color.magenta;
                
                break;
            case Emotion.Sadness:       //Second life
                GetComponent<Health>().ModifyHealth(2);
                headSprite.sprite = heads.happiness;
                headSprite.color = Color.blue;
                break;
            case Emotion.None:
                headSprite.sprite = heads.emotionless;
                headSprite.color = Color.white;
                break;
            default:
                break;
        }

    }

    private void ResetEmotionPower()
    {
        switch (currentEmotion)
        {
            case Emotion.Happiness:
                cm.AllowDoubleJump(false);
                break;
            case Emotion.Anger:
                hasPunchPower = false;
                break;
            case Emotion.Fear:
                cm.SetSpeed(cm.GetSpeed() / speedMultiplier);
                //Can't run
                break;
            case Emotion.Sadness:
                //No shield(Sadness power)
                break;
            default:
                break;
        }
        
    }

    private bool CanUsePunch()
    {
        return hasPunchPower && canPunch;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + sphereOverlapOffset, sphereOverlapRadius);
    }
}
