using System;
using UnityEngine;

public class HurtBehaviourScript : MonoBehaviour
{
    public static event Action OnPlayerHurt;

    private void OnTriggerEnter2D(Collider2D other)
    { 
        OnPlayerHurt?.Invoke();
        
    }
}
