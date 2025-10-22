using System.Text.RegularExpressions;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    [SerializeField] private AudioClip[] footsteps;
    [SerializeField] private AudioClip slice;
    [SerializeField] private AudioClip capture;
    [SerializeField] private AudioClip collect;
    private AudioSource source;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        source = GetComponent<AudioSource>();
    }

    public void playSound(AudioClip sound)
    {
        source.PlayOneShot(sound);
    }

    public void PlayFootsteps()
    {
        AudioClip clip = footsteps[Random.Range(0, footsteps.Length)];
        source.PlayOneShot(clip);
    }

    public void PlaySlice()
    {
        AudioClip clip = slice;
        source.PlayOneShot(clip);
    }

    public void PlayCapture()
    {
        AudioClip clip = capture;
        source.PlayOneShot(clip);
    }

    public void PlayCollect()
    {
        AudioClip clip = collect;
        source.PlayOneShot(clip);
    }
}
