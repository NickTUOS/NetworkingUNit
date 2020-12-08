#include "ConnectionInfoForm.h"



ConnectionInfoForm::ConnectionInfoForm(SDL_Window* pWindow, SDL_Renderer* pRenderer)
{
	driver = KW_CreateSDL2RenderDriver(pRenderer, pWindow);
	set = KW_LoadSurface(driver, "../Assets/tileset.png");
	gui= KW_Init(driver, set);
	font = KW_LoadFont(driver, "../Assets/DejaVuSans.ttf", 12);
	KW_SetFont(gui, font);

	KW_Rect FrameRect;
	FrameRect.x = 0;
	FrameRect.y = 0;
	FrameRect.w = 280;
	FrameRect.h = 100;
	KW_Widget * frame = KW_CreateFrame(gui, NULL, &FrameRect);

	KW_AddWidgetDragHandler(frame, &this->Drag);


	
	IpAddressEditTextRect.x = 5;
	IpAddressEditTextRect.y = 5;
	IpAddressEditTextRect.w = 128;
	IpAddressEditTextRect.h = 40;


	IPAddressEditbox = KW_CreateEditbox(gui, frame, "", &IpAddressEditTextRect);
	KW_AddWidgetKeyDownHandler(IPAddressEditbox, &this->OnTextChanged);
	KW_AddWidgetFocusGainHandler(IPAddressEditbox, &this->OnIpAddressFocuseGain);

	
	ConnectButtonrect.x = 128;
	ConnectButtonrect.y = 5;
	ConnectButtonrect.w = 128;
	ConnectButtonrect.h = 40;
	Connectbutton = KW_CreateButtonAndLabel(gui, frame, "Connect", &ConnectButtonrect);
	KW_AddWidgetMouseDownHandler(Connectbutton, &this->OnConnectClicked);



	
	ConnectButtonrect.x = 128;
	ConnectButtonrect.y = 50;
	ConnectButtonrect.w = 128;
	ConnectButtonrect.h = 40;
	makeServerbutton = KW_CreateButtonAndLabel(gui, frame, "Make Server", &ConnectButtonrect);
	KW_AddWidgetMouseDownHandler(makeServerbutton, &this->OnMakeServerClicked);
}


ConnectionInfoForm::~ConnectionInfoForm()
{
	KW_Quit(gui);

	/* Releases things */
	KW_ReleaseSurface(driver, set);
	KW_ReleaseFont(driver, font);
	KW_ReleaseRenderDriver(driver);
}

void ConnectionInfoForm::Draw()
{
	KW_ProcessEvents(gui);
	KW_Paint(gui);
}


void ConnectionInfoForm::OnConnectClicked(KW_Widget * widget, int b) {
	cout << "Attempting connection" << endl;
	udp = new UDPConnection(IP, localPort, remotePort);
}
void ConnectionInfoForm::OnMakeServerClicked(KW_Widget * widget, int b) {
	cout << "Making Server.." << endl;
	udp = new UDPConnection(IP, localPort, remotePort);
}

void ConnectionInfoForm::OnIpAddressFocuseGain(KW_Widget* widget)
{
	/*cout << "clearing text box" << endl;
	string text(KW_GetEditboxText(widget));
	text.resize(0);
	KW_SetEditboxText(widget, text.c_str());
	cout << "text box text after clear: "<< KW_GetEditboxText(widget) << endl;*/
}

void ConnectionInfoForm::OnTextChanged(KW_Widget * widget, SDL_Keycode key, SDL_Scancode s)
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

void ConnectionInfoForm::Drag(KW_Widget * widget, int x, int y, int xrel, int yrel) {
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