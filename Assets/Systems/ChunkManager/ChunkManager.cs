using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [SerializeField] GameObject _starterChunk;
    [SerializeField] GameObject[] _levelChunks;
    List<GameObject> chunks = new List<GameObject>();

    float _chunkYSize = 32f;
    Vector2 _currentChunkPos = Vector2.zero;

    Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;

        SpawnChunk(_starterChunk);
        SpawnChunk(_levelChunks[Random.Range(0, _levelChunks.Length)]);
    }

    void SpawnChunk(GameObject prefab) 
    {
        var chunk = Instantiate(prefab, _currentChunkPos, Quaternion.identity, transform);
        chunks.Add(chunk);
        _currentChunkPos.y += _chunkYSize;
    }

    void Update()
    {
        if(mainCam.transform.position.y > _currentChunkPos.y - _chunkYSize)
        {
            Debug.Log($"Current Chunk Pos: {_currentChunkPos.y}");
            SpawnChunk(_levelChunks[Random.Range(0, _levelChunks.Length)]);
        }
    }
}
