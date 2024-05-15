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

    public void GoToRegister()
    {
        //load the scene with the given name
        SceneManager.LoadScene(1);
    }


}
