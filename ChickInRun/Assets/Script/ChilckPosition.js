#pragma strict


public var wayPoint: GameObject ;
private var timer : float = 0.5f;


function Start () {

}

function Update () {
	if(timer > 0)
      {
           timer -= Time.deltaTime;
      }
     	 if(timer <= 0)
     	 {
          	 //The position of the waypoint will update to the player's position
         	 UpdatePosition();
           
         	  timer = 0.5f;
    	  }

}


function UpdatePosition()
 {
      //The wayPoint's position will now be the player's current position.
      wayPoint.transform.position = this.transform.position;
 }