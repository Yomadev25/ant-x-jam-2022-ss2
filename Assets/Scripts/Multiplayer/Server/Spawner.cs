using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Multiplayer.Online
{
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
            PhotonNetwork.Instantiate(_enemyPrefab.name, new Vector2(Random.Range(_minX, _maxX), 0f), Quaternion.identity);
        }
    }
}
