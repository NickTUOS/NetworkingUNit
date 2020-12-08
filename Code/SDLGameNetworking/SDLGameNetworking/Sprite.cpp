#include "Sprite.h"



Sprite::Sprite(SDL_Renderer* renderer, string filename, int xPos, int yPos, bool useTransparency): Bitmap (renderer, filename, xPos, yPos, useTransparency)
{
}


Sprite::~Sprite()
{
}
