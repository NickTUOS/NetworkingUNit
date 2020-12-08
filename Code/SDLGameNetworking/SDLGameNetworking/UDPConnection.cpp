#include "UDPConnection.h"



UDPConnection::UDPConnection(string remoteIP, int  localPort, int remotePort)
{
	m_Socket = NULL;

	if (SDLNet_Init() == -1)
	{
		cerr << "SDL_Net error: Failed to Init SDL_Net: ERROR: " << SDLNet_GetError() << endl;
	}

	m_Socket = SDLNet_UDP_Open(localPort);
	if (m_Socket == NULL)
	{
		cerr << "SDL_Net error: Failed to init socket: ERROR: " << SDLNet_GetError() << endl;
	}

	if (SDLNet_ResolveHost(&m_serverIP, remoteIP.c_str(), remotePort) == -1)
	{
		cerr<<"SDL_Net error: Failed to resolve host: ERROR: "<<SDLNet_GetError()<<endl;
	}

	m_pPacket = SDLNet_AllocPacket(512);
	if (m_pPacket == nullptr)
	{
		cerr << "SDL_Net error: Failed to alloc packet: ERROR: " << SDLNet_GetError() << endl;
	}
	else
	{
		m_pPacket->address.host = m_serverIP.host;
		m_pPacket->address.port = m_serverIP.port;
	}
}
UDPConnection::~UDPConnection()
{
	SDLNet_FreePacket(m_pPacket);
	SDLNet_Quit();
}

void UDPConnection::Send(DataPacket Data)
{
	//ISSUE HERE
	//how dose the reciver know what data type this is?
	//probably best to wrap this in a struck contaning data type and data.

	if (m_pPacket->maxlen < sizeof(Data))
	{
		cerr << "SDLNet Error: Data is too big to send! MaxLen: " << m_pPacket->maxlen << " size of Data: " << sizeof(Data) << endl;
	}

	memcpy(m_pPacket->data, &Data, sizeof(Data));
	m_pPacket->len = sizeof(Data);

	if (SDLNet_UDP_Send(m_Socket, -1, m_pPacket) == 0)
	{
		cerr << "SDLNet Error: SDLNet_UDP_Send Failed!: " << SDLNet_GetError() << endl;
	}
}


int UDPConnection::Recive(DataPacket *Data)
{
	int result = SDLNet_UDP_Recv(m_Socket, m_pPacket);
	if (result) // blocking?
	{

		DataPacket temp = *(DataPacket*)m_pPacket->data;
		size_t tempsize = temp.GetHashCode();

		if (PreviouseDataPcket != temp)
		{
			PreviouseDataPcket = temp;
			*Data = *(DataPacket*)m_pPacket->data;
		}

		Data = NULL;
	}
	return result;
}
