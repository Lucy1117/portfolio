#pragma once
class CWorldObject_S
{
public:
	//object�� ������.
	int m_id;

	D3DXVECTOR3 m_position; //object�� 3���� ��ǥ��.

	CWorldObject_S();
	~CWorldObject_S();
};

typedef RefCount<CWorldObject_S> CWorldObjectPtr_S;

