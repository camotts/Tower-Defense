using UnityEngine;
using UnityEngine.Networking;

public class Waypoint : MonoBehaviour
{
    private CharacterController _character;
    private int _currWaypoint;
    private Vector3 _moveDirection = Vector3.zero;


    private float _curTime;

    private TerrainGeneration _generation;
    public float LookSpeed = 6;
    public bool Loop = false;
    public float PauseDurration = 0;
    public float Walkspeed = (float).01;
    private Transform[] _waypoint;
    public int Gravity = 3;


    // Use this for initialization
    private void Start()
    {
        _generation = (TerrainGeneration) FindObjectOfType(typeof (TerrainGeneration));
        _character = GetComponent<CharacterController>();
        _waypoint = new Transform[_generation.pathOrder.Count];

        foreach (var tile in _generation.pathOrder)
        {
            _waypoint[_generation.pathOrder.IndexOf(tile)] = new GameObject().transform;
            _waypoint[_generation.pathOrder.IndexOf(tile)].position = new Vector3(tile.x * _generation.tileOffset, 0, tile.y * _generation.tileOffset);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (_currWaypoint < _waypoint.Length)
        {
            Patrol();
        }
        else
        {
            if (Loop)
            {
                _currWaypoint = 0;
            }
        }
    }

    private void Patrol()
    {
        var target = _waypoint[_currWaypoint].position;
        _moveDirection = target - transform.position;
        if (_moveDirection.magnitude < .5)
        {
            if (_curTime == 0)
            {
                _curTime = Time.time; // Pause over the Waypoint
            }
            if ((Time.time - _curTime) >= PauseDurration)
            {
                _currWaypoint++;
                _curTime = 0;

            }
        }
        else
        {

            _moveDirection.y -= Gravity*Time.deltaTime;
            var rotation = Quaternion.LookRotation(target - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * LookSpeed);
            _character.Move(_moveDirection.normalized*Time.deltaTime * Walkspeed);
        }

    }
}