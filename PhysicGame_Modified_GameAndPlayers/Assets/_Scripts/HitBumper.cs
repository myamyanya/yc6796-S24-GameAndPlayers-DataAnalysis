using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HitBumper : MonoBehaviour
{
    public float bumpForce;
    public GameObject playerVehicle;
    public Rigidbody vehicleRigidbody;

    public bool isHit = false;

    public GameObject spriteSample;
    // For Second-stage effects
    public GameObject spreiteSampleDissolve;

    private Transform mainCameraTransform;

    // For offsetting the sprite Y
    public float spriteOffsetY = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        // Loading sprites
        spriteSample = Resources.Load<GameObject>("Sprite/HitEffectSprite");

        // Load Second-stage effects
        spreiteSampleDissolve = Resources.Load<GameObject>("Sprite/HitEffectSpriteDissolve");

        // Find camera transform
        mainCameraTransform = Camera.main.transform;

        // Set tag for self
        gameObject.tag = "HardObject";

        bumpForce = UnityEngine.Random.Range(120000, 200000);
    }

    // Update is called once per frame
    void Update()
    {
        
        // Keep updatind the player
        playerVehicle = GameObject.Find("UtilityVehicle");

        if (playerVehicle != null)
        {
            vehicleRigidbody = playerVehicle.GetComponent<Rigidbody>();
        }

        if (isHit)
        {
            //
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject);

        if (collision.gameObject.tag == "PlayerWhole")
        {
            //Debug.Log("Im Hit!!");
            isHit = true;

            Vector3 direction = vehicleRigidbody.transform.position - transform.position;
            direction = direction.normalized;

            vehicleRigidbody.AddForceAtPosition(direction * bumpForce, collision.transform.position);

            ContactPoint contact = collision.contacts[0];
            Vector3 contactPoint = contact.point;
            contactPoint.y += spriteOffsetY;
            Quaternion contatctRotation = Quaternion.LookRotation(mainCameraTransform.position, Vector3.up);

            Instantiate(spriteSample, contactPoint, contatctRotation);
        }
    }
}
