#include "stdafx.h"
#include "Common.h"


static const PNGUID protocolV = {0x3003de6b,0xea63,0x490d,{0xbe,0xef,0x1a,0x44,0x15,0x4e,0xc3,0x1c}};
//������ GUID�� ��� �ϴµ�, pnguid.h�� ���ǵ� ������ PNGUID�̹Ƿ� PNGUID�� ��. ������Ʈ�� ���� ��ȭ�ε�.
Guid g_amgProtocolVersion = Guid::From(protocolV);

String g_amgDbCacheServerAuthenticationKey = L"epdlxjqpdltmsjandjfuqekwlsWkgkdk3920384g";