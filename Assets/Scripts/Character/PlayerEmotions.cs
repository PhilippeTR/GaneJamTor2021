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
    private Emotion currentEmotion = Emotion.None;

    public Emotion GetEmotion()
    {
        return currentEmotion;
    }

    public void SetEmotion(Emotion emotion)
    {
        currentEmotion = emotion;
    }

}
