using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
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
        [SerializeField] private Image _winIcon;
        [SerializeField] private Sprite[] _winSprite;
        [SerializeField] private TMP_Text _scoreResult1;
        [SerializeField] private TMP_Text _scoreResult2;
        [SerializeField] private TMP_Text _winResult;
        [SerializeField] private TMP_FontAsset _p1Font;
        [SerializeField] private TMP_FontAsset _p2Font;
        [SerializeField] private TMP_FontAsset _drawFont;

        [Header("TUTORIAL")]
        [SerializeField] private GameObject _player1Tutorial;
        [SerializeField] private GameObject _player2Tutorial;
        [SerializeField] private TMP_Text _player1ShootButton;
        [SerializeField] private TMP_Text _player2ShootButton;

        [SerializeField] Gun gun1;
        [SerializeField] Gun gun2;

        [Header("SOUND EFFECT")]
        [SerializeField] private AudioSource _notificationSound;
        [SerializeField] private AudioSource _completeSound;

        [SerializeField] private CanvasGroup _background;
        private PostProcessVolume _postProcess;
        DepthOfField depthOfField;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this.gameObject);
        }

        private void Start()
        {
            _player1ShootButton.text = KeyboardSetting.p1Key.ToString();
            _player2ShootButton.text = KeyboardSetting.p2Key.ToString();

            _postProcess = FindObjectOfType<PostProcessVolume>();
            _postProcess.profile.TryGetSettings(out depthOfField);
        }

        public void OnTutorialClose()
        {
            _player1Tutorial.LeanScale(Vector3.zero, 0.4f).setEaseInBack();
            _player2Tutorial.LeanScale(Vector3.zero, 0.4f).setEaseInBack();
        }

        public void OnNextWave()
        {
            _enemyCount[GameManager.instance.index - 1].sprite = _enemySprite[3];
        }

        public void OnNextRound()
        {
            _roundText.text = "R : " + GameManager.instance.round + "/10";

            for (int i = 0; i < _enemyCount.Length; i++)
            {
                _enemyCount[i].sprite = _enemySprite[0];
            }
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

        public void OnScore(int _player, int index)
        {
            _scoreText1.text = GameManager.instance.score1.ToString();
            _scoreText2.text = GameManager.instance.score2.ToString();

            _enemyCount[index - 1].sprite = _enemySprite[_player];
        }

        public void OnFlyAway(int index) => _enemyCount[index - 1].sprite = _enemySprite[4];

        public void OnNotification(string _text)
        {
            _notificationText.text = _text;

            _notificationBox.transform.localScale = Vector3.zero;
            _notificationBox.LeanScale(Vector3.one, 0.2f).setEaseInSine();
            _notificationSound.Play();
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
            _scoreResult1.text = GameManager.instance.score1.ToString();
            _scoreResult2.text = GameManager.instance.score2.ToString();
            
            if (GameManager.instance.score1 == GameManager.instance.score2)
            {
                _winResult.text = "DRAW";
                _winResult.font = _drawFont;
                _winIcon.sprite = _winSprite[0];
            }
            else if (GameManager.instance.score1 > GameManager.instance.score2)
            {
                _winResult.text = "PLAYER 1 WIN";
                _winResult.font = _p1Font;
                _winIcon.sprite = _winSprite[1];
            }
            else
            {
                _winResult.text = "PLAYER 2 WIN";
                _winResult.font = _p2Font;
                _winIcon.sprite = _winSprite[2];
            }

            _resultPanel.LeanScale(Vector3.one, 0.7f).setEaseOutQuart();
            _background.LeanAlpha(1, 0.7f);
            depthOfField.active = true;
            _completeSound.Play();
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
