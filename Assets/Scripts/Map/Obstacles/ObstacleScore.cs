using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScore : MonoBehaviour
{
    ObstacleParent parent;
    bool triggered = false;

    private void Start()
    {
        parent = GetComponentInParent<ObstacleParent>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            if (parent)
            {
                parent.ObstaclePassed();
                triggered = true;
            }
        }
    }

}
