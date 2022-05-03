using System;
using System.Threading.Tasks;
using SpaceInvaders.Interfaces;

namespace SpaceInvaders
{
    internal class Rocket : IRenderable, IUpdatable, IEliminatable
    {
        private const ConsoleColor Color = ConsoleColor.Red;
        private readonly int _left;
        private int _newTop;
        private int _curTop;
        private bool _toEliminate;
        private bool _dirToUp;
        private readonly Field _field;
        private Rocket(int left, int top, bool dirToUp)
        {
            _left = left;
            _newTop = top;
            _dirToUp = dirToUp;
            _field = Field.GetField();
        }

        public async Task Render()
        {
            await Task.Run(() =>
            {
                if (_toEliminate)
                {
                    _field.Write(_left, _curTop, ' ');
                    _field[_left, _curTop] = null;
                    return;
                }
                // Если две ракеты хотят переместится на одну клетку нужно уничтожить обе,
                //TODO Передалать
                if (_field[_left, _newTop] != null && _field[_left, _newTop] != this)
                {
                    (_field[_left, _newTop] as IEliminatable)?.Eliminate();
                    Eliminate();
                }
                Field.GetField().Write(_left, _curTop, ' ');
                _field[_left, _curTop] = null;
                _field.Write(_left, _newTop, 'R', Color);
                _field[_left, _newTop] = this;
            });
        }

        public async Task Update()
        {
            await Task.Run(() =>
            {
                _curTop = _newTop;
                _newTop += _dirToUp ? -1 : 1;
                //Проверяем столкновения с краем карты
                if (_newTop < 0 || _newTop > _field.Height)
                {
                    Eliminate();
                    return;
                }
                // Проверяем столкновения с объектами
                if (_field[_left, _newTop] is IEliminatable)
                {
                    ((IEliminatable)_field[_left, _newTop]).Eliminate();
                    Eliminate();
                }
            });
        }

        public void Eliminate()
        {            
            Task.Run(()=> Console.Beep(200, 100));
            _toEliminate = true;
        }

        public static void AddRocket(int left, int top, bool dirToUp = true)
        {
            Rocket newRocket = new Rocket(left, top, dirToUp);
            // Проверяем, вдруг мы выстрелили в упор, если так уничтожаем ракету и объект
            if (newRocket._field[left, top] is IEliminatable)
            {
                ((IEliminatable)newRocket._field[left, top]).Eliminate();
                newRocket.Eliminate();
                return;
            }
            newRocket._field[left, top] = newRocket;
        }
    }
}
