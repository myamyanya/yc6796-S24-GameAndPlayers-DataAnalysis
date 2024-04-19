using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oceanColliderControl : MonoBehaviour
{
    Collider oceanCollider;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManagerHolder").GetComponent<GameManager>();
        oceanCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        gameManager.oceanColliderRespawned.AddListener(RespawnCollider);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject);

        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("Player sinking");
            RemoveCollider();
        }
    }
    
    void RemoveCollider()
    {
        if (oceanCollider != null)
        {
            oceanCollider.enabled = false;
        }
    }

    void RespawnCollider()
    {
        oceanCollider.enabled = true;
    }
}
