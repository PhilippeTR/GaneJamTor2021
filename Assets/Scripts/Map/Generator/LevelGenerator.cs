using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    private Hashtable _obstaclesUsedNEW = new Hashtable();
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
        foreach (int i in Enum.GetValues(typeof(ObstacleType)))
        {
            _obstaclesUsedNEW[i] = new List<ObstacleSettings>();
        }

        int obstacleType;

        foreach (ObstacleSettings obstacleSetting in _generationSettings.GetObstacleSettings())
        {
            if (obstacleSetting.InitiallyAvailable)
            {
                obstacleType = (int)obstacleSetting.Obstacle.GetComponent<ObstacleParent>().GetObstacleType();
                ((List<ObstacleSettings>)_obstaclesUsedNEW[obstacleType]).Add(obstacleSetting);
            }
        }
        Debug.Log("DONE");
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
        int randomObstacleSettingIndex;
        Transform chunk;
        List<ObstacleSettings> obstacleSettings;
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

            // Choose a random obstacle type
            int obstacleType = Random.Range(0, _obstaclesUsedNEW.Count);
            obstacleSettings = (List<ObstacleSettings>)_obstaclesUsedNEW[obstacleType];

            // Choose a random obstacle
            randomObstacleSettingIndex = Random.Range(0, obstacleSettings.Count);

            chunk = _chunks[chunkIndex].transform;

            obstacle = Instantiate(obstacleSettings[randomObstacleSettingIndex].Obstacle, chunk.position, chunk.rotation, chunk);
            obstacle.GetComponent<ObstacleParent>().SetScoreManager(_scoreManager);
            obstacle.transform.localPosition = transform.right * (lastPosition - _chunkLength / 2.0f/* + obstacleSettings[randomObstacleSettingIndex].width / 2.0f*/) + Vector3.up * _groundOffset;

            //lastPosition += obstacleSettings[randomObstacleSettingIndex].width;
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
        List<ObstacleSettings> obstacleUsed = new List<ObstacleSettings>();

        foreach (int i in Enum.GetValues(typeof(ObstacleType)))
        {
            foreach (ObstacleSettings obstacleSetting in (List<ObstacleSettings>)_obstaclesUsedNEW[i])
            {
                obstacleUsed.Add(obstacleSetting);
            }
        }

        return obstacleUsed.ToArray();
    }

    public void AddObstaclesUsed(ObstacleSettings obstacleSetting)
    {
        int obstacleType = (int)obstacleSetting.Obstacle.GetComponent<ObstacleParent>().GetObstacleType();
        List<ObstacleSettings> obstacleSettings = (List<ObstacleSettings>)_obstaclesUsedNEW[obstacleType];

        if (obstacleSettings.IndexOf(obstacleSetting) <= -1)
        {
            obstacleSettings.Add(obstacleSetting);
            _obstaclesUsedNEW[obstacleType] = obstacleSettings;
        }
    }

    public void RemoveObstaclesUsed(ObstacleSettings obstacleSetting)
    {
        int obstacleType = (int)obstacleSetting.Obstacle.GetComponent<ObstacleParent>().GetObstacleType();
        List<ObstacleSettings> obstacleSettings = (List<ObstacleSettings>)_obstaclesUsedNEW[obstacleType];

        if (obstacleSettings.IndexOf(obstacleSetting) > -1)
        {
            obstacleSettings.Remove(obstacleSetting);
            _obstaclesUsedNEW[obstacleType] = obstacleSettings;
        }
    }
}
