using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMenu : MonoBehaviour
{
    public ScoreManager scoreMan;
    public DecisionMenu decMenu;
    public Text score;
    public Text scoreMin;
    // Start is called before the first frame update
    void Start()
    {
        scoreMin.text = "" + decMenu.scoreMin;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        score.text = "" + scoreMan.GetScore();
    }
}
