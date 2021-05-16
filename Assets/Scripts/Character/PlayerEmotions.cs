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
    private bool hasPunchPower = false;
    private bool canPunch = true;

    public float speedMultiplier = 1.5f;
    public float sphereOverlapRadius = .8f;
    public LayerMask layerMask;

    private void Start()
    {
        cm = GetComponent<CharacterMovement>();
        SetPower();
    }

    public void UpdateWithInput(Inputs inputs)
    {
        if(inputs.Punch)
        {
            if (hasPunchPower && canPunch)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position+ Vector3.right/2, sphereOverlapRadius, layerMask);
                
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
                _ = StartCoroutine(nameof(StartPunchCooldown));
            }
        }
       
    }

    IEnumerator StartPunchCooldown()
    {
        yield return new WaitForSeconds(5.0f);
        canPunch = true;
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
                break;
            case Emotion.Anger:
                hasPunchPower = true;
                break;
            case Emotion.Fear:          //Move faster
                cm.SetSpeed(cm.GetSpeed() * speedMultiplier);
                break;
            case Emotion.Sadness:       //Second life
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + Vector3.right/2, sphereOverlapRadius);
    }
}
