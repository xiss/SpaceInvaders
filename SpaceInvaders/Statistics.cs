using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpaceInvaders.Interfaces;

namespace SpaceInvaders
{
    internal class Statistics : IRenderable
    {
        private int _rocketUsed;
        public int RocketUsed
        {
            get => _rocketUsed;
            set
            {
                lock (this)
                {
                    _rocketUsed = value;
                }
            }
        }
        private int _enemyKilled;
        public int EnemyKilled
        {
            get => _enemyKilled;
            set
            {
                lock (this)
                {
                    _enemyKilled = value;
                }
            }
        }
        private int _framesPast;
        public int FramesPast
        {
            get => _framesPast;
            set
            {
                lock (this)
                {
                    _framesPast = value;
                }
            }
        }

        private int _structuresLost;
        public int StructuresLost
        {
            get => _structuresLost;
            set
            {
                lock (this)
                {
                    _structuresLost = value;
                }
            }
        }
        private static Statistics _instance;
        private bool _toRender = true;
        private readonly int _startColumn;
        private readonly int _lastColumn;

        private Statistics(int startColomn, int lastColumn)
        {
            _startColumn = startColomn;
            _lastColumn = lastColumn;
            GameWindow.Write(startColomn, 1, "Ракет потрачено:");
            GameWindow.Write(startColomn, 2, "Врагов Убито:");
            GameWindow.Write(startColomn, 3, "Кадров прошло:");
            GameWindow.Write(startColomn, 4, "Строений потеряно:");
        }
        public static Statistics GetStatistics(int startColumn, int lastColumn)
        {
            Statistics instance = _instance;
            if (instance != null)
            {
                return instance;
            }

            return (_instance = new Statistics(startColumn, lastColumn));
        }

        public static Statistics GetStatistics()
        {
            if (_instance == null)
                throw new NullReferenceException("Поле не было создано");
            return _instance;
        }

        public async Task Render(List<Task> tasks)
        {
            if (!_toRender) return;
            Task task = Task.Run(() =>
            {
                GameWindow.Write(_lastColumn - RocketUsed.ToString().Length, 1, RocketUsed.ToString());
                GameWindow.Write(_lastColumn - EnemyKilled.ToString().Length, 2, EnemyKilled.ToString());
                GameWindow.Write(_lastColumn - FramesPast.ToString().Length, 3, FramesPast.ToString());
                GameWindow.Write(_lastColumn - StructuresLost.ToString().Length, 4, StructuresLost.ToString());
            });
            tasks.Add(task);
            await task;
        }

        public void GameOver()
        {
            GameWindow.Write(_startColumn, 7, "ИГРА ОКОНЧЕНА!", ConsoleColor.Red);
        }
    }
}