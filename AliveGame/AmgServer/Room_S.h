#pragma once
#include "RemoteClient_S.h"
#include "WorldObject_S.h"


//����Ʈ������ Ŭ����
class CRoom_S
{
public:
	CRoom_S(void);
	~CRoom_S(void);

	//�� �̸�
	String m_name;

	//�� �濡 ������ �÷��̾� ���
	CFastMap<HostID, CRemoteClientPtr_S> m_players;

	//P2P group ID.
	//��Ƽĳ���ð� P2P��ſ��� ���.
	HostID m_p2pGroupID;

	//���� ���� �� ��ſ� �ʿ��� world object�� ����.
	CFastMap<int, CWorldObjectPtr_S> m_worldObjects;

	int m_nextNewID;

	bool m_maxman;
	bool m_lion;
	bool m_betakkuma;
	bool m_wazosky;

	/*
	//�� �濡 ������ �÷��̾� Ű �� ��. HostID�� ������ ������ �÷��̾��� �ĺ���.
	//CFastMap<HostID, shared_ptr<CRemoteClientPtr_S>> m_players;
	//CFastMap<HostID, shared_ptr<CRemoteClient_S>> m_players;
	//CFastMap<HostID, shared_ptr<CRemoteClient_S>> m_otherplayers;
	unordered_map<int, shared_ptr<CRemoteClient_S>> m_otherplayers;
	*/
};

typedef RefCount<CRoom_S> CRoomPtr_S;