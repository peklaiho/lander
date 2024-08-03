using System;
using SDL2;

namespace Lander
{
    class Text
    {
        private static IntPtr Font;

        public static void Init()
        {
            SDL_ttf.TTF_Init();
            Font = SDL_ttf.TTF_OpenFont("font/DroidSansMono.ttf", 16);
        }

        public static void Draw(string str, int x, int y)
        {
            DoDraw(MakeTexture(str), x, y);
        }

        private static void DoDraw(IntPtr txt, int x, int y)
        {
            uint format;
            int access, w, h;

            SDL.SDL_QueryTexture(txt, out format, out access, out w, out h);

            var from = Util.MakeSdlRect(0, 0, w, h);
            var to = Util.MakeSdlRect(x, y, w, h);

            SDL.SDL_RenderCopy(Render.MainRenderer, txt, ref from, ref to);
        }

        private static IntPtr MakeTexture(string str)
        {
            var surfacePtr = SDL_ttf.TTF_RenderUTF8_Blended(Font, str, Color.Text);
            var texturePtr = SDL.SDL_CreateTextureFromSurface(Render.MainRenderer, surfacePtr);

            SDL.SDL_FreeSurface(surfacePtr);

            return texturePtr;
        }
    }
}