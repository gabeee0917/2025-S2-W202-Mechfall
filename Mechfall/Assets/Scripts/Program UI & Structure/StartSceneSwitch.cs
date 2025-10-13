using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Script for the process that runs when click on the start game button at the very first scene. 
public class StartSceneSwitcher : MonoBehaviour
{
  public GameObject beam; 
  public Material change;
  public GameObject enemy;
  public AudioSource beamsound;

  // simulate beam glow by making it appear, make a sound effect sound, change scene to login scene 2 secs later
  public void LoadLogin()
  {
    beam.SetActive(true);
    enemy.SetActive(false);
    Image image = gameObject.GetComponent<Image>();
    image.material = change;
    StartCoroutine(Delay(2));
  }

  IEnumerator Delay(float n)
  {
    yield return new WaitForSeconds(n);
    SceneManager.LoadScene("Login");
  }

  public void PlayBeamSound()
  {
    beamsound.Play();
  }
}

