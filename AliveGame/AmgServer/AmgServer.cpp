// AmgServer.cpp : Defines the entry point for the console application.


#include "stdafx.h"
#include <cstdlib>
#include <ctime>
#include "AmgServer.h"
#include "..\AmgCommon\Common.h"




void CAmgServer::OnClientJoin(CNetClientInfo *clientInfo) {

	CriticalSectionLock lock(m_critSec, true);
	printf("%s: %d\n", __FUNCTION__, clientInfo->m_HostID);
}

void CAmgServer::OnClientLeave(CNetClientInfo *clientInfo, ErrorInfo *errorinfo, const ByteArray& comment) {

	CriticalSectionLock lock(m_critSec, true);
	printf("%s: %d\n", __FUNCTION__, clientInfo->m_HostID);

	CRoomPtr_S room;

	//플레이어 오브젝트를 만듬.
	CRemoteClientPtr_S player(new CRemoteClient_S);

	if (m_remoteClients.TryGetValue(clientInfo->m_HostID, room)) {
		//클라이언트 목록에서 클라이언트가 어느 방에 들어가있는지 찾음.
		room->m_players.TryGetValue(clientInfo->m_HostID, player);
		RemoveCharacter(room, player->m_characterNum);

		vector<int> others;
		for (auto otherIter : room->m_players) {
			others.push_back(otherIter.first);
		}
		m_S2CProxy.Player_Disappear((HostID*)others.data(), others.size(),
			RmiContext::ReliableSend,
			clientInfo->m_HostID, player->m_userID);

		room->m_players.RemoveKey(clientInfo->m_HostID);


		if (room->m_players.GetCount() == 0) {
			//방에 아무도 없는 경우 방을 파괴.
			m_rooms.RemoveKey(room->m_name);
		}

		m_remoteClients.RemoveKey(clientInfo->m_HostID);
	}

}

void CAmgServer::OnP2PGroupJoinMemberAckComplete(HostID groupHostID, HostID memberHostID, ErrorType result) {

}

void CAmgServer::OnUserWorkerThreadBegin() {

}

void CAmgServer::OnUserWorkerThreadEnd() {

}
//아래는 내부 에러 발생 시 print해주기 위해 쓴 것(프라우드넷 내부에서 에러로그를 발생 시키지만, 가시성을 높이기 위해 일단 구현)
void CAmgServer::OnError(ErrorInfo *errorInfo) {
	errorCheck = true;
	//printf("%s: %d\n", __FUNCTION__, StringW2A(errorInfo->ToString()));
}

void CAmgServer::OnWarning(ErrorInfo *errorInfo) {
	//printf("%s: %d\n", __FUNCTION__, StringW2A(errorInfo->ToString()));
}

void CAmgServer::OnInformation(ErrorInfo *errorInfo) {
	//printf("%s: %d\n", __FUNCTION__, StringW2A(errorInfo->ToString()));
}

void CAmgServer::OnException(const Exception &e) {
	//printf("%s: %d\n", __FUNCTION__, e.what());
}

void CAmgServer::OnNoRmiProcessed(RmiID rmiID) {
	throw std::exception("The method or operation is not implemented.");
}


//////////////dbhandler/////////////


void CAmgServer::OnJoinDbCacheServerComplete(ErrorInfo *info) {
	if (info->m_errorType == ErrorType_Ok) {
		puts("DB cache server ready.");
	}
	else {
		//printf("DB cache server failed1. reason: %s\n", StringW2A(info->ToString()));

		//m_runLoop = false;
	}
}

void CAmgServer::OnLeaveDbCacheServer(ErrorType reason) {
	//printf("DB cache server failed2.reason: %s\n", StringW2A(ErrorInfo::TypeToString(reason)));
	//m_runLoop = false;
}

//posted when DB flush is done
void CAmgServer::OnDbmsWriteDone(DbmsWritePropNodePendType type, Guid loadedDataGuid) {
}


