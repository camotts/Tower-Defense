using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public bool active = false;

    private Vector3 _direction;
    private Quaternion _lookRotation;
    public Text currHealth;
    private readonly Queue<Rigidbody> TriggerList = new Queue<Rigidbody>();

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (!active) return;
        if (TriggerList.Count() <= 0) return;

        var collider = TriggerList.Peek();
        if (collider == null)
        {
            TriggerList.Dequeue();
            return;
        }
        var huh = TriggerList.Contains(collider);
        var target = collider;
        _direction = (target.position - transform.position).normalized;
        _lookRotation = Quaternion.LookRotation(_direction);
        _lookRotation.x = 0;
        _lookRotation.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime*1);
        //transform.LookAt (target.transform);

        target.gameObject.GetComponent<Enemy>().Health -= 10;
        var health = target.gameObject.GetComponent<Enemy>().Health;
        if (health <= 0)
        {
            TriggerList.Dequeue();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Enemy") return;
        if (other.tag.Equals("Tower"))
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
        if (other.attachedRigidbody != null && !TriggerList.Contains(other.attachedRigidbody))
        {
            //add the object to the list
            TriggerList.Enqueue(other.attachedRigidbody);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody != null && TriggerList.Contains(other.attachedRigidbody))
        {
            //remove it from the list
            TriggerList.Dequeue();
        }
        if (other.tag.Equals("Tower"))
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