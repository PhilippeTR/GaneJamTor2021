using System;
using UnityEngine;

[Serializable]
public struct ObstacleSettings
{
    public GameObject Obstacle;
    public string Name;
    public float width;
    public bool InitiallyAvailable;
}

[CreateAssetMenu(fileName = "GenerationSettings", menuName = "GenerationSettings")]
public class GenerationSettings : ScriptableObject
{
    [SerializeField]
    private ObstacleSettings[] _obstacleSettings;
    [SerializeField]
    private float _minObstaclesDistance;
    [SerializeField]
    private float _maxObstaclesDistance;

    public ObstacleSettings[] GetObstacleSettings()
    {
        return _obstacleSettings;
    }

    public float GetMinObstaclesDistance()
    {
        return _minObstaclesDistance;
    }

    public float GetMaxObstaclesDistance()
    {
        return _maxObstaclesDistance;
    }
}
