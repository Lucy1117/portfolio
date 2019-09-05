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
	public int nameTag = 0; //태그
	private bool catchCheck = false;
	private bool plusCheck = false;
	private bool isFlag = false;
    private GameObject otherObject; //쫓고있는 오브젝트. 
	
	//private bool eatFlag = true;

	private GameObject charprefab;
    //private Movement charpreScript;

    //텔레포트할 때 pointList 바꿔주는 변수
    public int teleportVar=0;
	
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
		pointList = GameObject.FindGameObjectsWithTag ("WanderPoint");
		pointList2 = GameObject.FindGameObjectsWithTag ("WanderPoint2");
		pointList3 = GameObject.FindGameObjectsWithTag ("WanderPoint3");
		pointList4 = GameObject.FindGameObjectsWithTag ("WanderPoint4");

		charprefab = GameObject.Find ("Character_prefab");
		//charpreScript = charprefab.GetComponent <Movement> ();

		
		//쫓아야 할 오브젝트.
		//GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
		
		//도망쳐야 할 오브젝트.
		//enumyTransform = enumyPlayer.transform;
		
		//자신의 이름에 따라 쫓아야 할 오브젝트 지정.
		if(transform.gameObject.tag == "Red"){
			nameTag = 1;
            teleportVar = 2;
			objPlayer = GameObject.FindGameObjectWithTag("Orange");
			playerTransform = objPlayer.transform;
		}
		else if(transform.gameObject.tag == "Orange"){
			nameTag = 2;
            teleportVar = 3;
			objPlayer = GameObject.FindGameObjectWithTag("Yellow");
			playerTransform = objPlayer.transform;
		}
		else if(transform.gameObject.tag == "Yellow"){
			nameTag = 3;
            teleportVar = 4;
			objPlayer = GameObject.FindGameObjectWithTag("Green");
			playerTransform = objPlayer.transform;
		}
		else if(transform.gameObject.tag == "Green"){
			nameTag = 4;
            teleportVar = 1;
			objPlayer = GameObject.FindGameObjectWithTag("Blue");
			playerTransform = objPlayer.transform;
		}
		else if(transform.gameObject.tag == "Blue"){
            teleportVar = 2;
			objPlayer = GameObject.FindGameObjectWithTag("SkyBlue");
			playerTransform = objPlayer.transform;
			nameTag = 5;
		}
		else if(transform.gameObject.tag == "SkyBlue"){
            teleportVar = 3;
			objPlayer = GameObject.FindGameObjectWithTag("Purple");
			playerTransform = objPlayer.transform;
			nameTag = 6;
		}
		else if(transform.gameObject.tag == "Purple"){
            teleportVar = 4;
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

        //Set Random destination point first
        FindNextPoint();

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

        //FindNextPoint();

        /*
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
			curState = FSMState.Patrol;
		}
		*/

		//다음 것을 잡도록 넘어가는 코드.
		if(plusCheck){
            switch (nameTag)
            {
                case 1:
                    if (!GameObject.FindGameObjectWithTag("Orange"))
                    {
                        objPlayer = null;
                        playerTransform = null;
                        //nameTag++;
                        Debug.Log(nameTag + "주황이 없다.");
                        catchCheck = true;
                        CatchPlus();
                    }
                    else
                    {
                        objPlayer = GameObject.FindGameObjectWithTag("Orange");
                        playerTransform = objPlayer.transform;
                        //Debug.Log("1");
                        plusCheck = false;
                        curState = FSMState.Patrol;
                    }
                    break;
                case 2:
                    if (!GameObject.FindGameObjectWithTag("Yellow"))
                    {
                        objPlayer = null;
                        playerTransform = null;
                        //nameTag++;
                        Debug.Log(nameTag + "노랑이 없다.");
                        catchCheck = true;
                        CatchPlus();
                    }
                    else
                    {
                        objPlayer = GameObject.FindGameObjectWithTag("Yellow");
                        playerTransform = objPlayer.transform;
                        //Debug.Log("2");
                        plusCheck = false;
                        curState = FSMState.Patrol;
                    }
                    break;
                case 3:
                    if (!GameObject.FindGameObjectWithTag("Green"))
                    {
                        objPlayer = null;
                        playerTransform = null;
                        //nameTag++;
                        Debug.Log(nameTag + "초록이 없다.");
                        catchCheck = true;
                        CatchPlus();
                    }
                    else
                    {
                        objPlayer = GameObject.FindGameObjectWithTag("Green");
                        playerTransform = objPlayer.transform;
                        //Debug.Log("3");
                        plusCheck = false;
                        curState = FSMState.Patrol;
                    }
                    break;
                case 4:
                    if (!GameObject.FindGameObjectWithTag("Blue"))
                    {
                        objPlayer = null;
                        playerTransform = null;
                        //nameTag++;
                        Debug.Log(nameTag + "파랑이 없다.");
                        catchCheck = true;
                        CatchPlus();
                    }
                    else
                    {
                        objPlayer = GameObject.FindGameObjectWithTag("Blue");
                        playerTransform = objPlayer.transform;
                        //Debug.Log("4");
                        plusCheck = false;
                        curState = FSMState.Patrol;
                    }
                    break;
                case 5:
                    if (!GameObject.FindGameObjectWithTag("SkyBlue"))
                    {
                        objPlayer = null;
                        playerTransform = null;
                        //nameTag++;
                        Debug.Log(nameTag + "하늘이 없다.");
                        catchCheck = true;
                        CatchPlus();
                    }
                    else
                    {
                        objPlayer = GameObject.FindGameObjectWithTag("SkyBlue");
                        playerTransform = objPlayer.transform;
                        //Debug.Log("5");
                        plusCheck = false;
                        curState = FSMState.Patrol;
                    }
                    break;
                case 6:
                    if (!GameObject.FindGameObjectWithTag("Purple"))
                    {
                        objPlayer = null;
                        playerTransform = null;
                        //nameTag++;
                        Debug.Log(nameTag + "보라가 없다.");
                        catchCheck = true;
                        CatchPlus();
                        isFlag = true;
                    }
                    else
                    {
                        objPlayer = GameObject.FindGameObjectWithTag("Purple");
                        playerTransform = objPlayer.transform;
                        //Debug.Log("6");
                        plusCheck = false;
                        curState = FSMState.Patrol;
                    }
                    break;
                case 7:
                    if (!GameObject.FindGameObjectWithTag("Red"))
                    {
                        objPlayer = null;
                        playerTransform = null;
                        //nameTag = 1;
                        Debug.Log(nameTag + "빨강이 없다.");
                        catchCheck = true;
                        CatchPlus();
                        isFlag = true;
                    }
                    else
                    {
                        objPlayer = GameObject.FindGameObjectWithTag("Red");
                        playerTransform = objPlayer.transform;
                        //Debug.Log("7");
                        plusCheck = false;
                        curState = FSMState.Patrol;
                    }
                    break;
            }
        }
        /*
		if (destPos == null) {
			CatchPlus ();
		}
        */
		if(!playerTransform||playerTransform==null && catchCheck){
            Debug.Log("플레이어가 없어서 재탐색합니다_에러방지");
            plusCheck = true;
        }
    }


    public void CatchPlus(){
		if(nameTag < 7){
			Debug.Log (gameObject.name + " " + nameTag + "CatchPlus()호출");
			nameTag++;
			catchCheck = false;
		}
		else if(nameTag == 7){
			if(isFlag){
				nameTag = 1;
				catchCheck = false;
                Debug.Log(gameObject.name + " " + nameTag + "CatchPlus()호출_처음으로");
                isFlag = false;
			}
		}
		curState = FSMState.Patrol;
	}
	/// <summary>
	/// Patrol state
	/// </summary>
	protected void UpdatePatrolState()
	{
		GetComponent<Animation>().Play("Walk");

        //Find another random patrol point if the current point is reached
        if (Vector3.Distance(transform.position, destPos) <= 0.5f)
		{
            //Debug.Log("다음 장소를 찾아줘" + gameObject.name);
			FindNextPoint();
		}
		//Check the distance with player tank
		//When the distance is near, transition to chase state
		else if (playerTransform && Vector3.Distance(transform.position, playerTransform.position) <= chaseDist)
		{
            //Debug.Log("쫓아가자!");
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
        if(playerTransform)
        {
            destPos = playerTransform.position;
            //Check the distance with player tank
            //When the distance is near, transition to attack state
            float dist = Vector3.Distance(transform.position, playerTransform.position);
            if (dist <= catchDist)
            {
                curState = FSMState.Attack;

            }
            else if ((dist >= catchDist) && (dist < chaseDist))
            {
                curState = FSMState.Chase;
            }
            //Go back to patrol is it become too far
            else if (dist >= chaseDist)
            {
                curState = FSMState.Patrol;
                FindNextPoint();
            }

            //Go Forward
            Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);
            transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
        }
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

        //Debug.Log ("Attack");
        GetComponent<Animation>().Play("Catch");
		catchCheck = true;
		plusCheck = true;

        //if (destPos == null || !playerTransform)
        if (!playerTransform)
        {
            CatchPlus();
        }
        else
        {
            destPos = playerTransform.position;
        }
        switch (nameTag)
        {
            case 1:
                if(GameObject.FindGameObjectWithTag("Orange"))
                {
                    otherObject = GameObject.FindGameObjectWithTag("Orange");
                    otherObject.SendMessage("DeadOK");
                }
                break;
            case 2:
                if (GameObject.FindGameObjectWithTag("Yellow"))
                {
                    otherObject = GameObject.FindGameObjectWithTag("Yellow");
                    otherObject.SendMessage("DeadOK");
                }
                break;
            case 3:
                if (GameObject.FindGameObjectWithTag("Green"))
                {
                    otherObject = GameObject.FindGameObjectWithTag("Green");
                    otherObject.SendMessage("DeadOK");
                }
                break;
            case 4:
                if (GameObject.FindGameObjectWithTag("Blue"))
                {
                    otherObject = GameObject.FindGameObjectWithTag("Blue");
                    otherObject.SendMessage("DeadOK");
                }
                break;
            case 5:
                if (GameObject.FindGameObjectWithTag("SkyBlue"))
                {
                    otherObject = GameObject.FindGameObjectWithTag("SkyBlue");
                    otherObject.SendMessage("DeadOK");
                }
                break;
            case 6:
                if (GameObject.FindGameObjectWithTag("Purple"))
                {
                    otherObject = GameObject.FindGameObjectWithTag("Purple");
                    otherObject.SendMessage("DeadOK");
                }
                break;
            case 7:
                if (GameObject.FindGameObjectWithTag("Red"))
                {
                    otherObject = GameObject.FindGameObjectWithTag("Red");
                    otherObject.SendMessage("DeadOK");
                }
                isFlag = true;
                break;
        }

        if(charprefab)
        {
            charprefab.SendMessage("OtherDead");
        }
	}
	
	/// <summary>
	/// Dead state
	/// </summary>
	protected void UpdateDeadState()
	{
		//Show the dead animation with some physics effects
		if (bDead)
		{
			Debug.Log (gameObject.name + "죽음");
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
        //Debug.Log(teleportVar + "걷는중이야" + gameObject.name);

        if(teleportVar ==1)
        {
            int rndIndex = Random.Range(0, pointList.Length);
            Vector3 rndPosition = Vector3.zero;
            destPos = pointList[rndIndex].transform.position + rndPosition;
            if (IsInCurrentRange(destPos))
            {
                //rndPosition = new Vector3(Random.Range(-rndRadius, rndRadius), 0.0f, Random.Range(-rndRadius, rndRadius));
                destPos = pointList[rndIndex].transform.position + rndPosition;
            }
        }
        else if(teleportVar ==2)
        {
            int rndIndex = Random.Range(0, pointList2.Length);
            Vector3 rndPosition = Vector3.zero;
            destPos = pointList2[rndIndex].transform.position + rndPosition;
            if (IsInCurrentRange(destPos))
            {
                //rndPosition = new Vector3(Random.Range(-rndRadius, rndRadius), 0.0f, Random.Range(-rndRadius, rndRadius));
                destPos = pointList2[rndIndex].transform.position + rndPosition;
            }
        }
        else if (teleportVar == 3)
        {
            int rndIndex = Random.Range(0, pointList3.Length);
            Vector3 rndPosition = Vector3.zero;
            destPos = pointList3[rndIndex].transform.position + rndPosition;
            if (IsInCurrentRange(destPos))
            {
                //rndPosition = new Vector3(Random.Range(-rndRadius, rndRadius), 0.0f, Random.Range(-rndRadius, rndRadius));
                destPos = pointList3[rndIndex].transform.position + rndPosition;
            }
        }
        else if (teleportVar == 4)
        {
            int rndIndex = Random.Range(0, pointList4.Length);
            Vector3 rndPosition = Vector3.zero;
            destPos = pointList4[rndIndex].transform.position + rndPosition;
            if (IsInCurrentRange(destPos))
            {
                //rndPosition = new Vector3(Random.Range(-rndRadius, rndRadius), 0.0f, Random.Range(-rndRadius, rndRadius));
                destPos = pointList4[rndIndex].transform.position + rndPosition;
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
	void SpringToWinter()
    {
        bool check = true;
        destPos = this.transform.position;
        if (check && teleportVar == 1) {
			Debug.Log("봄에서 겨울로" + gameObject.name);
            teleportVar = 4;
            FindNextPoint();
            check = false;
        }
		if (check && teleportVar == 4) {
			Debug.Log("겨울에서 봄으로" + gameObject.name);
            teleportVar = 1;
            FindNextPoint();
            check = false;
        }
	}
	
	void SpringToSummer()
    {
        bool check = true;
        destPos = this.transform.position;
        if (check && teleportVar == 1)
        {
            Debug.Log("봄에서 여름으로" + gameObject.name);
            teleportVar = 2;
            FindNextPoint();
            check = false;
        }

        if (check && teleportVar == 2)
        {
            Debug.Log("여름에서 봄으로" + gameObject.name);
            teleportVar = 1;
            FindNextPoint();
            check = false;
        }
    }
	
	void SummerToFall()
    {
        bool check = true;
        destPos = this.transform.position;
        if (check && teleportVar == 2)
        {
            Debug.Log("여름에서 가을로" + gameObject.name);
            teleportVar = 3;
            FindNextPoint();
            check = false;
        }
        if (check && teleportVar == 3)
        {
            Debug.Log("가을에서 여름으로" + gameObject.name);
            teleportVar = 2;
            FindNextPoint();
            check = false;
        }
    }
	
	void FallToWinter()
    {
        bool check = true;
        destPos = this.transform.position;
        if (check && teleportVar == 3)
        {
            Debug.Log("가을에서 겨울로" + gameObject.name);
            teleportVar = 4;
            FindNextPoint();
            check = false;
        }

        if (check && teleportVar == 4)
        {
            Debug.Log("겨울에서 가을로" + gameObject.name);
            teleportVar = 3;
            FindNextPoint();
            check = false;
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