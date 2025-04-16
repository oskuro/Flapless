using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [SerializeField] GameObject _starterChunkPrefab;
    [SerializeField] GameObject[] _levelChunksPrefabs;

    [SerializeField] GameObject _levelChangerPrefab;
    List<GameObject> chunks = new List<GameObject>();

    float _chunkYSize = 36;
    Vector2 _currentChunkPos = Vector2.zero;

    Camera mainCam;
    private int _chunkLimit = 4;

    void Start()
    {
        mainCam = Camera.main;

        SpawnChunk(_starterChunkPrefab);
        SpawnChunk(_levelChunksPrefabs[Random.Range(0, _levelChunksPrefabs.Length)]);
    }

    void SpawnChunk(GameObject prefab) 
    {
        var chunk = Instantiate(prefab, _currentChunkPos, Quaternion.identity, transform);
        chunks.Add(chunk);
        _currentChunkPos.y += _chunkYSize;
    }

    void Update()
    {
        var chunkAllowed = chunks.Count < _chunkLimit;
        var nextChunkHeightReached = mainCam.transform.position.y > _currentChunkPos.y - _chunkYSize;

        if(nextChunkHeightReached)
        {
            if(chunkAllowed) 
            {
                SpawnChunk(_levelChunksPrefabs[Random.Range(0, _levelChunksPrefabs.Length)]);
            } else 
            {
                
            }
        }
    }
}
