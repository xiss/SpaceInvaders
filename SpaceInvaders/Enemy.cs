using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            _field = Field.GetFeild();
            _toRender = true;
        }
        public async Task Render(List<Task> tasks)
        {
            if (!_toRender) return;

            Task task = Task.Run(() =>
            {
                if (_toEliminate)
                {
                    _field.Write(_curLeft, _curTop, ' ');
                    _field[_curLeft, _curTop] = null;
                    return;
                }
                _field.Write(_curLeft, _curTop, ' ');
                _field[_curLeft, _curTop] = null;
                _field.Write(_newLeft, _newTop, 'E');
                _field[_newLeft, _newTop] = this;
                _curLeft = _newLeft;
                _curTop = _newTop;
                _toRender = false;
            });
            tasks.Add(task);
            await task;
        }

        public async Task Update(List<Task> tasks)
        {
            Task task = Task.Run(() =>
            {
                // Движемся в соответссвии с патерном
                // Джвижемсся раз в 30 кадров
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
            tasks.Add(task);
            await task;
        }

        public void Eliminate()
        {
            _toEliminate = true;
            Statistics.GetStatistics().EnemyKilled++;
        }

        private void addRocket()
        {
            // Проверяем что внизу нет других врагов, и ессли выпало случайное число запускаем ракету
            if (Game.Rnd.Next(1, 100) != 1 
                || _field[_curLeft, _curTop + 1] != null
                || _field[_curLeft, _curTop + 2] != null
                || _field[_curLeft, _curTop + 3] != null
                || _field[_curLeft, _curTop + 4] != null
                || _field[_curLeft, _curTop + 5] != null
                ) 
                return;
            Rocket.AddRocket(_curLeft,_curTop+1,false);
        }
    }
}
