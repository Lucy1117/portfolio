#include "stdafx.h"
#include "AmgServer.h"
#include <conio.h>



int _tmain(int argc, _TCHAR* argv[]) {
	///int _tmain�� �����ڵ�(Unicode, UTF-16) ����� ���� �ִ��� ���ο� ���� main�� wmain�� �����ؼ� ����.
	///�����ڵ尡 �����ִٸ� �����Ͻ� wmain����, �ƴϸ� main���� ����.
	///main�� char�� �μ��� ������, wmain�� wchar_t�� �μ��� ������.
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