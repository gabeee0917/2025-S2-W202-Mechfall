using UnityEngine;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserSession : MonoBehaviour
{
    public static UserSession Instance;

    public string userId;
    public string username;
    public long score;
    public long level;
    public FirebaseFirestore db;
    public TMP_Text displayUser;
    public TMP_Text saveDataStatus;
    private float spawnTime;
    private string previousUsername = "";
    public Button profile;
    void Start()
    {


    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }

    }


    public IEnumerator SaveDataToFireStore()
    {
        Dictionary<string, object> updatedData = new Dictionary<string, object>
    {
        { "score", (long)this.score },
        { "level", (long)this.level },
        { "username", this.username }
    };

        var task = db.Collection("users").Document(userId).SetAsync(updatedData, SetOptions.MergeAll);
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception == null)
        {
            StartCoroutine(SaveSuccess());
        }
        else
        {
            StartCoroutine(SaveFailed());
        }

        if (displayUser != null)
        {
            Instance.displayUser.text = username;
        }
    }


    public IEnumerator LoadData()
    {
        DocumentReference docRef = db.Collection("users").Document(userId);
        var getTask = docRef.GetSnapshotAsync();
        yield return new WaitUntil(() => getTask.IsCompleted);

        if (getTask.Exception == null && getTask.Result.Exists)
        {
            DocumentSnapshot snapshot = getTask.Result;


            this.username = snapshot.GetValue<string>("username");
            this.score = snapshot.GetValue<long>("score");
            this.level = snapshot.GetValue<long>("level");


            Instance.displayUser.text = Instance.username;


        }

    }

    void Update()
    {
        if (previousUsername != username)
        {
            Instance.displayUser.text = Instance.username;
            previousUsername = Instance.username;
        }


       
    }

    public void Logout()
    {
        var auth = FirebaseAuth.DefaultInstance;
        var user = auth.CurrentUser;

        if (auth != null)
        {
            AuthenticationManager am = FindObjectOfType<AuthenticationManager>();
            if (am != null)
            {
                am.Cleanup();
            }

            if (user != null)
            {
                auth.SignOut();
            }
        }

        userId = "";
        username = "";
        score = 0;
        level = 0;
        
        Instance.profile.gameObject.SetActive(false);

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Destroy(player);
        }

        GameObject playerUI = GameObject.FindWithTag("PlayerUI");
        if (playerUI != null)
        {
            Destroy(playerUI);
        }
        Time.timeScale = 1f; //without changing scale back to 1, if i logout while paused, the scene wont load properly and it is paused infinitely!!!
        StartCoroutine(LoadLoginScene());
    }


    private IEnumerator LoadLoginScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Login");
    }

    private IEnumerator SaveSuccess()
    {
        saveDataStatus.gameObject.SetActive(true);
        saveDataStatus.text = "Data Saved";
        yield return new WaitForSeconds(1f);
        saveDataStatus.gameObject.SetActive(false);
    }

    private IEnumerator SaveFailed()
    {
        saveDataStatus.gameObject.SetActive(true);
        saveDataStatus.text = "Save Failed";
        yield return new WaitForSeconds(1f);
        saveDataStatus.gameObject.SetActive(false);
    }

    void OnApplicationQuit()
    {

    }

    public void saveB()
    {
        StartCoroutine(SaveDataToFireStore());
    }
}
