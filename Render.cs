using System;
using SDL2;

namespace Lander
{
    class Render
    {
        public static IntPtr MainWindow;
        public static IntPtr MainRenderer;

        public static void Init()
        {
            SDL.SDL_Init(SDL.SDL_INIT_VIDEO);

            MainWindow = SDL.SDL_CreateWindow("Lander", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED,
                Config.WINDOW_WIDTH, Config.WINDOW_HEIGHT, 0);

            MainRenderer = SDL.SDL_CreateRenderer(MainWindow, -1, SDL.SDL_RendererFlags.SDL_RENDERER_SOFTWARE);
        }

        public static void Cleanup()
        {
            SDL.SDL_DestroyRenderer(MainRenderer);
            SDL.SDL_DestroyWindow(MainWindow);
        }

        public static void Draw()
        {
            // Background color
            Color.Set(Color.Sky);
            SDL.SDL_RenderClear(MainRenderer);

            DrawTerrain();

            if (State.Mode != State.STATE_LOSE) {
                DrawLander();
            }

            Text.Draw(String.Format("H-Vel {0:F2}", State.VelocityX), 0, 0);
            Text.Draw(String.Format("V-Vel {0:F2}", State.VelocityY), 0, 16);
            Text.Draw(String.Format("Fuel {0:F0}", State.Fuel), 0, 32);

            SDL.SDL_RenderPresent(MainRenderer);
        }

        private static void DrawTerrain()
        {
            for (int x = 0; x < Config.WINDOW_WIDTH; x++) {
                int y = State.GetTerrain(x);

                Color.Set(Color.Terrain);
                SDL.SDL_RenderDrawPoint(MainRenderer, x, y);
                SDL.SDL_RenderDrawPoint(MainRenderer, x, y + 1);

                Color.Set(Color.Terrain2);
                SDL.SDL_RenderDrawLine(MainRenderer, x, y + 2, x, Config.WINDOW_HEIGHT);
            }
        }

