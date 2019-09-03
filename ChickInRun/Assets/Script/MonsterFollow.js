#pragma strict

private var wayPoint :GameObject;
private var wayPointPos : Vector3;
private var wayPointRot : Quaternion;
//private var dir :Vector3;
 
 

 private var speed : float= 2.5;
 private var MaxDistance: float =6.0;
 
 
 function Start ()
 {
      wayPoint = GameObject.Find("wayPoint");
 }
 
 function Update ()
 {
 	//dir = ((wayPoint.position) - (transform.position));
 	//dir.z = 0.0f;
 	
 	wayPointPos = new Vector3(wayPoint.transform.position.x, transform.position.y, wayPoint.transform.position.z);
 	//wayPointRot = Quaternion.LookRotation (-wayPointPos, Vector3.up);
 	
 	

 	
 	
 	if(Vector3.Distance(transform.position,wayPoint.transform.position)<MaxDistance){
 	
 	
	      
	      //weasel will follow the waypoint.
	      transform.position = Vector3.MoveTowards(transform.position, wayPointPos, speed * Time.deltaTime);
          //transform.rotation = Quaternion.Slerp(transform.rotation, wayPointRot, Time.deltaTime * 2.0f);
          transform.LookAt(wayPointPos);
          GetComponent.<Animation>().Play("weaselRun");

 	}
 }