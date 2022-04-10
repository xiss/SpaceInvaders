using System;
using System.Collections.Generic;

namespace SpaceInvaders
{
    internal class Feild : IRenderable, IUpdatable
    {
        private static Feild _instance;
        public readonly int Height;
        public readonly int Width;

        private Feild() { }

        private Feild(int width, int height)
        {
            _state = new PixelType[width, height];
            Height = height;
            Width = width;
            _toRender = true;
        }
        public PixelType this [int left, int top]
        {
            get { return _state[left, top]; }
            set { _state[left, top] = value; }
        }

        static public Feild GetFeild(int width, int height)
        {
            if (_instance == null)
            {
                _instance = new Feild(width, height);
                _instance._content.Add(Player.GetPlayer());
            }
            return _instance;
        }

        public static Feild GetFeild()
        {
            if (_instance == null)
                throw new NullReferenceException("Поле не было создано");
            return _instance;
        }

        private readonly PixelType[,] _state;
        private bool _toRender;
        private readonly List<IRenderable> _content  = new List<IRenderable>();
        public void Render()
        {
            foreach (IRenderable item in _content)
            {
                item.Render();
            }
        }

        private void RenderSelf()
        {
            if (!_toRender) return;
            _toRender = false;
        }

        public void Update()
        {
            foreach (IUpdatable item in _content)
            {
                item.Update();
            }
        }

        public enum PixelType
        {
            Free,
            Player,
            Rocket,
            Swarm
        }
    }
}
