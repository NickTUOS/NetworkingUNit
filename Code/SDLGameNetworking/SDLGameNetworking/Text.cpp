#include "Text.h"



Text::Text(SDL_Renderer* pRenderer, const std::string message, SDL_Rect trasform, int FontSize)
{
	Transfrom = trasform;
	Message = message;
	m_pRenderer = pRenderer;
	font = TTF_OpenFont("../Assets/DejaVuSans.ttf", FontSize);
	TextColor = {255,255,255,255};
	surfaceMessage = TTF_RenderText_Solid(font, Message.c_str(), TextColor);
	MessageTexture = SDL_CreateTextureFromSurface(pRenderer, surfaceMessage);
}


Text::~Text()
{
	SDL_DestroyTexture(MessageTexture);
	SDL_FreeSurface(surfaceMessage);
	TTF_CloseFont(font);
	m_pRenderer = NULL;
}

void Text::Draw()
{
	SDL_RenderCopy(m_pRenderer, MessageTexture, NULL ,&Transfrom);
}