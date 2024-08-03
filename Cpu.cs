using System;

namespace Lander
{
    class Cpu
    {
        private const int MODE_SEARCH = 0;
        private const int MODE_LAND = 1;

        private static int Mode;
        private static bool GoRight;

        private static double LandingX;

        public static void Init(int rand)
        {
            Mode = MODE_SEARCH;
            GoRight = rand < 128;
        }

        public static void Think(int tick)
        {
            if (!State.CpuGame || State.Mode != State.STATE_PLAY) {
                return;
            }

            double maxVelX = 3.0;
            double minVelX = 2.5;

            if (Mode == MODE_SEARCH) {
                // Fire main thruster if we are too low
                Input.BottomThruster = State.LanderY > Config.SPAWN_HEIGHT + 10;

                if (GoRight) {
                    // Fire side thruster if required
                    Input.LeftThruster = State.VelocityX < minVelX;
                    Input.RightThruster = State.VelocityX > maxVelX;

                    // Search for landing spot
                    int flat = 1;
                    for (int x = (int) State.LanderX + 1; x < Config.WINDOW_WIDTH; x++) {
                        int prev = State.GetTerrain(x - 1);
                        int y = State.GetTerrain(x);

                        if (prev == y) {
                            flat++;

                            if (flat >= Config.LANDER_WIDTH + 6) {
                                Console.WriteLine("CPU found suitable landing location.");
                                LandingX = State.ViewOffset + x - (Config.LANDER_WIDTH / 2) - 2;
                                Mode = MODE_LAND;
                                break;
                            }
                        } else {
                            flat = 1;
                        }
                    }
                } else {
                    // Fire side thruster if required
                    Input.LeftThruster = State.VelocityX < -maxVelX;
                    Input.RightThruster = State.VelocityX > -minVelX;

                    // Search for landing spot
                    int flat = 1;
                    for (int x = (int) State.LanderX - 1; x >= 0; x--) {
                        int prev = State.GetTerrain(x + 1);
                        int y = State.GetTerrain(x);

                        if (prev == y) {
                            flat++;

                            if (flat >= Config.LANDER_WIDTH + 6) {
                                Console.WriteLine("CPU found suitable landing location.");
                                LandingX = State.ViewOffset + x + (Config.LANDER_WIDTH / 2) + 2;
                                Mode = MODE_LAND;
                                break;
                            }
                        } else {
                            flat = 1;
                        }
                    }
                }
            } else {
                // Landing mode

                // Calculate vertical target velocity
                double vVel = Math.Max(Math.Min((State.GetTerrain((int) State.LanderX) - State.LanderY) / 200, 1.0), 0.2);

                Input.BottomThruster = State.VelocityY > vVel;

                // Calculate horizontal target velocity
                double realLoc = State.ViewOffset + State.LanderX;
                double hVel = (LandingX - realLoc) / 100;

                Input.LeftThruster = State.VelocityX < hVel - 0.02;
                Input.RightThruster = State.VelocityX > hVel + 0.02;

                if (tick % 60 == 0) {
                    // Console.WriteLine("Hor {0:F2}, target {1:F2}", State.VelocityX, hVel);
                    // Console.WriteLine("Ver {0:F2}, target {1:F2}", State.VelocityY, vVel);
                }
            }
        }
    }
}