using UnityEngine;
using System.Collections.Generic;

public class TowerControl : MonoBehaviour {

    public GameObject[] Towers;
    public bool Placeable;
    public List<GameObject> collided = new List<GameObject>(); 

    private bool _placing;
    private GameObject currTower;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //airhorn tower
        if (Input.GetKeyDown("q"))
        {
            if (_placing)
            {
                if (!Placeable) return;
                currTower.GetComponent<Tower>().active = true;
                currTower.transform.parent = null;
                currTower = null;
                _placing = false;
            }
            else
            {
                 Debug.Log("DO THE THINGS");
                currTower = Instantiate(Towers[0], transform.position + (transform.forward*5) + -(transform.up*2), transform.rotation) as GameObject;
                currTower.transform.parent = transform;
                
                _placing = true;
            }
            
           
        }
	}

    void LateUpdate()
    {
        if (currTower != null)
        {
            currTower.transform.position = new Vector3(currTower.transform.position.x, 0, currTower.transform.position.z);
        }
    }
}
