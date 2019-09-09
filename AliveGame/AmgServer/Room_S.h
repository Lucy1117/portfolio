#pragma once
#include "RemoteClient_S.h"
#include "WorldObject_S.h"


//스마트포인터 클래스
class CRoom_S
{
public:
	CRoom_S(void);
	~CRoom_S(void);

	//방 이름
	String m_name;

	//이 방에 입장한 플레이어 목록
	CFastMap<HostID, CRemoteClientPtr_S> m_players;

	//P2P group ID.
	//멀티캐스팅과 P2P통신에서 사용.
	HostID m_p2pGroupID;

	//게임 진행 시 통신에 필요한 world object를 정의.
	CFastMap<int, CWorldObjectPtr_S> m_worldObjects;

	int m_nextNewID;

	bool m_maxman;
	bool m_lion;
	bool m_betakkuma;
	bool m_wazosky;

	/*
	//그 방에 입장한 플레이어 키 값 쌍. HostID가 서버에 접속한 플레이어의 식별자.
	//CFastMap<HostID, shared_ptr<CRemoteClientPtr_S>> m_players;
	//CFastMap<HostID, shared_ptr<CRemoteClient_S>> m_players;
	//CFastMap<HostID, shared_ptr<CRemoteClient_S>> m_otherplayers;
	unordered_map<int, shared_ptr<CRemoteClient_S>> m_otherplayers;
	*/
};

typedef RefCount<CRoom_S> CRoomPtr_S;