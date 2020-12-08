


#include <SDL.h>
#undef main
#include <SDL_image.h>

#include <iostream>
#include <String>
#include <sstream>

#include "Sprite.h"
#include "UDPConnection.h"
#include "Text.h"

#include "KW_gui.h"
#include "KW_button.h"
#include "SDL_image.h"
#include "KW_frame.h"
#include "KW_editbox.h"
#include "KW_renderdriver_sdl2.h"

#include <thread>




//TODO Clean up this bad code! stop being lazy!!!!
const int SCREEN_WIDTH = 640;
const int SCREEN_HEIGHT = 480;
bool canQuit = false;
Sprite* TestPlayer;


KW_RenderDriver * driver;
KW_Surface * set;
KW_GUI * gui;
KW_Font * font;

KW_Rect IpAddressEditTextRect;
KW_Widget * IPAddressEditbox;

KW_Rect ConnectButtonrect;
KW_Widget * Connectbutton;

KW_Rect MakeServerButtonrect;
KW_Widget * makeServerbutton;

UDPConnection* ClientSocket;
UDPConnection* ServerSocket;
std::string IP;
int32_t localPort = 1234;
int32_t remotePort = localPort+1;

void HandleEvents();


std::thread* Server;
void ServerDoWork()
{	
	while (true)
	{

		//cout << "server doing work" << endl;


		DataPacket recvedData;
		if (ServerSocket->Recive(&recvedData))
		{
			int i = (int)*recvedData.MessageData.c_str();
			cout << i<< endl;
		}
			

		//std::this_thread::sleep_for(1s);
		int i = 0;
	}
}


enum GameStates {
	SPLASH,
	CONNECTION_SCREEN,
	GAME
};
GameStates CurrentState = SPLASH;


void OnConnectClicked(KW_Widget * widget, int b) {
	cout << "Attempting connection" << endl;

	IP = KW_GetEditboxText(IPAddressEditbox);

	ClientSocket = new UDPConnection(IP, localPort, remotePort);

	CurrentState = GAME;
}
void OnMakeServerClicked(KW_Widget * widget, int b) {
	cout << "Making Server.." << endl;
	ServerSocket = new UDPConnection(IP, remotePort, localPort);

	Server = new std::thread(ServerDoWork);

	ClientSocket = new UDPConnection(IP, localPort, remotePort);
	CurrentState = GAME;
}

void OnIpAddressFocuseGain(KW_Widget* widget)
{
	/*cout << "clearing text box" << endl;
	string text(KW_GetEditboxText(widget));
	text.resize(0);
	KW_SetEditboxText(widget, text.c_str());
	cout << "text box text after clear: "<< KW_GetEditboxText(widget) << endl;*/
}

void OnTextChanged(KW_Widget * widget, SDL_Keycode key, SDL_Scancode s)
{
	if (key != SDLK_DELETE && key != SDLK_BACKSPACE && key != SDLK_LEFT && key != SDLK_RIGHT)
	{
		string text(KW_GetEditboxText(widget));

		/*if (text.length() >= 10)
		{
			text.resize(0);
			KW_SetEditboxText(widget, text.c_str());
		}*/
		string stop = ".";
		if (text.length() == 3)
		{
			KW_SetEditboxText(widget, stop.c_str());
		}
		else if (text.length() == 7)
		{
			KW_SetEditboxText(widget, stop.c_str());
		}
		text = KW_GetEditboxText(widget);
		KW_SetEditboxCursorPosition(widget, text.length());
	}

	/*
	{
		cout << KW_GetEditboxText(widget) << endl;
		KW_SetFocusedWidget(NULL);
	}*/
}

int dragmode = 0;
void Drag(KW_Widget * widget, int x, int y, int xrel, int yrel) {
	KW_Rect g;
	KW_GetWidgetGeometry(widget, &g);
	if (dragmode == 1) {
		g.w += xrel;
		g.h += yrel;
	}
	else {
		g.x += xrel;
		g.y += yrel;
	}
	KW_SetWidgetGeometry(widget, &g);
}







