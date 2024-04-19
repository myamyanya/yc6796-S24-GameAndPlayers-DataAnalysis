using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poolColliderControl : MonoBehaviour
{
    // The pool collider is in a holder object
    // Remove the collider of the whole terrain once hit the pool collider
    Collider terrainCollider;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManagerHolder").GetComponent<GameManager>();
        terrainCollider = gameObject.transform.parent.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        gameManager.oceanColliderRespawned.AddListener(RespawnCollider);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);

        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player sinking");
            RemoveCollider();
        }
    }
    
    void RemoveCollider()
    {
        if (terrainCollider != null)
        {
            terrainCollider.enabled = false;
        }
    }

    void RespawnCollider()
    {
        terrainCollider.enabled = true;
    }
}
