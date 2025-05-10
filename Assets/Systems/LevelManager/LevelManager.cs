using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private const float FIXED_TIMESCALE = 0.02f;
    [SerializeField] Transform _checkpoint;
    [SerializeField] GameObject _playerPrefab;
    Player _player;
    Health _playerHealth;

    [SerializeField] float _waitBeforeShowScore = 0.35f;
    [SerializeField] float _slowmoTimeScale = 0.25f;

    [SerializeField] Animator _gameOverAnimatorController;
    CinemachineCamera _followCam;
    int _hiddenBool;

    void Start()
    {
        SpawnPlayer();
        var g = GameObject.Find("CM_Follow");
        if(g != null)
            _followCam = g.GetComponent<CinemachineCamera>();
        
        if(_followCam != null)
            _followCam.Follow = _player.transform;

        _hiddenBool = Animator.StringToHash("Visible");
    }

    private void SpawnPlayer()
    {
        Vector3 spawnPos = new();
        if (_checkpoint) { spawnPos = _checkpoint.position; }

        var playerObject = Instantiate(_playerPrefab, spawnPos, Quaternion.identity);
        _player = playerObject.GetComponent<Player>();
        
        _playerHealth = playerObject.GetComponent<Health>();
        _player.OnDeath += PlayerDied;
    }

    public void PlayerDied(GameObject player)
    {
        StartCoroutine(ShowRestart());
    }

    public void Paus()
    {

    }
    
    IEnumerator ShowRestart()
    {
        ChangeTimeScale(_slowmoTimeScale);
        yield return new WaitForSecondsRealtime(_waitBeforeShowScore);
        ChangeTimeScale(1);
        _gameOverAnimatorController.SetBool(_hiddenBool, true);
    }

    /// <summary>
    /// Changes Time.timeScale and Time.fixedDeltaTime to the given value.
    /// Values should be a float between 0 and 1
    /// Calling the method without a value resets the timescale to 1 
    /// </summary>
    /// <param name="newTimeScale"></param>
    private void ChangeTimeScale(float newTimeScale = 1.0f) 
    {
        Time.timeScale = newTimeScale;
        Time.fixedDeltaTime = FIXED_TIMESCALE * newTimeScale;
    }

    void OnDisable()
    {
        if (_player)
        {
            _player.OnDeath -= PlayerDied;
        }
    }

    public void Return()
    {
        SceneManager.LoadScene("Main");
    }

    public void Restart() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextScene() 
    {
        var currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene + 1);

    }

}
