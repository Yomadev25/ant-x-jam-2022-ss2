using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer.Local
{
    public class GameManager : MonoBehaviour
    {
        public static Multiplayer.Local.GameManager instance;

        public event Action onRoundStart, onNextWave, onRoundEnd, onNextRound, onGameover;

        public float score1;
        public float score2;

        int totalCount = 10;
        [HideInInspector] public int index = 0;
        [HideInInspector] public int round = 1;
        [HideInInspector] public int enemyCount;

        [SerializeField] private Gun gun1;
        [SerializeField] private Gun gun2;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this.gameObject);
        }

        IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);
            UserInterface.instance.OnNotification("ROUND\n" + round.ToString());
            yield return new WaitForSeconds(2f);
            UserInterface.instance.OnCloseNotification();

            RoundStart();
        }

        public void GetScore(int _player, int _enemy, float _score)
        {
            if (_player == 1) score1 += _score;
            else score2 += _score;

            UserInterface.instance.OnScore(_player, _enemy);
        }

        #region ROUND MANAGER
        public void RoundStart()
        {
            Invoke("NextWave", 1f);
        }

        public void EnemyCheck()
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

        public void NextWave()
        {
            gun1.ammo = 3;
            gun2.ammo = 3;
            gun1._reloadSound.Play();

            UserInterface.instance.OnShoot();           

            int count = UnityEngine.Random.Range(1, 5);
            enemyCount = 0;
            for (int i = 0; i < count; i++)
            {               
                if (index >= totalCount) break;
                enemyCount++;
                index++;
                onNextWave?.Invoke();
                UserInterface.instance.OnNextWave();
            }
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
            index = 0;
            round++;
            UserInterface.instance.OnNextRound();
            UserInterface.instance.OnNotification("ROUND\n" + round.ToString());

            Invoke("RoundStart", 2f);
            UserInterface.instance.Invoke("OnCloseNotification", 2f);
        }

        public void Gameover()
        {
            UserInterface.instance.OnNotification("GAME OVER");
            UserInterface.instance.Invoke("OnResult", 1.5f);
        }
        #endregion
    }
}
