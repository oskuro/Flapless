using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [SerializeField] string _levelToChangeTo;

    void OnTriggerEnter2D(Collider2D collider) 
    {
        SceneManager.LoadScene(_levelToChangeTo);
    }
}
