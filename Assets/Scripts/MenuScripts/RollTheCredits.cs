using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * @Author Anna Thiele
 * This script translates the credits to "roll the credits"
 **/

public class RollTheCredits : MonoBehaviour
{
    // Transforms to act as start and end markers for the journey.
    public Transform startMarker;
    public Transform endMarker;

    //timer variables
    [SerializeField] private bool wait = false;
    [SerializeField] private float waitSec = 0.0f;
    private bool timer = false;
    private bool doneMoving = false;

    // Movement speed in units per second.
    public float speed = 1.0F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    void Start()
    {
        // Keep a note of the time the movement started.
        startTime = Time.time;

        // Calculate the journey length.
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);

    }

    // Move to the target end position.
    void Update()
    {
        //adds a delay before the journey
        if (!timer && wait && waitSec > 0.0f)
        {
            waitSec -= Time.deltaTime;
            if (waitSec <= 0.0f)
            {
                startTime = Time.time;
                wait = false;
            }
        }

        if (!wait && !doneMoving)
        {
            // Distance moved equals elapsed time times speed..
            float distCovered = (Time.time - startTime) * speed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.
            transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);

            if (transform.position == endMarker.position)
                doneMoving = true;
        }
    }

    public void reverseMove()
    {
        // Keep a note of the time the movement started.
        startTime = Time.time;

        // Calculate the journey length.
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);

        Transform temp = startMarker;
        startMarker = endMarker;
        endMarker = temp;
        doneMoving = false;
        Debug.Log("revers loc");

    }
}