//posted when loading data is failed.
void CAmgServer::OnExclusiveLoadDataFailed(CCallbackArgs& args) {
}

//...is successful.
bool CAmgServer::OnExclusiveLoadDataSuccess(CCallbackArgs& args) {
	//must return true in most cases.
	//if false, the loaded data will be instantly unloaded.
	return true;
}


//data loading is basically exlusive. only one game server can loac and access data tree.
//this is called back when oother game server is attempting to load a data tree which is
//already loaded by this game server.
void CAmgServer::OnDataUnloadRequested(CCallbackArgs& args) {
}

void CAmgServer::OnAddDataFailed(CCallbackArgs& args) {
	throw std::exception("The method or operation is not implemented.");
}

void CAmgServer::OnAddDataSuccess(CCallbackArgs& args) {
	throw std::exception("The method or operation is not implemented.");
}

void CAmgServer::OnUpdateDataFailed(CCallbackArgs& args) {
	throw std::exception("The method or operation is not implemented.");
}

void CAmgServer::OnUpdateDataSuccess(CCallbackArgs& args) {
	throw std::exception("The method or operation is not implemented.");
}

void CAmgServer::OnRemoveDataFailed(CCallbackArgs& args) {
	throw std::exception("The method or operation is not implemented.");
}

void CAmgServer::OnRemoveDataSuccess(CCallbackArgs& args) {
	throw std::exception("The method or operation is not implemented.");
}

void CAmgServer::OnAccessError(CCallbackArgs& args) {
	throw std::exception("The method or operation is not implemented.");
}

//--------------error

void CAmgServer::OnExclusiveLoadDataComplete(CCallbackArgs& args) {
}

void CAmgServer::OnDataForceUnloaded(CCallbackArgs& args) {
}

void CAmgServer::OnIsolateDataSuccess(CCallbackArgs& args) {
}

void CAmgServer::OnIsolateDataFailed(CCallbackArgs& args) {
}

void CAmgServer::OnDeisolateDataSuccess(CCallbackArgs& args) {
}

void CAmgServer::OnDeisolateDataFailed(CCallbackArgs& args) {
}
//-----------------




CAmgServer::~CAmgServer() {
	delete m_netServer;
}

CAmgServer::CAmgServer() {

	//프라우드넷의 메인 객체는 CNetServer라는 이름을 가진 클래스.
	m_netServer = CNetServer::Create();
	m_netServer->SetEventSink(this); //for receiveing events

	m_netServer->AttachProxy(&m_S2CProxy);
	m_netServer->AttachStub(this);
	//Proxy(송신)과 Stub(수신)을 NetServer에 부착시킴.

	m_dbCacheClient = CDbCacheClient2::New();
}


void CAmgServer::Start() {
	ErrorInfoPtr err;
	CStartServerParameter sp;
	sp.m_protocolVersion = g_amgProtocolVersion;
	sp.m_tcpPorts.Add(15001); //클라이언트와 똑같은 프로토콜 버전을 서버에도 지정. 프라우드넷은 다수의 TCP 리스닝 포트를 사용할 수 있음.
#pragma region horror

							  ///udp타입과 p2pgroupmember를 추가
	sp.m_udpPorts.Add(15001);
	sp.m_allowServerAsP2PGroupMember = true;
#pragma endregion

	m_netServer->Start(sp, err);

	//db cache Server와 db cache Client가 시작될때까지 기다림.
	//sleep은 추천하지 않는 방법이고, 더 좋은 방법은 
	//dbServer에 연결될 때까지 Loop를 돌며 확인하는 것.
	Sleep(3000);

	CDbCacheClient2ConnectParameter dp;
	dp.m_authenticationKey = g_amgDbCacheServerAuthenticationKey;
	dp.m_delegate = this;
	dp.m_serverAddr = L"172.30.1.15";
	//dp.m_serverAddr = L"192.168.0.11";
	//dp.m_serverAddr = L"localhost";
	//dp.m_serverAddr = L"192.168.99.13";
	dp.m_serverPort = 1433;

	if (!m_dbCacheClient->Connect(dp, err)) {
		//printf("cannot start DB cache client3! reason: %s\n", StringW2A(err->ToString()));
		//m_runLoop = false;
	}
}

