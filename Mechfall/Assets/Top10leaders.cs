using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions; 
using UnityEngine.UI; 
using TMPro;
using System.Collections;
using System.Linq; //language integrated query c# thingy to help do queries from the local system instead of having to do queries at firebase
public class TopThreeLeaderboard : MonoBehaviour
{
    FirebaseFirestore db;

    public TMP_Text scoreText;

    public GameObject scorePage;

    public TMP_InputField userSearch;
    public TMP_Text statusText;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        statusText.text = "";
        scoreText.text = "";
        StartCoroutine(GetTopTen());
    }

    public void searchUser()
    {
        string username = userSearch.text;
        statusText.text = "";

        Query userQuery = db.Collection("users").WhereEqualTo("username", username);

        userQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                statusText.text = "Failed to get data";
                return;
            }

            QuerySnapshot snapshot = task.Result;
            if (snapshot.Count == 0)
            {
                statusText.text = "No such user";
                return;
            }

            foreach (DocumentSnapshot doc in snapshot.Documents)
            {
                Dictionary<string, object> data = doc.ToDictionary();
                statusText.text = "Total score = " + data["score"].ToString() + "\nMax Level = " + data["maxlevel"].ToString();
                for (int n = 0; n < (long)data["maxlevel"]; n++)
                {
                    statusText.text += $"\nLevel{n+1} score = " +data[$"level{n+1}score"].ToString();
                }
            }

        });

    }


    public IEnumerator GetTopTen()
    {
        DocumentReference topTen = db.Collection("users").Document("usernames");

        var getTask = topTen.GetSnapshotAsync();
        yield return new WaitUntil(() => getTask.IsCompleted);

        if (getTask.IsFaulted || getTask.IsCanceled)
        {
            yield break;
        }

        DocumentSnapshot snapshot = getTask.Result;
        Dictionary<string, object> snappy = snapshot.ToDictionary();

        Dictionary<string, long> alldata = snappy.ToDictionary(
            kvp => kvp.Key,
            kvp => (long)kvp.Value
        );

        var top10 = alldata.OrderByDescending(pair => pair.Value).Take(10).ToList();

        int rank = 1;
        scoreText.text = "";
        foreach (var pair in top10)
        {
            scoreText.text += $"[Rank {rank}]  {pair.Key} ({pair.Value})\r\n"; //\n should work alone but i guess it's a unity thing. have to return carriage. 
            rank++;
        }



    }

    public void openScores()
    {
        scorePage.SetActive(true);

    }

    public void closeScores()
    {
        scorePage.SetActive(false);
    }

    public void refresh()
    {
        StartCoroutine(GetTopTen());
    }
}


