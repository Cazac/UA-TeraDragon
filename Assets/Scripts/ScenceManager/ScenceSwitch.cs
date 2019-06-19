using System;
using UnityEngine;
using UnityEngine.SceneManagement;


class ScenceSwitch : MonoBehaviour
{
    public void SwitchToScence(String scenceName)
    {
        SceneManager.LoadScene(scenceName);
    }
}
