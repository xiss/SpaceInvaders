using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpaceInvaders.Interfaces;

namespace SpaceInvaders
{
    internal class Rocket : IRenderable, IUpdatable, IEliminatable
    {
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
            _field = Field.GetFeild();
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
                Field.GetFeild().Write(_left, _curTop, ' ');
                _field[_left, _curTop] = null;
                _field.Write(_left, _newTop, 'R');
                _field[_left, _newTop] = this;
            });
        }

        public async Task Update()
        {
            //if(_toEliminate)return;
            //TODO иногда ракеты игрока переживают столкновения с ракетами врагов потомучто одновременно идут на одну клетку
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
