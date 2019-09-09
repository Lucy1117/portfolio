﻿



  
// Generated by PIDL compiler.
// Do not modify this file, but modify the source .pidl file.

#pragma once


#include "MultiNetwork_common.h"

namespace MultiC2S
{
	class Proxy : public ::Proud::IRmiProxy
	{
	public:
	virtual bool RequestServerIn ( ::Proud::HostID remote, ::Proud::RmiContext& rmiContext ) PN_SEALED; 
	virtual bool RequestServerIn ( ::Proud::HostID *remotes, int remoteCount, ::Proud::RmiContext &rmiContext)   PN_SEALED;  
	virtual bool RequestLogin ( ::Proud::HostID remote, ::Proud::RmiContext& rmiContext ,  const std::wstring & id,  const std::wstring & password) PN_SEALED; 
	virtual bool RequestLogin ( ::Proud::HostID *remotes, int remoteCount, ::Proud::RmiContext &rmiContext,  const std::wstring & id,  const std::wstring & password)   PN_SEALED;  
	virtual bool RequestRoomList ( ::Proud::HostID remote, ::Proud::RmiContext& rmiContext ) PN_SEALED; 
	virtual bool RequestRoomList ( ::Proud::HostID *remotes, int remoteCount, ::Proud::RmiContext &rmiContext)   PN_SEALED;  
	virtual bool RequestInRoom ( ::Proud::HostID remote, ::Proud::RmiContext& rmiContext ,  const String & roomName,  const bool & isNewRoom,  const std::wstring & id) PN_SEALED; 
	virtual bool RequestInRoom ( ::Proud::HostID *remotes, int remoteCount, ::Proud::RmiContext &rmiContext,  const String & roomName,  const bool & isNewRoom,  const std::wstring & id)   PN_SEALED;  
	virtual bool RequestFastRoom ( ::Proud::HostID remote, ::Proud::RmiContext& rmiContext ,  const std::wstring & id) PN_SEALED; 
	virtual bool RequestFastRoom ( ::Proud::HostID *remotes, int remoteCount, ::Proud::RmiContext &rmiContext,  const std::wstring & id)   PN_SEALED;  
	virtual bool RequestInGame ( ::Proud::HostID remote, ::Proud::RmiContext& rmiContext ,  const String & roomName) PN_SEALED; 
	virtual bool RequestInGame ( ::Proud::HostID *remotes, int remoteCount, ::Proud::RmiContext &rmiContext,  const String & roomName)   PN_SEALED;  
	virtual bool JoinGameScene ( ::Proud::HostID remote, ::Proud::RmiContext& rmiContext ,  const float & x,  const float & y,  const float & z,  const float & angle,  const int & animNum) PN_SEALED; 
	virtual bool JoinGameScene ( ::Proud::HostID *remotes, int remoteCount, ::Proud::RmiContext &rmiContext,  const float & x,  const float & y,  const float & z,  const float & angle,  const int & animNum)   PN_SEALED;  
	virtual bool Player_SMove ( ::Proud::HostID remote, ::Proud::RmiContext& rmiContext ,  const float & x,  const float & y,  const float & z,  const float & vx,  const float & vy,  const float & vz,  const float & angle,  const int & animNum) PN_SEALED; 
	virtual bool Player_SMove ( ::Proud::HostID *remotes, int remoteCount, ::Proud::RmiContext &rmiContext,  const float & x,  const float & y,  const float & z,  const float & vx,  const float & vy,  const float & vz,  const float & angle,  const int & animNum)   PN_SEALED;  
	virtual bool RequestAddTree ( ::Proud::HostID remote, ::Proud::RmiContext& rmiContext ,  const D3DXVECTOR3 & position) PN_SEALED; 
	virtual bool RequestAddTree ( ::Proud::HostID *remotes, int remoteCount, ::Proud::RmiContext &rmiContext,  const D3DXVECTOR3 & position)   PN_SEALED;  
	virtual bool RequestRemoveTree ( ::Proud::HostID remote, ::Proud::RmiContext& rmiContext ,  const int & treeID) PN_SEALED; 
	virtual bool RequestRemoveTree ( ::Proud::HostID *remotes, int remoteCount, ::Proud::RmiContext &rmiContext,  const int & treeID)   PN_SEALED;  
static const PNTCHAR* RmiName_RequestServerIn;
static const PNTCHAR* RmiName_RequestLogin;
static const PNTCHAR* RmiName_RequestRoomList;
static const PNTCHAR* RmiName_RequestInRoom;
static const PNTCHAR* RmiName_RequestFastRoom;
static const PNTCHAR* RmiName_RequestInGame;
static const PNTCHAR* RmiName_JoinGameScene;
static const PNTCHAR* RmiName_Player_SMove;
static const PNTCHAR* RmiName_RequestAddTree;
static const PNTCHAR* RmiName_RequestRemoveTree;
static const PNTCHAR* RmiName_First;
		Proxy()
		{
			if(m_signature != 1)
				::Proud::ShowUserMisuseError(::Proud::ProxyBadSignatureErrorText);
		}

