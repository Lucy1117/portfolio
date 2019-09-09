using System.Collections;
using System;
using UnityEngine;
using Nettention.Proud;
using UnityEngine.SceneManagement;

namespace JM.MyProject.MyGame
{
    //접속 전, 접속, 로그온 등을 정의하기 위한 상태 변수
    public enum ServerState
    {
        /// <summary>
        /// 서버 접속 이전의 클라이언트 상태
        /// </summary>
        Standby,
        /// <summary>
        /// 서버 접속을 요청하고 있는 중. 연결 중.
        /// </summary>
        ConnectingServer,
        /// <summary>
        /// 로그인 창에 입장한 상태
        /// </summary>
        StayLogin,
        /// <summary>
        /// 로그인을 요청하고 있는 중. 연결 중.
        /// </summary>
        ConnectingLogin,
        /// <summary>
        /// 로비 창에 입장한 상태
        /// </summary>
        StayLobby,
        /// <summary>
        /// 방 입장을 요청하고 있는 중. 방 입장에 연결 중.
        /// </summary>
        ConnectingLooby,
        /// <summary>
        /// 방에 입장해서 캐릭터 고르기. 이후 Start버튼을 누르면 게임씬에 연결
        /// </summary>
        InRoom,
        /// <summary>
        /// 게임 입장을 요청하고 있는 중. 게임씬에 연결 중
        /// </summary>
        ConnectionRoom,
        /// <summary>
        /// 게임씬 안. 게임을 하는 중.
        /// </summary>
        InGame,
        /// <summary>
        /// 모든 것에 대한 실패.
        /// </summary>
        Failed,
    };

    public class GameClient : MonoBehaviour
    {
        //접속할 기본 서버 주소 설정
        //서버의 IP주소를 넣음. 초기값으로 주고, 개인이 입력하지 못하도록 하는 게 좋을 듯.
        //string m_serverAddr = "203.252.215.230";
        //string m_serverAddr = "192.168.99.61";
        //string m_serverAddr = "192.168.99.13";
        string m_serverAddr = "172.30.1.15";
        //string m_serverAddr = "192.168.99.16";

        //로비에서 생성될 방 이름.
        string m_roomName = "New Room";

        bool m_requestNewRoom = false;

        //접속 실패 시 UI에서 보여줄 에러 메시지.
        string m_failMessage = " ";

        //ProudNet 클라이언트 모듈은 NetClient. 멤버 변수로 추가.
        NetClient m_netClient = new NetClient();

        string m_userID = "Max";
        string m_password = "1234";

        #region 생성될 캐릭터
        public GameObject m_localPlayer1;
        public GameObject m_localPlayer2;
        public GameObject m_localPlayer3;
        public GameObject m_localPlayer4;
        public GameObject m_otherPlayer1;
        public GameObject m_otherPlayer2;
        public GameObject m_otherPlayer3;
        public GameObject m_otherPlayer4;
        #endregion

        /// <summary>
        /// 생성되는 플레이어
        /// </summary>
        private GameObject m_PlayerInfo;
        /// <summary>
        /// PlayerMove를 하는 주기. 무조건 처음엔 실행되도록 음수값을 둔다.
        /// </summary>
        private float m_lastSendTime = -1;

        //Server to Client, Client to Server, Client to Client를 위한 선언.
        MultiC2S.Proxy m_C2SProxy = new MultiC2S.Proxy();
        MultiS2C.Stub m_S2CStub = new MultiS2C.Stub();
        MultiC2C.Stub m_C2CStub = new MultiC2C.Stub();
        MultiC2C.Proxy m_C2CProxy = new MultiC2C.Proxy();

        //멀티캐스트 P2P 통신할때 같은 방 내의 사람들에게만 메세지가 전송되도록 그룹아이디를 놓음.
        HostID m_p2pGroupID = HostID.HostID_None;

        #region 캐릭터 선택

