using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;

    [Header("SPAWN AREA")]
    [SerializeField] private float _minX;
    [SerializeField] private float _minY;

    void Awake()
    {
        GameManager.instance.onNextWave += Spawn;
    }

    public void Spawn()
    {
        Instantiate(_enemyPrefab, new Vector2(Random.Range(_minX, _minY), 0f), Quaternion.identity);
    }
}
