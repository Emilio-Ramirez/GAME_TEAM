using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ApiManager : MonoBehaviour
{
    private string baseUrl = "http://localhost:3000";

    IEnumerator GetCartas()
    {
        UnityWebRequest request = UnityWebRequest.Get(baseUrl + "/cartas");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Cartas: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error al obtener las cartas: " + request.error);
        }
    }

    IEnumerator RegisterUser(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        UnityWebRequest request = UnityWebRequest.Post(baseUrl + "/register", form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Usuario registrado: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error al registrar usuario: " + request.error);
        }
    }
}
