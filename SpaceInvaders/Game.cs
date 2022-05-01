using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    class Game
    {
        public static Random Rnd;
        private static bool _gameOver;
        static Game()
        {
            Rnd = new Random();
        }
        static async Task Main(string[] args)
        {
            GameWindow gameWindow = GameWindow.GetGameWindow();
            while (!_gameOver)
            {
                gameWindow.Input = Console.KeyAvailable ? Console.ReadKey(true) : new ConsoleKeyInfo();

                // если нажить Q выходим
                if (gameWindow.Input.Key == ConsoleKey.Q) _gameOver = true;

                // Пауза
                if (gameWindow.Input.Key == ConsoleKey.P)
                {
                    while (true)
                    {
                        if (ConsoleKey.P == Console.ReadKey(true).Key)
                            break;
                    }
                }
                // Запускаем все задачи на рендериг и обновление
                await gameWindow.Render();
                await gameWindow.Update();
#warning test
                Thread.Sleep (300);
                Statistics.GetStatistics().FramesPast++;
            }
            Console.ReadKey();
        }

        public static void GameOver()
        {
            Statistics.GetStatistics().GameOver();
            _gameOver = true;
        }
    }
}
