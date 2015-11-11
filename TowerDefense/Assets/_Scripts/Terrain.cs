using UnityEngine;
using System.Collections;

public class Terrain : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Tower" && !other.GetType().ToString().Equals("UnityEngine.SphereCollider"))
        {

            var tower = GameObject.FindGameObjectWithTag("Player").GetComponent<TowerControl>();
            tower.Placeable = false;
            tower.collided.Add(this.gameObject);
            var renderers = other.gameObject.GetComponentsInChildren<Renderer>();
            foreach (var render in renderers)
            {
                render.material.color = Color.red;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Tower" && !other.GetType().ToString().Equals("UnityEngine.SphereCollider"))
        {
            var tower = GameObject.FindGameObjectWithTag("Player").GetComponent<TowerControl>();

            tower.Placeable = true;
            tower.collided.Remove(this.gameObject);
            if (tower.collided.Count == 0)
            {
                var renderers = other.gameObject.GetComponentsInChildren<Renderer>();
                foreach (var render in renderers)
                {
                    render.material.color = Color.blue;
                }
            }
            
        }
    }
}
