﻿using System.Drawing;
using ConsoleSnakeCompetition.Utilities;
using static ConsoleSnakeCompetition.Classes.Snake.Snake;

namespace ConsoleSnakeCompetition.Classes.Snake
{
    public class SnakePart
    {
        public char Symbol
        {
            get; set;
        }
        public Point Position
        {
            get; set;
        }

        public Direction CurrentDirection
        {
            get; set;
        }

        public readonly ConsoleColor[] Colors = new ConsoleColor[15] { ConsoleColor.DarkBlue, ConsoleColor.DarkGreen, ConsoleColor.DarkCyan, ConsoleColor.DarkRed,
                ConsoleColor.DarkMagenta, ConsoleColor.DarkYellow, ConsoleColor.Gray, ConsoleColor.DarkGray, ConsoleColor.Blue, ConsoleColor.Green,
                ConsoleColor.Cyan, ConsoleColor.Red, ConsoleColor.Magenta, ConsoleColor.Yellow, ConsoleColor.White };

        public ConsoleColor Color
        {
            get; set;
        }

        public SnakePart(Point position, Direction direction, char symbol, ConsoleColor color = ConsoleColor.DarkBlue)
        {
            Position = position;
            CurrentDirection = direction;
            Symbol = symbol;
            Color = color;
        }

        public void DrawColorized(bool inverted)
        {
            ConsoleColor[] colors = inverted ? Colors.Reverse().ToArray() : Colors;
            var i = _GetColorIndex();
            Console.SetCursorPosition(Position.Y, Position.X);
            Output.Write(colors[i], Symbol);
            _SetNewColor(++i);
        }

        private void _SetNewColor(int i)
        {
            if (i >= 15) i = 0;
            Color = Colors[i];
        }

        private int _GetColorIndex()
        {
            var i = Array.IndexOf(Colors, Color);
            if (i == -1) i = 0;
            return i;
        }

        public void Draw()
        {
            Console.SetCursorPosition(Position.Y, Position.X);
            Console.Write(Symbol);
        }

        public void Erase()
        {
            Console.SetCursorPosition(Position.Y, Position.X);
            Console.Write(' ');
        }
    }
}


