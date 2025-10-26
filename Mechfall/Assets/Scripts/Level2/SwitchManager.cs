using UnityEngine;

public class SwitchManager : MonoBehaviour
{
    public int totalSwitches = 3;
    private int activatedCount = 0;

    public GameObject LevelEnder; 

    void Start()
    {
        LevelEnder.SetActive(false); 
    }

    public void SwitchActivated()
    {
        activatedCount++;
        SoundManager.instance.PlayCapture();
        if (activatedCount >= totalSwitches)
        {
            UnlockExit();
        }
    }

    void UnlockExit()
    {
        LevelEnder.SetActive(true);
    }
}
