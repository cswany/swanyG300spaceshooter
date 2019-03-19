using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestroyByContact : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string explosionFMOD = "event:/Explosion";
    FMOD.Studio.EventInstance explosionEv;

    private GameController gameController;

    public GameObject explosion;

    private void Start()
    {
        explosionEv = FMODUnity.RuntimeManager.CreateInstance(explosionFMOD);

        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(other.gameObject);
        Destroy(gameObject);
        explosionEv.start();
        gameController.GameOver();
    }

}