int CAmgServer::CreateRandomCharacter(CRoomPtr_S characterroom) {

	int randomCaracter;

	srand((unsigned int)time(NULL));
	randomCaracter = rand() % 4 + 1;

	switch (randomCaracter) {
	case 1:
		if (!characterroom->m_maxman) {
			characterroom->m_maxman = true;
			return 1;
			break;
		}
		else {
			randomCaracter = + 1;
		}
	case 2:
		if (!characterroom->m_lion) {
			characterroom->m_lion = true;
			return 2;
			break;
		}
		else {
			randomCaracter = + 1;
		}
	case 3:
		if (!characterroom->m_betakkuma) {
			characterroom->m_betakkuma = true;
			return 3;
			break;
		}
		else {
			randomCaracter = + 1;
		}
	case 4:
		if (!characterroom->m_wazosky) {
			characterroom->m_wazosky = true;
			return 4;
		}
		else {
			if (!characterroom->m_maxman) {
				characterroom->m_maxman = true;
				return 1;
				break;
			}
			if (!characterroom->m_lion) {
				characterroom->m_lion = true;
				return 2;
				break;
			}
			if (!characterroom->m_betakkuma) {
				characterroom->m_betakkuma = true;
				return 3;
				break;
			}
		}
	}
}

void CAmgServer::RemoveCharacter(CRoomPtr_S characterroom, int removeNum) {

	switch (removeNum) {
	case 1:
		characterroom->m_maxman = false;
		break;
	case 2:
		characterroom->m_lion = false;
		break;
	case 3:
		characterroom->m_betakkuma = false;
		break;
	case 4:
		characterroom->m_wazosky = false;
		break;
	}
}

///Client to Server.
///서버로의 입장을 요청
DEFRMI_MultiC2S_RequestServerIn(CAmgServer) {

	//critical section 임계영역
	//어떤 스레드가 이 영역에 접근하려면 지정된 시간만큼 대기해야 함
	CriticalSectionLock lock(m_critSec, true);

	//에러 체크해서 없으면
	if (!errorCheck) {
		m_S2CProxy.ReplyServerIn(remote, RmiContext::ReliableSend);
	}
	else { //에러가 있으면
		m_S2CProxy.ReplyServerFailed(remote, RmiContext::ReliableSend, L"예상치 못한 에러로 서버와의 연결이 끊어졌습니다.");
	}
	return true;
}

///Client to Server
DEFRMI_MultiC2S_RequestLogin(CAmgServer) {

	CriticalSectionLock lock(m_critSec, true);

	//cout << "RequestLogin    " << StringT2A(id.c_str()) << "  " << StringT2A(password.c_str()) << endl;

	///이제 여기의 분기에서 DB검색 후, 로그인 성공 실패 여부 판단하면 될 듯.
	///일단은 에러체크로 두는 걸로.
	if (!errorCheck) {
		m_S2CProxy.NotifyLoginSuccess(remote, RmiContext::ReliableSend);
	}
	else {
		m_S2CProxy.NotifyLoginFailed(remote, RmiContext::ReliableSend, L"로그인에 실패했습니다.");
	}

	return true;
}

DEFRMI_MultiC2S_RequestRoomList(CAmgServer) {

	CriticalSectionLock lock(m_critSec, true);

	return true;
}

