using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterHeads", order = 1)]
public class CharacterHeads : ScriptableObject
{
    public Sprite emotionless;
    public Sprite anger;
    public Sprite happiness;
    public Sprite sadness;
    public Sprite fear;

}
