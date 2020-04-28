using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 50f;
    [SerializeField] float fuel = 1000f;
    bool playerDead = false;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDead != true)
        {
            Thrust();
            Rotate();
        }
    }
    
    private void OnCollisionEnter(Collision other) {
        if (playerDead != false)
        {
            return;
        }
        switch (other.gameObject.tag)
        {
            case "Friendly":
                print("Ok");
                break;
            case "Fuel":
                print("Refueled");
                fuel = 100;
                break;
            case "Finish":
                Invoke("LoadNextScene", 1f);
                break;
            default:
            playerDead = true;
                Invoke("Restart", 1f);
                break;
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
        playerDead = false;
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space) && fuel > 0)
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            fuel -= 1;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true;
        
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;
    }
}
