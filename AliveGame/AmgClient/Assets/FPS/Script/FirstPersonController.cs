using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JM.MyProject.MyGame
{

    /// <summary>
    /// 캐릭터 상태 나타내는 열거형. 
    /// </summary>
    public enum CharacterState
    {
        Idle = 0,
        Walk = 1,
        Run = 2,
        ItemIdle = 3,
        ItemWalk = 4,
        ItemRun = 5,
        Dead = 6
    };

    public class FirstPersonController : MonoBehaviour
    {
        #region Animation Variables
        //애니메이션 클립 변수와 각 애니메이션의 속도 지정
        public AnimationClip idleAnimation;
        public AnimationClip walkAnimation;
        public AnimationClip runAnimation;
        public AnimationClip itemIdleAnimation;
        public AnimationClip itemWalkAnimation;
        public AnimationClip itemRunAnimation;
        public AnimationClip deadAnimation;

        //나중에 인스펙터에서 보면서 조정할 수 있게 일단 public으로
        public float idleMaxAnimationSpeed = 1.25f;
        public float walkMaxAnimationSpeed = 1.0f;
        public float runMaxAnimationSpeed = 1.0f;
        public float deadMaxAnimationSpeed = 1.0f;

        //Animation Component를 쉽게 쓰기 위한 변수
        private Animation _animation;
        private CharacterRunInput _runInput;

        //캐릭터의 상태를 나타내주는 상태변수.
        public CharacterState _characterState;
        

        #endregion

        #region Basic Variables
        public float walkSpeed;
        public float runSpeed;

        public float inAirControlAcceleration = 10.0f; //??? 공기 저항 가속도?

        public float gravity = 20.0f; //임의로 지정한 중력가속도.
        public float speedSmoothing = 10.0f;
        public float rotateSpeed = 500.0f;
        //public float trotAfterSeconds = 3.0f; //경보 후 멈추는 초.
        
        public Vector3 velocity = Vector3.zero;

        public float playerAngle = 0;
        public int characterAnim = 0;

        /// <summary>
        /// Server. 다른 클라이언트에게 각도를 전송할 때 쓰는 변수.
        /// </summary>
        // public float radAngle = 0;

        public Transform mainCam;
        private bool cameraDefaultCheck;
        private float defaultMainCamX;
        #endregion

        #region Etc Variables


        private Vector3 moveDirection = Vector3.zero; //x-z 평면 상에서의 현재방향.(0,0,0)

        //inspector 실험 결과 둘 다 계속 변화하는 변수
        private float verticalSpeed = 0.0f; // ??? 수직 속도.
        public float moveSpeed = 0.0f; //씬 안에서 캐릭터가 움직이는 속도. x-z평면 상에서의 속도.

        //??? 마지막 collision flags를 controller.Move로 반환.
        private CollisionFlags collisionFlags;

        //플레이어가 키를 누르면 활성화.
        private bool isMoving = false;
        //private float walkTimeStart = 0.0f;//걷기를 눌렀을 때 약간의 딜레이를 줄 수 있게. (필요 없으면 지워도 될 듯)

        private Vector3 inAirVelocity = Vector3.zero; //???
        
        public bool isControllable = true; //컨트롤 가능 여부.

        private Vector3 lastPos;

        /// <summary>
        /// Dead Animation이 시작하면 true.거기서부터 초를 세서 확인. 35Frame.
        /// </summary>
        public bool deadCheck;

        /// <summary>
        /// Item을 들고 있을 때와 아닐 때의 상태를 나타내주는 변수.
        /// Item을 들고 있으면 true
        /// </summary>
        public bool itemAnim;

        private AudioSource audiosource;

        private bool audioOK;

        #endregion


        //게임 시작 시 가장 먼저 초기화 되는 곳.
        void Awake()
        {
            //정면을 바라보도록.
            moveDirection = transform.TransformDirection(Vector3.forward);
            _animation = GetComponent<Animation>();
            if (!_animation)
            {
                Debug.Log("사용할 애니메이션이 없다.");
            }

            if (!idleAnimation)
            {
                _animation = null;
                Debug.Log("Idle Animation을 찾을 수 없습니다.");
            }

            if (!walkAnimation)
            {
                _animation = null;
                Debug.Log("Walk Animation을 찾을 수 없습니다.");
            }
            if (!runAnimation)
            {
                _animation = null;
                Debug.Log("Run Animation을 찾을 수 없습니다.");
            }
            if (!itemIdleAnimation)
            {
                _animation = null;
                Debug.Log("ItemIdle Animation을 찾을 수 없습니다.");
            }
            if (!itemWalkAnimation)
            {
                _animation = null;
                Debug.Log("ItemIdle Animation을 찾을 수 없습니다.");
            }
            if (!itemRunAnimation)
            {
                _animation = null;
                Debug.Log("ItemRun Animation을 찾을 수 없습니다.");
            }
            if (!deadAnimation)
            {
                _animation = null;
                Debug.Log("Dead Animation을 찾을 수 없습니다.");
            }

            _runInput = GetComponent<CharacterRunInput>();
            deadCheck = false;
            audiosource = GetComponent<AudioSource>();
            audioOK = true;
        }

        void UpdateSmoothedMovementDirection()
        {
            Transform cameraTransform = Camera.main.transform;
            bool grounded = IsGrounded();

            //---캐릭터가 보는 방향, 앞을 항상 받는 벡터.
            Vector3 forward = transform.forward;
            forward.y = 0;
            //normalized를 쓰면 벡터는 값을 갖지만 그 길이는 1.0이 된다.
            forward = forward.normalized; //생략해도 상관 없지만 이론적으론 있는 게 낫다.

            //항상 forward벡터에 직각이다.
            Vector3 right = new Vector3(forward.z, 0, -forward.x);


            //키보드나 조이스틱의 방향키. 항상 -1.0에서 1.0의 값을 가짐.
            float v = InputManager.MainVertical(); ; //수직 
            float h = InputManager.MainHorizontal();

            if(v == 0 && h == 0)
            {
                audioOK = true;
                audiosource.Stop();
            }
            else if(v != 0 || h != 0)
            {
                FootStepSound();
            }

            //조작키가 눌려서 그 절댓값이 0.1f이상이면 움직일 수 있도록 isMoving 변수를 활성화. 마찬가지로 0.1f의 텀.
            isMoving = Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f;

            /// 플레이어가 바라보는 방향의 벡터. 
            /// A, D 키에 따른 h의 변화는 x축을 대변하는 right벡터와 곱하고, 
            /// W, S키에 따른 v의 변화는 z축을 대변하는 forward에 곱함. 
            /// 이 두 벡터를 더해서 방향을 정함.
            Vector3 targetDirection = h * right + v * forward;
            if (grounded)
            {
                if (targetDirection != Vector3.zero)
                {
                    //플레이어가 보는 방향 벡터가 (0,0,0)이 아니면
                    if (moveSpeed < walkSpeed * 0.9f && grounded)
                    {
                        //moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth)의 값이 walkSpeed * 0.9f보다 작고 땅에 붙어있을 때
                        //moveDirection은 정면을 바라보도록 초기화 되어있음.
                        moveDirection = targetDirection.normalized;
                    }
                    else
                    {
                        //한쪽으로 회전. Quaternion.Slerp와 비슷하지만 이 함수에서는 각속도가 결코 maxDegreesDelta(3번째인수)를 초과하지 않는다.
                        moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
                        moveDirection = moveDirection.normalized;
                    }
                }

                float curSmooth = speedSmoothing * Time.deltaTime; //부드럽게 하는 효과.

                //Vector3.manitude는 벡터의 길이를 반환.
                //targetSpeed를 정함. 일정 이상 빨라지는 것을 방지.
                float targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f); //두 값 중 작은 값을 반환.


                if (itemAnim)
                {
                    _characterState = CharacterState.ItemIdle;
                }
                else
                {
                    _characterState = CharacterState.Idle;
                }

                #region Run State
                if (_runInput.RunKeyVertical(InputManager.MainVertical())
                    || _runInput.RunKeyHorizontal(InputManager.MainHorizontal()))
                {
                    targetSpeed *= runSpeed;

                    if (itemAnim)
                    {
                        _characterState = CharacterState.ItemRun;
                    }
                    else
                    {
                        _characterState = CharacterState.Run;
                    }
                }
                else
                {
                    targetSpeed *= walkSpeed;

                    if (itemAnim)
                    {
                        _characterState = CharacterState.ItemWalk;
                    }
                    else
                    {
                        _characterState = CharacterState.Walk;
                    }
                }
                #endregion



                moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);
                

                characterAnim = (int)_characterState;
            }
            else
            {
                if (isMoving)
                {
                    //targetDirection의 방향 값에 시간의 변화량을 곱하고 공기 가속도를 곱한 것을 가속도에 더함.
                    //inAirVelocity += targetDirection.normalized * Time.deltaTime * inAirControlAcceleration;
                }
            }
        }
        
        void ApplyGravity()
        {
            if (isControllable)
            { //컨트롤 하지 않을 때는 플레이어가 움직이지 않도록 함.
              //중력 적용.
                if (IsGrounded())
                {
                    //땅에 닿으면 점프 속도 0으로. 
                    //아닌 경우에는 중력가속도를 반대로 Time.deltaTime만큼.
                    verticalSpeed = 0.0f;
                }
                else
                {
                    verticalSpeed -= gravity * Time.deltaTime;
                }

            }
        }

        // Update is called once per frame
        void Update()
        {
            playerAngle = transform.rotation.eulerAngles.y;
            //radAngle = transform.rotation.y;

            if (isControllable)
            {
                UpdateSmoothedMovementDirection();

                //점프에 따른 중력 수정.
                ApplyGravity();

                //실제 움직임 계산.
                Vector3 movement = moveDirection * moveSpeed + new Vector3(0, verticalSpeed, 0) + inAirVelocity;
                movement *= Time.deltaTime;

                CharacterController controller = GetComponent<CharacterController>();
                collisionFlags = controller.Move(movement);
            }
            else
            {
                if(_characterState == CharacterState.Dead)
                {
                    audiosource.Stop();
                    _animation[deadAnimation.name].speed = deadMaxAnimationSpeed;
                    _animation.CrossFade(deadAnimation.name);
                    if (_animation.IsPlaying("Death"))
                    {
                        deadCheck = true;
                    }
                    else
                    {
                        deadCheck = false;
                    }
                }
                else
                {
                    if (itemAnim)
                    {
                        _characterState = CharacterState.ItemIdle;
                        _animation.CrossFade(itemIdleAnimation.name);
                    }
                    else
                    {
                        _characterState = CharacterState.Idle;
                        _animation.CrossFade(idleAnimation.name);
                    }
                }
               

                characterAnim = (int)_characterState;
            }

            velocity = (transform.position - lastPos) * 25;

            #region Animation Sector
            if (_animation)
            {
                if (this.isControllable && velocity.sqrMagnitude < 0.001f)
                {
                    //sqrMagnitude는 manitude값을 제곱한 값.
                    if (itemAnim)
                    {
                        _characterState = CharacterState.ItemIdle;
                        _animation[itemIdleAnimation.name].speed = idleMaxAnimationSpeed;
                        _animation.CrossFade(itemIdleAnimation.name);
                    }
                    else
                    {
                        _characterState = CharacterState.Idle;
                        _animation[idleAnimation.name].speed = idleMaxAnimationSpeed;
                        _animation.CrossFade(idleAnimation.name);
                    }
                    characterAnim = (int)_characterState;
                }
                else
                {
                    //0
                    if (_characterState == CharacterState.Idle)
                    {
                        _animation[idleAnimation.name].speed = idleMaxAnimationSpeed;
                        _animation.CrossFade(idleAnimation.name);
                    }
                    //1
                    else if (_characterState == CharacterState.Walk)
                    {
                        _animation[walkAnimation.name].speed = walkMaxAnimationSpeed;
                      
                        if (this.isControllable)
                        {
                            //Mathf.Clamp : 최소값과 최대값 제한 함수.
                            _animation[walkAnimation.name].speed = Mathf.Clamp(velocity.magnitude, 0.0f, walkMaxAnimationSpeed);
                        }
                        _animation.CrossFade(walkAnimation.name);
                    }
                    //2
                    else if (_characterState == CharacterState.Run)
                    {
                        _animation[runAnimation.name].speed = runMaxAnimationSpeed;
                        if (this.isControllable)
                        {
                            _animation[runAnimation.name].speed = Mathf.Clamp(velocity.magnitude, 0.0f, runMaxAnimationSpeed);
                        }
                        _animation.CrossFade(runAnimation.name);
                    }
                    //3
                    else if (_characterState == CharacterState.ItemIdle)
                    {
                        _animation[itemIdleAnimation.name].speed = idleMaxAnimationSpeed;
                        _animation.CrossFade(itemIdleAnimation.name);
                    }
                    //4
                    else if (_characterState == CharacterState.ItemWalk)
                    {
                        _animation[itemWalkAnimation.name].speed = walkMaxAnimationSpeed;
                        if (this.isControllable)
                        {
                            //Mathf.Clamp : 최소값과 최대값 제한 함수.
                            _animation[itemWalkAnimation.name].speed = Mathf.Clamp(velocity.magnitude, 0.0f, walkMaxAnimationSpeed);
                        }
                        _animation.CrossFade(itemWalkAnimation.name);
                    }
                    //5
                    else if (_characterState == CharacterState.ItemRun)
                    {
                        _animation[itemRunAnimation.name].speed = runMaxAnimationSpeed;
                        if (this.isControllable)
                        {
                            _animation[itemRunAnimation.name].speed = Mathf.Clamp(velocity.magnitude, 0.0f, runMaxAnimationSpeed);
                        }
                        _animation.CrossFade(itemRunAnimation.name);
                    }
                    //6
                    else if(_characterState == CharacterState.Dead)
                    {
                        audiosource.Stop();
                        _animation[deadAnimation.name].speed = deadMaxAnimationSpeed;
						_animation.CrossFade(deadAnimation.name);
                        if (_animation.IsPlaying("Death"))
                        {
                            deadCheck = true;
                        }
                        else
                        {
                            deadCheck = false;
                        }
                    }
                    characterAnim = (int)_characterState;
                }
            }

            #endregion

            //----------캐릭터 회전 추가---------

            //회전 키 값을 받아오는 변수.
            float tri = InputManager.RotateAxis();

            //움직임에 따른 회전 적용.
            if (IsGrounded() && this.isControllable)
            {
                if (tri < 0)
                {
                    transform.Rotate(0.0f, rotateSpeed * Time.deltaTime, 0.0f);
                }
                else if (tri > 0)
                {
                    transform.Rotate(0.0f, -rotateSpeed * Time.deltaTime, 0.0f);
                }

            }

            //---------------------------------

            //-----------캐릭터 위 아래 시점 이동 추가----------

            float vertiTri = InputManager.VerticalRotateAxis();

            if(IsGrounded() && this.isControllable)
            {
                //270degree를 기준으로 폭을 정함.
                ///이유는 모르겠으나, localEulerAngles를 사용했는데도, 카메라가 캐릭터에 달려있다보니
                ///그 transform의 정보를 받아와서 270도를 기준으로 위를 눌러도 값이 커지고 아래를 눌러도 값이 커지는 현상 발생.
                ///그래서, 7줄 아래 분기를 각각 나눠 아래를 눌렀을 때는 320도보다 클 경우,
                ///위를 눌렀을 때는 270보다 작을 경우로 코딩함.
                if (mainCam.localEulerAngles.x == 270.0f)
                {
                    cameraDefaultCheck = false;
                }
                if (cameraDefaultCheck && vertiTri == 0)
                {
                    mainCam.localRotation = Quaternion.Lerp(mainCam.localRotation, Quaternion.Euler(270.0f, 
                        mainCam.localEulerAngles.y, mainCam.localEulerAngles.z), 7.0f * Time.deltaTime);
                }
                else
                {
                    //아래
                    if (vertiTri < 0)
                    {
                        if (mainCam.localEulerAngles.x > 320.0f)
                        {
                            cameraDefaultCheck = true;
                        }
                        else
                        {
                            mainCam.Rotate(rotateSpeed * Time.deltaTime, 0.0f, 0.0f);
                        }
                    }
                    //위
                    else if (vertiTri > 0)
                    {
                        if (mainCam.localEulerAngles.x < 270.0f)
                        {
                            cameraDefaultCheck = true;
                        }
                        else
                        {
                            mainCam.Rotate(-rotateSpeed * Time.deltaTime, 0.0f, 0.0f);
                        }
                    }
                }
            }
            //--------------------------------

            lastPos = transform.position;
        }

        #region CallBack Return Value

        public bool IsGrounded()
        {
            //땅 위에 있는지 검사.
            return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
        }

        public bool IsMoving()
        {
            //방향키 값을 더한 게 0.5f가 넘어야 움직임. (키 겹침 방지를 위한 딜레이)
            return Mathf.Abs(InputManager.MainVertical()) + Mathf.Abs(InputManager.MainHorizontal()) > 0.5f;
        }
        
        #endregion

        /// <summary>
        /// 죽을 때마다 그 object에서 SendMessage로 호출.
        /// 플레이어가 죽으면, isControllable을 false로 바꾸어 움직일 수 없게 하고
        /// 캐릭터의 현재 상태를 Dead로 바꾼다.
        /// </summary>
        public void PlayerDead()
        {
            this.isControllable = false;
            _characterState = CharacterState.Dead;
        }

        /// <summary>
        /// GUI가 활성화되면 캐릭터가 멈춤
        /// </summary>
        /// <param name="guiStateNum"></param>
        public void ImActivate(int guiStateNum)
        {
            if (guiStateNum <= 7)
            {
                this.isControllable = false;
            }
            else if (guiStateNum == 8)
            {
                this.isControllable = true;
            }
        }

        private void FootStepSound()
        {
            if (isControllable && audioOK)
            {
                if (_characterState == CharacterState.Walk || _characterState == CharacterState.ItemWalk)
                {
                    audiosource.volume = 0.3f;
                    audiosource.pitch = 1;
                    audiosource.Play();
                    audioOK = false;
                }
            }
            else if(!isControllable)
            {
                audiosource.Stop(); 
            }

            if(_characterState == CharacterState.Run || _characterState == CharacterState.ItemRun)
            {
                audiosource.volume = 0.6f;
                audiosource.pitch = 2;
            }
        }
    }
}
