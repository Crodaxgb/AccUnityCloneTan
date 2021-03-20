using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject startingPoint, ballPrefab;
    public GameObject[] hitBoxes;
    private LineRenderer traceLine;
    private Camera cameraRef;
    private float strechValue = 50, spawnProbability = 0.4f;
    public float spawnBallInterval = 0.25f;
    public float ballCount = 20;
    private bool available = true;
    private int spawnLocationCount = 17, iteratedBall = 0, roundCounter = 0;
    RaycastHit2D hit;
    void Start()
    {
        //Get a reference to the line renderer
        traceLine = startingPoint.GetComponent<LineRenderer>();
        //Set its origin as the shooter object's position
        traceLine.SetPosition(0, startingPoint.transform.position);
        //Get a reference to the camera
        cameraRef = Camera.main;
        //Spawn initial blocks
        SpawnBlocks();
    }

    // Update is called once per frame
    void Update()
    {
        //If the turn is not ready to be played just return from this update
        if (!available)
        {
            traceLine.enabled = false;
            return;
        }
        traceLine.enabled = true;
        //Get the mouse position as pixel value from the screen and then convert it to the world space
        var projectedVector = cameraRef.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        //We're working on 2d make z value disappear
        projectedVector.z = 0;
        //Create a normalized direction from the origin to the mouse position
        var direction = (projectedVector - startingPoint.transform.position).normalized;
        //Cast a ray in the normalized direction from the shooter object position
        hit = Physics2D.Raycast(startingPoint.transform.position, direction);
        //If it hits something
        if(hit.collider != null)
        {
            //Set the line renderer's first position as the collision point
            traceLine.SetPosition(1, hit.point);
            //Calculate the direction of a cross product between hit normal and ray direction
            //This will give us a perpendicular vector (on the z plane) whose direction defines the hit.normal's
            //state. If the hit.normal has an angle larger than the direction it needs to be rotated clock-wise
            Vector3 depthCross = Vector3.Cross(direction, new Vector3(hit.normal.x, hit.normal.y, 0));
            //Get the angle between hit.normal & direction
            float angle = 180 - Vector2.Angle(hit.normal, direction);
            //Reverse the angle if the cross product lays on the +z plane as described above
            if(depthCross.z > 0)
            {
                angle *= -1;
            }
            //Rotate the vector on the xy plane
            Vector3 rotatedVector = Quaternion.Euler(0, 0, angle) * hit.normal;

            /*
             //Visualization
            Debug.DrawRay(hit.point, -direction, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.red);
            Debug.DrawRay(hit.point, rotatedVector, Color.green);
             */
            //If there's a hit increase the linerenderer's point count to three don't use ++ in here
            traceLine.positionCount = 3;
            //Set the last position slightly ahead of the calculated direction
            traceLine.SetPosition(2, new Vector3(hit.point.x, hit.point.y, 0) + rotatedVector * 5);

        }
        else
        {
            //If the ray continues without a hit set the linerenderer count to 2
            traceLine.positionCount = 2;
            //set the next point which will fall over to a point that is out of camera's fov
            traceLine.SetPosition(1, startingPoint.transform.position + direction * strechValue);
        }
        //Calculate the direction's rotation in degrees
        float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Set the shooter's rotation accordingly
        startingPoint.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 45);
        
        //If the mouse click triggered and the game is in an available state
        if(Input.GetKey(KeyCode.Mouse0) && available)
        {
            //Start spawning the balls
            StartCoroutine("SpawnBalls", direction);
            //Close the availability untill all the balls are destroyed
            available = false;
        }

    }

    /// <summary>
    /// Spawns ballCount amount of balls in the specified direction
    /// </summary>
    /// <param name="direction">Spawn direction for the balls</param>
    /// <returns></returns>
    IEnumerator SpawnBalls(Vector3 direction)
    {
        for(int ballIndex=0; ballIndex < ballCount; ballIndex++)
        {
            //Instantiate a ball then get its script & set its velocity
            GameObject instantiatedBall = 
                Instantiate(ballPrefab, startingPoint.transform.position, Quaternion.identity);
            instantiatedBall.GetComponent<BallMovement>().SetVelocity(direction);
            yield return new WaitForSeconds(spawnBallInterval);
        }
    }
    /// <summary>
    /// Spawns the blocks in randomized positions
    /// </summary>
    private void SpawnBlocks()
    {
        //For each point evaluate a random probability
        for(int spawnIterator=0;  spawnIterator<spawnLocationCount; spawnIterator++)
        {
            //If the probability is exceeds the randomized value spawn a block
            if(Random.Range(0.0f, 1.0f)<spawnProbability)
            {
                Vector3 spawnLocation = new Vector3(spawnIterator - 8.5f,4.5f,0);
                Instantiate(hitBoxes[Random.Range(0, hitBoxes.Length)], spawnLocation, Quaternion.identity);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //If the exited object has a ball tag destroy it and count the destroyed object's number
        if (collision.transform.tag.Equals("Ball"))
        {
            Destroy(collision.transform.gameObject);
            iteratedBall++;            
        }
        //If the destroyed object's count is equal to ball count you can start the process of the next round
        if(iteratedBall == ballCount)
        {
            Invoke("MoveBlocksDown", 1f);         
        }
    }

    /// <summary>
    /// Prepares the next round by arranging the required variables and moving every block on the scene 
    /// a bit lower
    /// </summary>
    private void MoveBlocksDown()
    {
        //Reset the destroyed ball count
        iteratedBall = 0;
        //Set the availability to true
        available = true;
        //For each game object which has a 'Block' tag on the scene do the following
        foreach(GameObject currentBlocks in GameObject.FindGameObjectsWithTag("Block"))
        {
            //Decrease their transform by a certain amount so they'll be displayer below their original positions
            currentBlocks.transform.position = 
                new Vector3(currentBlocks.transform.position.x, currentBlocks.transform.position.y - 1 ,0);
        }
        //If the round is an order of five increase the spawn probability
        if(roundCounter % 5 == 0 && roundCounter > 0)
        {
            spawnProbability += 0.05f;
        }
        //Spawn blocks
        SpawnBlocks();
        //Increase the round count
        roundCounter++;
    }
}
