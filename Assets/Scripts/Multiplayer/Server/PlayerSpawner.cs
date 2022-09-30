using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Multiplayer.Online
{
    public class PlayerSpawner : MonoBehaviourPun
    {
        Vector3 spawnPos = new Vector3(0, 1, -1.14f);

        void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate("Player1", spawnPos, Quaternion.identity);
            }
            else
            {
                PhotonNetwork.Instantiate("Player2", spawnPos, Quaternion.identity);
            }
        }
    }
}
