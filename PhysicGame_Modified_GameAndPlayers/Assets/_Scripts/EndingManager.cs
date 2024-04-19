using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    Scene currentScene;

    public string lastClientTag;

    public GameObject animalHolder;

    /*public GameObject lizzard;
    public GameObject fish;
    public GameObject rat;
    public GameObject deer;
    public GameObject bird;
    public GameObject snake;*/

    GameObject endingAnimal;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        endingAnimal = GameObject.FindGameObjectWithTag("Fish");
    }

    // Update is called once per frame
    void Update()
    {
        // For checking which scene the player is
        currentScene = SceneManager.GetActiveScene();

        if (currentScene.buildIndex == 1)
        {
            Debug.Log(lastClientTag);

            if (lastClientTag != null)
            {
                endingAnimal = GameObject.FindGameObjectWithTag(lastClientTag);
                endingAnimal.GetComponent<SkinnedMeshRenderer>().enabled = true;
                //Debug.Log(endingAnimal);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Application.Quit();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
        
    }
}
