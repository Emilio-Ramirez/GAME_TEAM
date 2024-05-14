using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine,UI;

public class Registration : MonoBehaviour
{
  public InputField usernameField;
  public InputField passwordField;

  public Button submitButton;

  public void CallRegister(){
    StartCoroutine(Register());
  }

  Inumerator Register()
  {
    WWW www = new WWW("http://localhost/sqlconnect/register.php?username=" + usernameField.text + "&password=" + passwordField.text);

  }




}


