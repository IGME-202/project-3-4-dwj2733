using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dean Jones
/// The Zombie class inherits from vehicle and seeks manager.Humans 
/// </summary>
public class Zombie : Vehicle
{
    //variables
    private GameObject closestHuman;
    
    //base methods
    void Start()
    {
        //sets mass and speed accordingly
        mass = 2f;
        maxSpeed = 6f;

        base.Start();
    }

    void Update()
    {
        base.Update();
    }

    //steering force method

    /// <summary>
    /// finds the closest human and pursues them
    /// </summary>
    public override Vector3 CalcSteeringForces()
    {
        Vector3 steering = new Vector3(0, 0, 0);

        if(manager.Humans.Count > 0)
        {
            //variables to track the closest human
            float minDistance = float.MaxValue;
            float sampleDistance = 0;
            closestHuman = manager.Humans[0];

            //loops through the manager.Humans to find the closest
            for (int i = 0; i < manager.Humans.Count; i++)
            {
                //calculates square of distance
                sampleDistance = Mathf.Pow(this.transform.position.x - manager.Humans[i].transform.position.x, 2f)
                                 + Mathf.Pow(this.transform.position.z - manager.Humans[i].transform.position.z, 2f);

                //adjust variables
                if (sampleDistance < minDistance)
                {
                    minDistance = sampleDistance;
                    closestHuman = manager.Humans[i];
                }
            }

            //seeks the closest human
            steering += Pursue(closestHuman.GetComponent<Human>());

        }
        else
        {
            steering += Wander();
        }

        return steering;

    }

    /// <summary>
    /// Method intended to separate this zombie from other zombies
    /// </summary>
    /// <returns>A steering force that separates this zombie from others</returns>
    public override Vector3 Separate()
    {
        Vector3 steering = new Vector3(0,0,0);
        float distance;

        for(int i = 0; i<manager.Zombies.Count; i++)
        {
            distance = Mathf.Pow(this.transform.position.x - manager.Zombies[i].transform.position.x, 2f)
                        + Mathf.Pow(this.transform.position.z - manager.Zombies[i].transform.position.z, 2f);
            if(distance != 0 && distance <= 8)
            {
                steering += Flee(manager.Zombies[i]) / distance;
            }
        }

        return steering;
    }

    /// <summary>
    /// Draws forward, side and target lines
    /// </summary>
    void OnRenderObject()
    {
        //only draws debug lines if in debug mode
        if(manager != null)
        {
            if (manager.debugMode)
            {
                //sets the forward material
                manager.forwardMat.SetPass(0);

                //draws the forward line
                GL.Begin(GL.LINES);
                GL.Vertex(position + upShift);
                GL.Vertex(position + transform.forward * 2f + upShift);
                GL.End();

                //sets the side material
                manager.sideMat.SetPass(0);

                //draws the side line
                GL.Begin(GL.LINES);
                GL.Vertex(position + upShift);
                GL.Vertex(position + Quaternion.Euler(0, 90, 0) * transform.forward * 2f + upShift);
                GL.End();

                if (manager.Humans.Count > 0 && closestHuman != null)
                {
                    //sets the target material
                    manager.targetMat.SetPass(0);

                    //draws the target line
                    GL.Begin(GL.LINES);
                    GL.Vertex(position + upShift);
                    GL.Vertex(closestHuman.GetComponent<Human>().FuturePosition + upShift);
                    GL.End();
                }
            }

        }
        
    }
}