		virtual ::Proud::RmiID* GetRmiIDList() PN_OVERRIDE { return g_RmiIDList; } 
		virtual int GetRmiIDListCount() PN_OVERRIDE { return g_RmiIDListCount; }
	};
}
namespace MultiS2C
{
	class Proxy : public ::Proud::IRmiProxy
	{
	public:
	virtual bool ReplyServerIn ( ::Proud::HostID remote, ::Proud::RmiContext& rmiContext ) PN_SEALED; 
	virtual bool ReplyServerIn ( ::Proud::HostID *remotes, int remoteCount, ::Proud::RmiContext &rmiContext)   PN_SEALED;  
	virtual bool ReplyServerFailed ( ::Proud::HostID remote, ::Proud::RmiContext& rmiContext ,  const std::wstring & reason) PN_SEALED; 
	virtual bool ReplyServerFailed ( ::Proud::HostID *remotes, int remoteCount, ::Proud::RmiContext &rmiContext,  const std::wstring & reason)   PN_SEALED;  
	virtual bool NotifyLoginSuccess ( ::Proud::HostID remote, ::Proud::RmiContext& rmiContext ) PN_SEALED; 
	virtual bool NotifyLoginSuccess ( ::Proud::HostID *remotes, int remoteCount, ::Proud::RmiContext &rmiContext)   PN_SEALED;  
	virtual bool NotifyLoginFailed ( ::Proud::HostID remote, ::Proud::RmiContext& rmiContext ,  const std::wstring & reason) PN_SEALED; 
	virtual bool NotifyLoginFailed ( ::Proud::HostID *remotes, int remoteCount, ::Proud::RmiContext &rmiContext,  const std::wstring & reason)   PN_SEALED;  
	virtual bool ReplyInRoom ( ::Proud::HostID remote, ::Proud::RmiContext& rmiContext ,  const int & result,  const String & comment,  const int & choiceCharacter) PN_SEALED; 
	virtual bool ReplyInRoom ( ::Proud::HostID *remotes, int remoteCount, ::Proud::RmiContext &rmiContext,  const int & result,  const String & comment,  const int & choiceCharacter)   PN_SEALED;  
	virtual bool ReplyInGame ( ::Proud::HostID remote, ::Proud::RmiContext& rmiContext ,  const int & result,  const String & comment) PN_SEALED; 
	virtual bool ReplyInGame ( ::Proud::HostID *remotes, int remoteCount, ::Proud::RmiContext &rmiContext,  const int & result,  const String & comment)   PN_SEALED;  
	virtual bool NotifyAddTree ( ::Proud::HostID remote, ::Proud::RmiContext& rmiContext ,  const int & treeID,  const D3DXVECTOR3 & position) PN_SEALED; 
	virtual bool NotifyAddTree ( ::Proud::HostID *remotes, int remoteCount, ::Proud::RmiContext &rmiContext,  const int & treeID,  const D3DXVECTOR3 & position)   PN_SEALED;  
	virtual bool NotifyRemoveTree ( ::Proud::HostID remote, ::Proud::RmiContext& rmiContext ,  const int & treeID) PN_SEALED; 
	virtual bool NotifyRemoveTree ( ::Proud::HostID *remotes, int remoteCount, ::Proud::RmiContext &rmiContext,  const int & treeID)   PN_SEALED;  
	virtual bool Player_Appear ( ::Proud::HostID remote, ::Proud::RmiContext& rmiContext ,  const int & hostID,  const std::wstring & userID,  const float & x,  const float & y,  const float & z,  const float & vx,  const float & vy,  const float & vz,  const float & angle,  const int & animNum,  const int & choiceCharacter) PN_SEALED; 
	virtual bool Player_Appear ( ::Proud::HostID *remotes, int remoteCount, ::Proud::RmiContext &rmiContext,  const int & hostID,  const std::wstring & userID,  const float & x,  const float & y,  const float & z,  const float & vx,  const float & vy,  const float & vz,  const float & angle,  const int & animNum,  const int & choiceCharacter)   PN_SEALED;  
	virtual bool Player_Disappear ( ::Proud::HostID remote, ::Proud::RmiContext& rmiContext ,  const int & hostID,  const std::wstring & userID) PN_SEALED; 
	virtual bool Player_Disappear ( ::Proud::HostID *remotes, int remoteCount, ::Proud::RmiContext &rmiContext,  const int & hostID,  const std::wstring & userID)   PN_SEALED;  
static const PNTCHAR* RmiName_ReplyServerIn;
static const PNTCHAR* RmiName_ReplyServerFailed;
static const PNTCHAR* RmiName_NotifyLoginSuccess;
static const PNTCHAR* RmiName_NotifyLoginFailed;
static const PNTCHAR* RmiName_ReplyInRoom;
static const PNTCHAR* RmiName_ReplyInGame;
static const PNTCHAR* RmiName_NotifyAddTree;
static const PNTCHAR* RmiName_NotifyRemoveTree;
static const PNTCHAR* RmiName_Player_Appear;
static const PNTCHAR* RmiName_Player_Disappear;
static const PNTCHAR* RmiName_First;
		Proxy()
		{
			if(m_signature != 1)
				::Proud::ShowUserMisuseError(::Proud::ProxyBadSignatureErrorText);
		}

