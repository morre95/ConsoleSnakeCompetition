﻿using System.Drawing;

namespace ConsoleSnakeCompetition.Classes
{
    public class Snake
    {
        public enum Direction
        {
            Up, Down, Left, Right
        }

        public char Symbol
        {
            get; set;
        }

        public readonly char[] BodyTailSymbols = new char[4] { '^', 'v', '<', '>' };

        public Direction CurrentDirection
        {
            get; set;
        }

        private SnakePart[] _body;
        private SnakePart Head => _body.First();
        private SnakePart Tail => _body.Last();

        private bool DrawColorized;

        public Snake(Point startingPoint, int length, char symbol = '#', bool drawColorized = false)
        {
            Symbol = symbol;
            DrawColorized = drawColorized;

            _body = Enumerable
                .Range(0, length)
                .Select(x => new SnakePart(startingPoint, Direction.Up, symbol))
                .ToArray();
        }

        public void Draw()
        {
            foreach (var snakePart in _body)
            {
                if (DrawColorized) snakePart.DrawColorized();
                else snakePart.Draw();
            }
        }

        public void Erase()
        {
            foreach (var snakePart in _body)
            {
                snakePart.Erase();
            }
        }

        public void AddLength(int length)
        {
            Erase();

            SnakePart newTail = new SnakePart(new Point(Tail.Position.X, Tail.Position.Y), _body[_body.Length - 1].CurrentDirection, BodyTailSymbols[(int)_body[_body.Length - 1].CurrentDirection]);

            Array.Resize(ref _body, _body.Length + length);
            for (var i = 1; i <= length; i++)
            {
                _body[_body.Length - i] = newTail;
            }

            Draw();
        }

        public void Move(int dx, int dy)
        {
            if (dx != 0)
            {
                CurrentDirection = dx <= 0 ? Direction.Up : Direction.Down;
            }
            else if (dy != 0)
            {
                CurrentDirection = dy <= 0 ? Direction.Left : Direction.Right;
            }

            var newHead = new SnakePart(new Point(Head.Position.X + dx, Head.Position.Y + dy), CurrentDirection, Symbol);

            Tail.Erase();
            for (var i = _body.Length - 1; i > 0; i--)
            {
                _body[i] = _body[i - 1];
                _body[i].Symbol = BodyTailSymbols[(int)_body[i].CurrentDirection];
            }
            _body[0] = newHead;
            Draw();
        }

        public void Move(Direction direction)
        {
            SnakePart newHead = null;

            switch (direction)
            {
                case Direction.Up:
                    newHead = new SnakePart(new Point(Head.Position.X - 1, Head.Position.Y), Direction.Up, Symbol);
                    CurrentDirection = Direction.Up;
                    break;
                case Direction.Down:
                    newHead = new SnakePart(new Point(Head.Position.X + 1, Head.Position.Y), Direction.Down, Symbol);
                    CurrentDirection = Direction.Down;
                    break;
                case Direction.Left:
                    newHead = new SnakePart(new Point(Head.Position.X, Head.Position.Y - 1), Direction.Left, Symbol);
                    CurrentDirection = Direction.Left;
                    break;
                case Direction.Right:
                    newHead = new SnakePart(new Point(Head.Position.X, Head.Position.Y + 1), Direction.Right, Symbol);
                    CurrentDirection = Direction.Right;
                    break;
            }

            Tail.Erase();
            for (var i = _body.Length - 1; i > 0; i--)
            {
                _body[i] = _body[i - 1];
                _body[i].Symbol = BodyTailSymbols[(int)_body[i].CurrentDirection];
            }
            _body[0] = newHead;
            Draw();
        }

        public int GetX()
        {
            return Head.Position.X;
        }

        public int GetY()
        {
            return Head.Position.Y;
        }
    }
}

