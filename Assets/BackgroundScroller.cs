using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour {

    public float scrollSpeed;
    public float tileSizeZ; // Background length in z-axis

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        
    }

        
    /// <summary>
    /// Mathf.Repeat loops a value t so that t is between 0 and given length.
    /// Jos t > length niin arvo on rajojen sisään jäävä jakojäännös. 
    /// 
    /// </summary>
	void Update () {
        // Time.time = time in game
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeZ);
        transform.position = startPosition + Vector3.forward * newPosition;
	}
}
