using System;
using System.Diagnostics;
using static System.Console;

namespace Zmeika_2
{
    internal class Program
    {
        private const int MapWidth = 30;
        private const int MapHeigh = 30;

        private const int ScreenWidth = MapWidth * 3;
        private const int ScreenHeigh = MapHeigh * 3;

        private const int FrameMS = 200;

        private const ConsoleColor BorderColor = ConsoleColor.White;
        private const ConsoleColor HeadColor = ConsoleColor.Red;
        private const ConsoleColor BodyColor = ConsoleColor.Red;
        private const ConsoleColor FoodColor = ConsoleColor.DarkGreen;


        private static readonly Random Random = new Random();
        static void Main(string[] args)
        {
            SetWindowSize (ScreenWidth, ScreenHeigh);
            SetBufferSize (ScreenWidth, ScreenHeigh);
            CursorVisible = false;
            while (true)
            {
                StarGame();
                Thread.Sleep(300);
                ReadKey();
            }     
        }
        static void StarGame()
        {
            Clear();
            DrawBorder();

            Direction currentMovment = Direction.Right;

            var snake = new Snake(10, 5, HeadColor, BodyColor);

            Pixel food = GenFood(snake);
            food.Drow();

            int score = 0;

            Stopwatch sw = new Stopwatch();

            while (true)
            {
                sw.Restart();

                Direction oldMovement = currentMovment;

                while (sw.ElapsedMilliseconds < FrameMS)
                {
                    if (currentMovment == oldMovement)
                    {
                        currentMovment = ReadMovement(currentMovment);
                    }
                }
                if(snake.Head.X == food.X && snake.Head.Y == food.Y)
                {
                    snake.Move(oldMovement, true);

                    food = GenFood(snake);
                    food.Drow();
                    score++;
                }
                else
                {
                    snake.Move(currentMovment);
                }

                if (snake.Head.X == MapWidth - 1
                    || snake.Head.X == 0
                    || snake.Head.Y == MapHeigh - 1
                    || snake.Head.Y == 0
                    || snake.Body.Any(b => b.X == snake.Head.X && b.Y == snake.Head.Y))
                    break;
            }
            snake.Clear();

            SetCursorPosition(ScreenWidth / 3, ScreenHeigh / 2);
            WriteLine($"Game Over, score: {score}");
        }

        static Pixel GenFood(Snake snake)
        {
            Pixel food;
            do
            {
                food = new Pixel(Random.Next(1, MapWidth - 2), Random.Next(1, MapHeigh - 2), FoodColor);
            } while (snake.Head.X == food.X && snake.Head.Y == food.Y
            || snake.Body.Any(b => b.X == food.X && b.Y == food.Y));
            return food;
        }
        static Direction ReadMovement(Direction currentDirection)
        {
            if(!KeyAvailable)
                    return currentDirection;

            ConsoleKey key = ReadKey(true).Key;

            currentDirection = key switch
            {
                ConsoleKey.UpArrow when currentDirection != Direction.Down => Direction.Up,
                ConsoleKey.DownArrow when currentDirection != Direction.Up => Direction.Down,
                ConsoleKey.LeftArrow when currentDirection != Direction.Right => Direction.Left,
                ConsoleKey.RightArrow when currentDirection != Direction.Left => Direction.Right,
                _ => currentDirection
            };

            return currentDirection;
        }
        static void DrawBorder()
        {
            for (int i = 0; i < MapWidth; i++)
            {
                new Pixel(x:i, y:0, BorderColor).Drow();
                new Pixel(x: i, y: MapHeigh - 1, BorderColor).Drow();
            }
            for (int i = 0; i < MapWidth; i++)
            {
                new Pixel(x: 0, y: i, BorderColor).Drow();
                new Pixel(x: MapHeigh - 1, y:i, BorderColor).Drow();
            }
        }
    }

   
}