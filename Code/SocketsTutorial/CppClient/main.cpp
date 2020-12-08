#define _WINSOCK_DEPRECATED_NO_WARNINGS //makse VSshut up
#include <iostream>
#include <sstream>
#include <WinSock2.h>
#pragma comment(lib, "Ws2_32.lib")

//Client

using namespace std;

int main()
{
	WSADATA WSAData; // data about the windows socket implementation

	SOCKET serverSocket; //the socket for the server we will connect to

	SOCKADDR_IN addr; // the adderss of the server we will connect to 

	WSAStartup(MAKEWORD(2, 0), &WSAData); //init the winsock DLL 
	
	serverSocket = socket(AF_INET, SOCK_STREAM, 0); //create the socket for the server

	addr.sin_addr.s_addr = inet_addr("127.0.0.1"); //set the server address, local host in our case as we are the server

	addr.sin_family = AF_INET; //set the address family, so the socket knows its for an internet address
	addr.sin_port = htons(1234); // set the port to listen to

	connect(serverSocket, (SOCKADDR*) &addr, sizeof(addr)); //attempt a connection to the server.

	cout << "Connected to server!" << endl;

	string msg;
	getline(cin, msg); // read a line of text from the console 
	//msg += "<EOF>";
	//memcpy(buffer, msg.c_str(), msg.length()); //copy the messageto the buffer

	send(serverSocket, msg.c_str(), msg.length(), 0); //send the buffer to the connected socket
	cout << "Message sent!" << endl;

	char buffer[1024]; //make a data buffer for our message
	memset(buffer, 0, sizeof(buffer)); // set buffer to 0

	cout << "Servers responce..." << endl;
	recv(serverSocket, buffer, sizeof(buffer), 0); // wait for responce, this methord is blocking
	cout << buffer << endl; // print responce from server

	closesocket(serverSocket); // close socket
	WSACleanup(); // clean up
	cout << "Socket Closed." << endl;
	
	system("pause");// lets you read the message

	return 0; // end!
}