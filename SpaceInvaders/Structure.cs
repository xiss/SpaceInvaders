using System;
using System.Threading.Tasks;
using SpaceInvaders.Interfaces;

namespace SpaceInvaders
{
    internal class Structure : IRenderable, IEliminatable
    {
        private bool _toEliminate;
        private readonly int _left;
        private readonly int _top;
        private bool _toRender;
        private readonly Field _field;
        private const ConsoleColor Color = ConsoleColor.Blue;

        public Structure(int left, int top)
        {
            _left = left;
            _top = top;
            _field = Field.GetFeild();
            _toRender = true;
        }

        public async Task Render()
        {
            await Task.Run(() =>
            {
                if (!_toRender) return;
                if (_toEliminate)
                {
                    _field.Write(_left, _top, ' ', Color);
                    _field[_left, _top] = null;
                    return;
                }
                _field.Write(_left, _top, 'S', Color);
                _toRender = false;
            });
        }

        public void Eliminate()
        {
            Statistics.GetStatistics().StructuresLost++;
            _toEliminate = true;
            _toRender = true;
        }
    }
}