DEFRMI_MultiC2S_RequestInRoom(CAmgServer) {

	CriticalSectionLock lock(m_critSec, true);

	CRoomPtr_S room;
	int mycharacterNum;

	//isNewRoom이 체크되면, roomName을 이름으로 갖는 새로운 방을 만들겠다는 뜻.
	if (isNewRoom) {
		if (m_rooms.TryGetValue(roomName, room)) {
			m_S2CProxy.ReplyInRoom(remote, RmiContext::ReliableSend, 1, L"이미 존재하는 방입니다.", 0);
			return true;
		}

		//새로운 방 만들기
		room = CRoomPtr_S(new CRoom_S);
		room->m_name = roomName;
		m_rooms.Add(roomName, room);
		room->m_p2pGroupID = m_netServer->CreateP2PGroup();
	}
	else {
		if (!m_rooms.TryGetValue(roomName, room)) {
			///m_rooms.TryGetValue(roomName,room)부터 실행한 후 !실행.
			///그러므로 roomName을 키로 갖는 값이 m_rooms Dictionary에 있으면 room포인터에 그 값을 넣는다.
			///이 room 정보를 바탕으로 else문 밖 하단의 ReplyInRoom이 실행될 수 있는 것.
			m_S2CProxy.ReplyInRoom(remote, RmiContext::ReliableSend, 1, L"존재하지 않는 방입니다.", 0);
			return true;
		}
	}

	///네명이 다 차면 못들어가게 하는 코드 작성. ReplyInRoom의 세 번째 인자가 3이면 꽉찼음을 Unity에서 명시적으로 보여줘야.
	if (room->m_players.GetCount() == 4) {

		m_S2CProxy.ReplyInRoom(remote, RmiContext::ReliableSend, 1, L"인원이 초과하여 입장할 수 없습니다.", 0);
		return true;
	}

	mycharacterNum = CreateRandomCharacter(room);

	m_S2CProxy.ReplyInRoom(remote, RmiContext::ReliableSend, 0, L"방에 입장합니다.", mycharacterNum);


	//플레이어 오브젝트를 만듬.
	CRemoteClientPtr_S player(new CRemoteClient_S);

	//플레이어가 방에 입장함.
	room->m_players.Add(remote, player);

	///horror
	player->m_userID = id;
	player->m_characterNum = mycharacterNum;

	m_netServer->JoinP2PGroup(remote, room->m_p2pGroupID);

	m_remoteClients.Add(remote, room);

	/* 후에 참고할 코딩
	for each(auto iWO in room->m_worldObjects) {
	int worldObjectID = iWO.GetFirst();
	CWorldObjectPtr_S worldObject = iWO.GetSecond();
	m_S2CProxy.NotifyAddTree(remote, RmiContext::ReliableSend, worldObjectID, worldObject->m_position);
	}
	*/
	return true;
}

DEFRMI_MultiC2S_RequestFastRoom(CAmgServer) {

	CriticalSectionLock lock(m_critSec, true);

	CRoomPtr_S room;

	int mycharacterNum;

	if (m_rooms.Count == 0) {
		//방이 존재하지 않습니다.
		m_S2CProxy.ReplyInRoom(remote, RmiContext::ReliableSend, 1, L"현재 열린 방이 없습니다.", 0);
		return true;
	}
	else {
		//방이 존재한다면, 방을 순회하며 4명 이하인 방에 입장할 수 있도록.
		for (auto roomIter : m_rooms) {
			if (m_rooms.Lookup(roomIter.first)->m_value->m_players.Count < 4) {
				printf("%d\n", m_rooms.Lookup(roomIter.first)->m_value->m_players.Count);
				m_rooms.TryGetValue(m_rooms.Lookup(roomIter.first)->m_key, room);
				mycharacterNum = CreateRandomCharacter(room);
				m_S2CProxy.ReplyInRoom(remote, RmiContext::ReliableSend, 2, L"4명 이하인 방을 찾아서 입장합니다.", mycharacterNum);
				//플레이어 오브젝트를 만듬.
				CRemoteClientPtr_S player(new CRemoteClient_S);

				//플레이어가 방에 입장함.
				room->m_players.Add(remote, player);

				///horror
				player->m_userID = id;
				player->m_characterNum = mycharacterNum;

				m_netServer->JoinP2PGroup(remote, room->m_p2pGroupID);

				m_remoteClients.Add(remote, room);

				printf("%d\n", m_rooms.Lookup(roomIter.first)->m_value->m_players.Count);

				return true;
			}
			else {
				m_S2CProxy.ReplyInRoom(remote, RmiContext::ReliableSend, 3, L"4명 이하인 방을 찾을 수 없습니다.", 0);
				return true;
			}
		}
	}
	return true;
}


