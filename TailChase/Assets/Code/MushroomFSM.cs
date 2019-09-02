using UnityEngine;
using System.Collections;

public class MushroomFSM : FSM {

	public enum FSMState
	{
		None,
		Patrol,
		Chase,
		Attack,
		Dead,
		Escape //쫓길 때의 상태.
	}
	
	//Current state that the NPC is reaching
	public FSMState curState;

	public int fieldNum;
	
	//Speed of the tank
	private float curSpeed;
	
	//Tank Rotation Speed
	private float curRotSpeed;
	
	//Whether the NPC is destroyed or not
	private bool bDead;
	//private int health
	private int nameTag = 0; //태그
	//private bool catchCheck = false;
	//private bool plusCheck = false;
	//private bool isFlag = false;
	private GameObject otherObject; //쫓고있는 오브젝트. 

	//int b=0;
	//Mushroom의 필요 태그를 찾아도 되는지 시작 지점을 나타내는.
	private bool once = true;

	private float deadDist = 1.5f;
	private Vector3 deadPos;
	private float randomTime;
	private float reTime = 2.0f;


	private GameObject generatorNo;
	private MushroomGenerator generatorScript;
	private int numPlus;

	
	//Initialize the Finite state machine for the NPC tank
	protected override void Initialize () 
	{
	
		GetComponent<Animation>().Play("Idle");
		curState = FSMState.Patrol;
		curSpeed = 5.0f;
		curRotSpeed = 10.0f;
		bDead = false;
		elapsedTime = 0.0f;
		randomTime = Random.Range (5.0f, 10.0f);

		if (fieldNum == 1) {
			//spring
			generatorNo = GameObject.Find ("MushroomGeneratePlaceSpring");
			generatorScript = generatorNo.GetComponent<MushroomGenerator> ();
			pointList = GameObject.FindGameObjectsWithTag ("WandarPoint");
		}
		else if(fieldNum == 2){
			//winter
			generatorNo = GameObject.Find ("MushroomGeneratePlaceWinter");
			generatorScript = generatorNo.GetComponent<MushroomGenerator> ();
			pointList = GameObject.FindGameObjectsWithTag ("WandarPoint2");
		}
		else if(fieldNum == 3){
			//summer
			generatorNo = GameObject.Find ("MushroomGeneratePlaceSummer");
			generatorScript = generatorNo.GetComponent<MushroomGenerator> ();
			pointList = GameObject.FindGameObjectsWithTag ("WandarPoint3");
		}
		else if(fieldNum == 4){
			//autumn
			generatorNo = GameObject.Find ("MushroomGeneratePlaceAutumn");
			generatorScript = generatorNo.GetComponent<MushroomGenerator> ();
			pointList = GameObject.FindGameObjectsWithTag ("WandarPoint4");
		}
		//1=spring, 2=winter, 3=summer, 4=autumn
		//Get the list of points		
		//Set Random destination point first
		FindNextPoint();


		//도망쳐야 할 오브젝트.
		//enumyTransform = enumyPlayer.transform;
		
		//자신의 이름에 따라 쫓아야 할 오브젝트 지정.
		
		if(!playerTransform){
			//Debug.Log (nameTag+"난 버섯인데 날 잡을 캐릭터가 없어.");
		}

		
	}
	
