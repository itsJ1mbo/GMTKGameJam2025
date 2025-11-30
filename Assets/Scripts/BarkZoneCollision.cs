using System;
using UnityEngine;

public class BarkZoneCollision : MonoBehaviour
{
    [SerializeField] private DogBarks dog;
    [SerializeField] private int frequency;
    
    private void OnTriggerEnter(Collider other)
    {
        dog.SetBarks(frequency);
    }
}
