using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JM.MyProject.MyGame
{
    public class EmptyStoryBox : MonoBehaviour
    {
        public GameObject doorObjA;
        public GameObject doorObjB;


        private bool storyStart;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (storyStart)
            {
                if(doorObjA.GetComponent<DoorAnim>().opening || doorObjB.GetComponent<DoorAnim>().opening)
                {
                    StoryOff();
                    storyStart = false;
                }
            }
        
        }

        /// <summary>
        /// 이 object가 가진 스토리가 시작할 때.
        /// 자식인 StoryInformObj에서 호출
        /// </summary>
        public void StoryOn()
        {
            Debug.Log("StoryOn호출");
            storyStart = true;
        }

        /// <summary>
        /// 이 object가 가진 스토리가 끝났을 때.
        /// 자식인 StoryInformObj에서 호출
        /// </summary>
        public void StoryOff()
        {
            Debug.Log("StoryOff호출");
            this.gameObject.BroadcastMessage("StoryEnd");
        }

        /// <summary>
        /// 이 object가 가진 스토리를 스킵할 때.
        /// 자식인 StoryInformObj의 MyStoryDetail.cs에서 호출
        /// </summary>
        public void StorySkip()
        {
            storyStart = false;
            doorObjA.SendMessage("RemoteDoorControl");
            doorObjB.SendMessage("RemoteDoorControl");
        }
    }
}
