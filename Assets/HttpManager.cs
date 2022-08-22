using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HttpManager : MonoBehaviour
{

    [SerializeField]
    private string URL;
    [SerializeField] List<Text> scoresUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ClickGetScores()
    {
        StartCoroutine(GetScores());
    }

    IEnumerator GetScores()
    {
        string url = URL + "/leaders";
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR " + www.error);
        }
        else if(www.responseCode == 200){
            //Debug.Log(www.downloadHandler.text);
            Scores resData = JsonUtility.FromJson<Scores>(www.downloadHandler.text);

            int index = 0;
            foreach (ScoreData score in resData.scores) //Por Santiago Lara
            {
                scoresUI[index].text = score.value.ToString() + " Points";
                Debug.Log(score.userId +" | "+score.value);
                index++;
            }
        }
        else
        {
            Debug.Log(www.error);
        }
    }
   
}


[System.Serializable]
public class ScoreData
{
    public int userId;
    public int value;

}

[System.Serializable]
public class Scores
{
    public ScoreData[] scores;
}
