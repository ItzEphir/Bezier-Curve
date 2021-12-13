using SFML.System;

namespace bieziezie
{
    public class Point
    {
        public Vector2f coord;

        public float X
        {
            get
            {
                return coord.X;
            }
            set
            {
                coord.X = X;
            }
        }

        public float Y
        {
            get
            {
                return coord.Y;
            }
            set
            {
                coord.Y = value;
            }
        }

        public float k;

        public Point(Vector2f _coord)
        {
            coord = _coord;
        }

        public Point(int x, int y)
        {
            coord = new Vector2f(x, y);
        }

        public Point(float x, float y)
        {
            coord = new Vector2f(x, y);
        }
    }
}
