using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class DoorBehaviourScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> levelDoors;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out Player play))
        {
            //check the closer door to the player

        }
    }
}
