#pragma strict

private var theCamera : Transform;
var walkSpeed : float = 3.0;
var gravity : float = 20.0;
var jumpSpeed : float = 8.0;


private var parentVar : GameObject;
private var parentJump : boolean;
private var parentWalk : boolean;
private var parentIdle : boolean;
//부모 변수

private var Camvar : GameObject;
//camera 받아오는 변수
private var velocity : Vector3;

private var myTransform : Transform;


function Start() 
{
    myTransform = this.transform;
	Camvar = GameObject.FindWithTag("MainCamera");
	parentVar = GameObject.FindWithTag("Chick");
}
 
function lateUpdate(){

}


 
function Update () {
	this.transform.parent = parentVar.transform;
	theCamera = Camvar.transform;
	
	velocity = this.transform.parent.gameObject.GetComponent(ThirdPersonPlayer).velocityToChild;
	parentJump = this.transform.parent.gameObject.GetComponent(ThirdPersonPlayer).jumpCheck;
	parentWalk = this.transform.parent.gameObject.GetComponent(ThirdPersonPlayer).walkCheck;
	parentIdle = this.transform.parent.gameObject.GetComponent(ThirdPersonPlayer).idleCheck;
	
	
	var controller : CharacterController = GetComponent(CharacterController);
	
	//if (controller.isGrounded){
		if(parentJump){
			velocity.y = jumpSpeed;
			GetComponent.<Animation>().Play("Jump");
		}		
		else if(parentWalk){
			GetComponent.<Animation>().CrossFade("Run",0.1);
			//transform.LookAt(transform.position + velocity);
			//transform.LookAt(transform.position);
		}
		else if(parentIdle){
			GetComponent.<Animation>().CrossFade("Idle",0.1);
		}
	//}

}

