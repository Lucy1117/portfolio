#pragma strict  
public var theCamera : Transform;
 
var walkSpeed : float = 3;
var gravity : float = 20.0;
var jumpSpeed : float = 5.0;

private var velocity : Vector3;

internal var velocityToChild : Vector3;
internal var jumpCheck : boolean = false;
internal var walkCheck : boolean = false;
internal var idleCheck : boolean = false;
internal var myTransform : Transform;

///점프 변수

public static var JUMP_HEIGHT_MAX :float =0.3f;
public var step_timer:float =0.0f;
private var timer: float = 0;

public var Jumping :boolean=false;
public var airControl:float=0.5; 
 
public var MoveDirection:Vector3=Vector3.zero;
public var jumpDirection:Vector3=Vector3.zero;
//
 
function Start() 
{
    myTransform = this.transform;
	GetComponent.<Animation>()["Run"].speed = 2.0;
}
 
 
 

 
 
 
 
function Update () {

	var controller : CharacterController = GetComponent(CharacterController);
	
	var inputX : float = Input.GetAxis("Vertical");
	var inputZ : float = Input.GetAxis("Horizontal");
	var moveVectorX : Vector3 = theCamera.forward * inputX;
  	var moveVectorZ : Vector3 = theCamera.right * inputZ;
	var moveVector : Vector3 = ( moveVectorX + moveVectorZ ).normalized;
		
		
////////////////////////////////////////////2go zone/////////////////////	
	 if(Input.GetKeyDown(KeyCode.Space)){
         
         jumpCheck = true;
         walkCheck = false;
         idleCheck = false;
         
         this.step_timer=0.5;
         
         if (controller.isGrounded){
            Jumping=true;
         }
         else
            Jumping=false;
         
         
         if (Jumping){   
         GetComponent.<Animation>().Play("Jump");
         //Jumping=false;
         jumpDirection=MoveDirection;
         
         this.transform.Translate((MoveDirection.normalized*jumpSpeed)*Time.deltaTime);
         transform.position += transform.up * jumpSpeed * Time.deltaTime;
         velocity.y += Mathf.Sqrt(0.2f*9.8f*ThirdPersonPlayer.JUMP_HEIGHT_MAX);
         
         velocity = Vector3(moveVector.x, 0, moveVector.z);
         velocity *= walkSpeed;
         }
         
         
         
         
      
         //this.is_key_released=false;
         //transform.position += transform.up * jumpSpeed * Time.deltaTime;
         
      }   

      if(this.step_timer>0&&Jumping){
      if(Input.GetKeyUp(KeyCode.Space)){
         step_timer=0;
         Jumping=false;
         
      }
      else{
         step_timer -=Time.deltaTime;
      
         transform.position += transform.up * jumpSpeed * Time.deltaTime;
         velocity = Vector3(moveVector.x, 0, moveVector.z);
         velocity *= walkSpeed;
      }
         
      
      if(timer<=0){
         velocity.y = 0;
      }
   }
      
   

			
///////////////////////////////////////////////////////////////////////
	
	
	if (controller.isGrounded){
	
		velocity = Vector3(moveVector.x, 0, moveVector.z);
		velocity *= walkSpeed;
		/*
		if(Input.GetButtonDown("Jump")){
			velocity.y = jumpSpeed;
			animation.Play("Jump");
			jumpCheck = true;
			walkCheck = false;
			idleCheck = false;
		}
		*/		
		if(velocity.magnitude > 0.5){
			GetComponent.<Animation>().CrossFade("Run",0.1);
			transform.LookAt(transform.position + velocity);
			walkCheck = true;
			jumpCheck = false;
			idleCheck = false;
		}
		else{
			GetComponent.<Animation>().CrossFade("Idle",0.1);
			idleCheck = true;
			jumpCheck = false;
			walkCheck = false;
		}
	}
	velocity.y -= gravity * Time.deltaTime;
	controller.Move(velocity * Time.deltaTime);
	velocityToChild = velocity;
}