	//Update each frame
	protected override void FSMUpdate()
	{
		switch (curState)
		{
		case FSMState.Patrol: UpdatePatrolState(); break;
		case FSMState.Chase: UpdateChaseState(); break;
		case FSMState.Attack: UpdateAttackState(); break;
		case FSMState.Dead: UpdateDeadState(); break;
		case FSMState.Escape: UpdateEscapeState(); break;
		}

		
		switch(nameTag){
		case 1: 
			if(!GameObject.FindGameObjectWithTag ("Red"))
				Destroy (gameObject);
			break;
		case 2: 
			if(!GameObject.FindGameObjectWithTag ("Orange"))
				Destroy (gameObject);
			break;
		case 3: 
			if(!GameObject.FindGameObjectWithTag ("Yellow"))
				Destroy (gameObject);
			break;
		case 4: 
			if(!GameObject.FindGameObjectWithTag ("Green"))
				Destroy (gameObject);
			break;
		case 5: 
			if(!GameObject.FindGameObjectWithTag ("Blue"))
				Destroy (gameObject);
			break;
		case 6: 
			if(!GameObject.FindGameObjectWithTag ("SkyBlue"))
				Destroy (gameObject);
			break;
		case 7: 
			if(!GameObject.FindGameObjectWithTag ("Purple"))
				Destroy (gameObject);
			break;
		}
		if(once){
			if(this.tag == "MushRed"){
				nameTag = 1;
				objPlayer = GameObject.FindGameObjectWithTag("Red");
				playerTransform = objPlayer.transform;
				once = false;
			}
			else if(this.tag == "MushOrange"){
				nameTag = 2;
				objPlayer = GameObject.FindGameObjectWithTag("Orange");
				playerTransform = objPlayer.transform;
				once = false;
			}
			else if(this.tag == "MushYellow"){
				nameTag = 3;
				objPlayer = GameObject.FindGameObjectWithTag("Yellow");
				playerTransform = objPlayer.transform;
				once = false;
			}
			else if(this.tag == "MushGreen"){
				nameTag = 4;
				objPlayer = GameObject.FindGameObjectWithTag("Green");
				playerTransform = objPlayer.transform;
				once = false;
			}
			else if(this.tag == "MushBlue"){
				objPlayer = GameObject.FindGameObjectWithTag("Blue");
				playerTransform = objPlayer.transform;
				nameTag = 5;
				once = false;
			}
			else if(this.tag == "MushSky"){
				objPlayer = GameObject.FindGameObjectWithTag("SkyBlue");
				playerTransform = objPlayer.transform;
				nameTag = 6;
				once = false;
			}
			else if(this.tag == "MushPurple"){
				objPlayer = GameObject.FindGameObjectWithTag("Purple");
				playerTransform = objPlayer.transform;
				nameTag = 7;
				once = false;
			}
		}
		deadPos = playerTransform.position;

		
		//Update the time
		
		//잡으러 올 캐릭터가 없으면 사라지는 코드.
		
	}
	
	/// <summary>
	/// Patrol state
	/// </summary>
	protected void UpdatePatrolState()
	{
		GetComponent<Animation>().CrossFade ("Walk", 0.1f);


		
		if (Vector3.Distance (transform.position, destPos) <= 0.5f) {
			FindNextPoint();
		}
		else if (Vector3.Distance(transform.position, deadPos) <= deadDist){
			DeadOK ();
		}


		elapsedTime += Time.deltaTime;

		if(elapsedTime > randomTime){
			reTime = 0.0f;
			curState = FSMState.Escape;
		}
	
		//Rotate to the target point
		Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);  
		
		//Go Forward
		transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);


	}
	
	/// <summary>
	/// Chase state
	/// </summary>
	protected void UpdateChaseState()
	{
	
	}
	
	protected void UpdateEscapeState()
	{
		GetComponent<Animation>().CrossFade ("Idle", 0.1f);

		reTime += Time.deltaTime;
		if(reTime > 2.0f){
			elapsedTime = 0.0f;
			randomTime = Random.Range (5.0f, 10.0f);
			curState = FSMState.Patrol;
		}
	}
	
	/// <summary>
	/// Attack state
	/// </summary>
	protected void UpdateAttackState()
	{
		//필요없음.
	}

	protected void UpdateDeadState()
	{
		//Show the dead animation with some physics effects
		if (bDead)
		{
			bDead = false;
			Destroy(gameObject);
		}
	} 
	
	protected void DeadOK(){

		switch(nameTag){
		case 1: 
			generatorScript.red--;
			break;
		case 2: 
			generatorScript.orange--;
			break;
		case 3: 
			generatorScript.yellow--;
			break;
		case 4: 
			generatorScript.green--;
			break;
		case 5: 
			generatorScript.blue--;
			break;
		case 6: 
			generatorScript.sky--;
			break;
		case 7: 
			generatorScript.purple--;
			break;
		}


		Debug.Log ("죽었어여!" + objPlayer);
		objPlayer.SendMessage("EatOK");
		curState = FSMState.Dead;
		bDead = true;
	}
	
	/// <summary>
	/// Find the next semi-random patrol point
	/// </summary>

	


	protected void FindNextPoint()
	{		//print("Finding next point");
		int rndIndex = Random.Range(0, pointList.Length);
		float rndRadius = 5.0f;
		//float rndRadius = 10.0f;
		
		Vector3 rndPosition = Vector3.zero;
		destPos = pointList[rndIndex].transform.position + rndPosition;
		if (IsInCurrentRange(destPos))
		{
			rndPosition = new Vector3(Random.Range (-rndRadius, rndRadius), 0.0f, Random.Range(-rndRadius, rndRadius));
			destPos = pointList[rndIndex].transform.position + rndPosition;
			
		}
	}
	
	/// <summary>
	/// Check whether the next random position is the same as current tank position
	/// </summary>
	/// <param name="pos">position to check</param>
	protected bool IsInCurrentRange(Vector3 pos)
	{
		float xPos = Mathf.Abs(pos.x - transform.position.x);
		float zPos = Mathf.Abs(pos.z - transform.position.z);
		
		if (xPos <= 50 && zPos <= 50)
			return true;
		
		return false;
	}

}
