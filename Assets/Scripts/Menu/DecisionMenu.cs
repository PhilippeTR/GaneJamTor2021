using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecisionMenu : MonoBehaviour
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
    private Button restartButton;

    [SerializeField]
    private LevelGenerator levelGen;

    private Emotion emoteSelected;
   // Start is called before the first frame update
   void Start()
    {
        PopUp.SetActive(false);
        backgroundImg.SetActive(false);
        emoteSelected = Emotion.None;
        levelGen.GenerateObstacles();
        levelGen.Scroll(true);
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

    void ActivateReborn() {
        PopUp.SetActive(true);
        backgroundImg.SetActive(true);
        emoteSelected = Emotion.None;
    }

    void SelectEmotion(Emotion em) {
        emoteSelected = em;
    }

	private void ResetGame()
	{
        StartCoroutine(WaitAndPrint());
    }

    IEnumerator WaitAndPrint()
    {
        // suspend execution for 5 seconds
        yield return new WaitForSeconds(5);
        levelGen.ResetLevel();
        levelGen.GenerateObstacles();
        levelGen.Scroll(true);
    }

}
