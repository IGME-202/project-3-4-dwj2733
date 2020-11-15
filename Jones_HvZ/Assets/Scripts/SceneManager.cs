using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Dean Jones
/// The scenemanager controls all the objects in the scene and allows the game to function as intended
/// </summary>
public class SceneManager : MonoBehaviour
{
    //variables
    private List<Zombie> zombies;
    private List<GameObject> zombieObjects;
    private List<Human> humans;
    private List<GameObject> humanObjects;
    private List<Obstacle> obstacles;
    private List<GameObject> obstacleObjects;
    public GameObject floor;
    public int zombieCount;
    public int humanCount;
    public int obstacleCount;
    public GameObject zombiePrefab;
    public GameObject humanPrefab;
    public GameObject obstaclePrefab;
    private GameObject zombie;
    private GameObject human;
    private GameObject obstacle;
    public Material forwardMat;
    public Material sideMat;
    public Material targetMat;
    public bool debugMode;

    //properties
    public List<GameObject> Zombies
    {
        get { return zombieObjects; }
    }

    public List<GameObject> Humans
    {
        get { return humanObjects; }
    }

    public List<Obstacle> Obstacles
    {
        get { return obstacles; }
    }

    // Start is called before the first frame update
    void Start()
    {
        //initializes debug mode
        debugMode = false;

        //creates lists of zombies, obstacles and humans, and populates
        zombieObjects = new List<GameObject>();
        humanObjects = new List<GameObject>();
        obstacleObjects = new List<GameObject>();
        for(int i = 0; i<zombieCount; i++)
        {
            zombie = Instantiate(zombiePrefab, new Vector3(Random.Range(floor.transform.localScale.x * -5, floor.transform.localScale.x * 5), 1f, Random.Range(floor.transform.localScale.z * -5, floor.transform.localScale.z * 5)), Quaternion.identity);
            zombieObjects.Add(zombie);
        }
        for (int i = 0; i < humanCount; i++)
        {
            human = Instantiate(humanPrefab, new Vector3(Random.Range(floor.transform.localScale.x * -5, floor.transform.localScale.x * 5), .5f, Random.Range(floor.transform.localScale.z * -5, floor.transform.localScale.z * 5)), Quaternion.identity);
            humanObjects.Add(human);
        }
        for(int i = 0; i<obstacleCount; i++)
        {
            obstacle = Instantiate(obstaclePrefab, new Vector3(Random.Range(floor.transform.localScale.x * -5, floor.transform.localScale.x * 5), .5f, Random.Range(floor.transform.localScale.z * -5, floor.transform.localScale.z * 5)), Quaternion.identity);
            obstacleObjects.Add(obstacle);
        }


        //gets all components from their gameObjects
        zombies = new List<Zombie>();
        for(int i = 0; i<zombieObjects.Count; i++)
        {
            zombies.Add(zombieObjects[i].GetComponent<Zombie>());
        }
        humans = new List<Human>();
        for(int i = 0; i<humanObjects.Count; i++)
        {
            humans.Add(humanObjects[i].GetComponent<Human>());
        }
        obstacles = new List<Obstacle>();
        for(int i = 0; i<obstacleObjects.Count; i++)
        {
            obstacles.Add(obstacleObjects[i].GetComponent<Obstacle>());
        }

        //assigns psg and zombie to humans
        for(int i = 0; i<humans.Count; i++)
        {
            humans[i].manager = this;
            humans[i].TopRight = new Vector3(floor.transform.localScale.x * 5, 0, floor.transform.localScale.z * 5);
            humans[i].BottomLeft = new Vector3(floor.transform.localScale.x * -5, 0, floor.transform.localScale.z * -5);
        }

        //assigns variables to zombies
        for(int i = 0; i<zombies.Count; i++)
        {
            zombies[i].manager = this;
            zombies[i].TopRight = new Vector3(floor.transform.localScale.x * 5, 0, floor.transform.localScale.z * 5);
            zombies[i].BottomLeft = new Vector3(floor.transform.localScale.x * -5, 0, floor.transform.localScale.z * -5);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //toggles debug mode
        if (Input.GetKeyDown(KeyCode.D))
        {
            debugMode = !debugMode;
        }

        //checks for collisions between humans and zombies and converts humans to zombies
        if (zombies.Count>0 && humans.Count > 0)
        {
            for (int i = 0; i < zombies.Count; i++)
            {
                for (int j = 0; j < humans.Count; j++)
                {
                    if (Colliding(zombies[i], humans[j]))
                    {
                        //creates new zombie from the human and adds to lists
                        zombie = Instantiate(zombiePrefab, new Vector3(humanObjects[j].transform.position.x, 1f, humanObjects[j].transform.position.z), humanObjects[j].transform.rotation);
                        zombies.Add(zombie.GetComponent<Zombie>());

                        zombies[zombies.Count - 1].manager = this;
                        zombies[zombies.Count - 1].TopRight = new Vector3(floor.transform.localScale.x * 5, 0, floor.transform.localScale.z * 5);
                        zombies[zombies.Count - 1].BottomLeft = new Vector3(floor.transform.localScale.x * -5, 0, floor.transform.localScale.z * -5);
                        
                        zombieObjects.Add(zombie);

                        //destroys the old human
                        DestroyImmediate(humanObjects[j]);
                        humanObjects.RemoveAt(j);
                        humans.RemoveAt(j);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Checks if 2 vehicles are colliding
    /// </summary>
    /// <param name="zombie">the zombie to be checked</param>
    /// <param name="human">the human to be checked</param>
    /// <returns></returns>
    bool Colliding(Vehicle zombie, Vehicle human)
    {
        return (Mathf.Pow(zombie.Center.x - human.Center.x, 2) + Mathf.Pow(zombie.Center.z - human.Center.z, 2) < Mathf.Pow(zombie.Radius + human.Radius, 2));
    }


    /// <summary>
    /// Displays the input commands for toggling debug lines
    /// </summary>
    void OnGUI()
    {
        GUI.Box(new Rect(5, 5, 200, 25), "Press D to toggle debug lines");
    }
}
