using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Transform _checkpoint;
    [SerializeField] GameObject _playerPrefab;
    PlayerBalloonLift _player;

    void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        Vector3 spawnPos = new();
        if (_checkpoint) { spawnPos = _checkpoint.position; }

        var playerObject = Instantiate(_playerPrefab, spawnPos, Quaternion.identity);
        _player = playerObject.GetComponent<PlayerBalloonLift>();

        _player.OnDeath += PlayerDied;
    }

    public void PlayerDied()
    {
        _player.gameObject.SetActive(false);
    }

    void OnDisable() 
    {
        if(_player) 
        {
            _player.OnDeath -= PlayerDied;
        }
    }

}