        private int characterNum;

        private string playerName1;
        private string playerName2;
        private string playerName3;
        private string playerName4;

        public string m_playerName1
        {
            get { return playerName1; }
            set { playerName1 = value; }
        }
        public string m_playerName2
        {
            get { return playerName2; }
            set { playerName2 = value; }
        }
        public string m_playerName3
        {
            get { return playerName3; }
            set { playerName3 = value; }
        }
        public string m_playerName4
        {
            get { return playerName4; }
            set { playerName4 = value; }
        }

        #endregion

        private ServerState m_state = ServerState.Standby;

        public ServerState p_state
        {
            get { return m_state; }
            set { m_state = value; }
        }

        void Start()
        {
            //서버의 RMI 함수와 연결.
            m_netClient.AttachProxy(m_C2SProxy);
            m_netClient.AttachStub(m_S2CStub);
            m_netClient.AttachProxy(m_C2CProxy);
            m_netClient.AttachStub(m_C2CStub);

            m_S2CStub.ReplyServerIn = (Nettention.Proud.HostID remote, Nettention.Proud.RmiContext rmiContext) =>
            {
                Debug.Log("ReplyServerIn()수신");
                m_state = ServerState.StayLogin;
                return true;
            };

            m_S2CStub.ReplyServerFailed = (Nettention.Proud.HostID remote, Nettention.Proud.RmiContext rmiContext, String reason) =>
            {
                Debug.Log("ReplyServerFailed()수신");
                m_state = ServerState.Failed;
                m_failMessage = reason;
                return true;
            };

            m_S2CStub.NotifyLoginSuccess = (Nettention.Proud.HostID remote, Nettention.Proud.RmiContext rmiContext) =>
            {
                Debug.Log("NotifyLoginSuccess()수신");
                m_state = ServerState.StayLobby;
                return true;
            };

            m_S2CStub.NotifyLoginFailed = (Nettention.Proud.HostID remote, Nettention.Proud.RmiContext rmiContext, String reason) =>
            {
                Debug.Log("NotifyLoginFailed()수신");
                m_state = ServerState.Failed;
                m_failMessage = reason;
                return true;
            };

            m_S2CStub.ReplyInRoom = (Nettention.Proud.HostID remote, Nettention.Proud.RmiContext rmiContext, int result, String comment, int choiceCharacter) =>
            {
                if (result == 0 || result == 2)
                {
                    if (result == 2)
                    {
                        GameObject.Find("ServerCanvas").SendMessage("FastRoomSucceed");
                    }
                    Debug.Log("ReplyLogon실행중");
                    m_state = ServerState.InRoom;

                    characterNum = choiceCharacter;

                    switch (choiceCharacter)
                    {
                        case 1:
                            //맥스
                            playerName1 = m_userID;
                            Debug.Log("맥스" + m_playerName1 + " " + playerName1);
                            break;
                        case 2:
                            //라이언
                            playerName2 = m_userID;
                            Debug.Log("라이언" + m_playerName2 + " " + playerName2);
                            break;
                        case 3:
                            //곰
                            playerName3 = m_userID;
                            Debug.Log("곰" + m_playerName3 + " " + playerName3);
                            break;
                        case 4:
                            //와죠스키
                            playerName4 = m_userID;
                            Debug.Log("와죠스키" + m_playerName4 + " " + playerName4);
                            break;
                        default:
                            break;
                    }
                    GameObject.Find("ServerCanvas").SendMessage("WritePlayerName");
                }
                else if (result == 3)
                {
                    m_state = ServerState.StayLobby;
                    m_failMessage = "입장 실패: " + result.ToString() + " " + comment;
                }
                else
                {
                    m_state = ServerState.Failed;
                    m_failMessage = "입장 실패: " + result.ToString() + " " + comment;
                }
                return true;
            };

            //다른 플레이어가 캐릭터를 선택하는 것을 2초마다 수신
            m_C2CStub.OtherCharacterSelect = (Nettention.Proud.HostID remote, Nettention.Proud.RmiContext rmiContext, System.String userID, int choiceCharacter) =>
            {
                switch (choiceCharacter)
                {
                    case 1:
                        //맥스
                        playerName1 = userID;
                        break;
                    case 2:
                        //라이언
                        playerName2 = userID;
                        break;
                    case 3:
                        //곰
                        playerName3 = userID;
                        break;
                    case 4:
                        //와죠스키
                        playerName4 = userID;
                        break;
                    default:
                        break;
                }
                GameObject.Find("ServerCanvas").SendMessage("WritePlayerName");
                return true;
            };

            m_S2CStub.ReplyInGame = (Nettention.Proud.HostID remote, Nettention.Proud.RmiContext rmiContext, int result, String comment) =>
            {
                if (result == 0)
                {
                    //캐릭터 생성. ReplyInGame에 들어갈 내용
                    GameObject myPlayer = null;
                    switch (characterNum)
                    {
                        case 1:
                            myPlayer = m_localPlayer1;
                            break;
                        case 2:
                            myPlayer = m_localPlayer2;
                            break;
                        case 3:
                            myPlayer = m_localPlayer3;
                            break;
                        case 4:
                            myPlayer = m_localPlayer4;
                            break;
                        default:
                            Debug.Log("캐릭터 선택 과정에서 오류가 발생했습니다.");
                            break;
                    }
                    Transform localcontrol = myPlayer.GetComponent<Transform>();

                    m_PlayerInfo = Instantiate(myPlayer,
                        new UnityEngine.Vector3(localcontrol.position.x, localcontrol.position.y, localcontrol.position.z),
                        Quaternion.identity);

                    m_C2SProxy.JoinGameScene(HostID.HostID_Server, RmiContext.ReliableSend,
                        m_PlayerInfo.transform.position.x,
                        m_PlayerInfo.transform.position.y,
                        m_PlayerInfo.transform.position.z,
                        //m_PlayerInfo.GetComponent<FirstPersonController>().moveSpeed,
                        0.0f,
                        m_PlayerInfo.GetComponent<FPSControl>().characterAnim);

                    m_state = ServerState.InGame;
                }
                else
                {
                    m_state = ServerState.Failed;
                    m_failMessage = "입장 실패: " + result.ToString() + " " + comment;
                }
                return true;
            };

            //Server to Client. 다른 플레이어 생성을 수신함.
            m_S2CStub.Player_Appear = (HostID remote, RmiContext rmiContect, int hostID, System.String userID, float x, float y, float z, float vx, float vy, float vz, float angle, int animNum, int choiceCharacter) =>
            {
                Debug.Log("다른 플레이어 생성");
                if (hostID != (int)m_netClient.GetLocalHostID())
                {
                    GameObject remotePlayerCharacter = null;
                    GameObject remotePlayerInfo = null;

                    switch (choiceCharacter)
                    {
                        case 1:
                            remotePlayerCharacter = m_otherPlayer1;
                            break;
                        case 2:
                            remotePlayerCharacter = m_otherPlayer2;
                            break;
                        case 3:
                            remotePlayerCharacter = m_otherPlayer3;
                            break;
                        case 4:
                            remotePlayerCharacter = m_otherPlayer4;
                            break;
                        default:
                            Debug.Log("다른 캐릭터를 받아오는 과정에서 오류가 발생했습니다.");
                            break;
                    }
                    Transform otherControl = remotePlayerCharacter.GetComponent<Transform>();
                    remotePlayerInfo = (GameObject)Instantiate(remotePlayerCharacter, new UnityEngine.Vector3(otherControl.position.x, otherControl.position.y, otherControl.position.z), Quaternion.identity);
                    remotePlayerInfo.name = "m_otherPlayer/" + hostID.ToString();
                }
                return true;
            };

            //Server to Client. 다른 플레이어 삭제를 수신.
            m_S2CStub.Player_Disappear = (Nettention.Proud.HostID remote, Nettention.Proud.RmiContext rmiContext, int hostID, System.String userID) =>
            {
                var g = GameObject.Find("m_otherPlayer/" + hostID.ToString());
                if (g != null)
                {
                    Debug.Log("다른 플레이어 삭제");
                    Destroy(g);
                }
                else
                {
                    GameObject.Find("ServerCanvas").SendMessage("RemovePlayerName", userID);
                }
                return true;
            };

            m_C2CStub.Player_CMove = (Nettention.Proud.HostID remote, Nettention.Proud.RmiContext rmiContext,
               float x, float y, float z, float vx, float vy, float vz, float angle, int animNum) =>
            {
                var g = GameObject.Find("m_otherPlayer/" + remote.ToString());
                Quaternion newAngle;
                if (g != null)
                {
                    var control = g.GetComponent<RemoteControl>();
                    var p = new Nettention.Proud.Vector3();
                    p.x = x;
                    p.y = y;
                    p.z = z;
                    var v = new Nettention.Proud.Vector3();
                    v.x = vx;
                    v.y = vy;
                    v.z = vz;
                    control.m_positionFollower.SetTarget(p, v);

                    newAngle = Quaternion.Euler(0, angle, 0);

                    g.transform.rotation = Quaternion.Slerp(g.transform.rotation, newAngle, 50.0f * Time.deltaTime);

                    control.animNum = animNum;
                }
                return true;
            };
        }

