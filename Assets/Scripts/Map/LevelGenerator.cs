using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Level")]
    [SerializeField]
    private GameObject[] _chunks;
    private Rigidbody[] _chunksRigidbody;
    [SerializeField]
    private float _chunkLength = 100.0f;
    private int _firstChunkIndex;

    [Header("Movement")]
    [SerializeField]
    private float _speed = 5.0f;
    private bool _scrolling = false;

    private void Awake()
    {
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

        if (generateForFirstChunk)
        {
            startingIndex = 1;
        }

        for(int i = startingIndex; i < _chunks.Length; i++)
        {
            GenerateObstacles(i);
        }
    }

    private void GenerateObstacles(int chunkIndex)
    {
        // TODO: Generate obstacles
        Debug.Log("GenerateObstacles " + chunkIndex);
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
                GenerateObstacles(i);

                _firstChunkIndex = (_firstChunkIndex + 1) % _chunks.Length;
            }
            else
            {
                // nextPosition is in local space, therefore must be converted in global space
                _chunksRigidbody[i].MovePosition(transform.position + nextPosition);
            }
        }
    }
}
