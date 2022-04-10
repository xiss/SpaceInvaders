using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

namespace SpaceInvaders
{
    internal class GameWindow : IRenderable, IUpdatable
    {
        // Размеры интерфейсса
        private const int Height = 50;
        private const int WidthField = 60;
        private const int WidthSidebar = 20;

        // Графика отрисовки интерфейса
        private const char Lt = '╔';
        private const char Rt = '╗';
        private const char Lb = '╚';
        private const char Rb = '╝';
        private const char Hor = '═';
        private const char Ver = '║';
        private const char Mt = '╦';
        private const char Mb = '╩';

        private Feild _feild;

        private static GameWindow _instance;
        private GameWindow()
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.SetWindowSize(WidthField + WidthSidebar, Height);
            Console.SetBufferSize(WidthField + WidthSidebar, Height);
            Console.Title = "SpaceInvaders ";
        }

        public static GameWindow GetGameWindow()
        {
            if (_instance == null)
            {
                _instance = new GameWindow();
                _instance._feild = Feild.GetFeild(WidthField, Height);
            }
            return _instance;
        }

        public ConsoleKeyInfo Input { get; set; }

        public void Render()
        {
            RenderSelf();
            _feild.Render();
        }

        private bool _toRender = true;

        private void RenderSelf()
        {
            if (!_toRender) return;
            // Рисуем углы
            Write(0, 0, Lt);
            Write(WidthField + WidthSidebar - 1, 0, Rt);
            Write(0, Height - 1, Lb);
            Write(WidthField + WidthSidebar - 1, Height - 1, Rb);
            // Рисуем вертикальные линии
            for (int h = 1; h < Height - 1; h++)
            {
                Write(0, h, Ver);
                Write(WidthField, h, Ver);
                Write(WidthField + WidthSidebar - 1, h, Ver);
            }
            // Рисуем горизонтальные линии
            for (int w = 1; w < WidthField + WidthSidebar - 1; w++)
            {
                Write(w, 0, Hor);
                Write(w, Height - 1, Hor);
            }
            // Рисуем стыки
            Write(WidthField, Height - 1, Mb);
            Write(WidthField, 0, Mt);
            _toRender = false;
        }

        public void Update()
        {
            (_feild as IUpdatable).Update();
        }

        public static void Write(int left, int top, char ch)
        {
            Console.SetCursorPosition(left, top);
            Console.Write(ch);
        }
    }
}
