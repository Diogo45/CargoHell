using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonController : MonoBehaviour
{



    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Leve0_1()
    {
        SceneManager.LoadScene("Level1");
    }


    public void Leve1_2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void Leve2_3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void Leve3_4()
    {
        SceneManager.LoadScene("Level4");
    }
}
