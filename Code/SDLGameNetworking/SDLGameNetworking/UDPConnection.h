#ifndef UDPCONNECTION_H
#define UDPCONNECTION_H

#include <SDL_net.h>
#include <iostream>
#include <string>

using namespace std;


struct DataPacket
{
	string SendersIPAddress;
	string SendersPort;
	int PacketID;
	string MessageData;

	DataPacket() {};

	DataPacket(string sendersIPAddress, string sendersPort, int packetID, string messageData)
	{
		SendersIPAddress = sendersIPAddress;
		SendersPort = SendersPort;
		PacketID = packetID;
		MessageData = MessageData;
	}
	size_t GetHashCode()
	{
		size_t hashIp= std::hash<string>()(SendersIPAddress);
		size_t hashPort= std::hash<string>()(SendersPort);
		size_t hashpacketID= std::hash<int>()(PacketID);
		size_t hashMessage= std::hash<string>()(MessageData);

		size_t result = hashIp ^ (hashPort << 1);
		result = result ^ (hashpacketID << 1);
		result = result ^ (hashMessage << 1);

		return result;
	}

	bool operator==(DataPacket& rhs)
	{
		return (this->GetHashCode() == rhs.GetHashCode());
	}
	bool operator!=(DataPacket& rhs)
	{
		size_t one = this->GetHashCode();
		size_t two = rhs.GetHashCode();
		return (this->GetHashCode() != rhs.GetHashCode());
	}
	
};

class UDPConnection
{
public:
	UDPConnection(string remoteIP ,int  localPort, int remotePort);
	~UDPConnection();

	void Send(DataPacket Data);

	int Recive(DataPacket *Data);


private:
	UDPsocket m_Socket;
	IPaddress m_serverIP;
	UDPpacket* m_pPacket;

	DataPacket PreviouseDataPcket;
};

#endif // !UDPCONNECTION_H