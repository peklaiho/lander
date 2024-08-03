using System;
using SDL2;

namespace Lander
{
    class Util
    {
        public static SDL.SDL_Color MakeSdlColor(byte r, byte g, byte b, byte a)
        {
            SDL.SDL_Color col;

            col.r = r;
            col.g = g;
            col.b = b;
            col.a = a;

            return col;
        }

        public static SDL.SDL_Rect MakeSdlRect(int x, int y, int w, int h)
        {
            SDL.SDL_Rect rect;

            rect.x = x;
            rect.y = y;
            rect.w = w;
            rect.h = h;

            return rect;
        }

        public static double RadToDeg(double val)
        {
            return val * (180 / Math.PI);
        }

        public static double DegToRad(double val)
        {
            return val * (Math.PI / 180);
        }

        public static uint Time()
        {
            return SDL.SDL_GetTicks();
        }
    }
}