using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;

    void Start()
    {
        if(enemyPrefabs.Length == 0)
            return;
            
        var randomIndex = Random.Range(0, enemyPrefabs.Length);
        Instantiate(enemyPrefabs[randomIndex], transform.position, transform.rotation, transform.parent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
