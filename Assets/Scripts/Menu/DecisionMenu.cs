using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DecisionMenu : MonoBehaviour, IHealthSubscriber
{
    [SerializeField]
    private List<Emotion> emotion;

    [SerializeField]
    private List<ObstacleType> obstacles;

    [SerializeField]
    private GameObject backgroundImg;

    [SerializeField]
    private GameObject PopUp;
    [SerializeField]
    private GameObject PopUpDeath;

    [SerializeField]
    private Text PopUpDeathText;

    [SerializeField]
    private Button restartButton;

    [SerializeField]
    private LevelGenerator levelGen;

    [SerializeField]
    private PlayerEmotions pPlayerEmote;

    [SerializeField]
    public int scoreMin;

    [SerializeField]
    private Health health;

    [SerializeField]
    private Image [] images;

    [SerializeField]
    private Sprite imagesDefault;

    [SerializeField]
    private ScoreManager scoreMan;

    private bool isFinish;
    private Emotion emoteSelected;

   // Start is called before the first frame update
   void Start()
    {
        PopUpDeath.SetActive(false);
        PopUp.SetActive(false);
        backgroundImg.SetActive(false);
        emoteSelected = Emotion.None;
        levelGen.GenerateObstacles();
        levelGen.Scroll(true);
        health.Subscribe(this);
        isFinish = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (emoteSelected == Emotion.None)
        {
            restartButton.enabled = false;
        }
        else
        {
            restartButton.enabled = true;
        }
    }
    private void ActivateDeath()
    {
        //TODO play BOOOO 
        PopUpDeath.SetActive(true);
        backgroundImg.SetActive(true);
        PopUpDeathText.text = "" + scoreMan.GetScore() + " Points!";
    }
    public void ActivateReborn() {
        //TODO play Hourra
        PopUp.SetActive(true);
        backgroundImg.SetActive(true);
        emoteSelected = Emotion.None;
        isFinish = true;
        for (int i = 0; i < 3; i++)
        {
            images[i].sprite = imagesDefault;
        }
    }

    public void SelectEmotion(int a)
    {
        if(isFinish)
            SelectEmotion((Emotion)a);
    }

    public void SelectEmotion(Emotion em) {
        Debug.Log(em);
        emoteSelected = em;
    }

    public void ResetGame()
	{
        isFinish = false;
        StartCoroutine(WaitAndPrint());
    }

    IEnumerator WaitAndPrint()
    {
        IEnumerable<ObstacleSettings> obstacleDif = levelGen.GetObstaclesAvailable().Except(levelGen.GetObstaclesUsed());
        int[] obstableAdded = {-1,-1,-1 };
        int maxSize = (3 > obstacleDif.Count()) ? obstacleDif.Count() : 3 ;
        for (int i = 0; i < maxSize; i++) {
            int numb = Random.Range(0, obstacleDif.Count());
            if (!obstableAdded.Contains(numb))
            {
                obstableAdded[i] = numb;
                levelGen.AddObstaclesUsed(obstacleDif.ElementAt(numb));
                images[i].sprite = obstacleDif.ElementAt(numb).MenuSprite;
            }
            else 
            {
                i--;
            }
        }
        pPlayerEmote.SetEmotion(emoteSelected);
        // suspend execution for 5 seconds
        yield return new WaitForSeconds(5);
        PopUp.SetActive(false);
        backgroundImg.SetActive(false);
        health.ModifyHealth(1);
        levelGen.ResetLevel();
        levelGen.GenerateObstacles();
        levelGen.Scroll(true);
        scoreMan.score = 0;
    }

	public void NotifyHealthChange(Health healthScript, int health)
	{

	}

	public void NotifyHealthDepleted(Health healthScript)
	{
        int score = scoreMan.GetScore();
        if (score < scoreMin)
        {
            isFinish = true;
            levelGen.Scroll(false);
            levelGen.DeleteAllObstacles();
            ActivateDeath();
        }
        else {
            isFinish = true;
            levelGen.Scroll(false);
            levelGen.DeleteAllObstacles();
            ActivateReborn();
        }

    }


}
