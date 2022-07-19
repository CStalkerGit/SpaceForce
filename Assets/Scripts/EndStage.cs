using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndStage : MonoBehaviour
{
    public void Exit()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
