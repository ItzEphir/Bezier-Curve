using System;
using SFML.System;
using System.Collections.Generic;

namespace bieziezie
{
    public static class Functions
    {
        public static float Distance(this Vector2f a, Vector2f b) // найти расстояние от одного вектора до другого по теореме Пифагора
        {
            Vector2f diff = a - b;
            return MathF.Sqrt(MathF.Pow(diff.X, 2) + MathF.Pow(diff.Y, 2));
        }

        public static float Distance(this Vector2f a)
        {
            return MathF.Sqrt(MathF.Pow(a.X, 2) + MathF.Pow(a.Y, 2));
        }

        public static Vector2f Normalise(this Vector2f a) // вычисляем вектор направления
        {
            float length = a.Distance();
            Vector2f dir = a / length;
            return dir;
        }

        public static Vector2f Normalise(this Vector2f a, Vector2f b)
        {
            float length = a.Distance(b);
            Vector2f dir = (a - b) / length;
            return dir;
        }

        public static List<Stick> Connect(this List<Point> listp) // соединяем точки
        {
            List<Stick> list = new List<Stick>();
            if (listp.Count > 1)
            {
                for (int i = 0; i < listp.Count - 1; i++)
                {
                    list.Add(new Stick(listp[i], listp[i + 1]));
                }
            }
            return list;
        }

        public static int Factorial(this int num) // факториал, который в итоге не понадобился, обидка :((
        {
            int numb = 1;

            for(int i = num; i > 1; i--)
            {
                numb *= i;
            }

            return numb;
        }

        public static int Summing(this int sum)
        {
            int numb = 0;

            for(int i = 0; i <= sum; i++)
            {
                numb += i;
            }

            return numb;
        }
    }
}
