namespace Lander
{
    class Config
    {
        public const int WINDOW_WIDTH = 1400;
        public const int WINDOW_HEIGHT = 600;

        public const int TARGET_FPS = 60;

        public const int TERRAIN_TOP = 240;
        public const int TERRAIN_BOTTOM = 590;

        public const int SCROLL_LIMIT = 300;

        public const int LANDER_BOTTOM_HEIGHT = 18;
        public const int LANDER_TOP_SIZE = 14;
        public const int LANDER_WIDTH = 40;
        public const int SPAWN_HEIGHT = 100;

        public static double[] MAX_WIN_VEL_X = { 0.50, 0.25, 0.175 };
        public static double[] MAX_WIN_VEL_Y = { 1.00, 0.50, 0.325 };

        public const double MIN_GRAVITY = 0.8;
        public const double MAX_GRAVITY = 1.6;
        public const double MIN_WIND = -0.3;
        public const double MAX_WIND = 0.3;
        public const double MAIN_THRUSTER_BOOST = 4;
        public const double SIDE_THRUSTER_BOOST = 2;

        public const double START_FUEL = 100;
        public const double CONSUME_FUEL = 5;
    }
}