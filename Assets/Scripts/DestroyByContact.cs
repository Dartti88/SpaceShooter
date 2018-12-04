using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour
{
    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue;
    private GameController gameController;
    private PlayerController playerController;
    void Start()
    {
        gameController = GameObject.FindObjectOfType<GameController>();
        if (gameController == null)
        {
            Debug.Log("Cannot Find 'GameController' script");
        }
        // Haetaan player controller
        playerController = GameObject.FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.Log("Cannot find 'PlayerController' script");
        }
    }

    // Other viittaa asteroidiin tai vihollisen boltiin törmäävään peliobjektiin (pelaaja tai projektiili) [voi tehdä myös other.CompareTag("xxx")]
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary" || other.tag == "Enemy")
        {
            return;
        }

        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }


        // Alkuperäinen koodi asteroidi-pelaaja törmäyksille
        /*
        if (other.tag == "Player")
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            gameController.GameOver();
        }
        gameController.AddScore(scoreValue);
        Destroy(other.gameObject);
        Destroy(gameObject);
        */

        // Eliaksen setit

        if (other.tag == "Player")
        {
            Debug.Log("osuma");
            // Asteroidi räjähtää, saadaan pisteet
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            gameController.AddScore(scoreValue);
            Destroy(gameObject);
            // Pelaaja ottaa osumaa, jos hp nollaan peli päättyy
            playerController.damage(1);
            if (playerController.getHp() == 0)
            {
                Destroy(other.gameObject);
                gameController.GameOver();
            }
        }
        // Muut tapaukset (asteroidiin osuu esim ammus)
        /*
        else
        {
            Instantiate(explosion, transform.position, transform.rotation);
            enemyHealth.damage(1);
            if (enemyHealth.getHp() == 0)
            {
                Destroy(other.gameObject);
                gameController.AddScore(scoreValue);
                Destroy(gameObject);
            }
        }
        */
    }
}

