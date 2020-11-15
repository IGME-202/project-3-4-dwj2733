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
    public SceneManager manager;

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
        get { return bottomLeft; }
    }

    public Vector3 TopRight
    {
        set { topRight = value; }
        get { return topRight; }
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
        get { return transform.position; }
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
        ApplyForce(CalcSteeringForces());

        //Calls Reynolds Obstacle Avoidance method
        ApplyForce(ObstacleAvoid());

        //applies friction if it is on
        if (frictionOn)
        {
            ApplyFriction(0f);
        }

        //steers to keep the object on the map
        ApplyForce(StayInBounds());

        //adds acceleration to velocity, standardized to time
        velocity += acceleration * Time.deltaTime;
        if(velocity.magnitude > maxSpeed)
        {
            velocity.Normalize();
            velocity *= maxSpeed;
        }

        //adds velocity to position, standardized to time
        position += velocity * Time.deltaTime;

        //resets acceleration
        acceleration = Vector3.zero;

        //fixes position of vehicle
        transform.position = position;

        //fixes rotation of vehicle
        if(velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(velocity);
        }
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
    public Vector3 Seek(Vector3 direction)
    {
        Vector3 desiredVelocity = Vector3.Normalize(direction) * maxSpeed;
        return desiredVelocity - velocity;
    }

    /// <summary>
    /// Seeks in the direction of a target by determining the direction vector, then calling seek in that direction.
    /// </summary>
    /// <param name="target">The game object to seek</param>
    public Vector3 Seek(GameObject target)
    {
        Vector3 direction = target.transform.position - this.transform.position;
        direction.y = 0;
        return Seek(direction);
    }

    /// <summary>
    /// Flees in the opposite direction of a given vector
    /// </summary>
    /// <param name="direction">The vector between the current object and the target object</param>
    public Vector3 Flee(Vector3 direction)
    {
        Vector3 desiredVelocity = Vector3.Normalize(direction) * maxSpeed;
        desiredVelocity *= -1;
        return desiredVelocity - velocity;
    }

    /// <summary>
    /// Flees in the opposite direction of a target by determining the direction vector, then calling seek in that direction.
    /// </summary>
    /// <param name="target">The game object to seek</param>
    public Vector3 Flee(GameObject target)
    {
        Vector3 direction = target.transform.position - this.transform.position;
        direction.y = 0;
        return Flee(direction);
    }

    /// <summary>
    /// Abstract method to be implemented in the other classes
    /// </summary>
    public abstract Vector3 CalcSteeringForces();

    /// <summary>
    /// applies forces to prevent the object from leaving the map
    /// </summary>
    public Vector3 StayInBounds()
    {
        Vector3 inbounds = new Vector3(0, 0, 0);

        //checks if the object is close to a wall, and if close enough, applies a force towards the center of the map
        if(position.x < bottomLeft.x + 5)
        {
            inbounds += (new Vector3(Mathf.Pow(5-(position.x-bottomLeft.x), 2f), 0, 0) * (1 + acceleration.magnitude/10));
        }
        if(position.z < bottomLeft.z + 5)
        {
            inbounds += (new Vector3(0, 0, Mathf.Pow(5-(position.z - bottomLeft.z), 2f)) * (1 + acceleration.magnitude / 10));
        }
        if(position.x > topRight.x - 5)
        {
            inbounds += (new Vector3(-Mathf.Pow(5-(topRight.x - position.x), 2f), 0, 0) * (1 + acceleration.magnitude / 10));
        }
        if(position.z > topRight.z - 5)
        {
            inbounds += (new Vector3(0, 0, -Mathf.Pow(5-(topRight.z - position.z), 2f)) * (1 + acceleration.magnitude / 10));
        }

        return inbounds;
    }

    /// <summary>
    /// Method to avoid obstacles using reynolds obstacle avoidance 
    /// </summary>
    /// <returns>a steering vector that steers the vehicle away from obstacles</returns>
    public Vector3 ObstacleAvoid()
    {
        Vector3 forward = this.transform.forward;
        Vector3 right = Quaternion.Euler(0,90,0) * this.transform.forward;
        Vector3 obstacle;
        Vector3 steering = new Vector3(0, 0, 0);
        float forwardDot;
        float rightDot;
        float strength;

        //loops through obstacles
        for(int i = 0; i<manager.Obstacles.Count; i++)
        {
            //sets variables
            obstacle = manager.Obstacles[i].Center - this.Center;
            forwardDot = forward.x * obstacle.x + forward.z * obstacle.z;
            rightDot = right.x * obstacle.x + right.z * obstacle.z;

            //ignores obstacles that are behind or too far away
            if (forwardDot > 0 && forwardDot < maxSpeed/mass)
            {
                //ignores obstacles that are not in the objects path
                if(Mathf.Abs(rightDot) < this.Radius + manager.Obstacles[i].Radius)
                {
                    strength = (maxSpeed/mass/2 + 1 + this.Radius + manager.Obstacles[i].Radius) / (maxSpeed/mass + 1);
                    steering += (this.transform.rotation * new Vector3(rightDot * -1, 0, 0)).normalized * maxSpeed * strength; 
                }
            }

        }

        return steering;
    }
}