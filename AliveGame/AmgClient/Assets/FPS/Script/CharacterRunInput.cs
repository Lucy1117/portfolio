
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JM.MyProject.MyGame
{
    public class CharacterRunInput : MonoBehaviour
    {
        /// <summary>
        /// 연속으로 입력을 받을 최소 초
        /// </summary>
        private float deftime = 0.4f;

        #region Vertical을 위한
        private float timerPlus;
        private float timerMinus;

        private bool checkVerticalPluszero = false;
        private bool checkPlusOnce = false;
        private bool checkPlusTwice = false;

        private bool checkVerticalMinuszero = false;
        private bool checkMinusOnce = false;
        private bool checkMinusTwice = false;

        #endregion

        #region Horizontal을 위한
        private float timerRight;
        private float timerLeft;

        private bool checkHorizontalRightzero = false;
        private bool checkRightOnce = false;
        private bool checkRightTwice = false;

        private bool checkHorizontalLeftzero = false;
        private bool checkLeftOnce = false;
        private bool checkLeftTwice = false;

        #endregion

        // Use this for initialization
        void Start()
        {
            timerPlus = deftime;
            timerMinus = deftime;
            timerRight = deftime;
            timerLeft = deftime;

        }

        // Update is called once per frame
        void Update()
        {

        }


        public bool RunKeyVertical(float inputvalue)
        {
            #region 앞으로 가는
            if (checkPlusOnce)
            {
                if (inputvalue == 0.0f)
                {//한번 누른 상태에서 버튼을 떼면
                    checkVerticalPluszero = true;
                }

                if (checkVerticalPluszero)
                {
                    if (checkPlusTwice)
                    {//두번 눌렀을 때
                        //Debug.Log("두번 눌렀음");
                        if (inputvalue == 0.0f)
                        {//두번 누른 상태에서 버튼에서 손을 뗐을 때.
                         //초기화
                            checkPlusOnce = false;
                            checkPlusTwice = false;
                            timerPlus = deftime;
                            return false;
                        }
                        return true;
                    }
                    else
                    {//한번 눌렀을 때
                        if (timerPlus > 0.0f && timerPlus < deftime)
                        {//일정 시간 내에
                            if (inputvalue > 0.0f)
                            {//두번 누르면
                                checkPlusTwice = true;
                            }
                        }
                    }
                }

                if (timerPlus < 0.0f)
                {//한번 누른 상태에서 일정 시간내에 안 누르면
                    checkPlusOnce = false;
                    checkPlusTwice = false;
                    timerPlus = deftime;
                    checkVerticalPluszero = false;
                }
                timerPlus -= Time.deltaTime;
            }
            else
            {//한번도 안눌렀을 때
                if (inputvalue > 0.0f)
                {//한번 누름
                    checkPlusOnce = true;
                }
                else if (inputvalue == 0.0f)
                {//한번도 안누름             
                    checkPlusOnce = false;
                    checkPlusTwice = false;
                    timerPlus = deftime;
                    checkVerticalPluszero = false;
                }
            }
            #endregion

            #region 뒤로가는
            if (checkMinusOnce)
            {
                if (inputvalue == 0.0f)
                {//한번 누른 상태에서 버튼을 떼면
                    checkVerticalMinuszero = true;
                }

                if (checkVerticalMinuszero)
                {
                    if (checkMinusTwice)
                    {//두번 눌렀을 때
                        //Debug.Log("두번 눌렀음");
                        if (inputvalue == 0.0f)
                        {//두번 누른 상태에서 버튼에서 손을 뗐을 때.
                         //초기화
                            checkMinusOnce = false;
                            checkMinusTwice = false;
                            timerMinus = deftime;
                            return false;
                        }
                        return true;
                    }
                    else
                    {//한번 눌렀을 때
                        if (timerMinus > 0.0f && timerMinus < deftime)
                        {//일정 시간 내에
                            if (inputvalue < 0.0f)
                            {//두번 누르면
                                checkMinusTwice = true;
                            }
                        }
                    }
                }

                if (timerMinus < 0.0f)
                {//한번 누른 상태에서 일정 시간내에 안 누르면
                    checkMinusOnce = false;
                    checkMinusTwice = false;
                    timerMinus = deftime;
                    checkVerticalMinuszero = false;
                }
                timerMinus -= Time.deltaTime;
            }
            else
            {//한번도 안눌렀을 때
                if (inputvalue < 0.0f)
                {//한번 누름
                    checkMinusOnce = true;
                }
                else if (inputvalue == 0.0f)
                {//한번도 안누름             
                    checkMinusOnce = false;
                    checkMinusTwice = false;
                    timerMinus = deftime;
                    checkVerticalMinuszero = false;
                }
            }
            #endregion
            return false;
        }

        public bool RunKeyHorizontal(float inputvalue)
        {
            #region 오른쪽으로 가는
            if (checkRightOnce)
            {
                if (inputvalue == 0.0f)
                {//한번 누른 상태에서 버튼을 떼면
                    checkHorizontalRightzero = true;
                }

                if (checkHorizontalRightzero)
                {
                    if (checkRightTwice)
                    {//두번 눌렀을 때
                        //Debug.Log("두번 눌렀음");
                        if (inputvalue == 0.0f)
                        {//두번 누른 상태에서 버튼에서 손을 뗐을 때.
                         //초기화
                            checkRightOnce = false;
                            checkRightTwice = false;
                            timerRight = deftime;
                            return false;
                        }
                        return true;
                    }
                    else
                    {//한번 눌렀을 때
                        if (timerRight > 0.0f && timerRight < deftime)
                        {//일정 시간 내에
                            if (inputvalue > 0.0f)
                            {//두번 누르면
                                checkRightTwice = true;
                            }
                        }
                    }
                }

                if (timerRight < 0.0f)
                {//한번 누른 상태에서 일정 시간내에 안 누르면
                    checkRightOnce = false;
                    checkRightTwice = false;
                    timerRight = deftime;
                    checkHorizontalRightzero = false;
                }
                timerRight -= Time.deltaTime;
            }
            else
            {//한번도 안눌렀을 때
                if (inputvalue > 0.0f)
                {//한번 누름
                    checkRightOnce = true;
                }
                else if (inputvalue == 0.0f)
                {//한번도 안누름             
                    checkRightOnce = false;
                    checkRightTwice = false;
                    timerRight = deftime;
                    checkHorizontalRightzero = false;
                }
            }
            #endregion
            
            #region 왼쪽으로가는
            if (checkLeftOnce)
            {
                if (inputvalue == 0.0f)
                {//한번 누른 상태에서 버튼을 떼면
                    checkHorizontalLeftzero = true;
                }

                if (checkHorizontalLeftzero)
                {
                    if (checkLeftTwice)
                    {//두번 눌렀을 때
                        //Debug.Log("두번 눌렀음");
                        if (inputvalue == 0.0f)
                        {//두번 누른 상태에서 버튼에서 손을 뗐을 때.
                         //초기화
                            checkLeftOnce = false;
                            checkLeftTwice = false;
                            timerLeft = deftime;
                            return false;
                        }
                        return true;
                    }
                    else
                    {//한번 눌렀을 때
                        if (timerLeft > 0.0f && timerLeft < deftime)
                        {//일정 시간 내에
                            if (inputvalue < 0.0f)
                            {//두번 누르면
                                checkLeftTwice = true;
                            }
                        }
                    }
                }

                if (timerLeft < 0.0f)
                {//한번 누른 상태에서 일정 시간내에 안 누르면
                    checkLeftOnce = false;
                    checkLeftTwice = false;
                    timerLeft = deftime;
                    checkHorizontalLeftzero = false;
                }
                timerLeft -= Time.deltaTime;
            }
            else
            {//한번도 안눌렀을 때
                if (inputvalue < 0.0f)
                {//한번 누름
                    checkLeftOnce = true;
                }
                else if (inputvalue == 0.0f)
                {//한번도 안누름             
                    checkLeftOnce = false;
                    checkLeftTwice = false;
                    timerLeft = deftime;
                    checkHorizontalLeftzero = false;
                }
            }
            #endregion
            return false;
        }
    }
}
