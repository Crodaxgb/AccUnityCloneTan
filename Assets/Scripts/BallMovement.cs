using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    Rigidbody2D rigidBodyRef;
    private float ballSpeed = 10;
    void Awake()
    {
        //get the rigidbody reference
        rigidBodyRef = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Sets the velocity of the rigidbody component so that it can move in the specified direction
    /// </summary>
    /// <param name="direction"></param>
    public void SetVelocity(Vector3 direction)
    {

        rigidBodyRef.velocity = direction * ballSpeed;
    }
}
