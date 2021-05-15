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
    public Sprite image;
    public ObstacleType GetObstacleType()
    {
        return type;
    }

    public void Dispose(bool noFade = false)
    {
        if (noFade)
        {
            Destroy(gameObject);
        }
        else
        {
            //Fade out, then dispose
            Destroy(gameObject);
        }
    }
}
