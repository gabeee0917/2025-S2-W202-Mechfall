using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ScreenshotFeature : MonoBehaviour
{
    public enum SaveLocation { ProjectFolder, PersistentDataPath }

    [Header("Trigger")]
    public bool enableInputTrigger = true;
    public KeyCode captureKey = KeyCode.F9;

    [Header("Save Settings")]
    public SaveLocation saveLocation = SaveLocation.ProjectFolder;
    public string subfolderName = "Screenshots";
    public string filenamePrefix = "Screenshot_";
    public bool useTimestamp = true;
    public bool useCounterIfSameSecond = true;
    public int supersize = 1; // 1 = native res, 2 = 2x, etc.

    [Header("Notification (optional)")]
    public bool showToast = true;
    public CanvasGroup toastCanvas;         // a small UI panel with text
    public TMPro.TextMeshProUGUI toastText; // optional (set if using TMP)
    public float toastDuration = 1.2f;

    [Header("Events")]
    public UnityEvent<string> OnScreenshotSaved; // passes file path

    static ScreenshotFeature _instance;
    static int _sameSecondCounter;
    static string _lastSecondStamp;

    void Awake()
    {
        if (_instance == null) _instance = this;
    }

    void Update()
    {
        if (!enableInputTrigger) return;
        if (Input.GetKeyDown(captureKey)) CaptureNow();
    }

    public static void CaptureNow(string customName = null)
    {
        if (_instance == null)
        {
            Debug.LogWarning("ScreenshotFeature not present in scene.");
            return;
        }
        _instance.CaptureInternal(customName);
    }

    void CaptureInternal(string customName)
    {
        string root = saveLocation == SaveLocation.ProjectFolder
            ? Path.GetFullPath(Path.Combine(Application.dataPath, ".."))
            : Application.persistentDataPath;

        string dir = string.IsNullOrEmpty(subfolderName) ? root : Path.Combine(root, subfolderName);
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        string stamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        if (_lastSecondStamp != stamp)
        {
            _lastSecondStamp = stamp;
            _sameSecondCounter = 0;
        }
        else
        {
            _sameSecondCounter++;
        }

        string baseName = !string.IsNullOrEmpty(customName) ? customName : filenamePrefix;
        string filename = useTimestamp ? $"{baseName}{stamp}" : baseName.TrimEnd('_');

        if (useCounterIfSameSecond && _sameSecondCounter > 0)
            filename += $"_{_sameSecondCounter:D2}";

        string path = Path.Combine(dir, $"{filename}.png");

        ScreenCapture.CaptureScreenshot(path, Mathf.Max(1, supersize));
        Debug.Log($"[ScreenshotFeature] Saved: {path}");

        if (showToast) ShowToast($"Screenshot saved:\n{filename}.png");
        OnScreenshotSaved?.Invoke(path);
    }

    async void ShowToast(string msg)
    {
        if (!toastCanvas) return;
        if (toastText) toastText.text = msg;

        toastCanvas.alpha = 0f;
        toastCanvas.gameObject.SetActive(true);

        float t = 0f;
        while (t < 0.12f) { t += Time.unscaledDeltaTime; toastCanvas.alpha = Mathf.InverseLerp(0f, 0.12f, t); await System.Threading.Tasks.Task.Yield(); }
        await System.Threading.Tasks.Task.Delay(Mathf.RoundToInt(toastDuration * 1000f));
        t = 0f;
        while (t < 0.2f) { t += Time.unscaledDeltaTime; toastCanvas.alpha = Mathf.InverseLerp(0.2f, 0f, t); await System.Threading.Tasks.Task.Yield(); }
        toastCanvas.gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    [MenuItem("Tools/Screenshot/Capture Now %#F9")] // Ctrl/Cmd + Shift + F9 in Editor
    static void EditorCaptureNow()
    {
        if (_instance == null)
        {
            var go = new GameObject("ScreenshotFeature (Temp)");
            _instance = go.AddComponent<ScreenshotFeature>();
            _instance.enableInputTrigger = false;
            _instance.saveLocation = SaveLocation.ProjectFolder;
        }
        CaptureNow();
    }
#endif
}
