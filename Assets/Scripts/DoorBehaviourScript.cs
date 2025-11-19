using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DoorBehaviourScript : MonoBehaviour
{
    private Rigidbody rb;
    private Player player;
    public static event Action<int, int> OnPlayerDoor;
    public int directionToNextDoor;
    public int exitDirection;

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnPlayerDoor?.Invoke(directionToNextDoor, exitDirection);
    }
}
