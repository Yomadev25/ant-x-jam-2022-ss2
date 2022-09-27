using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    Gun gun;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        gun = FindObjectOfType<Gun>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
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

        _notificationBox.transform.localScale = Vector3.zero;
        _notificationBox.LeanScale(Vector3.one, 0.2f).setEaseInSine();
    }

    public void OnCloseNotification()
    {
        _notificationBox.LeanScale(Vector3.zero, 0.2f).setEaseOutSine();
    }
}
