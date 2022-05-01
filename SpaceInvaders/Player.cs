using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpaceInvaders.Interfaces;

namespace SpaceInvaders
{
    internal class Player : IRenderable, IUpdatable, IEliminatable
    {
        private static Player _instance;
        private GameWindow _gameWindow;
        private Field _field;
        private bool _toRender;
        private int _curPos;
        private int _newPos;
        public event Action Eliminated;

        private Player()
        {
            _toRender = true;
            Eliminated += Game.GameOver;
        }

        public static Player GetPlayer()
        {
            if (_instance == null)
            {
                _instance = new Player();
                _instance._gameWindow = GameWindow.GetGameWindow();
                _instance._field = Field.GetFeild();

                // Определяем начальную позицию
                _instance._curPos = (_instance._field.Width / 2) - 1;
                _instance._newPos = _instance._curPos;
                _instance._field[_instance._curPos, _instance._field.Height] = _instance;
            }
            return _instance;
        }
        public async Task Render()
        {
            await Task.Run(() =>
            {
                if (_toRender)
                {
                    // Стираем старую позицию и рисуем новую, так же обновляем масссив для проверки коллизии
                    _field.Write(_curPos, _field.Height, ' ');
                    _field.Write(_newPos, _field.Height, 'P');
                    _field[_curPos, _field.Height] = null;
                    _field[_newPos, _field.Height] = this;

                    _toRender = false;
                    _curPos = _newPos;
                }
            });
        }

        public async Task Update()
        {
            await Task.Run(() =>
            {
                if (_gameWindow.Input.Key == ConsoleKey.A && _curPos > 0)
                {
                    _newPos--;
                    _toRender = true;
                }

                if (_gameWindow.Input.Key == ConsoleKey.D && _curPos < _field.Width)
                {
                    _newPos++;
                    _toRender = true;
                }

                if (_gameWindow.Input.Key == ConsoleKey.Spacebar)
                {
                    Rocket.AddRocket(_curPos, _field.Height - 1);
                    Statistics.GetStatistics().RocketUsed++;
                }
            });
        }

        public void Eliminate()
        {
            Eliminated?.Invoke();
        }
    }
}
