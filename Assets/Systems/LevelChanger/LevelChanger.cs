using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [SerializeField] string _levelToChangeTo;
    AsyncOperation _levelLoading;


    void Start()
    {
        _levelLoading = SceneManager.LoadSceneAsync(_levelToChangeTo);
        _levelLoading.allowSceneActivation = false;
    }
    
    void OnTriggerEnter2D(Collider2D collider) 
    {
        Debug.Log("on trigger enter: " + collider.gameObject.name);
        _levelLoading.allowSceneActivation = true;
    }
}
