using System;
using System.Collections.Generic;

namespace Lander
{
    class State
    {
        public const int STATE_PLAY = 0;
        public const int STATE_WIN = 1;
        public const int STATE_LOSE = 2;

        public const int DIFF_EASY = 0;
        public const int DIFF_NORMAL = 1;
        public const int DIFF_HARD = 2;

        public static int Mode = STATE_PLAY;
        public static int Diff;
        public static bool CpuGame;

        public static List<int> RightTerrain = new List<int>();
        public static List<int> LeftTerrain = new List<int>();

        public static double LanderX;
        public static double LanderY;
        public static double VelocityX;
        public static double VelocityY;

        public static double Gravity;
        public static double Wind;
        public static double Fuel;

        public static double ViewOffset = 0;

        private static Random rng = new Random();

        public static void Init()
        {
            NewGame(DIFF_NORMAL, false);
        }

        public static int GetTerrain(int viewX)
        {
            int index = (int) Math.Round(viewX + ViewOffset);

            if (index >= 0) {
                // Right side
                while (RightTerrain.Count <= index) {
                    GenerateTerrain(RightTerrain);
                }

                return RightTerrain[index];
            } else {
                // Left side

                // Reverse negative index
                index = -index - 1;

                while (LeftTerrain.Count <= index) {
                    GenerateTerrain(LeftTerrain);
                }

                return LeftTerrain[index];
            }
        }

        public static void NewGame(int diff, bool cpu)
        {
            Mode = STATE_PLAY;
            Diff = diff;
            CpuGame = cpu;

            LanderX = Config.WINDOW_WIDTH / 2;
            LanderY = Config.SPAWN_HEIGHT;
            VelocityX = 0;
            VelocityY = 0;

            Gravity = Config.MIN_GRAVITY + rng.NextDouble() * (Config.MAX_GRAVITY - Config.MIN_GRAVITY);
            Wind = Config.MIN_WIND + rng.NextDouble() * (Config.MAX_WIND - Config.MIN_WIND);
            Fuel = Config.START_FUEL;

            Input.BottomThruster = false;
            Input.LeftThruster = false;
            Input.RightThruster = false;

            string diffStr = "normal";
            if (diff == DIFF_EASY) {
                diffStr = "easy";
            } else if (diff == DIFF_HARD) {
                diffStr = "hard";
            }

            if (cpu) {
                Console.WriteLine("New CPU game ({0} difficulty)!", diffStr);
            } else {
                Console.WriteLine("New player game ({0} difficulty)!", diffStr);
            }

            Console.WriteLine("Gravity is at {0:F2} and wind is at {1:F2}.", Gravity, Wind);

            LeftTerrain.Clear();
            RightTerrain.Clear();

            // Generate first pixels of terrain
            int middle = Config.TERRAIN_TOP + (Config.TERRAIN_BOTTOM - Config.TERRAIN_TOP) / 2;
            int terrainSeed = rng.Next(middle - 100, middle + 100);

            if (terrainSeed <= middle) {
                // Above middle, generate top
                RightTerrain.Add(terrainSeed);
                RightTerrain.Add(terrainSeed + 1);
                LeftTerrain.Add(terrainSeed + 1);
                LeftTerrain.Add(terrainSeed + 2);
            } else {
                // Below middle, generate bottom
                RightTerrain.Add(terrainSeed);
                RightTerrain.Add(terrainSeed - 1);
                LeftTerrain.Add(terrainSeed - 1);
                LeftTerrain.Add(terrainSeed - 2);
            }

            if (cpu) {
                Cpu.Init(rng.Next(256));
            }
        }

        public static void GenerateTerrain(List<int> list)
        {
            int prev = list[list.Count - 2];
            int last = list[list.Count - 1];

            int r = rng.Next(128);

            bool nearTop = last <= Config.TERRAIN_TOP + 10;
            bool nearBottom = last >= Config.TERRAIN_BOTTOM - 10;
            bool slopeUp = last < prev;
            bool slopeDown = last > prev;

            if (nearTop) {
                if (r <= 3) {
                    list.Add(last - 1);
                } else {
                    list.Add(last + 1);
                }
            } else if (nearBottom) {
                if (r <= 3) {
                    list.Add(last + 1);
                } else {
                    list.Add(last - 1);
                }
            } else if (slopeDown) {
                if (r <= 3) {
                    list.Add(last);
                } else {
                    list.Add(last + 1);
                }
            } else if (slopeUp) {
                if (r <= 3) {
                    list.Add(last);
                } else {
                    list.Add(last - 1);
                }
            } else {
                if (r <= 4) {
                    list.Add(last + 1);
                } else if (r <= 9) {
                    list.Add(last - 1);
                } else {
                    list.Add(last);
                }
            }
        }
    }
}