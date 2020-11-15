using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Dean Jones
//The class holds properties necessary for obstacle avoidance
public class Obstacle : MonoBehaviour
{
    //properties

    /// <summary>
    /// calculates the radius as the average of the x and z scales
    /// </summary>
    public float Radius
    {
        get { return (transform.localScale.x + transform.localScale.z) / 4; }
    }

    /// <summary>
    /// returns the center as the position of the obstacle
    /// </summary>
    public Vector3 Center
    {
        get { return transform.position; }
    }
}
