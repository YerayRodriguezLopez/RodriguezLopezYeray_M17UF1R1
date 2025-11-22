using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<DoorBehaviourScript> doorsList;
    public int CurrentLevel = 0;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
