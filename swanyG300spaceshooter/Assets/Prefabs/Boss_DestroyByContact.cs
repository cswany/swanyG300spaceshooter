using UnityEngine;
using System.Collections;

public class Boss_DestroyByContact : MonoBehaviour
{
    public GameObject explosion;
    public GameObject playerExplosion;
    public GameObject bossExplosion;
    public int scoreValue;
    private Done_GameController gameController;
    public float health = 5;

    [FMODUnity.EventRef]
    public string explosionFMOD = "event:/Explosion";
    FMOD.Studio.EventInstance explosionEv;

    void Start()
    {
        explosionEv = FMODUnity.RuntimeManager.CreateInstance(explosionFMOD);

        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<Done_GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary" || other.tag == "Enemy")
        {
            return;
        }

        if (health > 1)
        {
            health = health - 1;
            if (other.tag == "Player")
            {
               Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                gameController.GameOver();
                explosionEv.start();
            }
            else
            {
                Instantiate(bossExplosion, other.transform.position, transform.rotation);
                Destroy(other.gameObject);
            }
        }
        else
        {
            if (explosion != null)
            {
                Instantiate(bossExplosion, transform.position, transform.rotation);
                explosionEv.start();
            }

            if (other.tag == "Player")
            {
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                gameController.GameOver();
                explosionEv.start();
            }

            gameController.AddScore(scoreValue);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
