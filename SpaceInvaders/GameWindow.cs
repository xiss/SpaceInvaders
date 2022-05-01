using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using SpaceInvaders.Interfaces;

namespace SpaceInvaders
{
    internal class GameWindow : IRenderable, IUpdatable
    {
        // Размеры интерфейсса
        private const int Height = 40;
        private const int WidthField = 60;
        private const int WidthSidebar = 22;

        // Графика отрисовки интерфейса
        private const char Lt = '╔';
        private const char Rt = '╗';
        private const char Lb = '╚';
        private const char Rb = '╝';
        private const char Hor = '═';
        private const char Ver = '║';
        private const char Mt = '╦';
        private const char Mb = '╩';
        private const ConsoleColor Color = ConsoleColor.White;

        private Field _field;
        private Statistics _statistics;

        private static GameWindow _instance;
        private GameWindow()
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.SetWindowSize(WidthField + WidthSidebar + 3, Height + 2);
            Console.SetBufferSize(WidthField + WidthSidebar + 3, Height + 2);
            Console.Title = "SpaceInvaders ";
        }

        public static GameWindow GetGameWindow()
        {
            if (_instance == null)
            {
                _instance = new GameWindow();
                _instance._field = Field.GetFeild(WidthField, Height);
                _instance._statistics = Statistics.GetStatistics(WidthField + 2, WidthField + 2 + WidthSidebar);
            }
            return _instance;
        }

        public ConsoleKeyInfo Input { get; set; }

        public async Task Render()
        {
            await Task.Run(() => RenderSelf());
            await Task.Run(() => _field.Render());
        }

        private bool _toRender = true;

        private async Task RenderSelf()
        {
            await Task.Run(() =>
            {
                if (!_toRender) return;
                // Рисуем углы
                Write(0, 0, Lt, Color);
                Write(WidthField + WidthSidebar + 2, 0, Rt, Color);
                Write(0, Height + 1, Lb, Color);
                Write(WidthField + WidthSidebar + 2, Height + 1, Rb, Color);
                // Рисуем вертикальные линии
                for (int h = 1; h < Height + 1; h++)
                {
                    Write(0, h, Ver, Color);
                    Write(WidthField + 1, h, Ver, Color);
                    Write(WidthField + WidthSidebar + 2, h, Ver, Color);
                }
                // Рисуем горизонтальные линии
                for (int w = 1; w < WidthField + WidthSidebar + 2; w++)
                {
                    Write(w, 0, Hor, Color);
                    Write(w, Height + 1, Hor, Color);
                }
                // Рисуем стыки
                Write(WidthField + 1, Height + 1, Mb, Color);
                Write(WidthField + 1, 0, Mt, Color);
                _toRender = false;
            });
            await Task.Run(() => _statistics.Render());
        }

        public async Task Update()
        {
            await Task.Run(() => _field.Update());
        }

        public static void Write(int left, int top, char ch, ConsoleColor color)
        {
            Write(left, top, ch.ToString(), color);
        }

        public static void Write(int left, int top, string str, ConsoleColor color)
        {
            lock (_instance)
            {
                Console.ForegroundColor = color;
                Console.SetCursorPosition(left, top);
                Console.Write(str);
                Console.ResetColor();
            }
        }
    }
}
