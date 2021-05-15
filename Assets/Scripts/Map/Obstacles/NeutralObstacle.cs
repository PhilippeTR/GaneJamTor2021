using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralObstacle : ObstacleParent
{
    
    void Awake()
    {
        type = ObstacleType.Neutral;
    }

}
