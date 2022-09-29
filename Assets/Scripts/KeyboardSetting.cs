using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KeyboardSetting : MonoBehaviour
{
    public static KeyCode p1Key = KeyCode.G;
    public static KeyCode p2Key = KeyCode.M;

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
                    if (index == 1) p1Key = vkey;
                    else p2Key = vkey;             
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
        }
        else
        {
            _p2Button.interactable = false;
            _p2KeyText.text = "Detecting..";
        }
    }

    public void ConfirmKey()
    {
        if (index == 1)
        {
            _p1Button.interactable = true;
            _p1KeyText.text = p1Key.ToString();
        }
        else
        {
            _p2Button.interactable = true;
            _p2KeyText.text = p2Key.ToString();
        }
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }
}
