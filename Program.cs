using System;
using SDL2;

namespace Lander
{
    class Program
    {
        public static bool Running = true;

        private static void MainLoop()
        {
            uint sleepTime = 1000 / Config.TARGET_FPS;
            int tick = 0;

            while (Running) {
                uint startTime = Util.Time();

                Input.Handle();

                Cpu.Think(tick);

                Updater.Update(tick);

                Render.Draw();

                uint elapsedTime = Util.Time() - startTime;
                if (elapsedTime < sleepTime) {
                    SDL.SDL_Delay(sleepTime - elapsedTime);
                }

                tick++;
            }
        }

        private static void Main(string[] args)
        {
            State.Init();
            Render.Init();
            Text.Init();

            MainLoop();

            Render.Cleanup();
            SDL.SDL_Quit();
        }
    }
}
