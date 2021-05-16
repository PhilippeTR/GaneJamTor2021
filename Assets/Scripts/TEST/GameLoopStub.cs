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

    // Get ALL the obstacles that could be used for generation
    //ObstacleSettings[] _levelGenerator.GetObstaclesAvailable()

    // Get ONLY the obstacles that are used for the generation
    //ObstacleSettings[] _levelGenerator.GetObstaclesUsed()

    //
    //_levelGenerator.AddObstaclesUsed(ObstacleSettings ObstacleSetting)
}