        void Update()
        {
            m_netClient.FrameMove();

            switch (m_state)
            {
                case ServerState.InGame:
                    InGameConnect();
                    break;
            }
        }

        public void OnGUI()
        {
            GUI.color = Color.black;
            switch (m_state)
            {
                case ServerState.Standby:
                    OnGUI_Stanby();
                    break;
                case ServerState.ConnectingServer:
                case ServerState.StayLogin:
                    OnGUI_StayLogin();
                    break;
                case ServerState.ConnectingLogin:
                case ServerState.StayLobby:
                    OnGUI_StayLobby();
                    break;
                case ServerState.ConnectingLooby:
                    break;
                case ServerState.InRoom:
                    OnGUI_InRoom();
                    break;
                case ServerState.ConnectionRoom:
                    break;
                case ServerState.InGame:
                    OnGUI_InGame();
                    break;
                case ServerState.Failed:
                    GUI.Label(new Rect(10, 30, 200, 80), m_failMessage);
                    if (GUI.Button(new Rect(10, 100, 180, 30), "Quit"))
                    {
                        Application.Quit();
                    }
                    break;
            }
        }

        #region OnGUI함수 모음
        /// <summary>
        /// 서버 모드 입장시 실행 될 GUI화면
        /// 마지막 배포할 때 삭제해야 함.
        /// 임의로 서버 아이피를 적을 수 있게 한 것(개발자 버전으로)
        /// </summary>
        private void OnGUI_Stanby()
        {
            GUI.color = Color.cyan;
            GUI.Label(new Rect(10, 110, 180, 30), "IP입력 : ");
            m_serverAddr = GUI.TextField(new Rect(70, 110, 180, 30), m_serverAddr);
        }


