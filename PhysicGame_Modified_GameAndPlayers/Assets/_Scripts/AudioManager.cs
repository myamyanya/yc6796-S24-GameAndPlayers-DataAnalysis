using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public AudioSource backgroundMusic;

    // Start is called before the first frame update
    void Start()
    {
        //backgroundMusic.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
}
