using System.Collections.Generic;
using SFML.Window;
using SFML.System;
using SFML.Graphics;
using System;

namespace bieziezie
{
    class Game
    {
        List<Point> myPoints = new List<Point>();
        List<List<Point>> listsPoints = new List<List<Point>>();
        List<Stick> mySticks = new List<Stick>();
        List<Point> rightPoints = new List<Point>();
        Point movingPoint;
        Point chosenPoint = null;

        int valuePoints = 4;
        public float kk = 0.025f;
        int counter = 0;
        float perfectk;
        float instantK;

        bool visible = true; // сюда можно добавить оскорблений Артема

        public Game()
        {
            listsPoints.Add(myPoints);
            perfectk = kk;
            instantK = perfectk / 10;
        }

        public void Draw(RenderWindow rw)
        {
            foreach (Stick s in mySticks)
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
                // foreach(Point p in movingPoints)
                // {
                //     CircleShape cs = new CircleShape();
                //     cs.Radius = 10;
                //     cs.Position = new Vector2f(p.X - cs.Radius, p.Y - cs.Radius);
                //     cs.FillColor = new Color(255, 0, 0);
                //     rw.Draw(cs);
                // }

                foreach(Point p in rightPoints)
                {
                    CircleShape cs = new CircleShape();
                    cs.Radius = 10;
                    cs.Position = new Vector2f(p.X - cs.Radius, p.Y - cs.Radius);
                    cs.FillColor = new Color(255, 0, 0);
                    rw.Draw(cs);
                }
            }

            if (movingPoint != null)
            {
                CircleShape cis = new CircleShape();
                cis.Radius = 10;
                cis.Position = new Vector2f(movingPoint.X - cis.Radius, movingPoint.Y - cis.Radius);
                cis.FillColor = new Color(0, 255, 0);
                rw.Draw(cis);
            }

            foreach (Point p in myPoints)
            {
                CircleShape cs = new CircleShape();
                cs.Radius = 10;
                cs.Position = new Vector2f(p.X - cs.Radius, p.Y - cs.Radius);
                cs.FillColor = new Color(255, 0, 0);
                rw.Draw(cs);
            }
            listsPoints.Clear();
            listsPoints.Add(myPoints);
            rightPoints.Clear();
            mySticks.Clear();
        }

        public void Next()
        {
            if(myPoints.Count == 1)
            {
                movingPoint = new Point(myPoints[0].coord.X, myPoints[0].coord.Y);
            }

            if (listsPoints[0].Count >= valuePoints)
            {
                List<Point> points = new List<Point>();

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

                rightPoints.Add(myPoints[0]);

                
                for (float k = kk; k < 1.0f; k += kk)
                {
                    for (int i = 0; i < listsPoints.Count; i++)
                    {
                        if (listsPoints[i].Count != 1)
                        {
                            for (int j = 0; j < listsPoints[i].Count - 1; j++)
                            {
                                points.Add(new Point(listsPoints[i][j].coord + k * listsPoints[i][j + 1].coord.Distance(listsPoints[i][j].coord) * (listsPoints[i][j + 1].coord - listsPoints[i][j].coord).Normalise()));
                            }
                            listsPoints.Add(points);
                            points = new List<Point>();
                        }
                    }
                    rightPoints.Add(listsPoints[listsPoints.Count - 1][listsPoints[listsPoints.Count - 1].Count - 1]);
                }

                rightPoints.Add(listsPoints[0][listsPoints[0].Count - 1]);

                mySticks = rightPoints.Connect();

                if (myPoints.Count == valuePoints && counter == 10)
                {
                    movingPoint.coord = countbiezie(instantK);
                }

                if (instantK < 1.0f)
                {
                    if (counter == 10)
                    {
                        instantK += perfectk / 10;
                    }
                }
                else
                {
                    instantK = perfectk / 10;
                }

                // foreach (Point point in rightPoints)
                // {
                //     movingPoints.Add(new Point(point.X, point.Y));
                // }

                // for (int i = 0; i < rightPoints.Count - 2; i++)
                // {
                //     movingPoints[i].coord -= rightPoints[i].coord.Distance(rightPoints[i + 1].coord) * (rightPoints[i + 1].coord - rightPoints[i].coord).Normalise() * instantK;
                // }
                // 
                // if (instantK < 1)
                // { 
                //     instantK += perfectk;
                // }
                // else
                // {
                //     instantK = perfectk;
                // }

                if (counter != 10)
                {
                    counter += 1;
                }
                else
                {
                    counter = 0;
                }
            }
        }

        public void KeyPressed(object sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Keyboard.Key.S:
                    visible = !visible;
                    break;
            }
        }

        public void MouseReleased(object sender, MouseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case Mouse.Button.Left:
                    chosenPoint = null;
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
                        myPoints.Add(new Point(e.X, e.Y));
                    }
                    else
                    {
                        Point temp = GetClosestPoint(e.X, e.Y);
                        if (temp != null)
                        {
                            chosenPoint = temp;
                        }
                    }
                    break;
            }
        }

        public void MouseMove(object sender, MouseMoveEventArgs e)
        {
            if (chosenPoint != null)
            {
                chosenPoint.coord = new Vector2f(e.X, e.Y);
            }
        }

        Point GetClosestPoint(float X, float Y)
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

        Vector2f countbiezie(float k)
        {
            List<List<Point>> ppps = new List<List<Point>>();

            ppps.Add(myPoints);
            List<Point> ps = new List<Point>();

            
            for (int i = 0; i < ppps.Count; i++)
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
            return coords;
        }
    }
}