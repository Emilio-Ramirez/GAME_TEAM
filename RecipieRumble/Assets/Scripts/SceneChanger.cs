using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
Script to provide a method to change scenes used from UI

Lisette Melo Reyes
*/
public class SceneChanger : MonoBehaviour
{
     public static void GoTo(string sceneName){
        //load the scene with the given name
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
     }
    
}
