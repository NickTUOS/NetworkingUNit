#ifndef BITMAP_H
#define BITMAP_H

#include <SDL.h>
#include <SDL_image.h>
#include <string>
#include <iostream>
using namespace std;

class Bitmap
{
public:
	Bitmap(SDL_Renderer* renderer, string filename, int xPos, int yPos, bool useTransparency = false);
	virtual ~Bitmap();

	void Draw();
	void Draw(SDL_Rect destRect);

	 int GetX() 
	{
		return m_x;
	}
	int GetY()
	{
		return m_y;
	}
	void SetX(int x)
	{
		m_x = x;
	}
	void SetY(int y)
	{
		m_y = y;
	}
	void AddX(int x)
	{
		m_x += x;
	}
	void AddY(int y)
	{
		m_y += y;
	}


private:
	SDL_Surface* m_pBitmapSurface; //software rendering
	SDL_Texture* m_pBitmapTexture; // GPU version of surface, faster!
	SDL_Renderer* m_pRenderer;

	int m_x;
	int m_y;
};

#endif // BITMAP_H