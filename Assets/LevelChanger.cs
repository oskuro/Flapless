using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [SerializeField] string _levelToChangeTo;
    AsyncOperation levelLoading;


    void Start()
    {
        levelLoading = SceneManager.LoadSceneAsync(_levelToChangeTo);
        levelLoading.allowSceneActivation = false;
    }
    
    void OnTriggerEnter2D(Collider2D collider) 
    {
        levelLoading.allowSceneActivation = true;
    }
}
