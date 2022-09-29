using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace Multiplayer.Local
{
    public class UserInterface : MonoBehaviour
    {
        public static UserInterface instance;

        [Header("USER INTERFACE")]
        [SerializeField] private Image[] _ammoIcon1;
        [SerializeField] private Image[] _ammoIcon2;
        [SerializeField] private Sprite[] _ammoSprite;
        [SerializeField] private TMP_Text _scoreText1;
        [SerializeField] private TMP_Text _scoreText2;
        [SerializeField] private Image[] _enemyCount;
        [SerializeField] private Sprite[] _enemySprite;
        [SerializeField] private TMP_Text _roundText;
        [SerializeField] private GameObject _notificationBox;
        [SerializeField] private TMP_Text _notificationText;

        [Header("RESULT BOARD")]
        [SerializeField] private GameObject _resultPanel;
        [SerializeField] private TMP_Text _scoreResult;
        [SerializeField] private TMP_Text _roundResult;

        [SerializeField] Gun gun1;
        [SerializeField] Gun gun2;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this.gameObject);
        }

        public void OnNextWave()
        {
            //_enemyCount[GameManager.instance.index].sprite = _enemySprite[1];
        }

        public void OnNextRound()
        {
            _roundText.text = "R : " + GameManager.instance.round + "/10";

            /*for (int i = 0; i < _enemyCount.Length; i++)
            {
                _enemyCount[i].sprite = _enemySprite[0];
            }*/
        }

        public void OnShoot()
        {
            for (int i = 0; i < _ammoIcon1.Length; i++)
            {
                if (i < gun1.ammo)
                    _ammoIcon1[i].sprite = _ammoSprite[1];
                else
                    _ammoIcon1[i].sprite = _ammoSprite[0];
            }

            for (int i = 0; i < _ammoIcon2.Length; i++)
            {
                if (i < gun2.ammo)
                    _ammoIcon2[i].sprite = _ammoSprite[1];
                else
                    _ammoIcon2[i].sprite = _ammoSprite[0];
            }
        }

        public void OnScore()
        {
            _scoreText1.text = GameManager.instance.score1.ToString();
            _scoreText2.text = GameManager.instance.score2.ToString();

            //_enemyCount[GameManager.instance.index].sprite = _enemySprite[3];
        }

        //public void OnFlyAway() => _enemyCount[GameManager.instance.index].sprite = _enemySprite[2];

        public void OnNotification(string _text)
        {
            _notificationText.text = _text;

            _notificationBox.transform.localScale = Vector3.zero;
            _notificationBox.LeanScale(Vector3.one, 0.2f).setEaseInSine();
        }

        public void OnCloseNotification()
        {
            _notificationBox.LeanScale(Vector3.zero, 0.2f).setEaseOutSine();
        }

        public void OnRoundEnd()
        {
            /*for (int i = 0; i < _enemyCount.Length; i++)
            {
                _enemyCount[i].sprite = _enemySprite[2];
            }
            for (int i = 0; i < GameManager.instance.count; i++)
            {
                if (i < GameManager.instance.count)
                    _enemyCount[i].sprite = _enemySprite[3];
            }*/
        }

        public void OnResult()
        {
            /*_scoreResult.text = GameManager.instance.score.ToString();
            _roundResult.text = GameManager.instance.round.ToString();

            _resultPanel.LeanScale(Vector3.one, 0.7f).setEaseInBack();*/
        }

        public void OnRestart()
        {
            Transition.instance.Goto(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
        }

        public void OnMenu()
        {
            Transition.instance.Goto(() => SceneManager.LoadScene("Mainmenu"));
        }
    }
}
