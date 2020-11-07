using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dean Jones
/// The human class runs away from the zombie in the scene and seeks the PSG
/// </summary>
public class Human : Vehicle
{
    //variables
    public SceneManager manager;
    private GameObject closestZombie;
    private Vector3 targetPoint;

    //base methods
    void Start()
    {
        //sets mass and speed accordingly
        mass = 1f;
        maxSpeed = 8f;

        //calls base start
        base.Start();

        //sets an initial target point
        targetPoint = new Vector3(Random.Range(BottomLeft.x + 5, TopRight.x - 5), .5f, Random.Range(BottomLeft.z + 5, TopRight.z - 5));
    }

    void Update()
    {
        base.Update();
    }

    //steering force method

    /// <summary>
    /// Flees the zombie and seeks the psg. Additionally flees the closest human to avoid clustering
    /// </summary>
    public override void CalcSteeringForces()
    {
        if(manager.Zombies.Count > 0)
        {
            //variables to track the closest zombie
            float minDistance = float.MaxValue;
            float sampleDistance = 0;
            closestZombie = manager.Zombies[0];

            //loops through the zombies to find the closest
            for (int i = 0; i < manager.Zombies.Count; i++)
            {
                //calculates square of distance
                sampleDistance = Mathf.Pow(this.transform.position.x - manager.Zombies[i].transform.position.x, 2f)
                                 + Mathf.Pow(this.transform.position.z - manager.Zombies[i].transform.position.z, 2f);

                //adjust variables
                if (sampleDistance < minDistance)
                {
                    minDistance = sampleDistance;
                    closestZombie = manager.Zombies[i];
                }
            }

            //only flees the zombie if the zombie is close enough
            if (Mathf.Pow(closestZombie.transform.position.x - this.transform.position.x, 2) +
               Mathf.Pow(closestZombie.transform.position.z - this.transform.position.z, 2) <=
               100)
            {
                Flee(closestZombie);
            }
            //if the closest zombie isnt close enough, finds a random point in the park to walk to
            else
            {
                //if close enough to the target point or a target point doesnt exist, generates a new target point
                if (Mathf.Pow(targetPoint.x - transform.position.x, 2) + Mathf.Pow(targetPoint.z - transform.position.z, 2) <= 1)
                {
                    targetPoint = new Vector3(Random.Range(BottomLeft.x + 5, TopRight.x - 5), .5f, Random.Range(BottomLeft.z + 5, TopRight.z - 5));
                }
                Seek(targetPoint - position);
            }

            //adjusts speed
            maxSpeed = 1000 / (Mathf.Pow(closestZombie.transform.position.x - this.transform.position.x, 2) +
               Mathf.Pow(closestZombie.transform.position.z - this.transform.position.z, 2));
            if (maxSpeed > 8f)
            {
                maxSpeed = 8f;
            }
            if (maxSpeed < 4f)
            {
                maxSpeed = 4f;
            }
        }
        else
        {
            //if close enough to the target point or a target point doesnt exist, generates a new target point
            if (Mathf.Pow(targetPoint.x - transform.position.x, 2) + Mathf.Pow(targetPoint.z - transform.position.z, 2) <= 1)
            {
                targetPoint = new Vector3(Random.Range(BottomLeft.x, TopRight.x), .5f, Random.Range(BottomLeft.z, TopRight.z));
            }
            Seek(targetPoint - position);
        }
    }

    /// <summary>
    /// Draws forward and side lines
    /// </summary>
    void OnRenderObject()
    {
        //sets the forward material
        manager.forwardMat.SetPass(0);

        //draws the forward line
        GL.Begin(GL.LINES);
        GL.Vertex(position);
        GL.Vertex(position + transform.forward * 1.5f);
        GL.End();

        //sets the side material
        manager.sideMat.SetPass(0);

        //draws the side line
        GL.Begin(GL.LINES);
        GL.Vertex(position);
        GL.Vertex(position + Quaternion.Euler(0, 90, 0) * transform.forward * 1.5f);
        GL.End();
    }
}
