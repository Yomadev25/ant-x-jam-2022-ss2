using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _modePanel;
    [SerializeField] private GameObject _keyboardSettingPanel;
    [SerializeField] private GameObject _multiplayerPanel;
    [SerializeField] private GameObject _onlinePanel;

    public void Play()
    {
        _menuPanel.transform.LeanMoveLocalX(-Screen.width * 6f, 0.5f).setEaseInOutSine();
        _modePanel.transform.LeanMoveLocalX(0f, 0.3f).setEaseInSine();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void KeySetting()
    {
        _keyboardSettingPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        _menuPanel.transform.LeanMoveLocalX(0f, 0.3f).setEaseInSine();
        _modePanel.transform.LeanMoveLocalX(Screen.width * 6f, 0.5f).setEaseInOutSine();
    }

    public void OnModeSelect(int index)
    {
        if (index < 2)
        {
            Transition.instance.Goto(() => SceneManager.LoadScene(index == 0 ? "SingleMode" : "Local"));
        }
        else
        {
            _multiplayerPanel.SetActive(true);
        }
    }

    public void OnOnline()
    {
        _onlinePanel.SetActive(true);
    }
}