        /// <summary>
        /// 로그인 화면에서 실행 될 GUI화면
        /// </summary>
        private void OnGUI_StayLogin()
        {
            //Canvas로 옮김
        }

        /// <summary>
        /// 0921추가
        /// ServerCreateGUI에서 호출. 플레이어의 정보를 보냄.
        /// </summary>
        /// <param name="playerID"></param>
        /// <param name="playerPW"></param>
        public void PlayerInform(string playerID, string playerPW)
        {
            m_userID = playerID;
            m_password = playerPW;
        }

        /// <summary>
        /// 0921추가
        /// ServerCreateGUI에서 호출. 방의 정보를 보냄.
        /// </summary>
        /// <param name="roomTag"></param>
        /// <param name="newRoomCh"></param>
        public void RoomInform(string roomTag, bool newRoomCh)
        {
            m_roomName = roomTag;
            m_requestNewRoom = newRoomCh;
        }

        /// <summary>
        /// 0921추가
        /// ServerCreateGUI에서 호출. 방의 정보를 보냄.
        /// </summary>
        public void RequestInGameFun()
        {
            m_C2SProxy.RequestInGame(HostID.HostID_Server, RmiContext.ReliableSend, m_roomName);
        }


        /// <summary>
        /// 로비화면에서 실행 될 GUI화면
        /// </summary>
        private void OnGUI_StayLobby()
        {
            GUI.Label(new Rect(10, 30, 200, 80), m_failMessage);
        }