DEFRMI_MultiC2S_RequestInGame(CAmgServer) {

	CriticalSectionLock lock(m_critSec, true);
	CRoomPtr_S room;

	m_rooms.TryGetValue(roomName, room);

	cout << "게임에 입장하고있어! \n";
	m_S2CProxy.ReplyInGame(remote, RmiContext::ReliableSend, 0, L"게임에 입장하는 중입니다.");
	return true;

	/*
	///네명이 들어오면 게임 입장을 허락하게 코드를 짜려했으나
	///room->m_plwyer.count에서 계속 오류 발생. CFastMap.h가 자꾸 뜸. 무슨 에러인지 해결하지 못함.
	///GetCount()도 써보았지만 해결 못함. size도 안 먹힘.
	///위에서 쓴 코딩도 아래처럼 써 보았지만 안됨.
	printf("%d\n",m_rooms.Lookup(roomName)->m_value->m_players.Count);

	if (m_rooms.Lookup(roomName)->m_value->m_players.Count == 4) {
		//방에 4명의 플레이어가 모였으므로 입장합니다.
		cout << "게임에 입장하고있어! \n";
		m_S2CProxy.ReplyInGame(remote, RmiContext::ReliableSend, 0, L"게임에 입장하는 중입니다.");
		return true;
	}
	else {
		//방에 총 4명의 플레이어가 아니므로 에러입니다.
		m_S2CProxy.ReplyInGame(remote, RmiContext::ReliableSend, 1, L"에러가 발견되었습니다. 4명의 플레이어가 아닙니다.");
		return true;
	}

	return true;
	*/
}

DEFRMI_MultiC2S_JoinGameScene(CAmgServer) {

	CriticalSectionLock lock(m_critSec, true);

	cout << "JoinGameScene 호출 됨! \n";

	CRoomPtr_S room;

	//auto it = room->m_players.find(remote);

	//room = m_remoteClients.find(remote);

	if (m_remoteClients.TryGetValue(remote, room)) {

		auto it = room->m_players.find(remote);

		if (it == room->m_players.end()) {

			return true;
		}

		auto& rc = it->second;

		//update player info
		rc->m_x = x;
		rc->m_y = y;
		rc->m_z = z;
		rc->m_angle = angle;
		rc->m_animNum = animNum;

		///함수 안에서 인자를 받았을 때 그 인자의 값이 정상적으로 들어왔는지 1차적 검사.
		///예기치 않은 값이 들어왔을 때 오류를 띄워 확인할 수 있다.
		assert(it->first == remote);

		for (auto otherIter : room->m_players) {
			///받은 플레이어 캐릭터 정보를 업데이트 함.
			///기존 클라이언트 목록을 순회하며 새 클라이언트 등장을 알림.(반대도)
			if (otherIter.first != it->first) {
				auto& otherRC = otherIter.second;
				m_S2CProxy.Player_Appear((HostID)it->first, RmiContext::ReliableSend,
					(HostID)otherIter.first,
					otherRC->m_userID,
					otherRC->m_x, otherRC->m_y, otherRC->m_z,
					otherRC->m_vx, otherRC->m_vy, otherRC->m_vz,
					otherRC->m_angle,
					otherRC->m_animNum,
					otherRC->m_characterNum);

				m_S2CProxy.Player_Appear((HostID)otherIter.first, RmiContext::ReliableSend,
					(HostID)it->first,
					rc->m_userID,
					rc->m_x, rc->m_y, rc->m_z,
					rc->m_vx, rc->m_vy, rc->m_vz,
					rc->m_angle,
					rc->m_animNum,
					rc->m_characterNum);
			}
		}
	}

	/*	auto it = room->m_players.find(remote);

	if (it == room->m_players.end()) {

	return true;
	}


	auto& rc = it->second;

	//update player info
	rc->m_x = x;
	rc->m_y = y;
	rc->m_z = z;
	rc->m_angle = angle;

	///함수 안에서 인자를 받았을 때 그 인자의 값이 정상적으로 들어왔는지 1차적 검사.
	///예기치 않은 값이 들어왔을 때 오류를 띄워 확인할 수 있다.
	assert(it->first == remote);

	for (auto otherIter : room->m_players) {
	///받은 플레이어 캐릭터 정보를 업데이트 함.
	///기존 클라이언트 목록을 순회하며 새 클라이언트 등장을 알림.(반대도)
	if (otherIter.first != it->first) {
	auto& otherRC = otherIter.second;
	m_S2CProxy.Player_Appear((HostID)it->first, RmiContext::ReliableSend,
	(HostID)otherIter.first,
	otherRC->m_userID,
	otherRC->m_x, otherRC->m_y, otherRC->m_z,
	otherRC->m_vx, otherRC->m_vy, otherRC->m_vz,
	otherRC->m_angle);

	m_S2CProxy.Player_Appear((HostID)otherIter.first, RmiContext::ReliableSend,
	(HostID)it->first,
	rc->m_userID,
	rc->m_x, rc->m_y, rc->m_z,
	rc->m_vx, rc->m_vy, rc->m_vz,
	rc->m_angle);
	}
	}*/

	return true;
}

