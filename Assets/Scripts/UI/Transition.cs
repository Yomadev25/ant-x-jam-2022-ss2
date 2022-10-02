using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public static Transition instance;

    [SerializeField] private RectTransform fader;
    [SerializeField] private AudioSource _zoomIn;
    [SerializeField] private AudioSource _zoomOut;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        LeanTween.scale(fader, Vector3.one, 0);
        LeanTween.scale(fader, Vector3.zero, 0.8f).setEaseInBack().setDelay(0.3f).setOnStart(() => _zoomOut.Play());       
    }   

    public void Goto(Action onComplete)
    {
        LeanTween.scale(fader, Vector3.zero, 0f);
        LeanTween.scale(fader, Vector3.one, 0.8f).setEaseInOutSine().setOnComplete(onComplete);
        _zoomIn.Play();
    }
}
