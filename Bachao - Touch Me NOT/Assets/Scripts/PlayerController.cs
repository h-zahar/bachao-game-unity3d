using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Declaring Rigidbody Component variable
    //private Rigidbody playerRb;

    // Private Variables to fix the speed and bounary
    private float speed = 20.0f;
    private float zBound = 14.5f;

    public GameObject powerupIndicator;

    public bool isTriggered;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // Getting Rigidbody Component to the variable
        // playerRb = GetComponent<Rigidbody>();

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameManager.isGameOn)
        {
            // Calling the method for player control
            PlayerMovement();

            // Calling the method for restricting area
            PlayerConstrains();
        }
    }

    // Method Created to get control over Player
    public void PlayerMovement()
    {

        // Declaring and Assigning Input Manager
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Adding force with the keyCodes refered in Input Manager using rigidbody.Addforce()
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed);
        transform.Translate(Vector3.forward * verticalInput * Time.deltaTime * speed);

        powerupIndicator.transform.position = transform.position;


    }

    // Method Created to restrict the boundary
    public void PlayerConstrains()
    {
        // If position of player towards z Axis gets over the z positive point
        if (transform.position.z >= zBound)
        {
            // Setting transform.position as current position; setting z axis to positive bound 
            transform.position = new Vector3(transform.position.x, transform.position.y, zBound);
        }

        // If position of player towards z Axis gets over the z negetive point
        if (transform.position.z <= -zBound)
        {
            // Setting transform.position as current position; setting z axis to negetive bound 
            transform.position = new Vector3(transform.position.x, transform.position.y, -zBound);
        }


    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isTriggered)
        {
            Destroy(gameObject);

            gameManager.isGameOver = true;
            Debug.Log("GAME OVER");
        }


        if (collision.gameObject.CompareTag("Electric Bar"))
        {
            Destroy(gameObject);

            powerupIndicator.SetActive(false);
            Debug.Log("GAME OVER");

            gameManager.isGameOver = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            isTriggered = true;
            powerupIndicator.SetActive(true);
            StartCoroutine(Powerupactivity());
        }
    }

    IEnumerator Powerupactivity()
    {
        yield return new WaitForSeconds(5);
        isTriggered = false;
        powerupIndicator.SetActive(false);
    }

}