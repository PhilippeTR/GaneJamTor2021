using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScore : MonoBehaviour
{
    ObstacleParent parent;
    private void Start()
    {
        parent = GetComponentInParent<ObstacleParent>();
    }
    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            if (parent)
            {
                parent.ObstaclePassed();
            }
        }
    }

}
