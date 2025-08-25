using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneSwitcher : MonoBehaviour
{
  public GameObject beam;
  public Material change;

  public void LoadLobby()
  {
    beam.SetActive(true);
    Image image = gameObject.GetComponent<Image>();
    image.material = change;
    StartCoroutine(Delay(1));
    
  }

  IEnumerator Delay(float n)
  {
    yield return new WaitForSeconds(n);
    SceneManager.LoadScene("Login");
  }    
        
}