        /// <summary>
        /// 방에 입장 후 실행 될 GUI화면
        /// </summary>
        private void OnGUI_InRoom()
        {
            //타 캐릭터의 상태를 받아오기 위해 계속 호출
            InRoomConnect();
        }


        /// <summary>
        /// 게임에 입장 후 실행 될 GUI화면
        /// </summary>
        private void OnGUI_InGame()
        {
            InGameConnect();
        }
        #endregion

        #region Update함수 모음

        /// <summary>
        /// 스토리 대전 모드 화면에서 서버에 접속할 때 실행되는 함수.
        /// </summary>
        private void ServerConnect()
        {
            m_netClient.JoinServerCompleteHandler = (ErrorInfo info, ByteArray replyFromServer) =>
            {
                Debug.Log("ServerConnect 연결");
                if (info.errorType == ErrorType.Ok)
                { //서버 접속 성공 처리.
                    Debug.Log("서버 접속 성공 처리.");
                    m_C2SProxy.RequestServerIn(HostID.HostID_Server, RmiContext.ReliableSend);
                }
                else
                { //서버 접속 실패 처리.
                    Debug.Log("서버 접속 실패 처리");
                    m_state = ServerState.Failed;
                    m_failMessage = info.ToString();
                }
            };
            //서버 연결 성공 후 갑자기 서버 연결이 끊어졌을 때. 비 정상적 접속 실패에 대한 처리.
            m_netClient.LeaveServerHandler = (ErrorInfo info) =>
            {
                m_state = ServerState.Failed;
                m_failMessage = "ServerConnect()에서 서버로부터의 연결에 실패했습니다: " + info.ToString();
            };

            NetConnectionParam cp = new NetConnectionParam();
            cp.serverIP = m_serverAddr;

            cp.serverPort = 15001;
            //프로토콜 버전은 GUID형태.
            //서버와 클라이언트가 버전이 같은 지를 판단하는 척도. 서버와 클라이언트 둘 다 같아야 접속 허가가 떨어짐.
            cp.protocolVersion = new Nettention.Proud.Guid("{0x3003de6b,0xea63,0x490d,{0xbe,0xef,0x1a,0x44,0x15,0x4e,0xc3,0x1c}}");

            //NetClient의 Connect 함수를 호출하면 서버로 접속을 시작.
            //이 함수는 호출하면 즉시 리턴, 비동기로 실행. 백그라운드에서 서버의 연결 과정이 진행 됨.
            m_netClient.Connect(cp);
        }

        /// <summary>
        /// 로그인 화면에서 로비에 입장할 때 실행되는 함수.
        /// </summary>
        private void LoginConnect()
        {
            m_C2SProxy.RequestLogin(HostID.HostID_Server, RmiContext.SecureReliableSend, m_userID, m_password);
        }