//Player Appear을 위해 필요하므로 서버에서 움직임에 대한 정보를 받아놓음.
DEFRMI_MultiC2S_Player_SMove(CAmgServer) {

	CriticalSectionLock lock(m_critSec, true);

	cout << "Player_SMove! \n";

	CRoomPtr_S room;

	//auto it = room->m_players.find(remote);

	//room = m_remoteClients.find(remote);

	if (m_remoteClients.TryGetValue(remote, room)) {

		auto it = room->m_players.find(remote);

		if (it == room->m_players.end()) {

			return true;
		}


		auto& rc = it->second;

		rc->m_x = x;
		rc->m_y = y;
		rc->m_z = z;
		rc->m_vx = vx;
		rc->m_vy = vy;
		rc->m_vz = vz;
		rc->m_angle = angle;
		rc->m_animNum = animNum;
	}


	return true;
}



///나무심기
DEFRMI_MultiC2S_RequestAddTree(CAmgServer) {

	CriticalSectionLock lock(m_critSec, true);

	CRoomPtr_S room;
	if (m_remoteClients.TryGetValue(remote, room)) {
		//나무 추가
		CWorldObjectPtr_S tree(new CWorldObject_S);
		tree->m_id = room->m_nextNewID;
		tree->m_position = position;

		room->m_worldObjects.Add(tree->m_id, tree);
		room->m_nextNewID++;

		//모든 이들에게 알리기.
		m_S2CProxy.NotifyAddTree(room->m_p2pGroupID, RmiContext::ReliableSend, tree->m_id, position);
	}
	return true;
}

///나무심기
DEFRMI_MultiC2S_RequestRemoveTree(CAmgServer) {

	CriticalSectionLock lock(m_critSec, true);

	CRoomPtr_S room;
	if (m_remoteClients.TryGetValue(remote, room)) {

		//나무 삭제
		CWorldObjectPtr_S tree;
		if (room->m_worldObjects.TryGetValue(treeID, tree)) {

			room->m_worldObjects.RemoveKey(treeID);

			//모든 이들에게 알리기.
			m_S2CProxy.NotifyRemoveTree(room->m_p2pGroupID, RmiContext::ReliableSend, treeID);
		}
	}
	return true;
}
