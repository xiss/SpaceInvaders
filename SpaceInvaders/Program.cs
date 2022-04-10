using System;
using System.ComponentModel.Design;
using System.Threading;

namespace SpaceInvaders
{
    class Program
    {
        static void Main(string[] args)
        {
            bool abort = false;
            GameWindow gameWindow = GameWindow.GetGameWindow();
            while (!abort)
            {
                gameWindow.Input = Console.KeyAvailable ? Console.ReadKey(true) : new ConsoleKeyInfo();

                // если нажить Q выходим
                if (gameWindow.Input.Key == ConsoleKey.Q) abort = true;

                // Пауза
                if (gameWindow.Input.Key == ConsoleKey.P)
                {
                    while (true)
                    {
                        if (ConsoleKey.P == Console.ReadKey(true).Key)
                            break;
                    }
                }


                gameWindow.Render();
                gameWindow.Update();
                Thread.Sleep(50);
            }
        }
    }
}
