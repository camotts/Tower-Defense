using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic; 

public class TowerLogic : MonoBehaviour {
	public Text currHealth;
	public Rigidbody target;

	private Quaternion _lookRotation;
	private Vector3 _direction;
	private List<Collider> TriggerList   = new List<Collider>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
			_direction = (target.position - transform.position).normalized;
			_lookRotation = Quaternion.LookRotation (_direction);
			_lookRotation.x = 0;
			_lookRotation.z = 0;
			transform.rotation = Quaternion.Slerp (transform.rotation, _lookRotation, Time.deltaTime * 1);
			//transform.LookAt (target.transform);
		} else {
			if(TriggerList.Count > 0){
				target = TriggerList[0].attachedRigidbody;
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Enemy") {
			if (target == null) {
				currHealth.text = "tower hit";
				target = other.attachedRigidbody;
			}
			if (!TriggerList.Contains (other)) {
				//add the object to the list
				TriggerList.Add (other);
			}
		}
	}

	void OnTriggerExit(Collider other){
		if (other.attachedRigidbody == target) {
			target=null;
		}
		if(TriggerList.Contains(other))
		{
			//remove it from the list
			TriggerList.Remove(other);
		}
	}
}
