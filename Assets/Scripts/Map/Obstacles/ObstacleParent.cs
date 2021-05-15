using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ObstacleType
{
    Neutral,
    Deadly,
    Slowing
}
public class ObstacleParent : MonoBehaviour
{
    protected ObstacleType type = ObstacleType.Neutral;
 
    public ObstacleType GetObstacleType()
    {
        return type;
    }

    private void Update()
    {
        //transform.position = transform.position + new Vector3(-5, 0, 0) * Time.deltaTime;
    }
}
