using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
   [SerializeField] private string sceneName;

   public void LoadScene()
   {
      Debug.Log("switching scene");
      SceneManager.LoadScene(sceneName);
   }
   
   public void LoadSceneAsync()
   {
      StartCoroutine(LoadAsync());
   }

   private IEnumerator LoadAsync()
   {
      var loader = SceneManager.LoadSceneAsync(sceneName);
      loader.allowSceneActivation = false;
      
      while (loader.progress < 0.9f)
      {
         yield return null;
      }

      loader.allowSceneActivation = true;
   }
}
