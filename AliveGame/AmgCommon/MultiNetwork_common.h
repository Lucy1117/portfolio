#pragma once

namespace MultiC2S
{
	//Message ID that replies to each RMI method. 
               
    static const ::Proud::RmiID Rmi_RequestServerIn = (::Proud::RmiID)(3000+1);
               
    static const ::Proud::RmiID Rmi_RequestLogin = (::Proud::RmiID)(3000+2);
               
    static const ::Proud::RmiID Rmi_RequestRoomList = (::Proud::RmiID)(3000+3);
               
    static const ::Proud::RmiID Rmi_RequestInRoom = (::Proud::RmiID)(3000+4);
               
    static const ::Proud::RmiID Rmi_RequestFastRoom = (::Proud::RmiID)(3000+5);
               
    static const ::Proud::RmiID Rmi_RequestInGame = (::Proud::RmiID)(3000+6);
               
    static const ::Proud::RmiID Rmi_JoinGameScene = (::Proud::RmiID)(3000+7);
               
    static const ::Proud::RmiID Rmi_Player_SMove = (::Proud::RmiID)(3000+8);
               
    static const ::Proud::RmiID Rmi_RequestAddTree = (::Proud::RmiID)(3000+9);
               
    static const ::Proud::RmiID Rmi_RequestRemoveTree = (::Proud::RmiID)(3000+10);

	// List that has RMI ID.
	extern ::Proud::RmiID g_RmiIDList[];
	// RmiID List Count
	extern int g_RmiIDListCount;
}

namespace MultiS2C
{
	//Message ID that replies to each RMI method. 
               
    static const ::Proud::RmiID Rmi_ReplyServerIn = (::Proud::RmiID)(4000+1);
               
    static const ::Proud::RmiID Rmi_ReplyServerFailed = (::Proud::RmiID)(4000+2);
               
    static const ::Proud::RmiID Rmi_NotifyLoginSuccess = (::Proud::RmiID)(4000+3);
               
    static const ::Proud::RmiID Rmi_NotifyLoginFailed = (::Proud::RmiID)(4000+4);
               
    static const ::Proud::RmiID Rmi_ReplyInRoom = (::Proud::RmiID)(4000+5);
               
    static const ::Proud::RmiID Rmi_ReplyInGame = (::Proud::RmiID)(4000+6);
               
    static const ::Proud::RmiID Rmi_NotifyAddTree = (::Proud::RmiID)(4000+7);
               
    static const ::Proud::RmiID Rmi_NotifyRemoveTree = (::Proud::RmiID)(4000+8);
               
    static const ::Proud::RmiID Rmi_Player_Appear = (::Proud::RmiID)(4000+9);
               
    static const ::Proud::RmiID Rmi_Player_Disappear = (::Proud::RmiID)(4000+10);

	// List that has RMI ID.
	extern ::Proud::RmiID g_RmiIDList[];
	// RmiID List Count
	extern int g_RmiIDListCount;
}

namespace MultiC2C
{
	//Message ID that replies to each RMI method. 
               
    static const ::Proud::RmiID Rmi_ScribblePoint = (::Proud::RmiID)(1000+1);
               
    static const ::Proud::RmiID Rmi_OtherCharacterSelect = (::Proud::RmiID)(1000+2);
               
    static const ::Proud::RmiID Rmi_Player_CMove = (::Proud::RmiID)(1000+3);

	// List that has RMI ID.
	extern ::Proud::RmiID g_RmiIDList[];
	// RmiID List Count
	extern int g_RmiIDListCount;
}

 
