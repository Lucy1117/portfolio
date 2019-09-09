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

	//�÷��̾� ������Ʈ�� ����.
	CRemoteClientPtr_S player(new CRemoteClient_S);

	if (m_remoteClients.TryGetValue(clientInfo->m_HostID, room)) {
		//Ŭ���̾�Ʈ ��Ͽ��� Ŭ���̾�Ʈ�� ��� �濡 ���ִ��� ã��.
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
			//�濡 �ƹ��� ���� ��� ���� �ı�.
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
//�Ʒ��� ���� ���� �߻� �� print���ֱ� ���� �� ��(������� ���ο��� �����α׸� �߻� ��Ű����, ���ü��� ���̱� ���� �ϴ� ����)
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

	//��������� ���� ��ü�� CNetServer��� �̸��� ���� Ŭ����.
	m_netServer = CNetServer::Create();
	m_netServer->SetEventSink(this); //for receiveing events

	m_netServer->AttachProxy(&m_S2CProxy);
	m_netServer->AttachStub(this);
	//Proxy(�۽�)�� Stub(����)�� NetServer�� ������Ŵ.

	m_dbCacheClient = CDbCacheClient2::New();
}


void CAmgServer::Start() {
	ErrorInfoPtr err;
	CStartServerParameter sp;
	sp.m_protocolVersion = g_amgProtocolVersion;
	sp.m_tcpPorts.Add(15001); //Ŭ���̾�Ʈ�� �Ȱ��� �������� ������ �������� ����. ��������� �ټ��� TCP ������ ��Ʈ�� ����� �� ����.
#pragma region horror

							  ///udpŸ�԰� p2pgroupmember�� �߰�
	sp.m_udpPorts.Add(15001);
	sp.m_allowServerAsP2PGroupMember = true;
#pragma endregion

	m_netServer->Start(sp, err);

	//db cache Server�� db cache Client�� ���۵ɶ����� ��ٸ�.
	//sleep�� ��õ���� �ʴ� ����̰�, �� ���� ����� 
	//dbServer�� ����� ������ Loop�� ���� Ȯ���ϴ� ��.
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
///�������� ������ ��û
DEFRMI_MultiC2S_RequestServerIn(CAmgServer) {

	//critical section �Ӱ迵��
	//� �����尡 �� ������ �����Ϸ��� ������ �ð���ŭ ����ؾ� ��
	CriticalSectionLock lock(m_critSec, true);

	//���� üũ�ؼ� ������
	if (!errorCheck) {
		m_S2CProxy.ReplyServerIn(remote, RmiContext::ReliableSend);
	}
	else { //������ ������
		m_S2CProxy.ReplyServerFailed(remote, RmiContext::ReliableSend, L"����ġ ���� ������ �������� ������ ���������ϴ�.");
	}
	return true;
}

///Client to Server
DEFRMI_MultiC2S_RequestLogin(CAmgServer) {

	CriticalSectionLock lock(m_critSec, true);

	//cout << "RequestLogin    " << StringT2A(id.c_str()) << "  " << StringT2A(password.c_str()) << endl;

	///���� ������ �б⿡�� DB�˻� ��, �α��� ���� ���� ���� �Ǵ��ϸ� �� ��.
	///�ϴ��� ����üũ�� �δ� �ɷ�.
	if (!errorCheck) {
		m_S2CProxy.NotifyLoginSuccess(remote, RmiContext::ReliableSend);
	}
	else {
		m_S2CProxy.NotifyLoginFailed(remote, RmiContext::ReliableSend, L"�α��ο� �����߽��ϴ�.");
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

	//isNewRoom�� üũ�Ǹ�, roomName�� �̸����� ���� ���ο� ���� ����ڴٴ� ��.
	if (isNewRoom) {
		if (m_rooms.TryGetValue(roomName, room)) {
			m_S2CProxy.ReplyInRoom(remote, RmiContext::ReliableSend, 1, L"�̹� �����ϴ� ���Դϴ�.", 0);
			return true;
		}

		//���ο� �� �����
		room = CRoomPtr_S(new CRoom_S);
		room->m_name = roomName;
		m_rooms.Add(roomName, room);
		room->m_p2pGroupID = m_netServer->CreateP2PGroup();
	}
	else {
		if (!m_rooms.TryGetValue(roomName, room)) {
			///m_rooms.TryGetValue(roomName,room)���� ������ �� !����.
			///�׷��Ƿ� roomName�� Ű�� ���� ���� m_rooms Dictionary�� ������ room�����Ϳ� �� ���� �ִ´�.
			///�� room ������ �������� else�� �� �ϴ��� ReplyInRoom�� ����� �� �ִ� ��.
			m_S2CProxy.ReplyInRoom(remote, RmiContext::ReliableSend, 1, L"�������� �ʴ� ���Դϴ�.", 0);
			return true;
		}
	}

	///�׸��� �� ���� ������ �ϴ� �ڵ� �ۼ�. ReplyInRoom�� �� ��° ���ڰ� 3�̸� ��á���� Unity���� ��������� �������.
	if (room->m_players.GetCount() == 4) {

		m_S2CProxy.ReplyInRoom(remote, RmiContext::ReliableSend, 1, L"�ο��� �ʰ��Ͽ� ������ �� �����ϴ�.", 0);
		return true;
	}

	mycharacterNum = CreateRandomCharacter(room);

	m_S2CProxy.ReplyInRoom(remote, RmiContext::ReliableSend, 0, L"�濡 �����մϴ�.", mycharacterNum);


	//�÷��̾� ������Ʈ�� ����.
	CRemoteClientPtr_S player(new CRemoteClient_S);

	//�÷��̾ �濡 ������.
	room->m_players.Add(remote, player);

	///horror
	player->m_userID = id;
	player->m_characterNum = mycharacterNum;

	m_netServer->JoinP2PGroup(remote, room->m_p2pGroupID);

	m_remoteClients.Add(remote, room);

	/* �Ŀ� ������ �ڵ�
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
		//���� �������� �ʽ��ϴ�.
		m_S2CProxy.ReplyInRoom(remote, RmiContext::ReliableSend, 1, L"���� ���� ���� �����ϴ�.", 0);
		return true;
	}
	else {
		//���� �����Ѵٸ�, ���� ��ȸ�ϸ� 4�� ������ �濡 ������ �� �ֵ���.
		for (auto roomIter : m_rooms) {
			if (m_rooms.Lookup(roomIter.first)->m_value->m_players.Count < 4) {
				printf("%d\n", m_rooms.Lookup(roomIter.first)->m_value->m_players.Count);
				m_rooms.TryGetValue(m_rooms.Lookup(roomIter.first)->m_key, room);
				mycharacterNum = CreateRandomCharacter(room);
				m_S2CProxy.ReplyInRoom(remote, RmiContext::ReliableSend, 2, L"4�� ������ ���� ã�Ƽ� �����մϴ�.", mycharacterNum);
				//�÷��̾� ������Ʈ�� ����.
				CRemoteClientPtr_S player(new CRemoteClient_S);

				//�÷��̾ �濡 ������.
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
				m_S2CProxy.ReplyInRoom(remote, RmiContext::ReliableSend, 3, L"4�� ������ ���� ã�� �� �����ϴ�.", 0);
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

	cout << "���ӿ� �����ϰ��־�! \n";
	m_S2CProxy.ReplyInGame(remote, RmiContext::ReliableSend, 0, L"���ӿ� �����ϴ� ���Դϴ�.");
	return true;

	/*
	///�׸��� ������ ���� ������ ����ϰ� �ڵ带 ¥��������
	///room->m_plwyer.count���� ��� ���� �߻�. CFastMap.h�� �ڲ� ��. ���� �������� �ذ����� ����.
	///GetCount()�� �Ẹ������ �ذ� ����. size�� �� ����.
	///������ �� �ڵ��� �Ʒ�ó�� �� �������� �ȵ�.
	printf("%d\n",m_rooms.Lookup(roomName)->m_value->m_players.Count);

	if (m_rooms.Lookup(roomName)->m_value->m_players.Count == 4) {
		//�濡 4���� �÷��̾ �����Ƿ� �����մϴ�.
		cout << "���ӿ� �����ϰ��־�! \n";
		m_S2CProxy.ReplyInGame(remote, RmiContext::ReliableSend, 0, L"���ӿ� �����ϴ� ���Դϴ�.");
		return true;
	}
	else {
		//�濡 �� 4���� �÷��̾ �ƴϹǷ� �����Դϴ�.
		m_S2CProxy.ReplyInGame(remote, RmiContext::ReliableSend, 1, L"������ �߰ߵǾ����ϴ�. 4���� �÷��̾ �ƴմϴ�.");
		return true;
	}

	return true;
	*/
}

DEFRMI_MultiC2S_JoinGameScene(CAmgServer) {

	CriticalSectionLock lock(m_critSec, true);

	cout << "JoinGameScene ȣ�� ��! \n";

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

		///�Լ� �ȿ��� ���ڸ� �޾��� �� �� ������ ���� ���������� ���Դ��� 1���� �˻�.
		///����ġ ���� ���� ������ �� ������ ��� Ȯ���� �� �ִ�.
		assert(it->first == remote);

		for (auto otherIter : room->m_players) {
			///���� �÷��̾� ĳ���� ������ ������Ʈ ��.
			///���� Ŭ���̾�Ʈ ����� ��ȸ�ϸ� �� Ŭ���̾�Ʈ ������ �˸�.(�ݴ뵵)
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

	///�Լ� �ȿ��� ���ڸ� �޾��� �� �� ������ ���� ���������� ���Դ��� 1���� �˻�.
	///����ġ ���� ���� ������ �� ������ ��� Ȯ���� �� �ִ�.
	assert(it->first == remote);

	for (auto otherIter : room->m_players) {
	///���� �÷��̾� ĳ���� ������ ������Ʈ ��.
	///���� Ŭ���̾�Ʈ ����� ��ȸ�ϸ� �� Ŭ���̾�Ʈ ������ �˸�.(�ݴ뵵)
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

//Player Appear�� ���� �ʿ��ϹǷ� �������� �����ӿ� ���� ������ �޾Ƴ���.
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



///�����ɱ�
DEFRMI_MultiC2S_RequestAddTree(CAmgServer) {

	CriticalSectionLock lock(m_critSec, true);

	CRoomPtr_S room;
	if (m_remoteClients.TryGetValue(remote, room)) {
		//���� �߰�
		CWorldObjectPtr_S tree(new CWorldObject_S);
		tree->m_id = room->m_nextNewID;
		tree->m_position = position;

		room->m_worldObjects.Add(tree->m_id, tree);
		room->m_nextNewID++;

		//��� �̵鿡�� �˸���.
		m_S2CProxy.NotifyAddTree(room->m_p2pGroupID, RmiContext::ReliableSend, tree->m_id, position);
	}
	return true;
}

///�����ɱ�
DEFRMI_MultiC2S_RequestRemoveTree(CAmgServer) {

	CriticalSectionLock lock(m_critSec, true);

	CRoomPtr_S room;
	if (m_remoteClients.TryGetValue(remote, room)) {

		//���� ����
		CWorldObjectPtr_S tree;
		if (room->m_worldObjects.TryGetValue(treeID, tree)) {

			room->m_worldObjects.RemoveKey(treeID);

			//��� �̵鿡�� �˸���.
			m_S2CProxy.NotifyRemoveTree(room->m_p2pGroupID, RmiContext::ReliableSend, treeID);
		}
	}
	return true;
}