int main(int argc, char *argv[])
{
	CurrentState = CONNECTION_SCREEN;
	

	SDL_Window* pWindow = NULL;
	SDL_Renderer* pRenderer = NULL;
	//SDL_Surface* pScreenSurface = NULL;

	if (SDL_Init(SDL_INIT_EVERYTHING) < 0)
	{
		cerr << "SDL couldnt not be initialized! ERROR: " << SDL_GetError() << endl;
	}
	else
	{

		pWindow = SDL_CreateWindow("SDL Network Game", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, SCREEN_WIDTH, SCREEN_HEIGHT,  SDL_WINDOW_SHOWN);
		if (pWindow == NULL)
		{
			cerr << "Window could not be created! ERROR: " << SDL_GetError() << endl;
		}
		else
		{


			pRenderer = SDL_CreateRenderer(pWindow, -1, SDL_RENDERER_ACCELERATED);
			if (pRenderer == NULL)
			{
				cerr << "Craeting Renderer Failed! SDL error: " << SDL_GetError() << endl;
			}

			//Initialize PNG loading
			int imgFlags = IMG_INIT_PNG;
			if (!IMG_Init(imgFlags) & imgFlags)
			{
				cerr << "SDL image could not Initilized! SDL_image error: " << IMG_GetError() << endl;
			}
			//Initialize SDL_ttf
			if (TTF_Init() == -1)
			{
				cerr << "SDL_TTF could not be Initilized! SDL_TTF error: " << TTF_GetError() << endl;
			}

			/*pScreenSurface = SDL_GetWindowSurface(pWindow);
			SDL_FillRect(pScreenSurface, NULL, SDL_MapRGB(pScreenSurface->format, 0, 0, 128));*/

			SDL_SetRenderDrawColor(pRenderer, 0, 0, 128, 0);

			TestPlayer = new Sprite(pRenderer, "../Assets/megaMan.png", 50, 50, true);





			

			/*std::cout << "Enter remote IP ( 127.0.0.1  for local connections ) : ";
			std::cin >> IP;
			std::cout << "...and remote port : ";
			std::cin >> remotePort;

			std::cout << "Enter local port : ";
			std::cin >> localPort;*/

			
			
			//UDPConnection udp2(IP, localPort+1, remotePort+1);
			//char command = ' ';
			/*while (true)
			{
				std::cout
					<< "Your command : "
					<< "\n\t1 : Send a message"
					<< "\n\t0 : Quit"
					<< "\n\t2 : Check for data"
					<< std::endl;

				std::cin >> command;

				if (command == '0')
				{
					break;
				}
				else if (command == '1')
				{
					string s = "this Is A Test";
					DataPacket DP{s};
					cout << sizeof(DP) << endl;
					udp.Send(DP);
				}
				else if (command == '2')
				{
					DataPacket recvedData;
					udp.Recive(&recvedData);

					cout << recvedData.MessageData << endl;
				}
				else
				{
					cout << "invalid command" << endl;
				}
			}*/

			driver = KW_CreateSDL2RenderDriver(pRenderer, pWindow);
			set = KW_LoadSurface(driver, "../Assets/tileset.png");
			gui = KW_Init(driver, set);
			font = KW_LoadFont(driver, "../Assets/DejaVuSans.ttf", 12);
			KW_SetFont(gui, font);

			KW_Rect FrameRect;
			FrameRect.w = 280;
			FrameRect.h = 100;
			FrameRect.x = (SCREEN_WIDTH/2)- (FrameRect.w/2);
			FrameRect.y = (SCREEN_HEIGHT/2)- (FrameRect.h /2);
			
			KW_Widget * frame = KW_CreateFrame(gui, NULL, &FrameRect);

			KW_AddWidgetDragHandler(frame, Drag);



			IpAddressEditTextRect.x = 5;
			IpAddressEditTextRect.y = 5;
			IpAddressEditTextRect.w = 128;
			IpAddressEditTextRect.h = 40;


			IPAddressEditbox = KW_CreateEditbox(gui, frame, "", &IpAddressEditTextRect);
			KW_AddWidgetKeyDownHandler(IPAddressEditbox, OnTextChanged);
			KW_AddWidgetFocusGainHandler(IPAddressEditbox, OnIpAddressFocuseGain);


			ConnectButtonrect.x = 128;
			ConnectButtonrect.y = 5;
			ConnectButtonrect.w = 128;
			ConnectButtonrect.h = 40;
			Connectbutton = KW_CreateButtonAndLabel(gui, frame, "Connect", &ConnectButtonrect);
			KW_AddWidgetMouseDownHandler(Connectbutton, OnConnectClicked);




			ConnectButtonrect.x = 128;
			ConnectButtonrect.y = 50;
			ConnectButtonrect.w = 128;
			ConnectButtonrect.h = 40;
			makeServerbutton = KW_CreateButtonAndLabel(gui, frame, "Make Server", &ConnectButtonrect);
			KW_AddWidgetMouseDownHandler(makeServerbutton, OnMakeServerClicked);



			SDL_Rect textrext;
			textrext.w = SCREEN_WIDTH;
			textrext.h = 100;
			textrext.x = 0;
			textrext.y = (SCREEN_HEIGHT /2) - (textrext.h/2);
			
			Text SplashText(pRenderer, "A Splash Screen GAME!", textrext, 48);
			int FrameCount = 0;

			string s;
			DataPacket DP;

			string ipString;
			int id = 0;
			std::stringstream ssipName;
			int returnedAddresses;

			while (!canQuit)
			{
				
				//SDL_UpdateWindowSurface(pWindow);

				SDL_RenderClear(pRenderer);
				//Draw stuff!!
				HandleEvents();

				switch (CurrentState)
				{
				case SPLASH:
					FrameCount++;
					SplashText.Draw();
					if (FrameCount >= 180)
						CurrentState = CONNECTION_SCREEN;
					break;
				case CONNECTION_SCREEN:
					KW_ProcessEvents(gui);
					KW_Paint(gui);
					break;
				case GAME:
					TestPlayer->Draw();
					id++;
					DP.PacketID = id;


					//todo loop to find first none 0 host.
					IPaddress addresses[5];
					returnedAddresses = SDLNet_GetLocalAddresses(addresses, 5);
					Uint8 parts[4];
					memcpy(parts, &addresses[2].host, sizeof(addresses[2].host));
					ssipName.str("");
					ssipName << std::to_string(parts[0])<<"."
						<<std::to_string(parts[1]) <<"."
						<< std::to_string(parts[2]) <<"."
						<< std::to_string(parts[3]);
					ipString = ssipName.str();
					DP.SendersIPAddress = ipString;
					DP.MessageData = TestPlayer->GetX();
					DP.SendersPort = std::to_string(localPort);

					ClientSocket->Send(DP);
					break;
				default:
					break;
				}


				SDL_RenderPresent(pRenderer);
				//SDL_GL_SwapWindow(pWindow);
				SDL_Delay(16);
			}


			
		}
		
		//destroy bitmap objects
		

		KW_Quit(gui);

		/* Releases things */
		KW_ReleaseSurface(driver, set);
		KW_ReleaseFont(driver, font);
		KW_ReleaseRenderDriver(driver);


		SDL_DestroyRenderer(pRenderer);
		SDL_DestroyWindow(pWindow);
		TTF_Quit();
		SDL_Quit();
	}

	return 0;
}


void HandleEvents()
{
	SDL_Event e;
	while (SDL_PollEvent(&e) != 0) // poll all events
	{
		switch (e.type)
		{
		case SDL_QUIT:
			//User requets quit
			canQuit = true;
			break;
		case SDL_MOUSEBUTTONDOWN:
		case SDL_MOUSEBUTTONUP:
		case SDL_MOUSEMOTION:
			int x, y;
			SDL_GetMouseState(&x, &y);
			bool leftButtonClick = (e.button.button == SDL_BUTTON_LEFT);
			break;
		}
	}

	const Uint8* currentKeyStates = SDL_GetKeyboardState(NULL);
	if (currentKeyStates[SDL_SCANCODE_ESCAPE])
	{
		canQuit = true;
	}
	if (currentKeyStates[SDL_SCANCODE_W])
	{
		TestPlayer->AddY(-1);
	}
	if (currentKeyStates[SDL_SCANCODE_S])
	{
		TestPlayer->AddY(1);
	}
	if (currentKeyStates[SDL_SCANCODE_A])
	{
		TestPlayer->AddX(-1);
	}
	if (currentKeyStates[SDL_SCANCODE_D])
	{
		TestPlayer->AddX(1);
	}
}








