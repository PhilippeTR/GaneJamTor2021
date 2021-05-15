using UnityEngine;

public enum ObstacleType
{
    Neutral,
    Deadly,
    Slowing
}

public abstract class ObstacleParent : MonoBehaviour
{
    protected ObstacleType type = ObstacleType.Neutral;
 
    public ObstacleType GetObstacleType()
    {
        return type;
    }
}
