using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System;

namespace bieziezie
{
    class Program
    {
        static Game myGame;

        static void Main(string[] args)
        {
            VideoMode vm = new VideoMode(800, 600);
            RenderWindow rw = new RenderWindow(vm, "BEZIE");

            myGame = new Game();

            rw.Closed += OnClose;
            rw.MouseButtonPressed += OnMousePressed;
            rw.KeyPressed += OnKeyPressed;
            rw.MouseMoved += OnMouseMoved;
            rw.MouseButtonReleased += OnMouseReleased;

            while (rw.IsOpen)
            {
                rw.DispatchEvents();

                myGame.Next();

                rw.Clear();

                myGame.Draw(rw);

                rw.Display();
            }
        }

        static void OnKeyPressed(object sender, KeyEventArgs e)
        {
            myGame.KeyPressed(sender, e);

            if (e.Code == Keyboard.Key.Escape)
            {
                (sender as RenderWindow)?.Close();
            }
        }

        static void OnMousePressed(object sender, MouseButtonEventArgs e)
        {
            myGame.MousePressed(sender, e);
        }

        static void OnMouseReleased(object sender, MouseButtonEventArgs e)
        {
            myGame.MouseReleased(sender, e);
        }

        static void OnClose(object sender, EventArgs e)
        {
            (sender as RenderWindow)?.Close();
        }

        static void OnMouseMoved(object sender, MouseMoveEventArgs e)
        {
            myGame.MouseMove(sender, e);
        }
    }
}