        private static void DrawLander()
        {
            Color.Set(State.Mode == State.STATE_WIN ? Color.Win : Color.Lander);

            int botHeight = Config.LANDER_BOTTOM_HEIGHT;
            int halfWidth = Config.LANDER_WIDTH / 2;

            int slope = 8;
            int x = (int) State.LanderX;
            int y = (int) State.LanderY;

            // Draw bottom
            SDL.SDL_RenderDrawLine(MainRenderer, x - halfWidth + slope, y, x + halfWidth - slope, y); // top
            SDL.SDL_RenderDrawLine(MainRenderer, x - halfWidth, y + botHeight, x + halfWidth, y + botHeight); // bottom
            SDL.SDL_RenderDrawLine(MainRenderer, x - halfWidth + slope, y, x - halfWidth, y + botHeight); // left
            SDL.SDL_RenderDrawLine(MainRenderer, x + halfWidth - slope, y, x + halfWidth, y + botHeight); // right

            // Inner lines
            SDL.SDL_RenderDrawLine(MainRenderer, x - halfWidth + slope + 1, y + 1, x + halfWidth - slope - 1, y + 1); // top
            SDL.SDL_RenderDrawLine(MainRenderer, x - halfWidth + 1, y + botHeight - 1, x + halfWidth - 1, y + botHeight - 1); // bottom
            SDL.SDL_RenderDrawLine(MainRenderer, x - halfWidth + slope + 1, y + 1, x - halfWidth + 1, y + botHeight - 1); // left
            SDL.SDL_RenderDrawLine(MainRenderer, x + halfWidth - slope - 1, y + 1, x + halfWidth - 1, y + botHeight - 1); // right

            // Draw top
            DrawRect(x - (Config.LANDER_TOP_SIZE / 2), y - Config.LANDER_TOP_SIZE + 2, Config.LANDER_TOP_SIZE, Config.LANDER_TOP_SIZE);
            DrawRect(x - (Config.LANDER_TOP_SIZE / 2) + 1, y - Config.LANDER_TOP_SIZE + 3, Config.LANDER_TOP_SIZE - 2, Config.LANDER_TOP_SIZE - 2);

            if (State.Mode == State.STATE_PLAY && State.Fuel > 0) {
                Color.Set(Color.Flame);

                // Bottom thruster
                int thrustMargin = 4;
                int thrustSize = 30;
                int halfThrustWidth = 8;

                if (Input.BottomThruster) {
                    SDL.SDL_RenderDrawLine(MainRenderer, x - halfThrustWidth, y + botHeight + thrustMargin, x + halfThrustWidth, y + botHeight + thrustMargin);
                    SDL.SDL_RenderDrawLine(MainRenderer, x - halfThrustWidth, y + botHeight + thrustMargin, x, y + botHeight + thrustSize);
                    SDL.SDL_RenderDrawLine(MainRenderer, x + halfThrustWidth, y + botHeight + thrustMargin, x, y + botHeight + thrustSize);

                    // Inner lines
                    SDL.SDL_RenderDrawLine(MainRenderer, x - halfThrustWidth + 1, y + botHeight + thrustMargin + 1, x + halfThrustWidth - 1, y + botHeight + thrustMargin + 1);
                    SDL.SDL_RenderDrawLine(MainRenderer, x - halfThrustWidth + 1, y + botHeight + thrustMargin + 1, x, y + botHeight + thrustSize - 1);
                    SDL.SDL_RenderDrawLine(MainRenderer, x + halfThrustWidth - 1, y + botHeight + thrustMargin + 1, x, y + botHeight + thrustSize - 1);
                }

                // Left thruster
                thrustMargin = 3;
                thrustSize = 16;
                halfThrustWidth = 4;
                int halfBotHeight = botHeight / 2;

                if (Input.LeftThruster) {
                    SDL.SDL_RenderDrawLine(MainRenderer, x - halfWidth - thrustMargin, y + halfBotHeight - halfThrustWidth, x - halfWidth - thrustMargin, y + halfBotHeight + halfThrustWidth);
                    SDL.SDL_RenderDrawLine(MainRenderer, x - halfWidth - thrustMargin, y + halfBotHeight - halfThrustWidth, x - halfWidth - thrustMargin - thrustSize, y + halfBotHeight);
                    SDL.SDL_RenderDrawLine(MainRenderer, x - halfWidth - thrustMargin, y + halfBotHeight + halfThrustWidth, x - halfWidth - thrustMargin - thrustSize, y + halfBotHeight);

                    // Inner lines
                    SDL.SDL_RenderDrawLine(MainRenderer, x - halfWidth - thrustMargin - 1, y + halfBotHeight - halfThrustWidth + 1, x - halfWidth - thrustMargin - 1, y + halfBotHeight + halfThrustWidth - 1);
                    SDL.SDL_RenderDrawLine(MainRenderer, x - halfWidth - thrustMargin - 1, y + halfBotHeight - halfThrustWidth + 1, x - halfWidth - thrustMargin - thrustSize + 1, y + halfBotHeight);
                    SDL.SDL_RenderDrawLine(MainRenderer, x - halfWidth - thrustMargin - 1, y + halfBotHeight + halfThrustWidth - 1, x - halfWidth - thrustMargin - thrustSize + 1, y + halfBotHeight);
                }

                // Right thruster
                if (Input.RightThruster) {
                    SDL.SDL_RenderDrawLine(MainRenderer, x + halfWidth + thrustMargin, y + halfBotHeight - halfThrustWidth, x + halfWidth + thrustMargin, y + halfBotHeight + halfThrustWidth);
                    SDL.SDL_RenderDrawLine(MainRenderer, x + halfWidth + thrustMargin, y + halfBotHeight - halfThrustWidth, x + halfWidth + thrustMargin + thrustSize, y + halfBotHeight);
                    SDL.SDL_RenderDrawLine(MainRenderer, x + halfWidth + thrustMargin, y + halfBotHeight + halfThrustWidth, x + halfWidth + thrustMargin + thrustSize, y + halfBotHeight);

                    // Inner lines
                    SDL.SDL_RenderDrawLine(MainRenderer, x + halfWidth + thrustMargin + 1, y + halfBotHeight - halfThrustWidth + 1, x + halfWidth + thrustMargin + 1, y + halfBotHeight + halfThrustWidth - 1);
                    SDL.SDL_RenderDrawLine(MainRenderer, x + halfWidth + thrustMargin + 1, y + halfBotHeight - halfThrustWidth + 1, x + halfWidth + thrustMargin + thrustSize - 1, y + halfBotHeight);
                    SDL.SDL_RenderDrawLine(MainRenderer, x + halfWidth + thrustMargin + 1, y + halfBotHeight + halfThrustWidth - 1, x + halfWidth + thrustMargin + thrustSize - 1, y + halfBotHeight);
                }
            }
        }

        private static void DrawRect(int x, int y, int w, int h)
        {
            var rect = Util.MakeSdlRect(x, y, w, h);
            SDL.SDL_RenderDrawRect(MainRenderer, ref rect);
        }
    }
}