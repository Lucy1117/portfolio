#include "stdafx.h"
#include "Common.h"


static const PNGUID protocolV = {0x3003de6b,0xea63,0x490d,{0xbe,0xef,0x1a,0x44,0x15,0x4e,0xc3,0x1c}};
//원래는 GUID를 써야 하는데, pnguid.h에 정의된 형식이 PNGUID이므로 PNGUID를 씀. 업데이트로 인한 변화인듯.
Guid g_amgProtocolVersion = Guid::From(protocolV);

String g_amgDbCacheServerAuthenticationKey = L"epdlxjqpdltmsjandjfuqekwlsWkgkdk3920384g";