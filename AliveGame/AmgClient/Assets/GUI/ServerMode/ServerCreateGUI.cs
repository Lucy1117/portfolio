using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace JM.MyProject.MyGame
{
    public class ServerCreateGUI : MonoBehaviour
    {
        #region public 변수
        /// <summary>
        /// 1 스토리 서버모드 선택시 나오는 Object Group
        /// </summary>
        public GameObject selectModeObj;
        /// <summary>
        /// 2 로그인 창에서 나오는 Object Group
        /// </summary>
        public GameObject loginModeObj;
        /// <summary>
        /// 3 방 입장할 때 나오는 Object Group
        /// </summary>
        public GameObject entranceModeObj;
        /// <summary>
        /// 4 방에 입장한 후 다른 플레이어가 들어길 기다리는 Object Group
        /// </summary>
        public GameObject inRoomModeObj;

        #endregion

        #region private 변수

        /// <summary>
        /// 0 서버를 총괄하는 GameClient
        /// </summary>
        private GameClient gameClient;

        /// <summary>
        /// 1 스토리 서버모드 선택시 나오는 스토리버튼
        /// </summary>
        private GameObject storyBT;
        /// <summary>
        /// 1 스토리 서버모드 선택시 나오는 멀티모드버튼
        /// </summary>
        private GameObject multiBT;

        /// <summary>
        /// 2 로그인 창에서 나오는 로그인버튼
        /// </summary>
        private GameObject loginBT;
        /// <summary>
        /// 2 id가 입력되면 true 아니면 false
        /// </summary>
        private bool idInputOK;
        /// <summary>
        /// 2 pw가 입력되면 true 아니면 false
        /// </summary>
        private bool pwInputOK;

        /// <summary>
        /// 2 플레이어가 입력한 ID
        /// </summary>
        private string playerID;
        /// <summary>
        /// 2 플레이어가 입력한 PW
        /// </summary>
        private string playerPW;

        /// <summary>
        /// 3 빠른 방 찾기 서비스 버튼
        /// </summary>
        private GameObject fastRoomBT;
        /// <summary>
        /// 3 방 입장 또는 생성 버튼
        /// </summary>
        private GameObject goNcreateBT;
        /// <summary>
        /// 3 방 이름
        /// </summary>
        private string roomNameStr;
        /// <summary>
        /// 3 방 이름을 적으면 true, 적지 않으면 false
        /// </summary>
        private bool roomNameOK;
        /// <summary>
        /// 3 방을 새로 만들고 싶으면 체크
        /// </summary>
        private bool newRoomCheck;
        /// <summary>
        /// 4 첫번째 캐릭터 유저 이름
        /// </summary>
        private GameObject playerName1;
        /// <summary>
        /// 4 두번째 캐릭터 유저 이름
        /// </summary>
        private GameObject playerName2;
        /// <summary>
        /// 4 세번째 캐릭터 유저 이름
        /// </summary>
        private GameObject playerName3;
        /// <summary>
        /// 4 네번째 캐릭터 유저 이름
        /// </summary>
        private GameObject playerName4;

        private bool player1On;
        private bool player2On;
        private bool player3On;
        private bool player4On;

        private int playerFull;

        private bool playerOnce;

        private float timer;


        #endregion

        private void Awake()
        {
            if (GameObject.Find("GameClient"))
            {
                //Debug.Log("확인"); //20190819 nullreference error나서 확인함
                gameClient = GameObject.Find("GameClient").GetComponent<GameClient>();
            }
            else
            {
                gameClient = null;
            }
        }
        // Use this for initialization
        void Start()
        {
            selectModeObj.SetActive(true);
            GameObject.Find("BackGroundSound").SendMessage("SoundBGMPlay", 1);
        }

        // Update is called once per frame
        void Update()
        {
            if (selectModeObj.activeSelf)
            {
                SelectModeFun();
            }
            if (loginModeObj.activeSelf)
            {
                LogInModeFun();
            }
            if (entranceModeObj.activeSelf)
            {
                EntranceModeFun();
            }
            if (inRoomModeObj.activeSelf)
            {
                InRoomModeFun();
            }
        }
        #region ServerMode
        /// <summary>
        /// selectMode가 켜지면 그 자식 Object를 넣음
        /// </summary>
        private void SelectModeFun()
        {
            if (storyBT == null)
            {
                if (selectModeObj.activeSelf)
                {
                    storyBT = GameObject.Find("Single");
                    multiBT = GameObject.Find("Multi");
                }
            }
        }
        /// <summary>
        /// 서버 입장 시 Single버튼을 누르면 실행되는 함수
        /// </summary>
        public void StoryBtClick()
        {
            if (gameClient.p_state == ServerState.Standby)
            {
                Debug.Log("스토리 모드 실행");
                GameObject.Find("ButtonSound").SendMessage("ButtonEffectPlay", 2);
                SceneManager.LoadScene("StartMenu");

                selectModeObj.SetActive(false);
            }
        }
        /// <summary>
        /// 서버 입장 시 Multi버튼을 누르면 실행되는 함수
        /// </summary>
        public void MultiBTClick()
        {
            if (gameClient.p_state == ServerState.Standby)
            {
                GameObject.Find("ButtonSound").SendMessage("ButtonEffectPlay", 2);
                gameClient.p_state = ServerState.ConnectingServer;
                gameClient.SendMessage("ServerConnect");

                selectModeObj.SetActive(false);
                loginModeObj.SetActive(true);
            }
        }

        #endregion

        #region LogInMode
        /// <summary>
        /// loginMode가 켜지면 그 자식 Object를 넣음
        /// </summary>
        private void LogInModeFun()
        {
            if (loginBT == null)
            {
                if (loginModeObj.activeSelf)
                {
                    loginBT = GameObject.Find("LogIn");
                }
            }
        }

        /// <summary>
        /// 로그인 창에서 로그인 버튼을 누르면 실행되는 함수
        /// </summary>
        public void LoginBTClick()
        {
            //ID와 PW를 모두 입력했을 때만 실행
            if (idInputOK && pwInputOK)
            {
                gameClient.PlayerInform(playerID, playerPW);
                if (gameClient.p_state == ServerState.StayLogin)
                {
                    GameObject.Find("ButtonSound").SendMessage("ButtonEffectPlay", 2);
                    gameClient.p_state = ServerState.ConnectingLogin;
                    gameClient.SendMessage("LoginConnect");

                    loginModeObj.SetActive(false);
                    entranceModeObj.SetActive(true);
                }
            }

        }
        /// <summary>
        /// 로그인 창에서 ID 입력을 완료하면 실행되는 함수
        /// </summary>
        public void IDEndEdit()
        {
            playerID = GameObject.Find("IDInputText").GetComponent<Text>().text;
            idInputOK = true;
            Debug.Log(playerID);
        }
        /// <summary>
        /// 로그인 창에서 PW 입력을 완료하면 실행되는 함수
        /// </summary>
        public void PWEndEdit()
        {
            playerPW = GameObject.Find("PWInputText").GetComponent<Text>().text;
            pwInputOK = true;
            Debug.Log(playerPW);
        }
        #endregion


        #region EntranceMode

        /// <summary>
        /// EntranceMode가 켜지면 그 자식 object를 넣음
        /// </summary>
        private void EntranceModeFun()
        {
            if (fastRoomBT == null)
            {
                if (entranceModeObj.activeSelf)
                {
                    fastRoomBT = GameObject.Find("FastRoomFind");
                    goNcreateBT = GameObject.Find("GoRoom");
                }
            }
        }

        /// <summary>
        /// 빠른 방 찾기 누른 경우.
        /// GameClient내의 함수를 호출하면 됨.
        /// </summary>
        public void FastRoomClick()
        {
            if (gameClient.p_state == ServerState.StayLobby)
            {
                GameObject.Find("ButtonSound").SendMessage("ButtonEffectPlay", 2);
                gameClient.p_state = ServerState.ConnectingLooby;
                gameClient.SendMessage("LobbyConnectFast");

                GameObject.Find("BackGroundSound").SendMessage("SoundBGMPlay", 2);
            }
        }

        private void FastRoomSucceed()
        {
            entranceModeObj.SetActive(false);
            inRoomModeObj.SetActive(true);

            playerName1 = GameObject.Find("PlayerName1");
            playerName2 = GameObject.Find("PlayerName2");
            playerName3 = GameObject.Find("PlayerName3");
            playerName4 = GameObject.Find("PlayerName4");
        }

        /// <summary>
        /// 해당 방에 입장하거나 새로 방을 만드는 함수.
        /// </summary>
        public void GoOrCreateClick()
        {
            if (roomNameOK)
            {
                gameClient.RoomInform(roomNameStr, newRoomCheck);

                if (gameClient.p_state == ServerState.StayLobby)
                {
                    gameClient.p_state = ServerState.ConnectingLooby;
                    gameClient.SendMessage("LobbyConnect");

                    entranceModeObj.SetActive(false);
                    inRoomModeObj.SetActive(true);
                }
            }
        }

        /// <summary>
        /// 방 이름 편집이 끝날 때 마다 호출.
        /// </summary>
        public void RoomNameEndEdit()
        {
            roomNameStr = GameObject.Find("NewRoomText").GetComponent<Text>().text;
            if (roomNameStr.Length < 2)
            {
                //GUI.Label(new Rect(10, 30, 200, 80), "방 이름이 너무 짧습니다.");
                roomNameOK = false;
            }
            else
            {
                //GUI.Label(new Rect(10, 30, 200, 80), " ");
                roomNameOK = true;
            }
        }

        /// <summary>
        /// 새로 만들기가 눌릴 때마다 호출.
        /// </summary>
        public void NewRoomEndEdit()
        {
            if (GameObject.Find("NewRoomCheck").GetComponent<Toggle>().isOn)
            {
                GameObject.Find("ButtonSound").SendMessage("ButtonEffectPlay", 2);
                GameObject.Find("GoRoom").transform.GetChild(0).gameObject.GetComponent<Text>().text = "생성";
                newRoomCheck = true;

                GameObject.Find("BackGroundSound").SendMessage("SoundBGMPlay", 2);
            }
            else
            {
                GameObject.Find("ButtonSound").SendMessage("ButtonEffectPlay", 2);
                GameObject.Find("GoRoom").transform.GetChild(0).gameObject.GetComponent<Text>().text = "입장";
                newRoomCheck = false;

                GameObject.Find("BackGroundSound").SendMessage("SoundBGMPlay", 2);
            }
        }

        #endregion

        #region InRoomMode

        /// <summary>
        /// InRoomMode가 켜지면 그 자식 object를 넣음
        /// </summary>
        private void InRoomModeFun()
        {
            if (playerName1 == null)
            {
                if (inRoomModeObj.activeSelf)
                {
                    playerName1 = GameObject.Find("PlayerName1");
                    playerName2 = GameObject.Find("PlayerName2");
                    playerName3 = GameObject.Find("PlayerName3");
                    playerName4 = GameObject.Find("PlayerName4");
                }
            }

            if (playerOnce)
            {
                timer += Time.deltaTime;

                if (timer > 5.0f)
                {
                    AllPlayerIn();
                    playerOnce = false;
                }
            }
        }

        /// <summary>
        /// 유저의 이름을 하단에 적음.
        /// </summary>
        private void WritePlayerName()
        {
            if (gameClient.m_playerName1 != null && !player1On)
            {
                playerName1.GetComponent<Text>().text = gameClient.m_playerName1;
                player1On = true;
                Debug.Log("플레이어1의 이름은" + playerName1.GetComponent<Text>().text + "입니다.");
            }
            if (gameClient.m_playerName2 != null && !player2On)
            {
                playerName2.GetComponent<Text>().text = gameClient.m_playerName2;
                player2On = true;
                Debug.Log("플레이어2의 이름은" + playerName2.GetComponent<Text>().text + "입니다.");
            }
            if (gameClient.m_playerName3 != null && !player3On)
            {
                playerName3.GetComponent<Text>().text = gameClient.m_playerName3;
                player3On = true;
                Debug.Log("플레이어3의 이름은" + playerName3.GetComponent<Text>().text + "입니다.");
            }
            if (gameClient.m_playerName4 != null && !player4On)
            {
                playerName4.GetComponent<Text>().text = gameClient.m_playerName4;
                player4On = true;
                Debug.Log("플레이어4의 이름은" + playerName4.GetComponent<Text>().text + "입니다.");
            }
            if (player1On && player2On && player3On && player4On)
            {
                playerOnce = true;
            }
        }
        /// <summary>
        /// 유저의 이름을 삭제
        /// </summary>
        private void RemovePlayerName(string username)
        {
            if (playerName1.GetComponent<Text>().text == username)
            {
                playerName1.GetComponent<Text>().text = "Player1";
                player1On = false;
            }
            else if (playerName2.GetComponent<Text>().text == username)
            {
                playerName2.GetComponent<Text>().text = "Player2";
                player2On = false;
            }
            else if (playerName3.GetComponent<Text>().text == username)
            {
                playerName3.GetComponent<Text>().text = "Player3";
                player3On = false;
            }
            else if (playerName4.GetComponent<Text>().text == username)
            {
                playerName4.GetComponent<Text>().text = "Player4";
                player4On = false;
            }
        }

        /// <summary>
        /// 내 방에 4명의 플레이어가 모이면 게임이 시작하게
        /// </summary>
        private void AllPlayerIn()
        {
            if (gameClient.p_state == ServerState.InRoom)
            {
                //GUI를 비춰주던 카메라를 끔
                GameObject.Find("Main Camera").SetActive(false);

                Cursor.visible = false;

                gameClient.p_state = ServerState.ConnectionRoom;
                gameClient.SendMessage("RequestInGameFun");

                inRoomModeObj.SetActive(false);

                GameObject.Find("BackGroundSound").SendMessage("SoundBGMPlay", 4);
            }
        }

        #endregion
    }
}