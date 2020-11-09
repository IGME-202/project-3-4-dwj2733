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
    public SceneManager manager;
    private GameObject closestHuman;
    //base methods
    void Start()
    {
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
    /// finds the closest human and seeks them
    /// </summary>
    public override void CalcSteeringForces()
    {
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
            Seek(closestHuman);

        }
        else
        {
            ApplyFriction(.9f);
        }

    }

    /// <summary>
    /// Draws forward, side and target lines
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
            GL.Vertex(position);
            GL.Vertex(position + transform.forward * 2f);
            GL.End();

            //sets the side material
            manager.sideMat.SetPass(0);

            //draws the side line
            GL.Begin(GL.LINES);
            GL.Vertex(position);
            GL.Vertex(position + Quaternion.Euler(0, 90, 0) * transform.forward * 2f);
            GL.End();

            if (manager.Humans.Count > 0 && closestHuman != null)
            {
                //sets the target material
                manager.targetMat.SetPass(0);

                //draws the target line
                GL.Begin(GL.LINES);
                GL.Vertex(position);
                GL.Vertex(closestHuman.transform.position);
                GL.End();
            }
        }
    }
}
