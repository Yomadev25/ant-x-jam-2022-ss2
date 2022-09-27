using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    public void OpenLink(string URL)
    {
        Application.OpenURL(URL);
    }
}
