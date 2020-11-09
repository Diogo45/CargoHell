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


    public void Level01()
    {
        SceneManager.LoadScene("TestLevel");
    }
}
