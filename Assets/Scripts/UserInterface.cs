using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public class UserInterface : MonoBehaviour
{
    public static UserInterface instance;

    [Header("USER INTERFACE")]
    [SerializeField] private Image[] _ammoIcon;
    [SerializeField] private Sprite[] _ammoSprite;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Image[] _enemyCount;
    [SerializeField] private Sprite[] _enemySprite;
    [SerializeField] private Slider _recentEnemy;
    [SerializeField] private TMP_Text _roundText;
    [SerializeField] private GameObject _notificationBox;
    [SerializeField] private TMP_Text _notificationText;
    [SerializeField] private Image _targetSlide;

    [Header("RESULT BOARD")]
    [SerializeField] private GameObject _resultPanel;
    [SerializeField] private TMP_Text _scoreResult;
    [SerializeField] private TMP_Text _roundResult;

    [Header("TUTORIAL")]
    [SerializeField] private GameObject _playerKeyboard;
    [SerializeField] private TMP_Text _playerShootButton;

    [Header("SOUND EFFECT")]
    [SerializeField] private AudioSource _notificationSound;
    [SerializeField] private AudioSource _completeSound;

    [SerializeField] private CanvasGroup _background;
    Gun gun;
    private PostProcessVolume _postProcess;
    DepthOfField depthOfField;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        gun = FindObjectOfType<Gun>();
    }

    private void Start()
    {
        _playerShootButton.text = KeyboardSetting.p1Key.ToString();
        _postProcess = FindObjectOfType<PostProcessVolume>();
        _postProcess.profile.TryGetSettings(out depthOfField);
    }

    public void OnTutorialClose()
    {
        _playerKeyboard.LeanScale(Vector3.zero, 0.4f).setEaseInBack();
    }

    public void OnNextWave()
    {
        for (int i = 0; i < _enemyCount.Length; i++)
        {
            _enemyCount[i].transform.localScale = Vector3.one;
        }
       
        _enemyCount[GameManager.instance.index].sprite = _enemySprite[1];
        _recentEnemy.value = GameManager.instance.index + 1;
    }

    public void OnNextRound()
    {
        _roundText.text = "R : " + GameManager.instance.round;

        for (int i = 0; i < _enemyCount.Length; i++)
        {
            _enemyCount[i].sprite = _enemySprite[0];
        }
        _targetSlide.fillAmount = (float)GameManager.instance.targetCount / 11f;
    }

    public void OnShoot()
    {
        for (int i = 0; i < _ammoIcon.Length; i++)
        {
            if (i < gun.ammo)
                _ammoIcon[i].sprite = _ammoSprite[1];
            else
                _ammoIcon[i].sprite = _ammoSprite[0];
        }
    }

    public void OnScore()
    {
        _scoreText.text = GameManager.instance.score.ToString();
        _enemyCount[GameManager.instance.index].sprite = _enemySprite[3];
    }

    public void OnFlyAway() => _enemyCount[GameManager.instance.index].sprite = _enemySprite[2];

    public void OnNotification(string _text)
    {
        _notificationText.text = _text;

        _notificationSound.Play();
        _notificationBox.transform.localScale = Vector3.zero;
        _notificationBox.LeanScale(Vector3.one, 0.2f).setEaseInSine();
    }

    public void OnCloseNotification()
    {
        _notificationBox.LeanScale(Vector3.zero, 0.2f).setEaseOutSine();
    }

    public void OnRoundEnd()
    {
        for (int i = 0; i < _enemyCount.Length; i++)
        {
            _enemyCount[i].sprite = _enemySprite[2];
        }
        for (int i = 0; i < GameManager.instance.count; i++)
        {
            if (i < GameManager.instance.count)
                _enemyCount[i].sprite = _enemySprite[3];
        }
    }

    public void OnResult()
    {
        _scoreResult.text = GameManager.instance.score.ToString();
        _roundResult.text = GameManager.instance.round.ToString();

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
