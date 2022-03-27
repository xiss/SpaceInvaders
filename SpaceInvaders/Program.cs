using System.ComponentModel.Design;
using System.Threading;

namespace SpaceInvaders
{
    class Program
    {
        static void Main(string[] args)
        {
            bool abort = false;
            GameWindow gameWindow = GameWindow.GetGameWindow();
            while (!abort)
            {
                gameWindow.Render();
                gameWindow.Update();
                Thread.Sleep(100);
            }
        }
    }
}
