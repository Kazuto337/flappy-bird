using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HttpManager : MonoBehaviour
{
    [SerializeField] List<Text> scoresUI;
    [SerializeField]
    private string URL, Token, Username;

    // Start is called before the first frame update
    void Start()
    {
        Token = PlayerPrefs.GetString("token");
        Username = PlayerPrefs.GetString("username");
        Debug.Log("TOKEN:" + Token);
        StartCoroutine(GetPerfil());
    }

    public void ClickSignUp()
    {

        AuthData data = new AuthData();

        data.username = GameObject.Find("InputFieldUsername").GetComponent<InputField>().text;
        data.password = GameObject.Find("InputFieldPassword").GetComponent<InputField>().text;

        string postData = JsonUtility.ToJson(data);
        StartCoroutine(SignUp(postData));
    }

    public void ClickLogin()
    {

        AuthData data = new AuthData();

        data.username = GameObject.Find("InputFieldUsername").GetComponent<InputField>().text;
        data.password = GameObject.Find("InputFieldPassword").GetComponent<InputField>().text;

        string postData = JsonUtility.ToJson(data);
        StartCoroutine(LogIn(postData));
    }


    IEnumerator GetPerfil()
    {
        string url = URL + "/api/usuarios/" + Username;
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("x-token", Token);
        yield return www.SendWebRequest();
        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR " + www.error);
        }
        else if (www.responseCode == 200)
        {
            //Debug.Log(www.downloadHandler.text);
            AuthData resData = JsonUtility.FromJson<AuthData>(www.downloadHandler.text);
            Debug.Log("Token valido " + resData.usuario.username + ", id:" + resData.usuario._id + " y su score es: " + resData.usuario.score);
            SceneManager.LoadScene("Game");
        }
        else
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
        }
    }
    IEnumerator SignUp(string postData)
    {
        Debug.Log(postData);


        string url = URL + "/api/usuarios";
        UnityWebRequest www = UnityWebRequest.Put(url, postData);
        www.method = "POST";
        www.SetRequestHeader("content-type", "application/json");

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR " + www.error);
        }
        else if (www.responseCode == 200)
        {
            //Debug.Log(www.downloadHandler.text);
            AuthData resData = JsonUtility.FromJson<AuthData>(www.downloadHandler.text);

            Debug.Log("Bienvenido " + resData.usuario.username + ", id:" + resData.usuario._id);

        }
        else
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
        }
    }
    IEnumerator LogIn(string postData)
    {
        Debug.Log("LOG IN :" + postData);
        string url = URL + "/api/auth/login";
        UnityWebRequest www = UnityWebRequest.Put(url, postData);
        www.method = "POST";
        www.SetRequestHeader("content-type", "application/json");
        yield return www.SendWebRequest();
        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR " + www.error);
        }
        else if (www.responseCode == 200)
        {
            //Debug.Log(www.downloadHandler.text);
            AuthData resData = JsonUtility.FromJson<AuthData>(www.downloadHandler.text);

            Debug.Log("Autenticado " + resData.usuario.username + ", id:" + resData.usuario._id);
            Debug.Log("TOKEN: " + resData.token);

            PlayerPrefs.SetString("token", resData.token);
            PlayerPrefs.SetString("username", resData.usuario.username);
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
        }
    }
}

[System.Serializable]
public class AuthData
{
    public string username;
    public string password;
    public UserData usuario;
    public string token;
}

[System.Serializable]
public class UserData
{
    public string _id;
    public string username;
    public bool estado;
    public int score;
}
