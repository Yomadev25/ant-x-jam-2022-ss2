using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KeyboardSetting : MonoBehaviour
{
    public static KeyCode p1Key = KeyCode.G;
    public static KeyCode p2Key = KeyCode.M;

    KeyCode p1Default = KeyCode.G;
    KeyCode p2Default = KeyCode.M;

    KeyCode p1Apply;
    KeyCode p2Apply;

    bool isChanging;
    int index;

    [SerializeField] private Button _p1Button;
    [SerializeField] private Button _p2Button;
    [SerializeField] private TMP_Text _p1KeyText;
    [SerializeField] private TMP_Text _p2KeyText;

    private void Start()
    {
        _p1KeyText.text = p1Key.ToString();
        _p2KeyText.text = p2Key.ToString();
    }

    void Update()
    {
        if (isChanging)
        {
            DetectInput();
        }
    }

    void DetectInput()
    {
        foreach (KeyCode vkey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(vkey))
            {
                if (vkey != KeyCode.Return)
                {
                    if (index == 1) p1Apply = vkey;
                    else p2Apply = vkey;             
                    isChanging = false;
                    ConfirmKey();
                }
            }
        }
    }

    public void ChangeKey(int _player)
    {
        index = _player;
        isChanging = true;

        if (_player == 1)
        {
            _p1Button.interactable = false;
            _p1KeyText.text = "Detecting..";
            _p1KeyText.fontSize = 34f;
        }
        else
        {
            _p2Button.interactable = false;
            _p2KeyText.text = "Detecting..";
            _p2KeyText.fontSize = 34f;
        }
    }

    public void ConfirmKey()
    {
        if (p1Apply == KeyCode.None) p1Apply = p1Default;
        if (p2Apply == KeyCode.None) p2Apply = p2Default;

        if (index == 1)
        {
            _p1Button.interactable = true;
            _p1KeyText.text = p1Apply.ToString();
            _p1KeyText.fontSize = 49f;
        }
        else
        {
            _p2Button.interactable = true;
            _p2KeyText.text = p2Apply.ToString();
            _p2KeyText.fontSize = 49f;
        }
    }

    public void Default(int _player)
    {
        if(_player == 1) p1Key = p1Default;
        else p2Key = p2Default;

        _p1KeyText.text = p1Key.ToString();
        _p2KeyText.text = p2Key.ToString();
    }

    public void Apply()
    {
        if (p1Apply == KeyCode.None) p1Apply = p1Default;
        if (p2Apply == KeyCode.None) p2Apply = p2Default;

        p1Key = p1Apply;
        p2Key = p2Apply;
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _p1KeyText.text = p1Key.ToString();
        _p2KeyText.text = p2Key.ToString();
    }
}
