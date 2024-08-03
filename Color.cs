using SDL2;

namespace Lander
{
    class Color
    {
        public static SDL.SDL_Color Black = Util.MakeSdlColor(0, 0, 0, 255);
        public static SDL.SDL_Color White = Util.MakeSdlColor(255, 255, 255, 255);

        public static SDL.SDL_Color Terrain = Util.MakeSdlColor(200, 200, 200, 255);
        public static SDL.SDL_Color Terrain2 = Util.MakeSdlColor(30, 30, 30, 255);
        public static SDL.SDL_Color Sky = Util.MakeSdlColor(0, 0, 50, 255);

        public static SDL.SDL_Color Lander = Util.MakeSdlColor(200, 200, 200, 255);
        public static SDL.SDL_Color Flame = Util.MakeSdlColor(200, 75, 0, 255);
        public static SDL.SDL_Color Win = Util.MakeSdlColor(0, 200, 0, 255);

        public static SDL.SDL_Color Text = Util.MakeSdlColor(255, 255, 255, 255);

        public static void Set(SDL.SDL_Color c)
        {
            SDL.SDL_SetRenderDrawColor(Render.MainRenderer, c.r, c.g, c.b, c.a);
        }
    }
}