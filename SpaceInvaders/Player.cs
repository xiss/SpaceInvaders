using System;

namespace SpaceInvaders
{
    internal class Player : IRenderable, IUpdatable
    {
        private static Player _instance;
        private GameWindow _gameWindow;
        private Feild _feild;
        private bool _toRender;
        private int _curPos;
        private int _newPos;

        private Player()
        {
            _toRender = true;
        }

        public static Player GetPlayer()
        {
            if (_instance == null)
            {
                _instance = new Player();
                _instance._gameWindow = GameWindow.GetGameWindow();
                _instance._feild = Feild.GetFeild();

                // Определяем начальную позицию
                _instance._curPos = _instance._feild. Width / 2;
                _instance._newPos = _instance._curPos;
                _instance._feild[_instance._curPos, _instance._feild.Height - 1] = Feild.PixelType.Player;
            }
            return _instance;
        }
        public void Render()
        {
            if (_toRender)
            {
                // Стираем старую позицию и рисуем новую, так же обновляем масссив для проверки коллизии
                GameWindow.Write(_curPos, _instance._feild.Height - 2, ' ');
                GameWindow.Write(_newPos, _instance._feild.Height - 2, 'P');
                _feild[_curPos, _instance._feild.Height - 2] = Feild.PixelType.Free;
                _feild[_newPos, _instance._feild.Height - 2] = Feild.PixelType.Player;
                
                _toRender = false;
                _curPos = _newPos;
            }
        }

        public void Update()
        {
            if (_gameWindow.Input.Key == ConsoleKey.A && _curPos > 1)
            {
                _newPos--;
                _toRender = true;
            }

            if (_gameWindow.Input.Key == ConsoleKey.D && _curPos < _feild.Width - 1)
            {
                _newPos++;
                _toRender = true;
            }
        }
    }
}
