using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private float timer = 0f;
    public float animationDuration = 0.12f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < animationDuration)
        {
            timer += Time.deltaTime;

        }
        else
        {
            Destroy(gameObject);
        }
    }

}
