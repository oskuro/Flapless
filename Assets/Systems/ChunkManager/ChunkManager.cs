using System.Linq;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [SerializeField] GameObject _starterChunk;
    [SerializeField] GameObject[] _levelChunks;

    float _chunkYSize = 32f;
    Vector2 _currentChunkPos = Vector2.zero;


    void Start()
    {
        SpawnChunk(_starterChunk);
        SpawnChunk(_levelChunks[Random.Range(0, _levelChunks.Length)]);
        

    }

    void SpawnChunk(GameObject prefab) 
    {
        Instantiate(prefab, _currentChunkPos, Quaternion.identity);
        _currentChunkPos.y += _chunkYSize;
    }

    void Update()
    {
        
    }
}
