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
   
   public void LoadScene(JSONNode node)
   {
      Debug.Log("switching scene");
      SceneManager.LoadScene(sceneName);
   }
}
