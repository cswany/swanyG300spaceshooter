using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}


public class PlayerController : MonoBehaviour
{

    [FMODUnity.EventRef]
    public string pewpew = "event:/Shoot";
    FMOD.Studio.EventInstance pewpewEv;
    public string explosionFMOD = "event:/Explosion";
    FMOD.Studio.EventInstance explosionEv;

    public float speed;
    public float tilt;
    Rigidbody rigidbody;
    public Boundary boundary;
    public GameObject explosion;
    public GameController gameController;

    public GameObject shot;
    public Transform shotSpawn;

    public float fireRate;
    private float nextFire;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        if (gameController != null)
        {
            gameController = gameController.GetComponent<GameController>();
        }
    }


    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            pewpewEv = FMODUnity.RuntimeManager.CreateInstance(pewpew);
            pewpewEv.start();
        }

    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rigidbody.velocity = movement * speed;

        rigidbody.position = new Vector3(Mathf.Clamp(rigidbody.position.x, boundary.xMin, boundary.xMax), 0.0f, Mathf.Clamp(rigidbody.position.z, boundary.zMin, boundary.zMax));

        rigidbody.rotation = Quaternion.Euler(0.0f, 0.0f, rigidbody.velocity.x * -tilt);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary")
        {
            return;
        }

        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            explosionEv.start();
        }

        if (other.tag == "Enemy")
        {
            Instantiate(explosion, other.transform.position, other.transform.rotation);
            gameController.GameOver();
            explosionEv.start();
        }

        Destroy(other.gameObject);
        gameController.GameOver();
        Destroy(gameObject);

    }
}