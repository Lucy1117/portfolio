#pragma once
#include "..\AmgCommon\MultiNetwork_proxy.h"
#include "..\AmgCommon\MultiNetwork_stub.h"
#include "Room_S.h"


class CAmgServer :public INetServerEvent, public MultiC2S::Stub, public IDbCacheClientDelegate2
{	//서버 메인 클래스
	//INetServerEvent 인터페이스 상속.
	//INetServerEvent는 서버에서 일어나는 여러 이벤트 콜백을 처리.

public:
	CNetServer* m_netServer;
	MultiS2C::Proxy m_S2CProxy;

	//현재 서버 옵션을 따로 설정하지 않아 서버 CPU 개수만큼 스레드가 만들어지므로
	//CriticalSection을 구현해서 내부를 보호해줘야함.
	CriticalSection m_critSec;

	//키 값을 쌍으로 갖는 방 목록.
	//roomName을 키로 갖고 room Class를 값으로 갖는 Dictianary
	CFastMap<String, CRoomPtr_S, StringTraitsW> m_rooms; //string을 키값으로 가지기 때문에 StringTraitsW라는 헬퍼 필요.

														 //어떤 클라이언트가 어떤 방에 있는지를 알려주는 목록
	CFastMap<HostID, CRoomPtr_S> m_remoteClients;

	CDbCacheClient2* m_dbCacheClient;

	CAmgServer();
	~CAmgServer();

	bool errorCheck = false;
	bool m_runLoop = true;

	//Player P2PGroup
	///HostID m_playerGroup = HostID_None;



	//순수 가상 함수 : 함수의 선언만 있고 정의는 없는것. 자식 클래스에서 반드시 재정의해야 함.
	//순수 가상 함수를 포함하는 클래스를 추상 클래스라고 함.
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
	//main의 CAmgServer에서 override error

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