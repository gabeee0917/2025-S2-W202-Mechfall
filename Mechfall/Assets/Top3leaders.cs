using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions; 
using UnityEngine.UI; 
using TMPro;

public class TopThreeLeaderboard : MonoBehaviour
{
    FirebaseFirestore db;

    public TMP_Text rank1Name;
    public TMP_Text rank1Score;

    public TMP_Text rank2Name;
    public TMP_Text rank2Score;

    public TMP_Text rank3Name;
    public TMP_Text rank3Score;

    public GameObject scorePage;


    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        GetTopThree();
    }

    public void GetTopThree()
    {
        Query topThree = db.Collection("users").OrderByDescending("score").Limit(3);

        topThree.GetSnapshotAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted || task.IsCanceled)
            {
                return;
            }
            QuerySnapshot snapshot = task.Result;


            List<UserDummyClass> topPlayers = new List<UserDummyClass>();

            foreach (DocumentSnapshot doc in snapshot.Documents)
            {
                Dictionary<string, object> data = doc.ToDictionary();


                string username = "";
                if (data.ContainsKey("username")) {
                    username = data["username"].ToString();
                }
                long score = System.Convert.ToInt64(data["score"]);
               

                topPlayers.Add(new UserDummyClass(username, score));
            }
            DisplayLeaderboard(topPlayers);
        });
    }


    void DisplayLeaderboard(List<UserDummyClass> topPlayers)
    {
        rank1Name.text = topPlayers[0].username;
        rank1Score.text = topPlayers[0].score.ToString();
        rank2Name.text = topPlayers[1].username;
        rank2Score.text = topPlayers[1].score.ToString();
        rank3Name.text = topPlayers[2].username;
        rank3Score.text = topPlayers[2].score.ToString();
    }

    public void openScores()
    {
        scorePage.SetActive(true);
  
    }

    public void closeScores()
    {
        scorePage.SetActive(false);
    }

}



public class UserDummyClass
{
    public string username;
    public long score;

    public UserDummyClass(string username, long score)
    {
        this.username = username;
        this.score = score;
    }
}
