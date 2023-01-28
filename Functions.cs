namespace HelperMathFunctions
{
    internal class Functions
    {
        static Random random = new Random();

        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        public struct CameraInfo
        {
            public Vector xyz;
            public Matrix facing;
            public float fov;
        }

        private static float Deg2Rad = 0.0174532925f;

        // Standard world-to-screen transformation to get screen coordinates from xyz-coordinates in the game
        public static Point WorldToScreen(Vector loc, CameraInfo cam)
        {
            var rc = new Rect();
            rc.left = 0;
            rc.right = 1920;
            rc.top = 0;
            rc.bottom = 1080;
            int[] size = new int[] { rc.right - rc.left, rc.bottom - rc.top };

            Vector diff = new Vector(loc.X, loc.Y, loc.Z) - cam.xyz;
            Matrix invertedMatrix = cam.facing.Inverse();
            float product = diff.X * cam.facing._x1 + diff.Y * cam.facing._x2 + diff.Z * cam.facing._x3;

            if (product < 0) return new Point(-1, -1);

            Vector view = new Vector(
            invertedMatrix._x1 * diff.X + invertedMatrix._y1 * diff.Y + invertedMatrix._z1 * diff.Z,
            invertedMatrix._x2 * diff.X + invertedMatrix._y2 * diff.Y + invertedMatrix._z2 * diff.Z,
            invertedMatrix._x3 * diff.X + invertedMatrix._y3 * diff.Y + invertedMatrix._z3 * diff.Z);

            Vector camera = new Vector(-view.Y, -view.Z, view.X);
            PointF gameScreen = new PointF((rc.right - rc.left) / 2.0f, (rc.bottom - rc.top) / 2.0f);
            PointF aspect = new PointF(gameScreen.X / (float)Math.Tan(((cam.fov * 55.0f) / 2.0f) * Deg2Rad), gameScreen.Y / (float)Math.Tan(((cam.fov * 35.0f) / 2.0f) * Deg2Rad));
            Point screenPos = new Point((int)(gameScreen.X + camera.X * aspect.X / camera.Z), (int)(gameScreen.Y + camera.Y * aspect.Y / camera.Z));

            // Return point -1, -1 if the resulting xy-coordinate was outside of the screen
            if (screenPos.X < 0 || screenPos.Y < 0 || screenPos.X > rc.right || screenPos.Y > rc.bottom)
                return new Point(-1, -1);

            return screenPos;
        }

        // Predict position of unit in a given time (duration) based on their current location and direction
        // Direction is determined by the angle the unit is facing at, as well as their relative movement direction (i.e. forward, front-left, etc.)
        internal static Vector PredictPosition(Vector currentLocation, float facing, byte moveDirection, float velocity, float duration)
        {
            float distance = velocity * duration;
            float forward = 0;
            float forward2 = 1;
            float right = 0;
            float multiplier = 1;

            if (moveDirection == 0)      // Not moving
            {
                forward2 = 0;
            }
            else if (moveDirection == 1) // Running forward
            {
                forward2 = 1;
            }
            else if (moveDirection == 2) // Running backward
            {
                forward2 = -1;
            }
            else if (moveDirection == 4) // Strafing left
            {
                right = 1;
            }
            else if (moveDirection == 5) // Strafing front-left
            {
                right = 1;
                forward = 1;
                multiplier = 1 / MathF.Sqrt(2);
            }
            else if (moveDirection == 6) // Strafing back-left
            {
                right = 1;
                forward = -1;
                multiplier = 1 / MathF.Sqrt(2);
            }
            else if (moveDirection == 8) // Strafing right
            {
                right = -1;
            }
            else if (moveDirection == 9) // Strafing front-right
            {
                right = -1;
                forward = 1;
                multiplier = 1 / MathF.Sqrt(2);
            }
            else if (moveDirection == 10) // Strafing back-right
            {
                right = -1;
                forward = -1;
                multiplier = 1 / MathF.Sqrt(2);
            }

            float x = currentLocation.X + (multiplier * distance * (((MathF.Cos(facing + (((float)Math.PI / 2) * right))) * forward2) + (MathF.Cos(facing)) * forward));
            float y = currentLocation.Y + (multiplier * distance * (((MathF.Sin(facing + (((float)Math.PI / 2) * right))) * forward2) + (MathF.Sin(facing)) * forward));

            // Since the movement direction is only specified in x and y-axis, we are unable to calculate a prediction for the z-coordinate
            // Because of this, the resulting predicted position uses the same z-coordinate as the starting position
            // The resulting coordinates should be intersected through ground to get a valid z-coordinate
            Vector predictedPos = new Vector(x, y, currentLocation.Z);
            return predictedPos;
        }


        // Box–Muller transformation for generating normally distributed random numbers
        // https://en.wikipedia.org/wiki/Box%E2%80%93Muller_transform
        public static int Muller(int mean, int std, int min, int max)
        {
            float i;
            float y;
            float u = (float)random.NextDouble();
            float v = (float)random.NextDouble() * (float)Math.PI * 2;
            u = MathF.Sqrt(-2 * MathF.Log(u)) * std;
            y = mean + u * MathF.Cos(v);

            if (y > max) return max;
            else if (y < min) return min;
            else return (int)MathF.Round(y);
        }

        public static int Muller_Skewed(int mean, int std, int min, int max, float skew)
        {
            float correctedSkewness = 1.667f * (MathF.Abs(skew) + 0.6f);
            float i;
            float y;
            float u = (float)random.NextDouble();
            float v = (float)random.NextDouble() * (float)Math.PI * 2;
            u = MathF.Sqrt(-2 * MathF.Log(u)) * std;
            y = mean + u * MathF.Cos(v);
            y = ((MathF.Pow((y * 20f) - 19f, correctedSkewness) - 1f) / 20f) / correctedSkewness;

            if (y > max) return max;
            else if (y < min) return min;
            else return (int)MathF.Round(y);
        }

        public static int Muller_Parabola(int mean, int std, int min, int max, float skew)
        {
            int avg = (min + max) / 2;
            float i;
            float y;
            float u = (float)random.NextDouble();
            float v = (float)random.NextDouble() * (float)Math.PI * 2;
            u = MathF.Sqrt(-2 * MathF.Log(u)) * std;
            y = mean + u * MathF.Cos(v);
            if (y < avg) y = mean - MathF.Pow(avg - y + 20, skew);
            else y = avg + MathF.Pow(y - avg + 20, skew);

            if (y > max) return max;
            else if (y < min) return min;
            else return (int)MathF.Round(y);
        }
    }
}
