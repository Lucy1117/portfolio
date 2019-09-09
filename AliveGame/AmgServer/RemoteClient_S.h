#pragma once
class CRemoteClient_S
{
public:
	CRemoteClient_S();
	~CRemoteClient_S();

	//서버에 접속한 각 클라이언트를 remote client라고 지칭
	//각 클라이언트에 대한 정보 입력
	//플레이어의 ID와 조종하는 캐릭터의 정보(x,y,z)좌표를 입력
	std::wstring m_userID;
	float m_x, m_y, m_z;
	float m_vx, m_vy, m_vz;
	float m_angle;
	int m_animNum;
	int m_characterNum;
};

typedef RefCount<CRemoteClient_S> CRemoteClientPtr_S;
