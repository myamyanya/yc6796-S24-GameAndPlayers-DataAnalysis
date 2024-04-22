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
                // DATA: the whole level is finished and player quit the game
                Tinylytics.AnalyticsManager.LogCustomMetric("Finished Game and Quit", System.DateTime.Now.ToString());
                
                Application.Quit();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                // DATA: the whole level is finished and player restart the game to play again
                Tinylytics.AnalyticsManager.LogCustomMetric("Game Restarted", System.DateTime.Now.ToString());
                
                SceneManager.LoadScene("SampleScene");
            }
        }
        
    }
}
