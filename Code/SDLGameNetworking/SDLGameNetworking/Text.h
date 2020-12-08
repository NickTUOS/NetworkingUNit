#ifndef TEXT_H
#include <SDL.h>
#include <SDL_ttf.h>
#include <string>

class Text
{
public:
	Text(SDL_Renderer* pRenderer, const std::string message, SDL_Rect transfrom, int FontSize = 24);
	~Text();
	void Draw();

	SDL_Rect Transfrom;
	std::string Message;
	SDL_Color TextColor;

private:
	TTF_Font* font;
	SDL_Renderer* m_pRenderer;
	SDL_Surface* surfaceMessage;
	SDL_Texture* MessageTexture;
	
};

#endif // !TEXT_H