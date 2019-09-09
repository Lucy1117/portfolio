using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nettention.Proud;

namespace JM.MyProject.MyGame
{
    public class RemoteControl : MonoBehaviour
    {
        public PositionFollower m_positionFollower = new PositionFollower();

        public AngleFollower m_rotationFollower = new AngleFollower();

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

        public int animNum;
        //캐릭터의 상태를 나타내주는 상태변수.
        public CharacterState _characterState;


        private void Awake()
        {
            _animation = GetComponent<Animation>();
            animNum = 0;
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            m_rotationFollower.FrameMove(Time.deltaTime);
            m_positionFollower.FrameMove(Time.deltaTime);

            var p = new Nettention.Proud.Vector3();
            var v = new Nettention.Proud.Vector3();
            m_positionFollower.GetFollower(ref p, ref v);

            transform.position = new UnityEngine.Vector3((float)p.x, (float)p.y, (float)p.z);

            CharacterAnimationFun();
        }

        public void CharacterAnimationFun()
        {//0
            if (animNum == 0)
            {
                _animation[idleAnimation.name].speed = idleMaxAnimationSpeed;
                _animation.CrossFade(idleAnimation.name);
            }
            //1
            else if (animNum == 1)
            {
                _animation[walkAnimation.name].speed = walkMaxAnimationSpeed;
                _animation.CrossFade(walkAnimation.name);
            }
            //2
            else if (animNum == 2)
            {
                _animation[runAnimation.name].speed = runMaxAnimationSpeed;
                _animation.CrossFade(runAnimation.name);
            }
            //3
            else if (animNum == 3)
            {
                _animation[itemIdleAnimation.name].speed = idleMaxAnimationSpeed;
                _animation.CrossFade(itemIdleAnimation.name);
            }
            //4
            else if (animNum == 4)
            {
                _animation[itemWalkAnimation.name].speed = walkMaxAnimationSpeed;
                _animation.CrossFade(itemWalkAnimation.name);
            }
            //5
            else if (animNum == 5)
            {
                _animation[itemRunAnimation.name].speed = runMaxAnimationSpeed;
                _animation.CrossFade(itemRunAnimation.name);
            }
            //6
            else if (animNum == 6)
            {
                _animation[deadAnimation.name].speed = deadMaxAnimationSpeed;
                _animation.CrossFade(deadAnimation.name);
            }
        }
    }
}