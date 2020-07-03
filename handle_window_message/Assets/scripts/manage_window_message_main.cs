using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manage_window_message_main : manage_window_message
{
    public static manage_window_message_main Instance = null;         

    protected void Awake()
    {
        base.Awake();
        
        if (Instance == null)
        {   
            Instance = this;
        }
        else if (Instance != this)
        {
           
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    protected void Start()
    {
        base.Start();
    }

    protected void Update()
    {
        base.Update();
    }
}
