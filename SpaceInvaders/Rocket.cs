﻿using System.Collections.Generic;
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
        private readonly Field _field;
        public Rocket(int left, int top)
        {
            _left = left;
            _newTop = top;
            _field = Field.GetFeild();
        }

        public async Task Render(List<Task> tasks)
        {
            Task task = Task.Run(() =>
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

            tasks.Add(task);
            await task;
        }

        public async Task Update(List<Task> tasks)
        {
            var task = Task.Run(() =>
            {
                _curTop = _newTop;
                _newTop--;
                //Проверяем столкновения
                if (_newTop < 0)
                {
                    Eliminate();
                    return;
                }
                if (_field[_left, _newTop] is IEliminatable)
                {
                    ((IEliminatable)_field[_left, _newTop]).Eliminate();
                    Eliminate();
                }
            });
            tasks.Add(task);
            await task;
        }

        public void Eliminate()
        {
            _toEliminate = true;
        }
    }
}