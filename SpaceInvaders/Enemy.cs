﻿using System;
using System.Threading.Tasks;
using SpaceInvaders.Interfaces;

namespace SpaceInvaders
{
    internal class Enemy : IRenderable, IUpdatable, IEliminatable
    {
        private int _newLeft;
        private int _newTop;
        private int _curLeft;
        private int _curTop;
        private bool _toRender;
        private bool _toEliminate;
        private Field _field;
        private const int RateOfFire = 100;

        private const ConsoleColor Color = ConsoleColor.DarkYellow;
        // Паттерн по которому будет двигаться рой
        private readonly ConsoleKey[] _movingPattern =
        {
            ConsoleKey.DownArrow,
            ConsoleKey.LeftArrow,
            ConsoleKey.DownArrow,
            ConsoleKey.LeftArrow,
            ConsoleKey.UpArrow,
            ConsoleKey.RightArrow,
            ConsoleKey.UpArrow,
            ConsoleKey.RightArrow,
        };
        private int _movingState;

        public Enemy(int left, int top)
        {
            _newLeft = left;
            _newTop = top;
            _field = Field.GetField();
            _toRender = true;
        }
        public async Task Render()
        {
            if (!_toRender) return;

            await Task.Run(() =>
            {
                if (_toEliminate)
                {
                    _field.Write(_curLeft, _curTop, ' ', Color);
                    _field[_curLeft, _curTop] = null;
                    return;
                }
                _field.Write(_curLeft, _curTop, ' ', Color);
                _field[_curLeft, _curTop] = null;
                _field.Write(_newLeft, _newTop, 'E', Color);
                _field[_newLeft, _newTop] = this;
                _curLeft = _newLeft;
                _curTop = _newTop;
                _toRender = false;
            });
        }

        public async Task Update()
        {
            await Task.Run(() =>
            {
                // Движемся в соответссвии с патерном
                // Джвижемся раз в 30 кадров
                if (Statistics.GetStatistics().FramesPast % 30 == 0)
                {
                    switch (_movingPattern[_movingState])
                    {
                        case ConsoleKey.DownArrow:
                            if (_field.Height > _curTop + 1 && _field[_curLeft, _curTop + 1] == null)
                                _newTop++;
                            break;
                        case ConsoleKey.UpArrow:
                            if (_curTop - 1 > 0 && _field[_curLeft, _curTop - 1] == null)
                                _newTop--;
                            break;
                        case ConsoleKey.LeftArrow:
                            if (_curLeft - 1 > 0 && _field[_curLeft - 1, _curTop] == null)
                                _newLeft--;
                            break;
                        case ConsoleKey.RightArrow:
                            if (_field.Width > _curLeft + 1 && _field[_curLeft + 1, _curTop] == null)
                                _newLeft++;
                            break;
                    }
                   _movingState = _movingState == _movingPattern.Length - 1 ? 0 : ++_movingState;
                }

                // Если свободная клетка это ракета то уничтожаемся
                if (_field[_newLeft, _newTop] is Rocket)
                {
                    Eliminate();
                    ((Rocket)_field[_newLeft, _newTop]).Eliminate();
                }

                // Выпускаем ракету
                addRocket();
                _toRender = true;
            });
        }

        public void Eliminate()
        {
            _toEliminate = true;
            Statistics.GetStatistics().EnemyKilled++;
        }

        private void addRocket()
        {
            // Проверяем что внизу нет других врагов, и ессли выпало случайное число запускаем ракету
            if (Game.Rnd.Next(1, RateOfFire) != 1
                || _field[_curLeft, _curTop + 1] != null
                || _field[_curLeft, _curTop + 2] != null
                || _field[_curLeft, _curTop + 3] != null
                || _field[_curLeft, _curTop + 4] != null
                || _field[_curLeft, _curTop + 5] != null
                )
                return;
            Rocket.AddRocket(_curLeft, _curTop + 1, false);
        }
    }
}
