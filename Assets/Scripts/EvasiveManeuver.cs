using UnityEngine;
using System.Collections;

public class EvasiveManeuver : MonoBehaviour
{

    public float dodge;         
    public float smoothing;     // how quick moves into dodge
    public float tilt;
    public Vector2 startWait;        // startWait range, wait time for evasive manouver
    public Vector2 maneuverTime;
    public Vector2 maneuverWait;
    public Boundary boundary;

    private float currentSpeed;
    private float targetManeuver;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = rb.velocity.z;
        StartCoroutine(Evade());
    }

    IEnumerator Evade()
    {
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));

        while (true)
        {
            // Mathf.sign palauttaa +1 tai -1 riippuen aluksen sijainnista x-akselilla, keskellä on nollakohta
            // "Positiivisella" puoliskolla liike on "negatiiviseen" suuntaan, jotta alus pysyy pelialueella
            targetManeuver = Random.Range(1, dodge) * -Mathf.Sign(transform.position.x);
            yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
            // Reset target = ship is going to nowhere
            targetManeuver = 0;
            yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));
        }
    }

    // Aluksen liikkuminen
    // MoveTowards never exceeds maxDelta, negative value pushes away from target
    // rb.velocity.x = nykyinen sijainti, joka muuttuu
    // targetManeuver = kohde
    // smoothing = 
    // clamp = rajat aluksen liikkeelle
    void FixedUpdate()
    {
        float newManeuver = Mathf.MoveTowards(rb.velocity.x, targetManeuver, Time.deltaTime * smoothing);
        rb.velocity = new Vector3(newManeuver, 0.0f, currentSpeed);
        rb.position = new Vector3
        (
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );
    
        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }
}