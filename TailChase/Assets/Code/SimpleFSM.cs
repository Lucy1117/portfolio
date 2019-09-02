using UnityEngine;
using System.Collections;

public class SimpleFSM : FSM 
{
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
	
	//Speed of the tank
	private float curSpeed;
	
	//Tank Rotation Speed
	private float curRotSpeed;
	
	//Whether the NPC is destroyed or not
	private bool bDead;
	//private int health;
	
	private float catchDist = 1.5f; //잡는 최소 거리.
	private float chaseDist = 10.0f; //쫓는 최소 거리.
	private int nameTag = 0; //태그
	private bool catchCheck = false;
	private bool plusCheck = false;
	private bool isFlag = false;
	private GameObject otherObject; //쫓고있는 오브젝트. 
	
	//private bool eatFlag = true;

	private GameObject charprefab;
	//private Movement charpreScript;
	
	private int a=0;
	
	//Initialize the Finite state machine for the NPC tank
	protected override void Initialize () 
	{
		GetComponent<Animation>().Play("Idle");
		curState = FSMState.Patrol;
		curSpeed = 5.0f;
		curRotSpeed = 10.0f;
		bDead = false;
		elapsedTime = 0.0f;
		
		
		//Get the list of points
		pointList = GameObject.FindGameObjectsWithTag ("WandarPoint");
		pointList2 = GameObject.FindGameObjectsWithTag ("WandarPoint2");
		pointList3 = GameObject.FindGameObjectsWithTag ("WandarPoint3");
		pointList4 = GameObject.FindGameObjectsWithTag ("WandarPoint4");

		charprefab = GameObject.Find ("Character_prefab");
		//charpreScript = charprefab.GetComponent <Movement> ();
		
		//Set Random destination point first
		FindNextPoint();
		
		//쫓아야 할 오브젝트.
		//GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
		
		//도망쳐야 할 오브젝트.
		//enumyTransform = enumyPlayer.transform;
		
		//자신의 이름에 따라 쫓아야 할 오브젝트 지정.
		if(transform.gameObject.tag == "Red"){
			nameTag = 1;
			a = 4;
			objPlayer = GameObject.FindGameObjectWithTag("Orange");
			playerTransform = objPlayer.transform;
		}
		else if(transform.gameObject.tag == "Orange"){
			nameTag = 2;
			a = 7;
			objPlayer = GameObject.FindGameObjectWithTag("Yellow");
			playerTransform = objPlayer.transform;
		}
		else if(transform.gameObject.tag == "Yellow"){
			nameTag = 3;
			a = 1;
			objPlayer = GameObject.FindGameObjectWithTag("Green");
			playerTransform = objPlayer.transform;
		}
		else if(transform.gameObject.tag == "Green"){
			nameTag = 4;
			a = 0;
			objPlayer = GameObject.FindGameObjectWithTag("Blue");
			playerTransform = objPlayer.transform;
		}
		else if(transform.gameObject.tag == "Blue"){
			a = 4;
			objPlayer = GameObject.FindGameObjectWithTag("SkyBlue");
			playerTransform = objPlayer.transform;
			nameTag = 5;
		}
		else if(transform.gameObject.tag == "SkyBlue"){
			a = 7;
			objPlayer = GameObject.FindGameObjectWithTag("Purple");
			playerTransform = objPlayer.transform;
			nameTag = 6;
		}
		else if(transform.gameObject.tag == "Purple"){
			a = 1;
			objPlayer = GameObject.FindGameObjectWithTag("Red");
			playerTransform = objPlayer.transform;
			nameTag = 7;
		}
		
		if(!playerTransform){
			objPlayer = null;
			playerTransform = null;
			print("Player doesn't exist.. Please add one with Tag named 'Player'");
			nameTag++;
			Debug.Log (nameTag+"초기화부분에서 없어서 다음 걸로 넘어가게");
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
		
		//Update the time
		elapsedTime += Time.deltaTime;
		
		if (a == 0) {
			pointList = GameObject.FindGameObjectsWithTag ("WandarPoint");
			a=10;
			SendMessage("FindNextPoint");
		}
		if (a == 11) {
			pointList = GameObject.FindGameObjectsWithTag ("WandarPoint");	
		}
		//winter
		if (a == 1) {
			
			pointList = GameObject.FindGameObjectsWithTag ("WandarPoint2");	
			a=2;
			SendMessage("FindNextPoint");
			
		}
		
		if (a == 3) {
			pointList = GameObject.FindGameObjectsWithTag ("WandarPoint2");	
		}
		//summer
		if (a == 4) {
			pointList = GameObject.FindGameObjectsWithTag ("WandarPoint3");	
			a=5;
			SendMessage("FindNextPoint");
		}
		if (a == 6) {
			pointList = GameObject.FindGameObjectsWithTag ("WandarPoint3");	
		}
		//fall
		if (a == 7) {
			pointList = GameObject.FindGameObjectsWithTag ("WandarPoint4");	
			a=8;
			SendMessage("FindNextPoint");	
		}
		if (a == 9) {
			pointList = GameObject.FindGameObjectsWithTag ("WandarPoint4");	
		}
		
		
		//잡았으면, 그 다음 타겟을 잡도록 함.
		if(catchCheck){
			if(nameTag < 7){
				Debug.Log (nameTag+"catchCheck plus");
				nameTag++;
				catchCheck = false;
			}
			else if(nameTag == 7){
				if(isFlag){
					nameTag = 1;
					catchCheck = false;
					Debug.Log (nameTag+"네임태그 처음으로");
					isFlag = false;
				}
			}
			//curState = FSMState.Patrol;
		}
		
		
		if(!playerTransform||playerTransform==null){
			catchCheck = true;
			Debug.Log ("없음.");
			if(nameTag == 7){
				nameTag = 1;
			}
			else{
				nameTag++;
			}
		}
		
		//다음 것을 잡도록 넘어가는 코드.
		if(plusCheck){
			switch(nameTag){
			case 1:
				if(!GameObject.FindGameObjectWithTag("Orange")){
					objPlayer = null;
					playerTransform = null;
					//nameTag++;
					Debug.Log (nameTag+"오렌지가 없다.");
					catchCheck = true;
					CatchPlus();
				}
				else{
					objPlayer = GameObject.FindGameObjectWithTag("Orange");
					playerTransform = objPlayer.transform;
					Debug.Log ("1");
					plusCheck=false;
					curState = FSMState.Patrol;
				}
				break;
			case 2:
				if(!GameObject.FindGameObjectWithTag("Yellow")){
					objPlayer = null;
					playerTransform = null;
					//nameTag++;
					Debug.Log (nameTag+"옐로우가 없다.");
					catchCheck = true;
					CatchPlus();
				}
				else{
					objPlayer = GameObject.FindGameObjectWithTag("Yellow");
					playerTransform = objPlayer.transform;
					Debug.Log ("2");
					plusCheck=false;
					curState = FSMState.Patrol;
				}
				break;
			case 3:
				if(!GameObject.FindGameObjectWithTag("Green")){
					objPlayer = null;
					playerTransform = null;
					//nameTag++;
					Debug.Log (nameTag+"초록이 없다.");
					catchCheck = true;
					CatchPlus();
				}
				else{
					objPlayer = GameObject.FindGameObjectWithTag("Green");
					playerTransform = objPlayer.transform;
					Debug.Log ("3");
					plusCheck=false;
					curState = FSMState.Patrol;
				}
				break;
			case 4:
				if(!GameObject.FindGameObjectWithTag("Blue")){
					objPlayer = null;
					playerTransform = null;
					//nameTag++;
					Debug.Log (nameTag+"블루가 없다.");
					catchCheck = true;
					CatchPlus();
				}
				else{
					objPlayer = GameObject.FindGameObjectWithTag("Blue");
					playerTransform = objPlayer.transform;
					Debug.Log ("4");
					plusCheck=false;
					curState = FSMState.Patrol;
				}
				break;
			case 5:
				if(!GameObject.FindGameObjectWithTag("SkyBlue")){
					objPlayer = null;
					playerTransform = null;
					//nameTag++;
					Debug.Log (nameTag+"스카이가 없다.");
					catchCheck = true;
					CatchPlus();
				}
				else{
					objPlayer = GameObject.FindGameObjectWithTag("SkyBlue");
					playerTransform = objPlayer.transform;
					Debug.Log ("5");
					plusCheck=false;
					curState = FSMState.Patrol;
				}
				break;
			case 6:
				if(!GameObject.FindGameObjectWithTag("Purple")){
					objPlayer = null;
					playerTransform = null;
					//nameTag++;
					Debug.Log (nameTag+"보라가 없다.");
					catchCheck = true;
					CatchPlus();
					isFlag = true;
				}
				else{
					objPlayer = GameObject.FindGameObjectWithTag("Purple");
					playerTransform = objPlayer.transform;
					Debug.Log ("6");
					plusCheck=false;
					curState = FSMState.Patrol;
				}
				break;
			case 7:	
				if(!GameObject.FindGameObjectWithTag("Red")){
					objPlayer = null;
					playerTransform = null;
					nameTag=1;
					Debug.Log (nameTag+"레드가 없다.");
					CatchPlus();
					isFlag = true;
				}
				else{
					objPlayer = GameObject.FindGameObjectWithTag("Red");
					playerTransform = objPlayer.transform;
					Debug.Log ("7");
					plusCheck=false;
					curState = FSMState.Patrol;
				}
				break;
			}
		}

		if (destPos == null) {
			CatchPlus ();
		}
		
	}


	public void CatchPlus(){
		if(nameTag < 7){
			Debug.Log (nameTag+"catchCheck plus");
			nameTag++;
			catchCheck = false;
		}
		else if(nameTag == 7){
			if(isFlag){
				nameTag = 1;
				catchCheck = false;
				Debug.Log (nameTag+"네임태그 처음으로");
				isFlag = false;
			}
		}
		//curState = FSMState.Patrol;
	}
	/// <summary>
	/// Patrol state
	/// </summary>
	protected void UpdatePatrolState()
	{
		GetComponent<Animation>().Play("Walk");
		//Debug.Log ("Walk");
		//Find another random patrol point if the current point is reached
		if (Vector3.Distance(transform.position, destPos) <= 0.5f)
		{
			FindNextPoint();
		}
		//Check the distance with player tank
		//When the distance is near, transition to chase state
		else if (Vector3.Distance(transform.position, playerTransform.position) <= chaseDist)
		{

			curState = FSMState.Chase;
		
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
		GetComponent<Animation>().Play("Run");
		//Debug.Log ("Run");
		//Set the target position as the player position
		if(!playerTransform){
			CatchPlus();
		}
		else{
			destPos = playerTransform.position;
			//curState = FSMState.Patrol;
		}
		
		
		//Check the distance with player tank
		//When the distance is near, transition to attack state
		float dist = Vector3.Distance(transform.position, playerTransform.position);
		if (dist <= catchDist)
		{
			curState = FSMState.Attack;
		}
		else if((dist >= catchDist) && (dist < chaseDist)){
			curState = FSMState.Chase;
		}
		//Go back to patrol is it become too far
		else if (dist >= chaseDist)
		{
			curState = FSMState.Patrol;
		}
		
		//Go Forward
		Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);
		transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
	}
	
	protected void UpdateEscapeState()
	{

	}
	
	/// <summary>
	/// Attack state
	/// </summary>
	protected void UpdateAttackState()
	{
		//Set the target position as the player positio

		if (destPos == null) {
			CatchPlus ();
		}
		else{
			destPos = playerTransform.position;
		}
		//Debug.Log ("Attack");
		GetComponent<Animation>().Play("Catch");
		catchCheck = true;
		plusCheck = true;
		
		switch(nameTag){
		case 1:
			otherObject = GameObject.FindGameObjectWithTag ("Orange");
			otherObject.SendMessage("DeadOK");
			break;
		case 2:
			otherObject = GameObject.FindGameObjectWithTag ("Yellow");
			otherObject.SendMessage("DeadOK");
			break;
		case 3:
			otherObject = GameObject.FindGameObjectWithTag ("Green");
			otherObject.SendMessage("DeadOK");
			break;
		case 4:
			otherObject = GameObject.FindGameObjectWithTag ("Blue");
			otherObject.SendMessage("DeadOK");
			break;
		case 5:
			otherObject = GameObject.FindGameObjectWithTag ("SkyBlue");
			otherObject.SendMessage("DeadOK");
			break;
		case 6:
			otherObject = GameObject.FindGameObjectWithTag ("Purple");
			otherObject.SendMessage("DeadOK");
			break;
		case 7:
			otherObject = GameObject.FindGameObjectWithTag ("Red");
			otherObject.SendMessage("DeadOK");
			isFlag = true;
			break;
		}		
		
		charprefab.SendMessage ("OtherDead");
	}
	
	/// <summary>
	/// Dead state
	/// </summary>
	
	
	protected void UpdateDeadState()
	{
		//Show the dead animation with some physics effects
		if (bDead)
		{
			Debug.Log ("AI 죽었어!!!");
			bDead = false;
			Destroy(gameObject);
		}
	}
	
	/// <summary>
	/// Check the collision with the bullet
	/// </summary>
	/// <param name="collision"></param>
	void OnCollisionEnter(Collision collision)
	{
		//Reduce health
		//if(collision.gameObject.tag == "Bullet")
		//	health -= collision.gameObject.GetComponent<Bullet>().damage;
	}   
	
	protected void DeadOK(){
		curState = FSMState.Dead;
		bDead = true;
	}
	
	protected void EatOK(){
	//	eatFlag = true;
	}
	
	/// <summary>
	/// Find the next semi-random patrol point
	/// </summary>
	protected void FindNextPoint()
	{
		//print("Finding next point");
		int rndIndex = Random.Range(0, pointList.Length);
		//float rndRadius = 10.0f;
		
		Vector3 rndPosition = Vector3.zero;
		destPos = pointList[rndIndex].transform.position + rndPosition;
		
		//Check Range
		//Prevent to decide the random point as the same as before
		
		if (IsInCurrentRange(destPos))
		{
			if(a==10){
				destPos = pointList[rndIndex].transform.position + rndPosition;
				a=11;
			}
			
			if(a==11){
				destPos = pointList[rndIndex].transform.position + rndPosition;
			}
			//	if(collider.gameObject.name == "PORTAL_spring_winter"){
			if(a==2){
				
				destPos = pointList[rndIndex].transform.position + rndPosition;
				
				a = 3;
			}
			//if(collider.gameObject.name == "PORTAL_spring_winter"){
			//Debug.Log("gr");
			//destPos = pointList[rndIndex].transform.position + rndPosition;
			//a=1;
			//	}
			
			if(a==3){
				destPos = pointList[rndIndex].transform.position + rndPosition;
			}
			
			if(a==5){
				
				destPos = pointList[rndIndex].transform.position + rndPosition;
				
				a = 6;
			}
			
			if(a==6){
				destPos = pointList[rndIndex].transform.position + rndPosition;
			}
			//}
			if(a==8){
				destPos = pointList[rndIndex].transform.position + rndPosition;
				
				a = 9;
				
			}
			if(a==9){
				destPos = pointList[rndIndex].transform.position + rndPosition;
			}
		}
		/*
		if (IsInCurrentRange(destPos))
		{
			rndPosition = new Vector3(Random.Range(-rndRadius, rndRadius), 0.0f, Random.Range(-rndRadius, rndRadius));
			destPos = pointList[rndIndex].transform.position + rndPosition;
		}
		*/
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
	void sw(){
		
		if (a == 11) {
			Debug.Log("correct");
			a = 1;
			
		}
		
		if (a == 3) {
			Debug.Log("winter-spring");
			a=0;		
		}
	}
	
	void ss(){
		
		if(a==11){
			a = 4;
		}
		
		if (a == 6) {
			
			a=0;		
		}
	}
	
	void sf(){
		if (a == 6) {
			a = 7;
		}
		
		if (a == 9) {
			a=4;		
		}
	}
	
	void fw(){
		//a = 1;
		
		if(a==9){
			a=1;
		}
		
		if(a==3){
			a=7;
		}
	}
	
	public void Stop(){
		if (curState == FSMState.Chase) {
			curState = FSMState.Patrol;
		}
		if (curState == FSMState.Patrol){
			curState = FSMState.Patrol;
		}
	}
}