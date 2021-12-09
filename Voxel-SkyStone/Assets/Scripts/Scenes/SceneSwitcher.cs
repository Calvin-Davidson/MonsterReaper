using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
   [SerializeField] private string sceneName;

   public void LoadScene()
   {
      SceneManager.LoadScene(sceneName);
   }
}
