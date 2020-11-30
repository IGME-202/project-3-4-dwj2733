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
    private List<GameObject> zombieDebugs;
    private List<Human> humans;
    private List<GameObject> humanObjects;
    private List<GameObject> humanDebugs;
    private List<Obstacle> obstacles;
    private List<GameObject> obstacleObjects;
    public GameObject floor;
    public int zombieCount;
    public int humanCount;
    public int obstacleCount;
    public GameObject zombiePrefab;
    public GameObject humanPrefab;
    public GameObject obstaclePrefab;
    public GameObject zombieFuturePrefab;
    public GameObject humanFuturePrefab;
    private GameObject zombie;
    private GameObject zombieDebug;
    private GameObject human;
    private GameObject humanDebug;
    private GameObject obstacle;
    public Material forwardMat;
    public Material sideMat;
    public Material targetMat;
    public bool debugMode;
    public Camera[] cameras;
    private int currentCameraIndex;
    private int trackingIndex;
    private Vector3 upShift;

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

        //misc. variables
        trackingIndex = 0;
        upShift = new Vector3(0, 1, 0);

        //creates lists of zombies, obstacles and humans, and populates
        zombieObjects = new List<GameObject>();
        zombieDebugs = new List<GameObject>();
        zombies = new List<Zombie>();
        humanObjects = new List<GameObject>();
        humanDebugs = new List<GameObject>();
        humans = new List<Human>();
        obstacleObjects = new List<GameObject>();
        obstacles = new List<Obstacle>();
        for (int i = 0; i<zombieCount; i++)
        {
            CreateZombie();
        }
        for (int i = 0; i < humanCount; i++)
        {
            CreateHuman();
        }
        for(int i = 0; i<obstacleCount; i++)
        {
            CreateObstacle();
        }

        //camera controls
        currentCameraIndex = 0;

        // Turn all cameras off, except the first default one
        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        // If any cameras were added to the controller, enable the first one
        if (cameras.Length > 0)
        {
            cameras[0].gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //moves debug objects
        if (debugMode)
        {
            for (int i = 0; i < humanDebugs.Count; i++)
            {
                humanDebugs[i].transform.position = humans[i].FuturePosition + upShift;
            }
            for (int i = 0; i < zombieDebugs.Count; i++)
            {
                zombieDebugs[i].transform.position = zombies[i].FuturePosition + upShift;
            }
        }
        
        //toggles debug mode
        if (Input.GetKeyDown(KeyCode.D))
        {
            debugMode = !debugMode;
            if (debugMode)
            {
                for(int i = 0; i<humanDebugs.Count; i++)
                {
                    humanDebugs[i].SetActive(true);
                }
                for (int i = 0; i < zombieDebugs.Count; i++)
                {
                    zombieDebugs[i].SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i < humanDebugs.Count; i++)
                {
                    humanDebugs[i].SetActive(false);
                }
                for (int i = 0; i<zombieDebugs.Count; i++)
                {
                    zombieDebugs[i].SetActive(false);
                }
            }
        }

        //adds zombie
        if (Input.GetKeyDown(KeyCode.Z))
        {
            CreateZombie();
        }

        //adds human
        if (Input.GetKeyDown(KeyCode.A))
        {
            CreateHuman();
        }

        //removes random zombie
        if (Input.GetKeyDown(KeyCode.X)&&zombieObjects.Count>0)
        {
            DestroyZombie(Random.Range(0,zombieObjects.Count));
        }

        //removes random human
        if (Input.GetKeyDown(KeyCode.S) && humanObjects.Count > 0)
        {
            DestroyHuman(Random.Range(0, humanObjects.Count));
        }

        //adds obstacle
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateObstacle();
        }

        //removes random obstacle
        if (Input.GetKeyDown(KeyCode.W) && obstacleObjects.Count > 0)
        {
            DestroyObstacle(Random.Range(0, obstacleObjects.Count));
        }

        // Press the 1 key to select the first camera
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //deactivates old camera 
            cameras[currentCameraIndex].gameObject.SetActive(false);

            // Sets new camera index
            currentCameraIndex = 0;

            //activates new camera
            cameras[currentCameraIndex].gameObject.SetActive(true);
        }

        // Press the 2 key to select the second camera
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //deactivates old camera 
            cameras[currentCameraIndex].gameObject.SetActive(false);

            // Sets new camera index
            currentCameraIndex = 1;

            //activates new camera
            cameras[currentCameraIndex].gameObject.SetActive(true);
        }

        // Press the 3 key to select the first camera
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //deactivates old camera 
            cameras[currentCameraIndex].gameObject.SetActive(false);

            // Sets new camera index
            currentCameraIndex = 2;

            //activates new camera
            cameras[currentCameraIndex].gameObject.SetActive(true);
        }

        //lowers the tracking index
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            trackingIndex--;
        }

        //raises the tracking index
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            trackingIndex++;
        }

        //updates camera 3 position 
        if(currentCameraIndex == 2 && humans.Count + zombies.Count > 0)
        {
            //constrains the tracking index
            if(trackingIndex >= humans.Count + zombies.Count)
            {
                trackingIndex -= humans.Count + zombies.Count;
            }
            if(trackingIndex < 0)
            {
                trackingIndex += humans.Count + zombies.Count;
            }
            //tracks the appropriate human or zombie
            if(trackingIndex < humans.Count)
            {
                cameras[2].gameObject.transform.position = humans[trackingIndex].transform.position + humans[trackingIndex].transform.forward * .1f + new Vector3(0, 1.5f, 0);
                cameras[2].gameObject.transform.rotation = humans[trackingIndex].transform.rotation;
            }
            else if(trackingIndex < humans.Count + zombies.Count)
            {
                cameras[2].gameObject.transform.position = zombies[trackingIndex - humans.Count].transform.position + zombies[trackingIndex - humans.Count].transform.forward * .2f + new Vector3(0, 1.5f, 0);
                cameras[2].gameObject.transform.rotation = zombies[trackingIndex - humans.Count].transform.rotation;
            }
        }
        //changes to the default camera if no humans or zombies are left
        else if(currentCameraIndex == 2)
        {
            //deactivates old camera 
            cameras[currentCameraIndex].gameObject.SetActive(false);

            // Sets new camera index
            currentCameraIndex = 0;

            //activates new camera
            cameras[currentCameraIndex].gameObject.SetActive(true);
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
                        zombie = Instantiate(zombiePrefab, new Vector3(humanObjects[j].transform.position.x, 0, humanObjects[j].transform.position.z), humanObjects[j].transform.rotation);
                        zombies.Add(zombie.GetComponent<Zombie>());
                        zombieDebug = Instantiate(zombieDebug, zombie.transform.position, zombie.transform.rotation);
                        zombieDebugs.Add(zombieDebug);

                        //prevents the camera from swapping characters if the camera was on a zombie
                        if(trackingIndex >= humans.Count)
                        {
                            trackingIndex--;
                        }

                        zombie.GetComponent<Zombie>().manager = this;
                        zombie.GetComponent<Zombie>().TopRight = new Vector3(floor.transform.localScale.x * 5, 0, floor.transform.localScale.z * 5);
                        zombie.GetComponent<Zombie>().BottomLeft = new Vector3(floor.transform.localScale.x * -5, 0, floor.transform.localScale.z * -5);
                        
                        zombieObjects.Add(zombie);

                        //destroys the old human
                        DestroyHuman(j);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Creates a zombie at a random location
    /// </summary>
    public void CreateZombie()
    {
        zombie = Instantiate(zombiePrefab, new Vector3(Random.Range(floor.transform.localScale.x * -2.5f, floor.transform.localScale.x * 2.5f), 0, Random.Range(floor.transform.localScale.z * -2.5f, floor.transform.localScale.z * 2.5f)), Quaternion.identity);
        zombieObjects.Add(zombie);
        zombies.Add(zombie.GetComponent<Zombie>());
        zombieDebug = Instantiate(zombieFuturePrefab, zombie.transform.position, Quaternion.identity);
        zombieDebugs.Add(zombieDebug);
        zombieDebug.SetActive(false);
        zombie.GetComponent<Zombie>().manager = this;
        zombie.GetComponent<Zombie>().TopRight = new Vector3(floor.transform.localScale.x * 5, 0, floor.transform.localScale.z * 5);
        zombie.GetComponent<Zombie>().BottomLeft = new Vector3(floor.transform.localScale.x * -5, 0, floor.transform.localScale.z * -5);
    }

    /// <summary>
    /// Destroys the zombie at the specified index
    /// </summary>
    /// <param name="index">The index of the zombie to be removed</param>
    public void DestroyZombie(int index)
    {
        DestroyImmediate(zombieObjects[index]);
        DestroyImmediate(zombieDebugs[index]);
        zombieObjects.RemoveAt(index);
        zombies.RemoveAt(index);
        zombieDebugs.RemoveAt(index);
    }

    /// <summary>
    /// creates a human at a random location
    /// </summary>
    public void CreateHuman()
    {
        //prevents the camera from swapping characters if the camera was on a zombie
        if (trackingIndex >= humans.Count)
        {
            trackingIndex++;
        }

        human = Instantiate(humanPrefab, new Vector3(Random.Range(floor.transform.localScale.x * -4.5f, floor.transform.localScale.x * 4.5f), 0, Random.Range(floor.transform.localScale.z * -4.5f, floor.transform.localScale.z * 4.5f)), Quaternion.identity);
        humanObjects.Add(human);
        humans.Add(human.GetComponent<Human>());
        humanDebug = Instantiate(humanFuturePrefab, human.transform.position, Quaternion.identity);
        humanDebugs.Add(humanDebug);
        humanDebug.SetActive(false);
        human.GetComponent<Human>().manager = this;
        human.GetComponent<Human>().TopRight = new Vector3(floor.transform.localScale.x * 5, 0, floor.transform.localScale.z * 5);
        human.GetComponent<Human>().BottomLeft = new Vector3(floor.transform.localScale.x * -5, 0, floor.transform.localScale.z * -5);
    }

    /// <summary>
    /// destroys the human at the specified index
    /// </summary>
    /// <param name="index">The index of the human to be removed</param>
    public void DestroyHuman(int index)
    {
        DestroyImmediate(humanObjects[index]);
        DestroyImmediate(humanDebugs[index]);
        humanObjects.RemoveAt(index);
        humans.RemoveAt(index);
        humanDebugs.RemoveAt(index);
    }

    /// <summary>
    /// Creates an obstacle
    /// </summary>
    public void CreateObstacle()
    {
        obstacle = Instantiate(obstaclePrefab, new Vector3(Random.Range(floor.transform.localScale.x * -4, floor.transform.localScale.x * 4), 0, Random.Range(floor.transform.localScale.z * -4, floor.transform.localScale.z * 4)), Quaternion.identity);
        obstacleObjects.Add(obstacle);
        obstacles.Add(obstacle.GetComponent<Obstacle>());
    }

    /// <summary>
    /// Destroys the human at the specified index
    /// </summary>
    /// <param name="index">The index of the obstacle to be removed</param>
    public void DestroyObstacle(int index)
    {
        DestroyImmediate(obstacleObjects[index]);
        obstacleObjects.RemoveAt(index);
        obstacles.RemoveAt(index);
    }

    /// <summary>
    /// Checks if 2 vehicles are colliding
    /// </summary>
    /// <param name="zombie">the zombie to be checked</param>
    /// <param name="human">the human to be checked</param>
    /// <returns></returns>
    public bool Colliding(Vehicle zombie, Vehicle human)
    {
        return (Mathf.Pow(zombie.Center.x - human.Center.x, 2) + Mathf.Pow(zombie.Center.z - human.Center.z, 2) < Mathf.Pow(zombie.Radius + human.Radius, 2));
    }


    /// <summary>
    /// Displays the input commands for toggling debug lines
    /// </summary>
    void OnGUI()
    {
        GUI.Box(new Rect(5, 5, 200, 115), "Press D to toggle debug lines\nPress A to add Human\nPress S to remove Human\nPress Z to add Zombie\nPress X to remove Zombie\nPress Q to add Tree\nPress W to remove Tree");
        GUI.Box(new Rect(210, 5, 100, 25), "Zombies: " + zombies.Count);
        GUI.Box(new Rect(315, 5, 100, 25), "Humans: " + humans.Count);
        GUI.Box(new Rect(210, 35, 205, 40), "Press 1,2 and 3 to\nchange cameras");

        if(currentCameraIndex == 0)
        {
            GUI.Box(new Rect(210, 80, 205, 25), "Current View: Main Camera");
        }
        if(currentCameraIndex == 1)
        {
            GUI.Box(new Rect(210, 80, 205, 25), "Current View: Top Camera");
        }
        if (currentCameraIndex == 2)
        {
            if (trackingIndex < humans.Count)
            {
                GUI.Box(new Rect(210, 80, 205, 40), "Current View: FPS Camera\nTracking: Human #" + (trackingIndex + 1));
            }
            else
            {
                GUI.Box(new Rect(210, 80, 205, 40), "Current View: FPS Camera\nTracking: Zombie #" + (trackingIndex - humans.Count + 1));
            }
            GUI.Box(new Rect(210, 125, 205, 40), "Use Left and Right Arrow Keys\nto change tracking");
        }

    }
}
