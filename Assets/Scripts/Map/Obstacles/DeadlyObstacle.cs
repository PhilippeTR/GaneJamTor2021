using UnityEngine;

public class DeadlyObstacle : ObstacleParent
{
    /*void Awake()
    {
        type = ObstacleType.Deadly;
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Kill the player");
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth)
            {
                playerHealth.ModifyHealth(-1);
            }
                
        }
    }
}
