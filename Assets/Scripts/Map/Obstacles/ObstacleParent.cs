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
    public SpriteRenderer spriteRenderer;
    public float fadeSpeed = 1;
    public int scoreIncrement = 10;
    private bool DisposeTriggered = false;

    private ScoreManager scoreManager;
    public ObstacleType GetObstacleType()
    {
        return type;
    }

    public void Dispose(bool noFade = false)
    {
        if (DisposeTriggered)
        {
            return;
        }

        DisposeTriggered = true;

        //Disable collider
        Collider c = GetComponent<Collider>();
        if (c)
        {
            c.enabled = false;
        }
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
        //Renderer r = GetComponent<Renderer>();
        Color c = spriteRenderer.material.color;
        float a;
        if (c != null)
            
        while (c.a >= 0)
        {
            c = spriteRenderer.material.color;
            a = c.a;
            //GetComponent<Renderer>().material.color -= (fadeSpeed * Time.deltaTime);

            spriteRenderer.material.color = new Color(c.r,c.g,c.b, a- fadeSpeed * Time.deltaTime);
            Debug.Log("alpha = " + spriteRenderer.material.color.a);
            yield return null;
        }

        Destroy(gameObject);
    }

    public void SetScoreManager(ScoreManager sm)
    {
        scoreManager = sm;
    }

    public void ObstaclePassed()
    {
        if (scoreManager)
        {
            scoreManager.AddScore(scoreIncrement);
        }
    }
}
