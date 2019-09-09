#include "stdafx.h"
#include "AmgServer.h"
#include <conio.h>



int _tmain(int argc, _TCHAR* argv[]) {
	///int _tmain은 유니코드(Unicode, UTF-16) 기능이 켜져 있는지 여부에 따라 main과 wmain중 선택해서 쓴다.
	///유니코드가 켜져있다면 컴파일시 wmain으로, 아니면 main으로 쓴다.
	///main은 char형 인수를 가지며, wmain은 wchar_t형 인수를 가진다.
	puts("ProudNet example: a quite simple social game server.");
	puts("ESC: Quit");

	CAmgServer* server = new CAmgServer();

	server->Start();

	while (server->m_runLoop) {
		if (_kbhit()) {
			int ch = _getch();
			if (ch == 27) { //ESC
				break;
			}
		}

		Sleep(1000);
	}

	delete server;

	return 0;
}