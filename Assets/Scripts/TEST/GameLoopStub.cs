using UnityEngine;

public class GameLoopStub : MonoBehaviour
{
    [SerializeField]
    private LevelGenerator _levelGenerator;

    public void StartGame()
    {
        _levelGenerator.GenerateObstacles();
        _levelGenerator.Scroll(true);
    }

    public void StopOnDeath()
    {
        _levelGenerator.Scroll(false);
        _levelGenerator.DeleteAllObstacles();
    }

    public void NextRun()
    {
        _levelGenerator.ResetLevel();
        StartGame();
    }
}
