using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private bool isMultiplayer;

    public void Pause()
    {
        Time.timeScale = 0;
        this.gameObject.SetActive(true);

        if (!isMultiplayer)
        {
            Gun[] gun = FindObjectsOfType<Gun>();
            for (int i = 0; i < gun.Length; i++)
            {
                gun[i].enabled = false;
            }
        }
        else
        {
            Multiplayer.Local.Gun[] gun = FindObjectsOfType<Multiplayer.Local.Gun>();
            for (int i = 0; i < gun.Length; i++)
            {
                gun[i].enabled = false;
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        this.gameObject.SetActive(false);

        if (!isMultiplayer)
        {
            Gun[] gun = FindObjectsOfType<Gun>();
            for (int i = 0; i < gun.Length; i++)
            {
                gun[i].enabled = true;
            }
        }
        else
        {
            Multiplayer.Local.Gun[] gun = FindObjectsOfType<Multiplayer.Local.Gun>();
            for (int i = 0; i < gun.Length; i++)
            {
                gun[i].enabled = true;
            }
        }
    }

    public void Menu()
    {
        Time.timeScale = 1;
        Transition.instance.Goto(() => SceneManager.LoadScene("Mainmenu"));
    }
}
