using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Script for the process that runs when click on the start game button. 
public class StartSceneSwitcher : MonoBehaviour
{
  public GameObject beam; // simulate beam glow 
  public Material change;

  public AudioSource beamsound;
  public void LoadLogin()
  {
    beam.SetActive(true);
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

