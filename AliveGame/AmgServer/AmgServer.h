#pragma once
#include "..\AmgCommon\MultiNetwork_proxy.h"
#include "..\AmgCommon\MultiNetwork_stub.h"
#include "Room_S.h"


class CAmgServer :public INetServerEvent, public MultiC2S::Stub, public IDbCacheClientDelegate2
{	//���� ���� Ŭ����
	//INetServerEvent �������̽� ���.
	//INetServerEvent�� �������� �Ͼ�� ���� �̺�Ʈ �ݹ��� ó��.

public:
	CNetServer* m_netServer;
	MultiS2C::Proxy m_S2CProxy;

	//���� ���� �ɼ��� ���� �������� �ʾ� ���� CPU ������ŭ �����尡 ��������Ƿ�
	//CriticalSection�� �����ؼ� ���θ� ��ȣ�������.
	CriticalSection m_critSec;

	//Ű ���� ������ ���� �� ���.
	//roomName�� Ű�� ���� room Class�� ������ ���� Dictianary
	CFastMap<String, CRoomPtr_S, StringTraitsW> m_rooms; //string�� Ű������ ������ ������ StringTraitsW��� ���� �ʿ�.

														 //� Ŭ���̾�Ʈ�� � �濡 �ִ����� �˷��ִ� ���
	CFastMap<HostID, CRoomPtr_S> m_remoteClients;

	CDbCacheClient2* m_dbCacheClient;

	CAmgServer();
	~CAmgServer();

	bool errorCheck = false;
	bool m_runLoop = true;

	//Player P2PGroup
	///HostID m_playerGroup = HostID_None;



	//���� ���� �Լ� : �Լ��� ���� �ְ� ���Ǵ� ���°�. �ڽ� Ŭ�������� �ݵ�� �������ؾ� ��.
	//���� ���� �Լ��� �����ϴ� Ŭ������ �߻� Ŭ������� ��.
	virtual void OnClientJoin(CNetClientInfo *clientInfo);

	virtual void OnClientLeave(CNetClientInfo *clientInfo, ErrorInfo *errorinfo, const ByteArray& comment);

	virtual void OnP2PGroupJoinMemberAckComplete(HostID groupHostID, HostID memberHostID, ErrorType result);

	virtual void OnUserWorkerThreadBegin();

	virtual void OnUserWorkerThreadEnd();

	virtual void OnError(ErrorInfo *errorInfo);

	virtual void OnWarning(ErrorInfo *errorInfo);

	virtual void OnInformation(ErrorInfo *errorInfo);

	virtual void OnException(const Exception &e);

	virtual void OnNoRmiProcessed(RmiID rmiID);

	///////////////////////////

	virtual void OnJoinDbCacheServerComplete(ErrorInfo* info);

	virtual void OnLeaveDbCacheServer(ErrorType reason);

	virtual void OnDbmsWriteDone(DbmsWritePropNodePendType type, Guid loadedDataGuid);

	virtual void OnExclusiveLoadDataFailed(CCallbackArgs& args);

	virtual bool OnExclusiveLoadDataSuccess(CCallbackArgs& args);

	virtual void OnDataUnloadRequested(CCallbackArgs& args);

	virtual void OnAddDataFailed(CCallbackArgs& args);

	virtual void OnAddDataSuccess(CCallbackArgs& args);

	virtual void OnUpdateDataFailed(CCallbackArgs& args);

	virtual void OnUpdateDataSuccess(CCallbackArgs& args);

	virtual void OnRemoveDataFailed(CCallbackArgs& args);

	virtual void OnRemoveDataSuccess(CCallbackArgs& args);

	virtual void OnAccessError(CCallbackArgs& args);

	//-------------
	//main�� CAmgServer���� override error

	virtual void OnExclusiveLoadDataComplete(CCallbackArgs& args);

	virtual void OnDataForceUnloaded(CCallbackArgs& args);

	virtual void OnIsolateDataSuccess(CCallbackArgs& args);

	virtual void OnIsolateDataFailed(CCallbackArgs& args);

	virtual void OnDeisolateDataSuccess(CCallbackArgs& args);

	virtual void OnDeisolateDataFailed(CCallbackArgs& args);

	//------------


	void Start();

	int CreateRandomCharacter(CRoomPtr_S characterroom);
	void RemoveCharacter(CRoomPtr_S characterroom, int removeNum);

	DECRMI_MultiC2S_RequestServerIn;
	DECRMI_MultiC2S_RequestLogin;
	DECRMI_MultiC2S_RequestRoomList;
	DECRMI_MultiC2S_RequestInRoom;
	DECRMI_MultiC2S_RequestFastRoom;
	DECRMI_MultiC2S_RequestInGame;
	DECRMI_MultiC2S_JoinGameScene;
	DECRMI_MultiC2S_RequestAddTree;
	DECRMI_MultiC2S_RequestRemoveTree;
	DECRMI_MultiC2S_Player_SMove;

};