using System;
using System.Collections.Generic;

namespace SpaceInvaders
{
    internal class GameWindow : IRenderable, IUpdatable
    {
        // Размеры интерфейсса
        private const int Height = 50;
        private const int WidthField = 60;
        private const int WidthSidebar = 20;

        // Графика отрисовки интерфейса
        private const char Lt = '┌';
        private const char Rt = '┐';
        private const char Lb = '└';
        private const char Rb = '┘';
        private const char Hor = '─';
        private const char Ver = '│';
        private const char Mt = '┬';
        private const char Mb = '┴';

        private Feild feild;

        private static GameWindow instance = null;
        private GameWindow()
        {
            feild = new Feild();
        }

        public static GameWindow GetGameWindow()
        {
            if (instance != null) return instance;
            Console.CursorVisible = false;

            Console.SetWindowSize(WidthField + WidthSidebar, Height);
            Console.SetBufferSize(WidthField + WidthSidebar, Height);
            Console.Title = "SpaceInvaders ";

            return new GameWindow();
        }

        public void Render()
        {
            RenderSelf();
            feild.Render();
        }

        public bool ToRender { get; set; } = true;

        private void RenderSelf()
        {
            if (!ToRender) return;
            Console.Clear();
            // Рисуем углы
            Console.SetCursorPosition(0, 0);
            Console.Write(Lt);
            Console.SetCursorPosition(WidthField + WidthSidebar - 1, 0);
            Console.Write(Rt);
            Console.SetCursorPosition(0, Height - 1);
            Console.Write(Lb);
            Console.SetCursorPosition(WidthField + WidthSidebar - 1, Height - 1);
            Console.Write(Rb);
            // Рисуем вертикальные линии
            for (int h = 1; h < Height - 1; h++)
            {
                Console.SetCursorPosition(0, h);
                Console.Write(Ver);
                Console.SetCursorPosition(WidthField, h);
                Console.Write(Ver);
                Console.SetCursorPosition(WidthField + WidthSidebar - 1, h);
                Console.Write(Ver);
            }
            // Рисуем горизонтальные линии
            for (int w = 1; w < WidthField + WidthSidebar - 1; w++)
            {
                Console.SetCursorPosition(w, 0);
                Console.Write(Hor);
                Console.SetCursorPosition(w, Height - 1);
                Console.Write(Hor);
            }
            // Рисуем стыки
            Console.SetCursorPosition(WidthField, Height - 1);
            Console.Write(Mb);
            Console.SetCursorPosition(WidthField, 0);
            Console.Write(Mt);

            ToRender = false;
        }

        public void Update()
        {
            (feild as IUpdatable).Update();
        }
    }
}
