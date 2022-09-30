using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

namespace Multiplayer.Online
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static Multiplayer.Online.GameManager instance;

        public event Action onRoundStart, onNextWave, onRoundEnd, onNextRound, onGameover;

        public float score1;
        public float score2;

        int totalCount = 10;
        [HideInInspector] public int index = 0;
        [HideInInspector] public int round = 1;
        [HideInInspector] public int enemyCount;

        public Gun gun1;
        public Gun gun2;

        PhotonView _photonView;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this.gameObject);
        }

        IEnumerator Start()
        {
            _photonView = GetComponent<PhotonView>();

            yield return new WaitForSeconds(1f);
            //UserInterface.instance.OnNotification("ROUND\n" + round.ToString());
            yield return new WaitForSeconds(2f);
            //UserInterface.instance.OnCloseNotification();

            if(_photonView.IsMine) RoundStart();
        }

        public void GetScore(int _player, int _enemy, float _score)
        {
            _photonView.RPC(nameof(RPCGetScore), RpcTarget.All, _player, _enemy, _score);
        }

        #region ROUND MANAGER
        public void RoundStart()
        {
            Invoke("NextWave", 1f);
        }

        public void EnemyCheck()
        {
            _photonView.RPC(nameof(RPCEnemyCheck), RpcTarget.All);
        }

        public void NextWave()
        {
            _photonView.RPC(nameof(RPCNextWave), RpcTarget.All);
        }

        public void RoundEnd()
        {
            if (round < 10)
            {
                NextRound();
            }
            else
            {
                Gameover();
            }
        }

        public void NextRound()
        {
            _photonView.RPC(nameof(RPCNextRound), RpcTarget.All);
        }

        public void Gameover()
        {
            _photonView.RPC(nameof(RPCGameover), RpcTarget.All);
        }
        #endregion

        [PunRPC]
        public void RPCGetScore(int _player, int _enemy, float _score)
        {
            if (_player == 1) score1 += _score;
            else score2 += _score;

            //UserInterface.instance.OnScore(_player, _enemy);
        }

        [PunRPC]
        public void RPCEnemyCheck()
        {
            enemyCount--;
            if (!(enemyCount <= 0)) return;

            if (index >= totalCount)
            {
                Invoke(nameof(RoundEnd), 0.5f);
            }
            else
            {
                Invoke("NextWave", 0.5f);
            }
        }

        [PunRPC]
        public void RPCNextWave()
        {
            gun1.ammo = 3;
            gun2.ammo = 3;

            //UserInterface.instance.OnShoot();

            int count = UnityEngine.Random.Range(1, 5);
            enemyCount = 0;
            for (int i = 0; i < count; i++)
            {
                if (index >= totalCount) break;
                enemyCount++;
                index++;
                onNextWave?.Invoke();
                //UserInterface.instance.OnNextWave();
            }
        }

        [PunRPC]
        public void RPCNextRound()
        {
            index = 0;
            round++;
            //UserInterface.instance.OnNextRound();
            //UserInterface.instance.OnNotification("ROUND\n" + round.ToString());

            Invoke("RoundStart", 2f);
            //UserInterface.instance.Invoke("OnCloseNotification", 2f);
        }

        [PunRPC]
        public void RPCGameover()
        {
            //UserInterface.instance.OnNotification("GAME OVER");
            //UserInterface.instance.Invoke("OnResult", 1.5f);
        }
    }
}
