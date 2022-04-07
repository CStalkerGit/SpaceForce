using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string nextScene;

    public void OnStart()
    {
        SceneManager.LoadScene(nextScene);
    }
}
