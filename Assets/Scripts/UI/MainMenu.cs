using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _modePanel;

    private void Start()
    {
        Debug.Log(-Screen.width);
    }

    public void Play()
    {
        _menuPanel.transform.LeanMoveLocalX(-Screen.width * 6f, 0.5f).setEaseInOutSine();
        _modePanel.transform.LeanMoveLocalX(0f, 0.3f).setEaseInSine();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        _menuPanel.transform.LeanMoveLocalX(0f, 0.3f).setEaseInSine();
        _modePanel.transform.LeanMoveLocalX(Screen.width * 6f, 0.5f).setEaseInOutSine();
    }

    public void OnModeSelect(int index)
    {
        Transition.instance.Goto(() => SceneManager.LoadScene(index == 0? "SingleMode" : "Local"));
    }
}
