using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BreezyCode.Classes
{
    /// <summary>
    /// 
    /// Given three points in a [N x M] matrix
    ///     1. start point (anywhere in the matrix)
    ///     2. end point
    ///     3. wall point
    /// Constraints:
    ///     a. You can move only right or up
    ///     b. If you hit wall, you cannot move beyound it.
    /// Question:
    ///     Print any one path from start -> end
    /// </summary>
    public class MappingPath
    {
        public enum DirectionBias
        {
            Right = 0,
            Up = 1
        };

        private readonly int _rectWidth;
        private readonly int _rectHeight;
        private readonly DirectionBias _directionBias = DirectionBias.Right;

        public MappingPath(int rectWidth, int rectHeight, DirectionBias directionBias)
        {
            _rectWidth = rectWidth;
            _rectHeight = rectHeight;
            _directionBias = directionBias;
        }

        public string GetPath(Point start, Point end, List<Point> wallPts)
        {
            return Walk(start, end, wallPts, "");
        }

        private string Walk(Point current, Point end, List<Point> wallPts, string path)
        {
            if (wallPts.Contains(current) || current.X >= _rectWidth || current.Y >= _rectHeight) return null;
    
            if (current == end)
                return path;

            // setup the moves, also based on DirectionBias whether Right or Up
            List<Tuple<int, int, string>> moves = new List<Tuple<int, int, string>>
            {
                new Tuple<int, int, string>(1, 0, "R"),
                new Tuple<int, int, string>(0, 1, "U")
            };
            if (_directionBias == DirectionBias.Up)
                moves.Reverse();

            foreach (Tuple<int, int, string> move in moves)
            {
                string pathPlus = Walk(new Point(current.X + move.Item1, current.Y + move.Item2),
                    end, wallPts, path + move.Item3);
                if (!string.IsNullOrEmpty(pathPlus))
                    return pathPlus;
            }

            return null;
        }    
    }
}