        /// <summary>
        /// 로비 화면에서 방에 입장할 때 실행되는 함수.
        /// </summary>
        private void LobbyConnect()
        {
            //Reliable 형식에으로 암호화.
            //방을 새로 만들거나 이미 있는 방에서 검색해서 들어갈때
            m_C2SProxy.RequestInRoom(HostID.HostID_Server, RmiContext.ReliableSend, m_roomName, m_requestNewRoom, m_userID);

            ///p2p통신 할 때 쓰는 듯.
            m_netClient.P2PMemberJoinHandler = (HostID memberHostID, HostID groupHostID, int memberCount, ByteArray replyFromServer) =>
            {
                m_p2pGroupID = groupHostID;
            };
        }

        private void LobbyConnectFast()
        {
            //빠른대전 구현
            m_C2SProxy.RequestFastRoom(HostID.HostID_Server, RmiContext.ReliableSend, m_userID);
            ///p2p통신 할 때 쓰는 듯.
            m_netClient.P2PMemberJoinHandler = (HostID memberHostID, HostID groupHostID, int memberCount, ByteArray replyFromServer) =>
            {
                m_p2pGroupID = groupHostID;
            };
        }

        /// <summary>
        /// 방에 입장한 상태에서 실행되는 함수.
        /// </summary>
        private void InRoomConnect()
        {
            if (m_netClient != null)
            {
                if (m_netClient.GetLocalHostID() != HostID.HostID_None)
                {
                    if (m_lastSendTime < 0 || Time.time - m_lastSendTime > 2.0f) //2초마다 다른 캐릭터가 입장했는지 확인함
                    {

                        var sendOption = new RmiContext();

                        //일단 UDP로 메시지를 보냄. 패킷이 조금 유실되는 것을 감수하고!
                        sendOption.reliability = MessageReliability.MessageReliability_Unreliable;
                        sendOption.maxDirectP2PMulticastCount = 30;
                        sendOption.enableLoopback = false;

                        m_C2CProxy.OtherCharacterSelect(m_p2pGroupID, sendOption, m_userID, characterNum);

                        m_lastSendTime = Time.time;
                    }
                }
            }
        }

        /// <summary>
        /// 게임씬에 입장한 상태에서 실행되는 함수
        /// </summary>
        private void InGameConnect()
        {
            if (m_netClient != null)
            {
                if (m_netClient.GetLocalHostID() != HostID.HostID_None)
                {
                    if (m_lastSendTime < 0 || Time.time - m_lastSendTime > 0.1f) //0.1초마다 보내줌.
                    {
                        //pc: player controller
                        var pc = m_PlayerInfo.GetComponent<FPSControl>();

                        var sendOption = new RmiContext();

                        //일단 UDP로 메시지를 보냄. 패킷이 조금 유실되는 것을 감수하고!
                        sendOption.reliability = MessageReliability.MessageReliability_Unreliable;
                        sendOption.maxDirectP2PMulticastCount = 30;
                        sendOption.enableLoopback = false;

                        m_C2CProxy.Player_CMove(m_p2pGroupID, sendOption,
                         m_PlayerInfo.transform.position.x,
                         m_PlayerInfo.transform.position.y,
                         m_PlayerInfo.transform.position.z,
                         pc.velocity.x,
                         pc.velocity.y,
                         pc.velocity.z,
                         pc.transform.rotation.eulerAngles.y,
                         pc.characterAnim);
                        m_C2SProxy.Player_SMove(m_p2pGroupID, sendOption,
                         m_PlayerInfo.transform.position.x,
                         m_PlayerInfo.transform.position.y,
                         m_PlayerInfo.transform.position.z,
                         pc.velocity.x,
                         pc.velocity.y,
                         pc.velocity.z,
                         pc.transform.rotation.eulerAngles.y,
                         pc.characterAnim);

                        m_lastSendTime = Time.time;
                    }
                }
            }
        }

        #endregion
        void OnDestroy()
        {//에디터에서 실행중인 게임을 종료시키더라도 object들이 모두 파괴되지 않으므로, 명시적으로 표현.
            m_netClient.Dispose();
        }

    }
}