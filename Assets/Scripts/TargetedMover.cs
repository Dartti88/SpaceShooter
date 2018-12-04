using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mover class for homing missile that has a target
/// </summary>
public class TargetedMover : MonoBehaviour {

    private GameObject targetEnemy;

    public float speed;

	void Start ()
    {
        targetEnemy = GameObject.FindWithTag("Enemy");
        //targetEnemy = GameObject.Find("Enemy Ship");
        if (targetEnemy == null)
        {
            Debug.Log("No target");
        }
    }

    void Update () {        
        if (targetEnemy != null)
        {
            float step = speed * Time.deltaTime;
            Transform target = targetEnemy.transform;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }
        else
        {
            GetComponent<Rigidbody>().velocity = transform.forward * speed;
        }
        
    }

    public void Fire()
    {

    }
}
//GameObject temp = GameObject.Find("house");
//Transform houseTransform = temp.GetComponent<Transform>();