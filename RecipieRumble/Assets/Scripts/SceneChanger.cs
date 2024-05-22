
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static void GoTo(string sceneName)
    {
        //load the scene with the given name
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public static void GoToRegister()
    {
        //load the scene with the given name
        SceneManager.LoadScene(1);
    }
    public static void GoToLogin()
      {
        //load the scene with the given name
        SceneManager.LoadScene(2);
      }
    public static void GoToHome()
      {
        //load the scene with the given name
        SceneManager.LoadScene(0);
      }
    public static void GoToGame()
      {
      //load the scene with the given name
      SceneManager.LoadScene(3);
      }


}

