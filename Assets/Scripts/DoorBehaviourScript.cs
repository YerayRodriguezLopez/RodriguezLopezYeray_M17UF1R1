using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DoorBehaviourScript : MonoBehaviour
{
    private Rigidbody rb;
    private Player player;
    public static event Action OnPlayerDoor;
    private void OnTriggerEnter2D(Collider2D other)
    {
        OnPlayerDoor?.Invoke();
    }
    
}