		virtual ::Proud::RmiID* GetRmiIDList() PN_OVERRIDE { return g_RmiIDList; } 
		virtual int GetRmiIDListCount() PN_OVERRIDE { return g_RmiIDListCount; }
	};
}
namespace MultiC2C
{
	class Proxy : public ::Proud::IRmiProxy
	{
	public:
	virtual bool ScribblePoint ( ::Proud::HostID remote, ::Proud::RmiContext& rmiContext ,  const D3DXVECTOR3 & point) PN_SEALED; 
	virtual bool ScribblePoint ( ::Proud::HostID *remotes, int remoteCount, ::Proud::RmiContext &rmiContext,  const D3DXVECTOR3 & point)   PN_SEALED;  
	virtual bool OtherCharacterSelect ( ::Proud::HostID remote, ::Proud::RmiContext& rmiContext ,  const std::wstring & userID,  const int & otherChoiceCharacter) PN_SEALED; 
	virtual bool OtherCharacterSelect ( ::Proud::HostID *remotes, int remoteCount, ::Proud::RmiContext &rmiContext,  const std::wstring & userID,  const int & otherChoiceCharacter)   PN_SEALED;  
	virtual bool Player_CMove ( ::Proud::HostID remote, ::Proud::RmiContext& rmiContext ,  const float & x,  const float & y,  const float & z,  const float & vx,  const float & vy,  const float & vz,  const float & angle,  const int & animNum) PN_SEALED; 
	virtual bool Player_CMove ( ::Proud::HostID *remotes, int remoteCount, ::Proud::RmiContext &rmiContext,  const float & x,  const float & y,  const float & z,  const float & vx,  const float & vy,  const float & vz,  const float & angle,  const int & animNum)   PN_SEALED;  
static const PNTCHAR* RmiName_ScribblePoint;
static const PNTCHAR* RmiName_OtherCharacterSelect;
static const PNTCHAR* RmiName_Player_CMove;
static const PNTCHAR* RmiName_First;
		Proxy()
		{
			if(m_signature != 1)
				::Proud::ShowUserMisuseError(::Proud::ProxyBadSignatureErrorText);
		}

		virtual ::Proud::RmiID* GetRmiIDList() PN_OVERRIDE { return g_RmiIDList; } 
		virtual int GetRmiIDListCount() PN_OVERRIDE { return g_RmiIDListCount; }
	};
}
