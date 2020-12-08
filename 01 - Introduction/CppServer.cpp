#include <iostream>
#include <WinSock2.h>
#pragma comment(lib, "Ws2_32.lib")

using namespace std;

int main()
{
	WSADATA WASData; // data about the windows socket implementation

	SOCKET serverSocket, clientSocket; //the connections of this server and the client we will connect to

	SOCKADDR_IN serverAddr, clientAddr; //addresses of the server and the colent we will connect to

	WSAStartup(MAKEWORD(2, 0), &WASData); //Initualizes the winsock DLL

	serverSocket = socket(AF_INET, SOCK_STREAM, 0); //creates a new socket
														//AF_INET = Address family internet
														// SOCK_SREAM = streaming data
														//protocal, 0 as we dont care what protocal we use right now.

	serverAddr.sin_addr.s_addr = INADDR_ANY; //try ANY
	serverAddr.sin_family = AF_INET;
	serverAddr.sin_port = htons(1234); // set the port number

	bind(serverSocket,(SOCKADDR*)&serverAddr, sizeof(serverAddr)); // bind the socket and the address togethor.
	listen(serverSocket, 0); //places the socket in a state where it is listening for incomming connections
	while (true) // forever
	{
		cout << "Listening for incoming Connections..." << endl;

		char buffer[1024]; //make a buffer to stor data in
		int clientAddrSize = sizeof(clientAddr); // get the size of the client address object
		if ((clientSocket = accept(serverSocket, (SOCKADDR*)&clientAddr, &clientAddrSize)) != INVALID_SOCKET) //if a new connection exists, accept it and create a new socket
																												//we bind server soceck and client addr to client socket as this combonation makes the socket
		{
			memset(buffer, 0, sizeof(buffer)); // set buffer to 0, to be safe
			cout << "Client connected!" << endl;
			recv(clientSocket, buffer, sizeof(buffer), 0); //read data from the incomming connection
			cout << "Client Says: " << buffer << endl; // print the clents message
			
			string responceText = "responce from server: ";
			responceText += buffer; //add our responce to the  clients message

			memcpy(buffer, responceText.c_str(), responceText.length()); // copy the new resonce over the buffer

			send(clientSocket, buffer, sizeof(buffer), 0); //send the new responce back to the client

			closesocket(clientSocket); // close the connection
			cout << "client disconnected." << endl;
		}
	}
	return 0;
}