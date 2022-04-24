using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaders.Interfaces;

namespace SpaceInvaders
{
    internal class Field : IRenderable, IUpdatable
    {
        private static Field _instance;
        public readonly int Height;
        public readonly int Width;
        private readonly IRenderable[,] _state;

        private Field() { }

        private Field(int width, int height)
        {
            _state = new IRenderable[width, height];
            Height = height - 1;
            Width = width - 1;
        }
        public IRenderable this[int left, int top]
        {
            get => _state[left, top];
            set
            {
                lock (_instance)
                {
                    _state[left, top] = value;
                }
            }
        }

        public static Field GetFeild(int width, int height)
        {
            if (_instance == null)
            {
                _instance = new Field(width, height);
                Player.GetPlayer();
                //Добавляем врагов
                for (int i = width / 4; i < width - width / 4; i = i + 2)
                {
                    _instance[i, 3] = new Enemy(i, 3);
                    _instance[i, 4] = new Enemy(i, 4);
                    _instance[i, 5] = new Enemy(i, 5);
                }
                //Добавляем строения
                for (int i = 0; i < width; i++)
                {
                    if ((i / 5) % 2 == 0)
                    {
                        _instance[i, height - 4] = new Structure(i, height - 4);
                        _instance[i, height - 2] = new Structure(i, height - 2);
                        _instance[i, height - 3] = new Structure(i, height - 3);
                    }
                }
            }
            return _instance;
        }

        public static Field GetFeild()
        {
            if (_instance == null)
                throw new NullReferenceException("Поле не было создано");
            return _instance;
        }

        public async Task Render(List<Task> tasks)
        {
            foreach (var item in _state.Cast<IRenderable>().Select(i => i).Where(i => i != null))
            {
                Task task = Task.Run(() => item.Render(tasks));
                tasks.Add(task);
                await task;
            }
        }

        //TODO может добавить эту функцию в классс игрока?
        public void AddRocket(int left, int top)
        {
            _state[left, top] = new Rocket(left, top);
        }

        public async Task Update(List<Task> tasks)
        {
            foreach (var item in _state.Cast<IUpdatable>().Select(i => i).Where(i => i != null))
            {
                Task task = Task.Run(() => item.Update(tasks));
                tasks.Add(task);
                await task;
            }
        }

        public void Write(int left, int top, char ch)
        {
            lock (_instance)
            {
                if (left < 0 || top < 0 || left > Width || top > Height)
                {
                    throw new ArgumentOutOfRangeException();
                }
                GameWindow.Write(left + 1, top + 1, ch);
            }
        }
    }
}
