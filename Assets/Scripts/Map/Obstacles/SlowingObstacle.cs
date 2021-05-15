using UnityEngine;

public class SlowingObstacle : ObstacleParent
{
    void Awake()
    {
        type = ObstacleType.Slowing;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Slow the player");
            //other.GetComponent<CharacterMovement>();
        }
    }
}
