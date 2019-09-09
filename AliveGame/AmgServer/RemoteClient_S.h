#pragma once
class CRemoteClient_S
{
public:
	CRemoteClient_S();
	~CRemoteClient_S();

	//������ ������ �� Ŭ���̾�Ʈ�� remote client��� ��Ī
	//�� Ŭ���̾�Ʈ�� ���� ���� �Է�
	//�÷��̾��� ID�� �����ϴ� ĳ������ ����(x,y,z)��ǥ�� �Է�
	std::wstring m_userID;
	float m_x, m_y, m_z;
	float m_vx, m_vy, m_vz;
	float m_angle;
	int m_animNum;
	int m_characterNum;
};

typedef RefCount<CRemoteClient_S> CRemoteClientPtr_S;
