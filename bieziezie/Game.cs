using System.Collections.Generic;
using SFML.Window;
using SFML.System;
using SFML.Graphics;
using System;

namespace bieziezie
{
    class Game
    {
        List<Point> myPoints = new List<Point>(); // список изначальных точек
        List<List<Point>> listsPoints = new List<List<Point>>(); // список всех списков кнопок
        List<Stick> mySticks = new List<Stick>(); // список палочек
        List<Point> rightPoints = new List<Point>(); // список точек кривой
        Point movingPoint; // зеленая двигающаяся точка
        Point chosenPoint = null; // выбранная точка

        int valuePoints = 4; // количество изначальных точек
        public float kk = 0.05f; // точность кривой
        int counter = 0; // счетчик передвижения точки
        float perfectk; // закрепленное значение точности
        float instantK; // значение параметра двигающейся точки

        bool visible = false; // показывать или нет точки
        bool allVisible = false; // показывать абсолютно все точки (осторожно, снижает производительность)
        // сюда можно добавить оскорблений Артема

        public Game()
        {
            listsPoints.Add(myPoints); // добавляем изначальные точки
            perfectk = kk; // приравниваем закрепленное значение к меняющейся
            instantK = perfectk / 10; // считаем значение передвигающейся точки
        }

        public void Draw(RenderWindow rw)
        {
            foreach (Stick s in mySticks) // рисуем линии
            {
                Vertex[] va = new Vertex[2];
                va[0] = new Vertex(s.A.coord);
                va[0].Color = new Color(255, 255, 255);
                va[1] = new Vertex(s.B.coord);
                va[1].Color = new Color(255, 255, 255);

                rw.Draw(va, PrimitiveType.Lines);
            }

            if (visible)
            {
                foreach(Point p in rightPoints) // рисуем точки кривой
                {
                    CircleShape cs = new CircleShape();
                    cs.Radius = 10;
                    cs.Position = new Vector2f(p.X - cs.Radius, p.Y - cs.Radius);
                    cs.FillColor = new Color(255, 0, 0);
                    rw.Draw(cs);
                }
            }

            if (allVisible)
            {
                foreach(List<Point> ls in listsPoints)
                {
                    foreach(Point p in ls)
                    {
                        CircleShape cs = new CircleShape();
                        cs.Radius = 10;
                        cs.Position = new Vector2f(p.X - cs.Radius, p.Y - cs.Radius);
                        cs.FillColor = new Color(255, 0, 0);
                        rw.Draw(cs);
                    }
                }
            }

            if (movingPoint != null) // рисуем двигающуюся точку
            {
                CircleShape cis = new CircleShape();
                cis.Radius = 10;
                cis.Position = new Vector2f(movingPoint.X - cis.Radius, movingPoint.Y - cis.Radius);
                cis.FillColor = new Color(0, 255, 0);
                rw.Draw(cis);
            }

            foreach (Point p in myPoints) // рисуем изначальные точки
            {
                CircleShape cs = new CircleShape();
                cs.Radius = 10;
                cs.Position = new Vector2f(p.X - cs.Radius, p.Y - cs.Radius);
                cs.FillColor = new Color(255, 0, 0);
                rw.Draw(cs);
            }

            // обнуляем все списки
            listsPoints.Clear();
            listsPoints.Add(myPoints);
            rightPoints.Clear();
            mySticks.Clear();
        }

