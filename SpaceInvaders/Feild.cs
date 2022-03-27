using System;
using System.Collections.Generic;

namespace SpaceInvaders
{
    internal class Feild : IRenderable, IUpdatable
    {
        private char[] heartBeats = new[] { '-', '\\', '|', '/' };

        private int pos = 0;
        private bool toRender { get; set; } = true;
        private List<IRenderable> content { get; set; }
        public void Render()
        {
            RenderSelf();
        }

        private void RenderSelf()
        {
            if (!toRender) return;

            Console.SetCursorPosition(3, 3);
            Console.Write(heartBeats[pos]);
            pos = pos == 3 ? 0 : pos + 1;
            toRender = false;
        }

        public void Update()
        {
            UpdateSelf();
        }

        private void UpdateSelf()
        {
            toRender = true;
        }
    }
}
