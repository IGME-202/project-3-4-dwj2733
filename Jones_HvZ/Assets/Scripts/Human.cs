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

    //base methods
    void Start()
    {
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
    /// Flees the zombie and seeks the psg. Additionally flees the closest human to avoid clustering
    /// </summary>
    public override void CalcSteeringForces()
    {
        //variables to track the closest zombie
        float minDistance = float.MaxValue;
        float sampleDistance = 0;
        closestZombie = manager.Zombies[0];

        //loops through the humans to find the closest
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
           25)
        {
            Flee(closestZombie);
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
        GL.Vertex(position + velocity.normalized * 1.5f);
        GL.End();

        //sets the side material
        manager.sideMat.SetPass(0);

        //draws the side line
        GL.Begin(GL.LINES);
        GL.Vertex(position);
        GL.Vertex(position + Quaternion.Euler(0, 90, 0) * velocity.normalized * 1.5f);
        GL.End();
    }
}
