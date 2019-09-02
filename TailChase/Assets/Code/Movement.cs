using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	
	public Transform mainCam;
	
	public float speed = 10.0f;
	public float mass = 5.0f;
	public float force = 50.0f;
	public float minimumDistToAvoid = 20.0f;
	
	//Actual speed of the vehicle 
	private float curSpeed;
	private Vector3 targetPoint;
	
	//추가한 변수.
	private bool mouseCheck = true;
	public int tagName=0;
	private int colName;
	private GameObject enumyPlayer;
	private Transform enumyTransform;
	protected Vector3 enumyPos;
	private float dist = 0.0f;
	private float catchDist = 1.0f; //잡는 최소 거리.

	//다른 오브젝트의 C# 불러오기.
	private GameObject otherObject;
	private ColorSelect other;
	private bool onceA = false;

	private GameObject barObject;

	private bool eatFlag = false;

	private bool chaseFlag = false;

	private int layerMaskGround = 1 << 9;
	private int layerMaskObstacle = 1 << 8;

	private int aiNum;

	// Use this for initialization
	void Start () 
	{
		mass = 5.0f;        
		//캐릭터가 카메라를 쳐다보도록 하는 코드. 일단 카메라의 z축만 받아온다.
		targetPoint = new Vector3(this.transform.position.x, this.transform.position.y, mainCam.position.z);
		//targetPoint = Vector3.zero;
		GetComponent<Animation>().Play("Idle");
		GetComponent<Animation>() ["Walk"].speed = 2.0f;
		onceA = true;

		otherObject = GameObject.FindGameObjectWithTag ("ColorGUI");
		other = otherObject.GetComponent<ColorSelect>();

		barObject = GameObject.Find ("GUIBar");
	}

	// Update is called once per frame
	void Update () 
	{
		//Vehicle move by mouse click
		
		RaycastHit hit;
		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		
		//캐릭터는 무시하고 땅만 검사하는 코드.
		
		if (Input.GetMouseButton (0) && Physics.Raycast (ray, out hit, 30.0f, layerMaskGround)) {
			targetPoint = hit.point;
			mouseCheck = true;
			GetComponent<Animation>().CrossFade("Walk");
		} 
		else{
			mouseCheck = false;
			GetComponent<Animation>().CrossFade("Idle",0.1f);
		}

		Vector3 dir = (targetPoint - transform.position);
		dir.Normalize();

		if(Physics.Raycast (ray, out hit, 10.0f, layerMaskObstacle)){
			AvoidObstacles(ref dir);
		}
			
		curSpeed = speed * Time.deltaTime;

		var rot = Quaternion.LookRotation (dir);
		transform.rotation = Quaternion.Slerp (transform.rotation, rot, 5.0f * Time.deltaTime);
		

		if (mouseCheck) {//마우스 놓았을 때 바로 멈추도록.
			transform.position += transform.forward * curSpeed;
		}
		else{
			curSpeed = 0.0f;
			GetComponent<Animation>().CrossFade("Idle",0.1f);
		}

		if(chaseFlag){
			if(enumyPlayer == null){
				if(colName > 7){
					colName++;
				}
				else if(colName == 7){
					colName = 1;
				}
				onceA = true;
			}
			Chase ();
		}
		OtherDead ();

	}

	//AIGenerator에서 AI가 생성되었으니 잡으러 가도 된다고 메세지를 보낸다.
	public void ChaseOK(){
		chaseFlag = true;
	}

	//ColorSelect에서 내가 고른 이름을 받아와서 태그를 바꿔준다.
	public void TagName(){
		tagName = other.buttonNum;
		switch(tagName){
			case 1:
				transform.gameObject.tag = "Red";
				break;
			case 2:
				transform.gameObject.tag = "Orange";
				break;
			case 3:
				transform.gameObject.tag = "Yellow";
				break;
			case 4:
				transform.gameObject.tag = "Green";
				break;
			case 5:
				transform.gameObject.tag = "Blue";
				break;
			case 6:
				transform.gameObject.tag = "SkyBlue";
				break;
			case 7:	
				transform.gameObject.tag = "Purple";
				break;
		}
		colName = tagName;		
	}
	
	//Calculate the new directional vector to avoid the obstacle
	public void AvoidObstacles(ref Vector3 dir)
	{
		RaycastHit hit;

		//Check that the vehicle hit with the obstacles within it's minimum distance to avoid
		if (Physics.Raycast(transform.position, transform.forward, out hit, minimumDistToAvoid, layerMaskObstacle))
		{
			//Get the normal of the hit point to calculate the new direction
			Vector3 hitNormal = hit.normal;
			hitNormal.y = 0.0f; //Don't want to move in Y-Space
			
			//Get the new directional vector by adding force to vehicle's current forward vector
			dir = transform.forward + hitNormal * force;
		}
		else{
			curSpeed = 0.0f;
		}
		
	}
	/*
	//적이 있는지 체크해서 없으면 그 다음 것을 잡도록 cName을 하나 올린다.
	protected void enumyCheck(int cName){
		if(cName > 7){
			cName++;
		}
		else if(cName == 7){
			cName = 1;
		}
		onceA = true;
	}
*/

	//잡으러 가는 것에 대한 코드. 적의 이름을 받고, 그 적을 enumyTransform에 넣어 추적한다.
	//이름을 넣는 과정은 once를 통해 딱 한 번 하도록 한다.
	protected void Chase(){
		switch(colName){
		case 1:
			if(!GameObject.FindGameObjectWithTag("Orange")){
				enumyPlayer = null;
			}
			else{
				if(onceA){
					enumyPlayer = GameObject.FindGameObjectWithTag("Orange");
					enumyTransform = enumyPlayer.transform;
					onceA = false;
				}
				CatchOK();
			}
			//CatchOK();
			break;
		case 2:
			if(!GameObject.FindGameObjectWithTag("Yellow")){
				enumyPlayer = null;
			}
			else{
				if(onceA){
					enumyPlayer = GameObject.FindGameObjectWithTag("Yellow");
					enumyTransform = enumyPlayer.transform;
					onceA = false;
				}
				CatchOK();
			}
			//CatchOK();
			break;
		case 3:
			if(!GameObject.FindGameObjectWithTag("Green")){
				enumyPlayer = null;
			}
			else{
				if(onceA){
				enumyPlayer = GameObject.FindGameObjectWithTag("Green");
				enumyTransform = enumyPlayer.transform;
				onceA = false;
				}
				CatchOK();
			}
			//CatchOK();
			break;
		case 4:
			if(!GameObject.FindGameObjectWithTag("Blue")){
				enumyPlayer = null;
			}
			else{
				if(onceA){
				enumyPlayer = GameObject.FindGameObjectWithTag("Blue");
				enumyTransform = enumyPlayer.transform;
				onceA = false;
				}
				CatchOK();
			}
			//CatchOK();
			break;
		case 5:
			if(!GameObject.FindGameObjectWithTag("SkyBlue")){
				enumyPlayer = null;
			}
			else{
				if(onceA){
				enumyPlayer = GameObject.FindGameObjectWithTag("SkyBlue");
				enumyTransform = enumyPlayer.transform;
				onceA = false;
				}
				CatchOK();
			}
			//CatchOK();
			break;
		case 6:
			if(!GameObject.FindGameObjectWithTag("Purple")){
				enumyPlayer = null;
			}
			else{
				if(onceA){
				enumyPlayer = GameObject.FindGameObjectWithTag("Purple");
				enumyTransform = enumyPlayer.transform;
				onceA = false;
				}
				CatchOK();
			}
			//CatchOK();
			break;
		case 7:	
			if(!GameObject.FindGameObjectWithTag("Red")){
				enumyPlayer = null;
			}
			else{
				if(onceA){
				enumyPlayer = GameObject.FindGameObjectWithTag("Red");
				enumyTransform = enumyPlayer.transform;
				onceA = false;
				}
				CatchOK();
			}
			//CatchOK();
			break;
		}
	}

	//계속 업데이트 되는 코드. 잡으러 갈 때 적과의 거리를 계산해 터치되면 적을 잡아먹는다. 적은 사라진다.
	protected void CatchOK(){
		enumyPos = enumyTransform.position;
		dist = Vector3.Distance(transform.position, enumyTransform.position);
		if(dist < catchDist){
			if(eatFlag){
				GetComponent<Animation>().Play("Catch");
				enumyPlayer.SendMessage("DeadOK");
				colName++;
				onceA = true;
				//eatFlag = true;
			}
		}
	}

	public void OtherDead(){
		aiNum = 7;
		if(!GameObject.FindGameObjectWithTag("Red")){
			aiNum--;
		}
		if(!GameObject.FindGameObjectWithTag("Orange")){
			aiNum--;
		}
		if(!GameObject.FindGameObjectWithTag("Yellow")){
			aiNum--;
		}
		if(!GameObject.FindGameObjectWithTag("Green")){
			aiNum--;
		}
		if(!GameObject.FindGameObjectWithTag("Blue")){
			aiNum--;
		}
		if(!GameObject.FindGameObjectWithTag("SkyBlue")){
			aiNum--;
		}
		if(!GameObject.FindGameObjectWithTag("Purple")){
			aiNum--;
		}
	}
	//내가 죽었음을 알려주는.
	protected void DeadOK(){
		OtherDead ();
		if(aiNum == 2){
			eatFlag = true;
			//Clear
			GameObject.FindWithTag("ClearM").SendMessage("ClearMenu");
		}
		else if(aiNum==1||aiNum==0){
			Debug.Log ("aiNum clear");
		}
		else{
			GameObject.Find("BGMFieldDead").SendMessage("DeadBGM");
			GameObject.FindWithTag("DeadM").SendMessage("DeadMenu");
			Destroy (gameObject);
		}
	}


	//버섯 하나를 먹었으니 이제 다른 캐릭터를 잡아도 된다고 알려준다.
	protected void EatOK(){
		eatFlag = true;
		barObject.SendMessage ("Guagestart");
	}

	//게이지가 다 떨어져서 다시 버섯을 먹어야 한다. 
	protected void EatNO(){
		eatFlag = false;
	}



}
