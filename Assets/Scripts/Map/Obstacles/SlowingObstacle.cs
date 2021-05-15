using UnityEngine;

public class SlowingObstacle : ObstacleParent
{
    public float obstacleSpeed = 300f;
    private float beforeSpeed = 0f;
    void Awake()
    {
        type = ObstacleType.Slowing;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Slow the player");
            CharacterMovement cm = other.GetComponent<CharacterMovement>();
            if (cm)
            {
                beforeSpeed = cm.GetSpeed();
                cm.SetSpeed(obstacleSpeed);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(beforeSpeed != 0f)
            {
                Debug.Log("Give player normal speed");
                CharacterMovement cm = other.GetComponent<CharacterMovement>();
                if (cm)
                {
                    cm.SetSpeed(beforeSpeed);
                    beforeSpeed = 0f;
                }
            }
        }
    }
}
