using UnityEngine;
using System.Collections;

public class rolerBall : MonoBehaviour {

	public Transform destination;
	
	private NavMeshAgent agent;
	
	void Start () 
	{
		agent = gameObject.GetComponent<NavMeshAgent>();
		
		agent.SetDestination(destination.position);
	}
	
}
