#pragma once
#include "Bitmap.h"
class Sprite :
	public Bitmap
{
public:
	Sprite(SDL_Renderer* renderer, string filename, int xPos, int yPos, bool useTransparency);
	~Sprite();
};

