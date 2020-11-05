using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dean Jones
/// The psg class simply teleports whenever a human gets too close to it
/// </summary>
public class PSG : MonoBehaviour
{
    //variables
    public List<GameObject> humans;
    public GameObject floor;

    // Update is called once per frame
    void Update()
    {
        //variables to track the closest human
        float minDistance = float.MaxValue;
        float sampleDistance = 0;

        //loops through the humans to find the closest
        for (int i = 0; i < humans.Count; i++)
        {
            //calculates square of distance
            sampleDistance = Mathf.Pow(this.transform.position.x - humans[i].transform.position.x, 2f)
                             + Mathf.Pow(this.transform.position.z - humans[i].transform.position.z, 2f);

            //adjust variables
            if (sampleDistance < minDistance)
            {
                minDistance = sampleDistance;
            }
        }

        if (minDistance <= 1)
        {
            Teleport();
        }
    }

    /// <summary>
    /// Teleports to a new location on the floor
    /// </summary>
    public void Teleport()
    {
        this.transform.position = new Vector3(Random.Range(floor.transform.localScale.x * -5, floor.transform.localScale.x * 5),
                                              transform.position.y, 
                                              Random.Range(floor.transform.localScale.z * -5, floor.transform.localScale.z * 5));
    }
}
