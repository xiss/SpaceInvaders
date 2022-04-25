using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpaceInvaders.Interfaces;

namespace SpaceInvaders
{
    internal class Structure : IRenderable, IEliminatable,IUpdatable
    {
        private bool _toEliminate;
        private readonly int _left;
        private readonly int _top;
        private bool _toRender;
        private readonly Field _field;

        public Structure(int left, int top)
        {
            _left = left;
            _top = top;
            _field = Field.GetFeild();
            _toRender = true;
        }

        public async Task Render(List<Task> tasks)
        {
            Task task = Task.Run(() =>
            {
                if (!_toRender) return;
                if (_toEliminate)
                {
                    _field.Write(_left, _top, ' ');
                    _field[_left, _top] = null;
                    return;
                }
                _field.Write(_left, _top, 'S');
                _toRender = false;
            });
            tasks.Add(task);
            await task;
        }

        public void Eliminate()
        {
            Statistics.GetStatistics().StructuresLost++;
            _toEliminate = true;
            _toRender = true;
        }

        //TODO Убарать этот метод
        public async Task Update(List<Task> tasks)
        {
            return; //throw new NotImplementedException();
        }
    }
}
