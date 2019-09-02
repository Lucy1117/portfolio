#pragma strict

private var wayPoint :GameObject;
private var wayPointPos : Vector3;
//private var wayPointRot : Quaternion;

private var runawayPos : Vector3;
private var timer : float = 1.0f;


 
 

 private var speed : float= 3.0;
 private var MaxDistance: float =2.0;
 private var MinDistance: float =0.5;
 
 
 function Start ()
 {
      wayPoint = GameObject.Find("wayPoint");
 }
 
 function Update ()
 {

 	
 	//wayPointPos = new Vector3(wayPoint.transform.position.x, transform.position.y, wayPoint.transform.position.z);
 	//wayPointRot = Quaternion.LookRotation (-wayPointPos, Vector3.up);
 	runawayPos=transform.position - wayPoint.transform.position;
 	//runawayPos.z = 0.0f;


 	
 	if(Vector3.Distance(transform.position,wayPoint.transform.position)<MaxDistance){
 		  	if(timer > 0)
    		  {
         		  timer -= Time.deltaTime;
     		  }
 	
	      
	      //weasel will follow the waypoint.
	      transform.position = Vector3.MoveTowards(transform.position, runawayPos*MinDistance, speed * Time.deltaTime);
	       GetComponent.<Animation>().Play("mannyonsam_Walk");
          
          transform.LookAt(-wayPointPos);

 	}
 	
 	if(timer <= 0)
     	 {
     	 	stop();
     	 	timer = 1.0f;
     	 
     	 }
 	
 }
 
 function stop(){
 
 	yield WaitForSeconds(2.0);
 }