using System;
using UnityEngine;

public class HurtBehaviourScript : MonoBehaviour
{
    private Rigidbody rb;
    private Player player;
    public static event Action OnPlayerHurt;

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnPlayerHurt?.Invoke();
    }
}
