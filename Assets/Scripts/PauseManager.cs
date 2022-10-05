using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private bool isMultiplayer;

    private PostProcessVolume _postProcess;
    DepthOfField depthOfField;

    private void Awake()
    {
        _postProcess = FindObjectOfType<PostProcessVolume>();
        _postProcess.profile.TryGetSettings(out depthOfField);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        this.gameObject.SetActive(true);
        depthOfField.active = true;

        if (!isMultiplayer)
        {
            Gun[] gun = FindObjectsOfType<Gun>();
            Enemy[] enemy = FindObjectsOfType<Enemy>();
            for (int i = 0; i < gun.Length; i++)
            {
                gun[i].enabled = false;
            }
            for (int i = 0; i < enemy.Length; i++)
            {
                enemy[i]._flySound.Stop();
            }
        }
        else
        {
            Multiplayer.Local.Gun[] gun = FindObjectsOfType<Multiplayer.Local.Gun>();          
            Multiplayer.Local.Enemy[] enemy = FindObjectsOfType<Multiplayer.Local.Enemy>();
            for (int i = 0; i < gun.Length; i++)
            {
                gun[i].enabled = false;
            }
            for (int i = 0; i < enemy.Length; i++)
            {
                enemy[i]._flySound.Stop();
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
        depthOfField.active = false;

        if (!isMultiplayer)
        {
            Gun[] gun = FindObjectsOfType<Gun>();
            Enemy[] enemy = FindObjectsOfType<Enemy>();
            for (int i = 0; i < gun.Length; i++)
            {
                gun[i].enabled = true;
            }
            for (int i = 0; i < enemy.Length; i++)
            {
                enemy[i]._flySound.Play();
            }
        }
        else
        {
            Multiplayer.Local.Gun[] gun = FindObjectsOfType<Multiplayer.Local.Gun>();
            Multiplayer.Local.Enemy[] enemy = FindObjectsOfType<Multiplayer.Local.Enemy>();
            for (int i = 0; i < gun.Length; i++)
            {
                gun[i].enabled = true;
            }
            for (int i = 0; i < enemy.Length; i++)
            {
                enemy[i]._flySound.Play();
            }
        }
    }

    public void Menu()
    {
        Time.timeScale = 1;
        Transition.instance.Goto(() => SceneManager.LoadScene("Mainmenu"));
    }
}
