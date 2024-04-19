using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    GameObject gameManagerObject;
    GameManager gameManager;
    private bool isPaused;

    public Transform playerVehicle;
    public Vector3 cameraOffset;
    public float cameraLerpValue;

    // For camera rotating (test)
    public float rotationSpeed;

    // For inst the camera rotation
    private Quaternion initialRotation;
    private bool isResetting = false;
    public float resetSpeed = 15.0f;

    // Start is called before the first frame update
    void Start()
    {
        initialRotation = transform.rotation;

        gameManagerObject = GameObject.Find("GameManagerHolder");
        gameManager = gameManagerObject.GetComponent<GameManager>();

        //InitializingCamera();

        cameraOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(gameManager.isOver);
        if (gameManager.isOver == false)
        {
            playerVehicle = GameObject.FindWithTag("PlayerWhole").transform;
            transform.position = Vector3.Lerp(transform.position, playerVehicle.position + cameraOffset, cameraLerpValue);
        }
        
        if (gameManager != null)
        {
            gameManager.playerRespawned.AddListener(InitializingCamera);

            /*if (gameManager.isPaused && Input.GetKey(KeyCode.R))
            {
                InitializingCamera();
            }*/

        }

        // Camera rotating
        if(gameManager.isStart && !gameManager.isPaused && !gameManager.isOver)
        {
            if (Input.GetMouseButton(1))
            {
                float mouseX = Input.GetAxis("Mouse X");

                transform.Rotate(Vector3.up, mouseX * rotationSpeed, Space.World);

                //transform.rotation = Quaternion.Lerp(transform.rotation, playerVehicle.rotation, cameraLerpValue);
            }
        }
        
    }

    public void InitializingCamera()
    {
        isResetting = true;

        StartCoroutine(ResetCameraRotation());

        /*Debug.Log("Resetting camera");
        Debug.Log(initialRotation);

        Quaternion targetRotation = initialRotation;
        float step = resetSpeed * Time.deltaTime;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);*/

    }

    IEnumerator ResetCameraRotation()
    {
        float t = 0.0f;
        Quaternion currentRotation = transform.rotation;

        while (t < 1.0f)
        {
            t += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(currentRotation, initialRotation, t);
            yield return null;
        }

        transform.rotation = initialRotation;
        isResetting = false;
    }
}
