using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JM.MyProject.MyGame
{

    public class FPSControl : MonoBehaviour
    {

        public Transform theCamera;

        public float walkSpeed = 10.0f;
        public float runSpeed = 20.0f;
        public float gravity = 20.0f;

        public Vector3 velocity;

        public Vector3 MoveDirection = Vector3.zero;

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

        private Vector3 camVector;
        private Vector3 camValue;

        /// <summary>
        /// Item을 들고 있을 때와 아닐 때의 상태를 나타내주는 변수.
        /// Item을 들고 있으면 true
        /// </summary>
        public bool itemAnim;

        //캐릭터의 상태를 나타내주는 상태변수.
        public CharacterState _characterState;

        public bool isControllable = true; //컨트롤 가능 여부.


        public int characterAnim = 0;

        /// <summary>
        /// Dead Animation이 시작하면 true.거기서부터 초를 세서 확인. 35Frame.
        /// </summary>
        public bool deadCheck;


        // Use this for initialization
        void Start() {
            _animation = GetComponent<Animation>();
            _runInput = GetComponent<CharacterRunInput>();
            _animation[runAnimation.name].speed = 2.0f;
        }

        // Update is called once per frame
        void Update() {

            CharacterController controller = GetComponent<CharacterController>();

            float inputX = InputManager.ServerVertical();
            float inputZ = InputManager.ServerHorizontal();
            Vector3 moveVectorX = theCamera.forward.normalized * inputX;
            Vector3 moveVectorZ = theCamera.right.normalized * inputZ;

            //camVector = (theCamera.forward + theCamera.right).normalized;
            camVector = (theCamera.forward).normalized;
            camValue = new Vector3(camVector.x, 0, camVector.z);
            transform.LookAt(transform.position + camValue);
            //Quaternion rotation = Quaternion.LookRotation(camValue);
            //transform.rotation = rotation;

            if (controller.isGrounded)
            {
                Vector3 moveVector = (moveVectorX + moveVectorZ).normalized;
            
                velocity = new Vector3(moveVector.x, 0, moveVector.z);

                if (_runInput.RunKeyVertical(InputManager.ServerVertical())
                  || _runInput.RunKeyHorizontal(InputManager.ServerHorizontal()))
                {
                    Debug.Log("연속키");
                    velocity *= runSpeed;

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
                    velocity *= walkSpeed;

                    if (itemAnim)
                    {
                        _characterState = CharacterState.ItemWalk;
                    }
                    else
                    {
                        _characterState = CharacterState.Walk;
                    }
                }

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
                        else if (_characterState == CharacterState.Dead)
                        {
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
            }
            velocity.y -= gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}
