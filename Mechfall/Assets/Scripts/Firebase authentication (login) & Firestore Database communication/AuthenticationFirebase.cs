using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using System;

// Script to attach to a firebase manager gameobject in Login scene to manage login/register and getting data from Firebase and firestore 
public class AuthenticationManager : MonoBehaviour
{
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public TMP_InputField loginEmail;
    public TMP_InputField loginPW;
    public TMP_Text loginWarning;
    public TMP_Text loginConfirmt;
    public TMP_InputField registerUsername;
    public TMP_InputField registerEmail;
    public TMP_InputField registerPW;
    public TMP_InputField verifyPW;
    public TMP_Text registerText;
    public GameObject registerScreen;
    public FirebaseFirestore db;

    // On awake of the gameobject holding this script, check dependencies and initialise firebase make sure it stays active the entire time till program shuts down.
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(CheckFirebaseDependencies());
    }

    private IEnumerator CheckFirebaseDependencies()
    {
        var checkTask = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => checkTask.IsCompleted);

        dependencyStatus = checkTask.Result;
        if (dependencyStatus == DependencyStatus.Available)
        {
            InitializeFirebase();
        }
    }

    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);

        if (UserSession.Instance == null)
        {
            Instantiate(Resources.Load<GameObject>("UserSession"));
        }

        UserSession.Instance.db = db;
    }

    //This ienumerator will be used in the login ienumerator, to load the lobby when login is successful
    private IEnumerator LoadLobbyAfterLoadData()
    {
        yield return StartCoroutine(UserSession.Instance.LoadData());

        if (UserSession.Instance.profile != null)
        {
            UserSession.Instance.profile.gameObject.SetActive(true);
        }

        SceneManager.LoadScene("Lobby");
    }

    // function to check login, assign to loginbutton onclick
    public void LoginButton()
    {
        StartCoroutine(Login(loginEmail.text, loginPW.text));
    }

    // function to register, assign to registerbutton onclick
    public void RegisterButton()
    {
        StartCoroutine(Register(registerEmail.text, registerPW.text, registerUsername.text));
    }

    //login ienumerator process that uses firebase-specific authentication login functions / updates UserSession prefab up to date data
    private IEnumerator Login(string email, string password)
    {
        loginWarning.text = "";
        loginConfirmt.text = "";

        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            var firebaseEx = loginTask.Exception.GetBaseException() as FirebaseException;
            if (firebaseEx != null)
            {
                var errorCode = (AuthError)firebaseEx.ErrorCode;
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        loginWarning.text = "Type in your email!";
                        break;
                    case AuthError.MissingPassword:
                        loginWarning.text = "Type in your password";
                        break;
                    case AuthError.WrongPassword:
                        loginWarning.text = "Wrong password!";
                        break;
                    case AuthError.InvalidEmail:
                        loginWarning.text = "Invalid email!";
                        break;
                    case AuthError.UserNotFound:
                        loginWarning.text = "User not found!";
                        break;
                    default:
                        loginWarning.text = "Login Failed!";
                        break;
                }
            }
            else
            {
                loginWarning.text = "Login Failed!";
            }
        }
        else
        {
            User = loginTask.Result.User;
            loginConfirmt.text = "Login success!";
            UserSession.Instance.profile.gameObject.SetActive(true);

            if (User != null)
            {
                UserSession.Instance.userId = User.UserId;
            }

            StartCoroutine(LoadLobbyAfterLoadData());
        }
    }

    //register ienumerator process that uses firebase-specific authentication register functions / updates UserSession prefab up to date data
    private IEnumerator Register(string email, string password, string username)
    {
        registerText.text = "";

        if (string.IsNullOrWhiteSpace(username))
        {
            registerText.text = "Type in the username you want!";
            yield break;
        }
        else if (password != verifyPW.text)
        {
            registerText.text = "Password doesn't match!";
            yield break;
        }
        else
        {
            DocumentReference userDoc = db.Collection("users").Document("usernames");
            var usernamecheck = userDoc.GetSnapshotAsync();
            yield return new WaitUntil(() => usernamecheck.IsCompleted);

            if (usernamecheck.IsFaulted || usernamecheck.IsCanceled)
            {
                registerText.text = "Error occured during username check";
                yield break;
            }

            DocumentSnapshot docSnapshot = usernamecheck.Result;
            if (docSnapshot.Exists && docSnapshot.ContainsField(username))
            {
                registerText.text = "Username already exists!";
                yield break;
            }

            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(() => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                var firebaseEx = registerTask.Exception.GetBaseException() as FirebaseException;
                if (firebaseEx != null)
                {
                    var errorCode = (AuthError)firebaseEx.ErrorCode;
                    switch (errorCode)
                    {
                        case AuthError.MissingEmail:
                            registerText.text = "Type in your email!";
                            break;
                        case AuthError.MissingPassword:
                            registerText.text = "Type in your password";
                            break;
                        case AuthError.WeakPassword:
                            registerText.text = "PW must be at least 8 characters!";
                            break;
                        case AuthError.EmailAlreadyInUse:
                            registerText.text = "Email already in use!";
                            break;
                        default:
                            registerText.text = "Register Failed!";
                            break;
                    }
                }
                else
                {
                    registerText.text = "Register Failed!";
                }
            }
            else
            {
                User = registerTask.Result.User;
                registerText.text = "Register Success!";
                if (User != null)
                {
                    while (FirebaseAuth.DefaultInstance.CurrentUser == null)
                    {
                        yield return null;
                    }

                    UserSession.Instance.userId = User.UserId;
                    UserSession.Instance.username = username;
                    UserSession.Instance.score = 0;
                    UserSession.Instance.maxlevel = 1;

                    for (int n = 0; n < 10; n++)
                    {
                        UserSession.Instance.levelscores[n] = 0;
                    }

                    UserSession.Instance.profilemessage = "Sup";
                    UserSession.Instance.PvPWin = 0;
                    UserSession.Instance.PvPLose = 0;

                    yield return StartCoroutine(UserSession.Instance.SaveDataToFireStore());
                }
            }
        }
    }

    public void OpenRegisterScreen()
    {
        registerScreen.SetActive(true);
    }

    public void CloseRegisterScreen()
    {
        registerScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // function that detects changes in authentication state changes, not utilised yet as we manually control things but if project becomes bigger will have to utilise
    void AuthStateChanged(object sender, EventArgs eventArgs)
    {
        if (auth.CurrentUser != User)
        {
            bool signedIn = User != auth.CurrentUser && auth.CurrentUser != null;

            if (!signedIn && User != null)
            {

            }

            User = auth.CurrentUser;

            if (signedIn)
            {
                if (UserSession.Instance == null)
                {
                    Instantiate(Resources.Load<GameObject>("UserSession"));
                }

                UserSession.Instance.userId = User.UserId;
                UserSession.Instance.db = db;
                StartCoroutine(LoadLobbyAfterLoadData());
            }
        }
    }

    public void Cleanup()
    {
        if (auth != null)
        {
            auth.StateChanged -= AuthStateChanged;
        }
    }

}
