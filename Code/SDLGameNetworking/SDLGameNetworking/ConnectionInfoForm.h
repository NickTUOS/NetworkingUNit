#ifndef CONNECTIONINFOFORM_H
#include <SDL.h>
#include <string>
#include <iostream>

#include "UDPConnection.h"

#include "KW_gui.h"
#include "KW_frame.h"
#include "KW_label.h"
#include "KW_renderdriver_sdl2.h"
#include "KW_editbox.h"
#include "KW_button.h"
//#include "KW_rect.h"

using namespace std;

class ConnectionInfoForm
{
public:
	ConnectionInfoForm(SDL_Window* pWindow,	SDL_Renderer* pRenderer);
	~ConnectionInfoForm();

	void Draw();
private:
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

	UDPConnection* udp;
	std::string IP;
	int32_t localPort = 0;
	int32_t remotePort = 0;


	void OnConnectClicked(KW_Widget * widget, int b);
	void OnMakeServerClicked(KW_Widget * widget, int b);
	void OnIpAddressFocuseGain(KW_Widget* widget);
	void OnTextChanged(KW_Widget * widget, SDL_Keycode key, SDL_Scancode s);
	int dragmode = 0;
	void Drag(KW_Widget * widget, int x, int y, int xrel, int yrel);

};

#endif // !CONNECTIONINFOFORM_H