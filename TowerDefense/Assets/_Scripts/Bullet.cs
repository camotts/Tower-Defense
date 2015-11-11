using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float bulletSpeed = 100;
	public Rigidbody bullet;
	public GameObject tower;
	
	
	void Fire()
	{
		Rigidbody bulletClone = (Rigidbody) Instantiate(bullet, (transform.position + new Vector3(0,0,0)), transform.rotation);

		TowerLogic target = (TowerLogic)transform.parent.GetComponent ("TowerLogic").GetComponent (typeof(TowerLogic));
		bulletClone.transform.LookAt (target.target.transform);
		bulletClone.detectCollisions = false;
		bulletClone.velocity = transform.forward * 5/*bulletSpeed*/;
	}
	
	void Update () 
	{
		if (Input.GetButtonDown("Fire1"))
			Fire();
		//Debug.Log (transform.position);
	}
}
