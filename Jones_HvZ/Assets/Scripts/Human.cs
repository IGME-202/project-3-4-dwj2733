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
    private GameObject closestZombie;

    //base methods
    void Start()
    {
        //sets mass and speed accordingly
        mass = 1f;
        maxSpeed = 8f;

        base.Start();
    }

    void Update()
    {
        base.Update();
    }

    //steering force method

    /// <summary>
    /// Evades the closest zombie if one is in range, otherwise wanders
    /// </summary>
    public override Vector3 CalcSteeringForces()
    {
        Vector3 steering = new Vector3(0, 0, 0);

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



                //only flees the zombie if the zombie is close enough
                if (Mathf.Pow(manager.Zombies[i].transform.position.x - this.transform.position.x, 2f) +
                   Mathf.Pow(manager.Zombies[i].transform.position.z - this.transform.position.z, 2f) <=
                   100)
                {
                    steering += Evade(manager.Zombies[i].GetComponent<Zombie>());
                }

                //adjust variables
                if (sampleDistance < minDistance)
                {
                    minDistance = sampleDistance;
                    closestZombie = manager.Zombies[i];
                }
            }
            //if the closest zombie isnt close enough, finds a random point in the park to walk to
            if(minDistance > 100)
            {
                steering += Wander();
            }
        }
        else
        {
            steering += Wander();
        }

        return steering;
    }

    /// <summary>
    /// Method intended to separate this human from other humans
    /// </summary>
    /// <returns>A steering force that separates this human from others</returns>
    public override Vector3 Separate()
    {
        Vector3 steering = new Vector3(0, 0, 0);
        float distance;

        for (int i = 0; i < manager.Humans.Count; i++)
        {
            distance = Mathf.Pow(this.transform.position.x - manager.Humans[i].transform.position.x, 2f)
                        + Mathf.Pow(this.transform.position.z - manager.Humans[i].transform.position.z, 2f);
            if (distance != 0 && distance <= 8)
            {
                steering += Flee(manager.Humans[i]) / distance;
            }
        }

        return steering;
    }

    /// <summary>
    /// Draws forward and side lines
    /// </summary>
    void OnRenderObject()
    {
        //only draws debug lines if in debug mode
        if (manager.debugMode)
        {
            //sets the forward material
            manager.forwardMat.SetPass(0);

            //draws the forward line
            GL.Begin(GL.LINES);
            GL.Vertex(position + upShift);
            GL.Vertex(position + transform.forward * 1.5f + upShift);
            GL.End();

            //sets the side material
            manager.sideMat.SetPass(0);

            //draws the side line
            GL.Begin(GL.LINES);
            GL.Vertex(position + upShift);
            GL.Vertex(position + Quaternion.Euler(0, 90, 0) * transform.forward * 1.5f + upShift);
            GL.End();
        }
    }
}
