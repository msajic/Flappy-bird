using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_manu : MonoBehaviour
{
    public void pokreni_igru()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void izadji()
    {
        Application.Quit();
    }
}
