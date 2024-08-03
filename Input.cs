using SDL2;

namespace Lander
{
    class Input
    {
        public static bool BottomThruster = false;
        public static bool LeftThruster = false;
        public static bool RightThruster = false;

        public static void Handle()
        {
            SDL.SDL_Event evt;

            while (SDL.SDL_PollEvent(out evt) != 0) {
                switch (evt.type) {
                    case SDL.SDL_EventType.SDL_QUIT:
                        Program.Running = false;
                        break;
                    case SDL.SDL_EventType.SDL_KEYDOWN:
                        KeyDown(evt.key);
                        break;
                    case SDL.SDL_EventType.SDL_KEYUP:
                        KeyUp(evt.key);
                        break;
                }
            }
        }

        private static void KeyDown(SDL.SDL_KeyboardEvent evt)
        {
            SDL.SDL_Keycode key = evt.keysym.sym;

            if (key == SDL.SDL_Keycode.SDLK_ESCAPE) {
                Program.Running = false;
            } else if (key == SDL.SDL_Keycode.SDLK_F1) {
                State.NewGame(State.DIFF_EASY, false);
            } else if (key == SDL.SDL_Keycode.SDLK_F2) {
                State.NewGame(State.DIFF_NORMAL, false);
            } else if (key == SDL.SDL_Keycode.SDLK_F3) {
                State.NewGame(State.DIFF_HARD, false);
            } else if (key == SDL.SDL_Keycode.SDLK_F4) {
                State.NewGame(State.DIFF_HARD, true);
            } else if (!State.CpuGame && (key == SDL.SDL_Keycode.SDLK_s || key == SDL.SDL_Keycode.SDLK_DOWN)) {
                BottomThruster = true;
            } else if (!State.CpuGame && (key == SDL.SDL_Keycode.SDLK_a || key == SDL.SDL_Keycode.SDLK_LEFT)) {
                LeftThruster = true;
            } else if (!State.CpuGame && (key == SDL.SDL_Keycode.SDLK_d || key == SDL.SDL_Keycode.SDLK_RIGHT)) {
                RightThruster = true;
            }
        }

        private static void KeyUp(SDL.SDL_KeyboardEvent evt)
        {
            SDL.SDL_Keycode key = evt.keysym.sym;

            if (!State.CpuGame && (key == SDL.SDL_Keycode.SDLK_s || key == SDL.SDL_Keycode.SDLK_DOWN)) {
                BottomThruster = false;
            } else if (!State.CpuGame && (key == SDL.SDL_Keycode.SDLK_a || key == SDL.SDL_Keycode.SDLK_LEFT)) {
                LeftThruster = false;
            } else if (!State.CpuGame && (key == SDL.SDL_Keycode.SDLK_d || key == SDL.SDL_Keycode.SDLK_RIGHT)) {
                RightThruster = false;
            }
        }
    }
}