using UnityEngine;
using System.Collections;

public class FSM : MonoBehaviour 
{
	//Player Transform
	protected Transform playerTransform;
	protected Transform enumyTransform;
	protected GameObject enumyPlayer;
	protected GameObject objPlayer;
	
	//Next destination position of the NPC Tank
	protected Vector3 destPos;
	protected Vector3 enumyPos;
	
	//List of points for patrolling
	protected GameObject[] pointList;
	protected GameObject[] pointList2;
	protected GameObject[] pointList3;
	protected GameObject[] pointList4;
	protected GameObject[] pointList5;
	
	//Bullet shooting rate
	//protected float shootRate;
	protected float elapsedTime;
	protected float elapsedTimeA;

	
	//Tank Turret
	//public Transform turret { get; set; }
	//public Transform bulletSpawnPoint { get; set; }
	
	protected virtual void Initialize() { }
	protected virtual void FSMUpdate() { }
	protected virtual void FSMFixedUpdate() { }
	
	// Use this for initialization
	void Start () 
	{
		Initialize();
	}
	
	// Update is called once per frame
	void Update () 
	{
		FSMUpdate();
	}
	
	void FixedUpdate()
	{
		FSMFixedUpdate();
	}    
}