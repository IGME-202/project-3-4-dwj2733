using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dean Jones
/// The vehicle class handles application of forces and movement of all objects using the vehicle class.
/// </summary>
public abstract class Vehicle : MonoBehaviour
{
    //variables
    protected float mass;
    protected Vector3 position;
    protected Vector3 velocity;
    protected Vector3 acceleration;
    public bool frictionOn;
    protected float maxSpeed;

    //camera variables
    private Vector3 bottomLeft;
    private Vector3 topRight;

    //properties
    public float Mass
    {
        get { return mass; }
        set { mass = value; }
    }

    public Vector3 BottomLeft
    {
        set { bottomLeft = value; }
    }

    public Vector3 TopRight
    {
        set { topRight = value; }
    }

    /// <summary>
    /// calculates the radius as the average of the x and z scales
    /// </summary>
    public float Radius
    {
        get { return (transform.localScale.x + transform.localScale.z) / 4; }
    }

    /// <summary>
    /// returns the center as the position of the vehicle
    /// </summary>
    public Vector3 Center
    {
        get { return position; }
    }

    //Start is called once when the object is created
    protected void Start()
    {
        position = transform.position;
        velocity = Vector3.zero;
        acceleration = Vector3.zero;
        frictionOn = false;
    }

    // Update is called once per frame
    protected void Update()
    {
        //Calls steering force method
        CalcSteeringForces();

        //applies friction if it is on
        if (frictionOn)
        {
            ApplyFriction(0f);
        }

        //adds acceleration to velocity, standardized to time
        velocity += acceleration * Time.deltaTime;
        if(velocity.magnitude > maxSpeed)
        {
            velocity.Normalize();
            velocity *= maxSpeed;
        }

        //applies the bounce to keep the object on screen
        Bounce();

        //adds velocity to position, standardized to time
        position += velocity * Time.deltaTime;

        //resets acceleration
        acceleration = Vector3.zero;

        //fixes position of vehicle
        transform.position = position;

        //fixes rotation of vehicle
        transform.rotation = Quaternion.LookRotation(velocity);
    }

    //methods

    /// <summary>
    /// adds a force to the vehicles acceleration vector
    /// </summary>
    /// <param name="force">the direction and magnitude of the force to be added</param>
    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    /// <summary>
    /// applies a force in the direction opposite to velocity
    /// </summary>
    /// <param name="coeffecient">the magnitude of the friction vector</param>
    public void ApplyFriction(float coeffecient)
    {
        //sets the friction vector in the opposite direction of velocity and normalizes
        Vector3 friction = -1 * velocity;
        friction.Normalize();

        //scales the friction vector and applies it
        friction = friction * coeffecient;
        acceleration += friction;

    }

    /// <summary>
    /// bounces the vehicle so it always remains on screen
    /// </summary>
    public void Bounce()
    {
        //checks each boundary and wraps accordingly
        if (position.x < bottomLeft.x)
        {
            velocity.x = Mathf.Abs(velocity.x) * .9f;
        }
        if (position.z < bottomLeft.z)
        {
            velocity.z = Mathf.Abs(velocity.z) * .9f;
        }
        if (position.x > topRight.x)
        {
            velocity.x = -1 * Mathf.Abs(velocity.x) * .9f;
        }
        if (position.z > topRight.z)
        {
            velocity.z = -1 * Mathf.Abs(velocity.z) * .9f;
        }
    }

    //steering methods

    /// <summary>
    /// Seeks in the direction of a given vector
    /// </summary>
    /// <param name="direction">The vector between the current object and the target object</param>
    public void Seek(Vector3 direction)
    {
        Vector3 desiredVelocity = Vector3.Normalize(direction) * maxSpeed;
        ApplyForce(desiredVelocity - velocity);
    }

    /// <summary>
    /// Seeks in the direction of a target by determining the direction vector, then calling seek in that direction.
    /// </summary>
    /// <param name="target">The game object to seek</param>
    public void Seek(GameObject target)
    {
        Vector3 direction = target.transform.position - this.transform.position;
        direction.y = 0;
        Seek(direction);
    }

    /// <summary>
    /// Flees in the opposite direction of a given vector
    /// </summary>
    /// <param name="direction">The vector between the current object and the target object</param>
    public void Flee(Vector3 direction)
    {
        Vector3 desiredVelocity = Vector3.Normalize(direction) * maxSpeed;
        desiredVelocity *= -1;
        ApplyForce(desiredVelocity - velocity);
    }

    /// <summary>
    /// Flees in the opposite direction of a target by determining the direction vector, then calling seek in that direction.
    /// </summary>
    /// <param name="target">The game object to seek</param>
    public void Flee(GameObject target)
    {
        Vector3 direction = target.transform.position - this.transform.position;
        direction.y = 0;
        Flee(direction);
    }

    /// <summary>
    /// Abstract method to be implemented in the other classes
    /// </summary>
    public abstract void CalcSteeringForces();

}