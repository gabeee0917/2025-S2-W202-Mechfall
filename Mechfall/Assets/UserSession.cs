using UnityEngine;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class UserSession : MonoBehaviour
{
    public static UserSession Instance;

    public string userId;
    public string username;
    public long score;
    public long maxlevel;
    public FirebaseFirestore db;
    public TMP_Text displayUser;
    public TMP_Text saveDataStatus;
    private float spawnTime;
    private string previousUsername = "";
    public Button profile;

    public long[] levelscores;
    void Start()
    {
        levelscores = new long[10];

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
        { "username", this.username },
        { "maxlevel", (long)this.maxlevel },
        { "score", (long)this.score },
        { "level1score", (long)this.levelscores[0]},
        { "level2score", (long)this.levelscores[1]},
        { "level3score", (long)this.levelscores[2]},
        { "level4score", (long)this.levelscores[3]},
        { "level5score", (long)this.levelscores[4]},
        { "level6score", (long)this.levelscores[5]},
        { "level7score", (long)this.levelscores[6]},
        { "level8score", (long)this.levelscores[7]},
        { "level9score", (long)this.levelscores[8]},
        { "level10score", (long)this.levelscores[9]},
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

        Dictionary<string, object> updateUsernameList = new Dictionary<string, object>
        {
            { this.username, (long)this.score }
        };

        db.Collection("users").Document("usernames").SetAsync(updateUsernameList, SetOptions.MergeAll);

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
            this.maxlevel = snapshot.GetValue<long>("maxlevel");
            this.levelscores[0] = snapshot.GetValue<long>("level1score");
            this.levelscores[1] = snapshot.GetValue<long>("level2score");
            this.levelscores[2] = snapshot.GetValue<long>("level3score");
            this.levelscores[3] = snapshot.GetValue<long>("level4score");
            this.levelscores[4] = snapshot.GetValue<long>("level5score");
            this.levelscores[5] = snapshot.GetValue<long>("level6score");
            this.levelscores[6] = snapshot.GetValue<long>("level7score");
            this.levelscores[7] = snapshot.GetValue<long>("level8score");
            this.levelscores[8] = snapshot.GetValue<long>("level9score");
            this.levelscores[9] = snapshot.GetValue<long>("level10score");


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
        maxlevel = 0;
        for (int n = 0; n < 10; n++)
        {
            levelscores[n] = 0;
        }


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

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TurnOffSingUI4Multi(scene);
    }

    void TurnOffSingUI4Multi(Scene scene)
    {
        if (scene.name == "Trial1")
        {

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        else
        {
             foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }


}
