using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    public float fadeSpeed = .05f;
    public ObstacleType GetObstacleType()
    {
        return type;
    }

    public void Dispose(bool noFade = false)
    {
        //Disable collider
        //Don't allow recalling this method
        if (noFade)
        {
            Destroy(gameObject);
        }
        else
        {
            //Fade out, then dispose
            StartCoroutine(nameof(FadeOut));
            
            
        }
    }

    private IEnumerator FadeOut()
    {
        Renderer r = GetComponent<Renderer>();
        Color c = r.material.color;
        if (c != null)
            //float a = c.a;
            while (r.material.color.a >= 0)
            {
                //GetComponent<Renderer>().material.color -= (fadeSpeed * Time.deltaTime);
                Debug.Log("alpha = " + r.material.color.a);
                yield return null;
            }
        Destroy(gameObject);
    }
}
