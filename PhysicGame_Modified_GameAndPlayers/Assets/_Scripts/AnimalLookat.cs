using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalLookat : MonoBehaviour
{
    public GameObject player;
    private float distance;
    //public float rotationSpeed = 5.0f;
    public AudioClip baa;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        baa = Resources.Load<AudioClip>("Audio/lamb-baa");
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("PlayerWhole");

        distance = Vector3.Distance(player.transform.position, transform.position);
        
        if (distance <= 10)
        {
            /*Vector3 directionToPlayer = player.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);*/

            //transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerWhole" && baa != null && !audioSource.isPlaying)
        {
            audioSource.clip = baa;
            audioSource.PlayOneShot(baa);
        }
    }
}