        public void Next()
        {
            if (allVisible)
            {
                visible = true;
            }

            if(myPoints.Count == 1) 
            {
                movingPoint = new Point(myPoints[0].coord.X, myPoints[0].coord.Y); // добавляем двигающуюся точку с координатами первой изначальной точки
            }

            if (listsPoints[0].Count >= valuePoints)
            {
                List<Point> points = new List<Point>(); // временный список

                // память.........
                // for (int i = 0; i < listsPoints.Count; i++)
                // {
                //     if (listsPoints[i].Count != 1)
                //     {
                //         for (float k = kk; k < 1.0f; k += kk)
                //         {
                //             for (int j = 0; j < listsPoints[i].Count - 1; j++)
                //             {
                //                 points.Add(new Point(listsPoints[i][j].coord + k * listsPoints[i][j + 1].coord.Distance(listsPoints[i][j].coord) * (listsPoints[i][j + 1].coord - listsPoints[i][j].coord).Normalise()));
                //             }
                //             listsPoints.Add(points);
                //             points = new List<Point>();
                //         }
                //     }
                //     else
                //     {
                //         break;
                //     }
                // }

                rightPoints.Add(myPoints[0]); // добавляем в отображаемые точки изначальную точку

                
                for (float k = kk; k < 1.0f; k += kk) // фор с параметром, который увеличивается на заданный параметр kk
                {
                    for (int i = 0; i < listsPoints.Count; i++) // просматриваем список списков
                    {
                        if (listsPoints[i].Count != 1) // прекращаем бесконечный фор
                        {
                            for (int j = 0; j < listsPoints[i].Count - 1; j++) // просматриваем список по индексу
                            {
                                points.Add(new Point(listsPoints[i][j].coord + k * listsPoints[i][j + 1].coord.Distance(listsPoints[i][j].coord) * (listsPoints[i][j + 1].coord - listsPoints[i][j].coord).Normalise()));
                                // пояснение формулы: к выбранной точке мы прибавляем вектор, который равен перемножению вектора направления на значение k и длину от точки до следующей
                            }
                            listsPoints.Add(points); // добавляем в список списков список
                            points = new List<Point>(); // обнуляем список
                        }
                    }
                    rightPoints.Add(listsPoints[listsPoints.Count - 1][listsPoints[listsPoints.Count - 1].Count - 1]); // выбираем нужные точки
                }

                rightPoints.Add(listsPoints[0][listsPoints[0].Count - 1]); // добавляем последнюю точку

                mySticks = rightPoints.Connect(); // соединяем точки линиями

                if (myPoints.Count == valuePoints && counter == 10)
                {
                    movingPoint.coord = countbiezie(instantK); // раз в 10 кадров (можно поменять) считаем координаты двигающейся точки
                }

                if (instantK < 1.0f) 
                {
                    if (counter == 10)
                    {
                        instantK += perfectk / 10; // обновляем параметр
                    }
                }
                else
                {
                    instantK = perfectk / 10; // обнуляем параметр
                }

                if (counter != 10)
                {
                    counter += 1; // обновляем счетчик кадров
                }
                else
                {
                    counter = 0; // обнуляем счетчик кадров
                }
            }
        }

        public void KeyPressed(object sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Keyboard.Key.S:
                    visible = !visible; // делаем видимыми точки
                    break;
                case Keyboard.Key.F8:
                    allVisible = !allVisible;
                    break;
            }
        }

        public void MouseReleased(object sender, MouseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case Mouse.Button.Left:
                    chosenPoint = null; // отпускаем кнопку
                    break;
            }
        }

        public void MousePressed(object sender, MouseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case Mouse.Button.Left:
                    if (myPoints.Count < valuePoints)
                    {
                        myPoints.Add(new Point(e.X, e.Y)); // если нет точки, то создаем ее
                    }
                    else
                    {
                        Point temp = GetClosestPoint(e.X, e.Y);
                        if (temp != null)
                        {
                            chosenPoint = temp; // если есть точка, то можем перемещать ее
                        }
                    }
                    break;
            }
        }

        public void MouseMove(object sender, MouseMoveEventArgs e)
        {
            if (chosenPoint != null)
            {
                chosenPoint.coord = new Vector2f(e.X, e.Y); // передвигаем точку
            }
        }

        Point GetClosestPoint(float X, float Y) // получаем ближайшую координату к точке
        {
            Point res = null;

            foreach (Point p in myPoints)
            {
                if (new Vector2f(p.X, p.Y).Distance(new Vector2f(X, Y)) < 10)
                {
                    res = p; 
                }
            }

            return res;
        }

        Vector2f countbiezie(float k) // считаем координаты точки по списку изначальных точек и значению k
        {
            List<List<Point>> ppps = new List<List<Point>>(); // список списков

            ppps.Add(myPoints); // добавляем изначальные точки
            List<Point> ps = new List<Point>(); // временный список

            
            for (int i = 0; i < ppps.Count; i++) // то же самое, что и в основном цикле, только с заданным значением k
            {
                if (ppps[i].Count != 1)
                {
                    for (int j = 0; j < ppps[i].Count - 1; j++)
                    {
                        ps.Add(new Point(ppps[i][j].coord + k * ppps[i][j + 1].coord.Distance(ppps[i][j].coord) * (ppps[i][j + 1].coord - ppps[i][j].coord).Normalise()));
                    }

                    ppps.Add(ps);
                    ps = new List<Point>();
                }
                else
                {
                    break;
                }
            }

            Vector2f coords = ppps[ppps.Count - 1][ppps[ppps.Count - 1].Count - 1].coord; 
            return coords; // возвращаем координаты
        }
    }
}