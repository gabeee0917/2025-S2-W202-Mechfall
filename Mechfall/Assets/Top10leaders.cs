using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions; 
using UnityEngine.UI; 
using TMPro;
using System.Collections;
using System.Linq; //language integrated query c# thingy to help do queries from the local system instead of having to do queries at firebase server


public class TopTenLeaderboard : MonoBehaviour
{
    FirebaseFirestore db;

    public TMP_Text scoreText;

    public GameObject scorePage;

    public TMP_InputField userSearch;
    public TMP_Text statusText;

    // on start of the gameobject holding this, which in our game is the UserSession prefab which will not be destroyed until program ends, get top ten scores. 
    // ensures that each time player checks top ten using a top ten button, it doesnt keep calling to firebase and waste up reads. 
    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        statusText.text = "";
        scoreText.text = "";
        StartCoroutine(GetTopTen());
    }

    // function to search scores and details based on username. Queries the firestore db and reads the searched user's doc (gets snapshot). 
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
                    statusText.text += $"\nLevel{n + 1} score = " + data[$"level{n + 1}score"].ToString();
                }
                statusText.text += "\n\n-- " + data["username"] +"'s message -- \n" + data["profilemessage"];
            }

        });

    }


    // usernames and thier highscores are stored in one document named usernames in a collection called users. Get a snapshot of this doc and do a query locally to find top ten scores.
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

    // function to add to a refresh button. dont spam as it will do a read every time you press the button. 
    public void refresh()
    {
        StartCoroutine(GetTopTen());
    }
}


