#include "Bitmap.h"



Bitmap::Bitmap(SDL_Renderer* renderer, string filename, int xPos, int yPos, bool useTransparency)
{
	m_pRenderer = renderer;
	m_pBitmapSurface = IMG_Load(filename.c_str());
	//m_pBitmapSurface = SDL_LoadBMP(filename.c_str());
	if (!m_pBitmapSurface)
	{
		cerr << "SURFACE for bitmap " << filename << " not loaded" << endl << IMG_GetError() << endl;
		//cerr << "SURFACE for bitmap " << filename << " not loaded" << endl << SDL_GetError() << endl;
	}
	else
	{
		if (useTransparency)
		{
			Uint32 colourKey = SDL_MapRGB(m_pBitmapSurface->format, 255, 0, 255);
			SDL_SetColorKey(m_pBitmapSurface, SDL_TRUE, colourKey);
		}

		m_pBitmapTexture = SDL_CreateTextureFromSurface(m_pRenderer, m_pBitmapSurface);
		if (!m_pBitmapTexture)
		{
			cerr << "Texture for bitmap " << filename << " not loaded" << endl << SDL_GetError() << endl;
		}

		m_x = xPos;
		m_y = yPos;
	}
}


Bitmap::~Bitmap()
{
	if (m_pBitmapTexture)
		SDL_DestroyTexture(m_pBitmapTexture);

	if (m_pBitmapSurface)
		SDL_FreeSurface(m_pBitmapSurface);
}

void Bitmap::Draw()
{
	Draw({ m_x, m_y, m_pBitmapSurface->w, m_pBitmapSurface->h });
}

void Bitmap::Draw(SDL_Rect destRect)
{
	if (m_pBitmapTexture)
	{
		SDL_RenderCopy(m_pRenderer, m_pBitmapTexture, NULL, &destRect);
	}
}
