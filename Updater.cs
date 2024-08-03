using System;

namespace Lander
{
    class Updater
    {
        public static void Update(int tick)
        {
            if (State.Mode != State.STATE_PLAY) {
                return;
            }

            // Check for end of game
            if (CheckEnd()) {
                return;
            }

            // Calculate speed per frame
            double gravity = State.Gravity / Config.TARGET_FPS;
            double wind = State.Wind / Config.TARGET_FPS;
            double mainThrust = Config.MAIN_THRUSTER_BOOST / Config.TARGET_FPS;
            double sideThrust = Config.SIDE_THRUSTER_BOOST / Config.TARGET_FPS;

            State.VelocityX += wind;
            State.VelocityY += gravity;

            if (State.Fuel > 0) {
                double fuelConsumption = Config.CONSUME_FUEL / Config.TARGET_FPS;

                if (Input.BottomThruster) {
                    State.VelocityY -= mainThrust;
                    State.Fuel -= fuelConsumption;
                }
                if (Input.LeftThruster) {
                    State.VelocityX += sideThrust;
                    State.Fuel -= fuelConsumption / 2;
                }
                if (Input.RightThruster) {
                    State.VelocityX -= sideThrust;
                    State.Fuel -= fuelConsumption / 2;
                }
            }

            State.LanderX += State.VelocityX;
            State.LanderY += State.VelocityY;

            // Update location or scroll terrain
            double overLeftLimit = Config.SCROLL_LIMIT - State.LanderX;
            double overRightLimit = State.LanderX - (Config.WINDOW_WIDTH - Config.SCROLL_LIMIT);

            if (overLeftLimit > 0) {
                State.ViewOffset -= overLeftLimit;
                State.LanderX += overLeftLimit;
            } else if (overRightLimit > 0) {
                State.ViewOffset += overRightLimit;
                State.LanderX -= overRightLimit;
            }
        }

        private static bool CheckEnd()
        {
            int halfWidth = Config.LANDER_WIDTH / 2;
            int losePixels = 0;
            int winPixels = 0;
            int floorY = (int) Math.Round(State.LanderY) + Config.LANDER_BOTTOM_HEIGHT;

            for (int x = (int) Math.Round(State.LanderX) - halfWidth; x <= State.LanderX + halfWidth; x++) {
                if (State.GetTerrain(x) < floorY) {
                    losePixels++;
                } else if (State.GetTerrain(x) == floorY) {
                    winPixels++;
                }
            }

            if (losePixels > 0) {
                Console.WriteLine("The lander crashes on the ground!");
                State.Mode = State.STATE_LOSE;
                return true;
            }

            if (winPixels > 0) {
                if (winPixels < Config.LANDER_WIDTH) {
                    Console.WriteLine("The lander is destroyed due to lack of flat land! ({0} / {1}).", winPixels, Config.LANDER_WIDTH);
                    State.Mode = State.STATE_LOSE;
                    return true;
                } else {
                    // Check that velocity is within allowed limits
                    double maxVelX = Config.MAX_WIN_VEL_X[State.Diff];
                    double maxVelY = Config.MAX_WIN_VEL_Y[State.Diff];

                    if (Math.Abs(State.VelocityX) > maxVelX) {
                        Console.WriteLine("The lander is destroyed due to too high horizontal velocity! ({0:F2}, max: {1:F2}).", State.VelocityX, maxVelX);
                        State.Mode = State.STATE_LOSE;
                        return true;
                    } else if (Math.Abs(State.VelocityY) > maxVelY) {
                        Console.WriteLine("The lander is destroyed due to too high vertical velocity! ({0:F2}, max: {1:F2}).", State.VelocityY, maxVelY);
                        State.Mode = State.STATE_LOSE;
                        return true;
                    }

                    // Victory!
                    Console.WriteLine("The landing is successful!");
                    State.Mode = State.STATE_WIN;
                    return true;
                }
            }

            return false;
        }
    }
}