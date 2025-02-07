using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    public bool CanInteract => throw new System.NotImplementedException();
    [SerializeField] GameObject prefab;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnInteract()
    {
        throw new System.NotImplementedException();
    }
}
