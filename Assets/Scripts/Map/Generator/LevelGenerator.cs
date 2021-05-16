using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Generation")]
    [SerializeField]
    private GenerationSettings _generationSettings;
    [SerializeField]
    private ScoreManager _scoreManager;
    [SerializeField]
    private GameObject[] _chunks;
    private Rigidbody[] _chunksRigidbody;
    [SerializeField]
    private float _chunkLength = 50.0f;
    [SerializeField]
    private float _groundOffset = 2.5f;
    private int _firstChunkIndex;
    private List<ObstacleSettings> _obstaclesUsed = new List<ObstacleSettings>();

    [Header("Movement")]
    [SerializeField]
    private float _speed = 5.0f;
    private bool _scrolling = false;

    private void Awake()
    {
        if (!_scoreManager)
        {
            Debug.LogError("No ScoreManager was given to the LevelGenerator");
        }

        // Store the rigidbody of all the chunks
        _chunksRigidbody = new Rigidbody[_chunks.Length];

        for (int i = 0; i < _chunks.Length; i++)
        {
            _chunksRigidbody[i] = _chunks[i].GetComponent<Rigidbody>();

            if (!_chunksRigidbody[i])
            {
                Debug.LogError("The chunk " + _chunks[i].name + " doesn't have a rigidbody");
            }
        }

        // Store initially available obstacles
        foreach(ObstacleSettings obstacleSetting in _generationSettings.GetObstacleSettings())
        {
            if (obstacleSetting.InitiallyAvailable)
            {
                _obstaclesUsed.Add(obstacleSetting);
            }
        }

        ResetLevel();
    }

    // Replace chunks and delete all obstacles
    public void ResetLevel()
    {
        for (int i = 0; i < _chunks.Length; i++)
        {
            _chunks[i].transform.localPosition = transform.right * _chunkLength * i;
            DeleteObstacle(i);
        }

        _firstChunkIndex = 0;
    }

    public void GenerateObstacles(bool generateForFirstChunk = false)
    {
        int startingIndex = 0;

        if (!generateForFirstChunk)
        {
            startingIndex = 1;
        }

        for(int i = startingIndex; i < _chunks.Length; i++)
        {
            GenerateObstacle(i);
        }
    }

    private void GenerateObstacle(int chunkIndex)
    {
        float lastPosition = .0f;
        GameObject obstacle;

        while (_chunkLength - lastPosition >= _generationSettings.GetMinObstaclesDistance())
        {
            if (_chunkLength - lastPosition < _generationSettings.GetMaxObstaclesDistance())
            {
                lastPosition = _chunkLength;
            }
            else
            {
                lastPosition += Random.Range(_generationSettings.GetMinObstaclesDistance(), _generationSettings.GetMaxObstaclesDistance());
            }
            
            obstacle = Instantiate(_obstaclesUsed[Random.Range(0, _obstaclesUsed.Count - 1)].Obstacle, _chunks[chunkIndex].transform.position, _chunks[chunkIndex].transform.rotation, _chunks[chunkIndex].transform);
            obstacle.GetComponent<ObstacleParent>().SetScoreManager(_scoreManager);
            obstacle.transform.localPosition = transform.right * (lastPosition - _chunkLength / 2.0f) + Vector3.up * _groundOffset;
        }
    }

    public void DeleteAllObstacles()
    {
        for (int i = 0; i < _chunks.Length; i++)
        {
            DeleteObstacle(i);
        }
    }

    private void DeleteObstacle(int chunkIndex)
    {
        if (-1 >= chunkIndex && chunkIndex >= _chunks.Length)
        {
            return;
        }

        ObstacleParent[] obstacles = _chunks[chunkIndex].GetComponentsInChildren<ObstacleParent>();

        foreach (ObstacleParent obstacle in obstacles)
        {
            obstacle.Dispose();
        }
    }

    public void Scroll(bool scroll)
    {
        _scrolling = scroll;
    }

    private void FixedUpdate()
    {
        if (!_scrolling)
        {
            return;
        }

        Vector3 nextPosition;

        for (int i = 0; i < _chunksRigidbody.Length; i++)
        {
            nextPosition = _chunks[i].transform.localPosition - transform.right * _speed * Time.fixedDeltaTime;

            if (nextPosition.x < -_chunkLength)
            {
                _chunksRigidbody[i].transform.localPosition = transform.right * _chunkLength * 2 + transform.right * (nextPosition.x + _chunkLength);

                DeleteObstacle(i);
                GenerateObstacle(i);

                _firstChunkIndex = (_firstChunkIndex + 1) % _chunks.Length;
            }
            else
            {
                // nextPosition is in local space, therefore must be converted in global space
                _chunksRigidbody[i].MovePosition(transform.position + nextPosition);
            }
        }
    }

    // Get ALL the obstacles that could be used for generation
    public ObstacleSettings[] GetObstaclesAvailable()
    {
        return _generationSettings.GetObstacleSettings();
    }

    // Get ONLY the obstacles that are used for the generation
    public ObstacleSettings[] GetObstaclesUsed()
    {
        return _obstaclesUsed.ToArray();
    }

    public void AddObstaclesUsed(ObstacleSettings ObstacleSetting)
    {
        if (_obstaclesUsed.IndexOf(ObstacleSetting) <= -1)
        {
            _obstaclesUsed.Add(ObstacleSetting);
        }
    }

    public void RemoveObstaclesUsed(ObstacleSettings ObstacleSetting)
    {
        if (_obstaclesUsed.IndexOf(ObstacleSetting) > -1)
        {
            _obstaclesUsed.Remove(ObstacleSetting);
        }
    }
}
