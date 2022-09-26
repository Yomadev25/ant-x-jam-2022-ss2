using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;

    [Header("SPAWN AREA")]
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;

    void Start()
    {
        GameManager.instance.onNextWave += Spawn;
    }

    public void Spawn()
    {
        Instantiate(_enemyPrefab, new Vector2(Random.Range(_minX, _maxX), 0f), Quaternion.identity);
    }
}
