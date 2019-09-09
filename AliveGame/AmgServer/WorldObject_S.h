#pragma once
class CWorldObject_S
{
public:
	//object의 고유값.
	int m_id;

	D3DXVECTOR3 m_position; //object의 3차원 좌표값.

	CWorldObject_S();
	~CWorldObject_S();
};

typedef RefCount<CWorldObject_S> CWorldObjectPtr_S;

