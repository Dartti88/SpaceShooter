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

        // Collision with player
        if (other.tag == "Player")
        {
            // Asteroid exlposion, scorevalue gets added, asteroid is destroyed
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            gameController.AddScore(scoreValue);
            Destroy(gameObject);
            // Player takes damage, if hp goes to 0 player ship gets destroyed and game ends
            playerController.damage(1);
            if (playerController.getHp() == 0)
            {
                Destroy(other.gameObject);
                gameController.GameOver();
            }
        }
        // When other, non-player, object game object hits object
        else
        {
            gameController.AddScore(scoreValue);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}