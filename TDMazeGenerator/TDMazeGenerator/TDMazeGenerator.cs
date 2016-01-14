namespace TDMazeGenerator
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class TDMazeGenerator
    {
        public static char[,] maze;

        private static int longestPathCount = 0;
        private static int currentPathCount = 0;

        private static List<char> currentPath = new List<char>();

        private static List<List<char>> longestPaths = new List<List<char>>();

        public static void Main()
        {
            GenerateAllMazeCombinations(2, 10, 2, 10);

            //Console.Write("Number of rows: ");
            //int rows = int.Parse(Console.ReadLine());

            //Console.Write("Number of columns: ");
            //int columns = int.Parse(Console.ReadLine());

            //Console.Clear();
            //Console.WriteLine();

            ////GenerateMaze(rows, columns);

            //FindLongestPath(0, 0, 'S');

            ////Console.WriteLine("Longest path: {0}.", longestPathCount);
            ////Console.WriteLine("Paths: {0}.", longestPaths.Count);

            ////PrintPaths();

            //List<Maze> mazes = GetAllMazes(rows, columns, longestPaths);

            ////foreach (var maze in mazes)
            ////{
            ////    PrintMaze(maze);
            ////}

            //SaveAllMazeCombinations(mazes);
        }

        public static void PrintPaths()
        {
            Console.WriteLine();
            Console.WriteLine("Path count: {0}", longestPaths.Count);
            Console.WriteLine();

            foreach (var path in longestPaths)
            {
                foreach (var direction in path)
                {
                    Console.Write(direction + " ");
                }

                Console.Write(" -- {0}.", path.Count);

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public static bool CanGo(char direction, int currentRow, int currentColumn)
        {
            if (currentRow >= maze.GetLength(0) || currentRow < 0)
            {
                return false;
            }

            if (currentColumn >= maze.GetLength(1) || currentColumn < 0)
            {
                return false;
            }

            if (direction == 'L')
            {
                for (int row = currentRow - 1; row <= currentRow + 1; row++)
                {
                    for (int column = currentColumn - 1; column <= currentColumn; column++)
                    {
                        if (!(row >= maze.GetLength(0) || row < 0) && !(column >= maze.GetLength(1) || column < 0))
                        {
                            if (maze[row, column] == 'v')
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            else if (direction == 'U')
            {
                for (int row = currentRow - 1; row <= currentRow; row++)
                {
                    for (int column = currentColumn - 1; column <= currentColumn + 1; column++)
                    {
                        if (!(row >= maze.GetLength(0) || row < 0) && !(column >= maze.GetLength(1) || column < 0))
                        {
                            if (maze[row, column] == 'v')
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            else if (direction == 'R')
            {
                for (int row = currentRow - 1; row <= currentRow + 1; row++)
                {
                    for (int column = currentColumn; column <= currentColumn + 1; column++)
                    {
                        if (!(row >= maze.GetLength(0) || row < 0) && !(column >= maze.GetLength(1) || column < 0))
                        {
                            if (maze[row, column] == 'v')
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            else if (direction == 'D')
            {
                for (int row = currentRow; row <= currentRow + 1; row++)
                {
                    for (int column = currentColumn - 1; column <= currentColumn + 1; column++)
                    {
                        if (!(row >= maze.GetLength(0) || row < 0) && !(column >= maze.GetLength(1) || column < 0))
                        {
                            if (maze[row, column] == 'v')
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        public static void GenerateMaze(int rows, int columns)
        {
            maze = new char[rows, columns];

            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    maze[i, j] = '-';
                }
            }

            maze[0, 0] = 's';
            maze[rows - 1, columns - 1] = 'e';
        }

        public static void FindLongestPath(int row, int column, char currentPosition)
        {
            currentPath.Add(currentPosition);

            if (row >= maze.GetLength(0) || row < 0)
            {
                currentPath.RemoveAt(currentPath.Count - 1);
                return;
            }

            if (column >= maze.GetLength(1) || column < 0)
            {
                currentPath.RemoveAt(currentPath.Count - 1);
                return;
            }

            if (maze[row, column] == 'v')
            {
                currentPath.RemoveAt(currentPath.Count - 1);
                return;
            }

            if (currentPathCount >= 3)
            {
                if (!CanGo(currentPosition, row, column))
                {
                    currentPath.RemoveAt(currentPath.Count - 1);
                    return;
                }
            }

            currentPathCount++;

            if (maze[row, column] == 'e')
            {
                if (currentPathCount > longestPathCount)
                {
                    longestPaths = new List<List<char>>();
                }

                if (currentPathCount >= longestPathCount)
                {
                    longestPathCount = currentPathCount;

                    longestPaths.Add(new List<char>(currentPath));
                }

                currentPath.RemoveAt(currentPath.Count - 1);
                currentPathCount--;

                return;
            }

            maze[row, column] = 'v';

            FindLongestPath(row, column - 1, 'L'); // LEFT
            FindLongestPath(row - 1, column, 'U'); // UP
            FindLongestPath(row, column + 1, 'R'); // RIGHT
            FindLongestPath(row + 1, column, 'D'); // DOWN

            maze[row, column] = '-';
            currentPathCount--;
            currentPath.RemoveAt(currentPath.Count - 1);
        }

        public static List<Maze> GetAllMazes(int rows, int columns, List<List<char>> paths)
        {
            List<Maze> mazes = new List<Maze>();

            Maze maze;

            foreach (var path in paths)
            {
                maze = ConvertPathToMaze(rows, columns, path);

                mazes.Add(maze);
            }

            return mazes;
        }

        private static Maze ConvertPathToMaze(int rows, int columns, List<char> path)
        {
            char[,] pathMaze = new char[rows, columns];

            int currentRow = 0;
            int currentColumn = 0;
            Maze maze;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    pathMaze[i, j] = '#';
                }
            }

            foreach (var direction in path)
            {
                switch (direction)
                {
                    case 'L':
                        currentColumn--;
                        break;
                    case 'U':
                        currentRow--;
                        break;
                    case 'R':
                        currentColumn++;
                        break;
                    case 'D':
                        currentRow++;
                        break;
                }

                pathMaze[currentRow, currentColumn] = '+';
            }

            maze = new Maze(pathMaze, path);

            return maze;
        }

        public static void PrintMaze(Maze maze)
        {
            Console.WriteLine();
            Console.WriteLine("[Path - {0}]", maze.MazePath.Count);
            Console.WriteLine();

            for (int i = 0; i < maze.MazeArray.GetLength(0); i++)
            {
                for (int j = 0; j < maze.MazeArray.GetLength(1); j++)
                {
                    if (maze.MazeArray[i, j] == '+')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }

                    Console.Write(maze.MazeArray[i, j] + " ");
                    Console.ResetColor();
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        public static void SaveAllMazeCombinations(List<Maze> mazes)
        {
            string fileName = mazes[0].MazeArray.GetLength(0) + "R" + " - " + mazes[0].MazeArray.GetLength(1) + "C" + ".txt";

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine("Longest path: {0}.", mazes[mazes.Count - 1].PathLength);
                sw.WriteLine("Maze count: {0}.", mazes.Count);
                sw.WriteLine();

                foreach (var maze in mazes)
                {
                    sw.WriteLine("[Path Length: {0}.]", maze.PathLength);
                    sw.Write("Maze Path: ");
                    sw.WriteLine(string.Join("-", maze.MazePath));
                    sw.WriteLine();

                    for (int row = 0; row < maze.MazeArray.GetLength(0); row++)
                    {
                        for (int column = 0; column < maze.MazeArray.GetLength(1); column++)
                        {
                            sw.Write(maze.MazeArray[row, column] + " ");
                        }

                        sw.WriteLine();
                    }

                    sw.WriteLine();
                }
            }

            Console.Clear();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Mazes generated successfully!");
            Console.ResetColor();

            Console.WriteLine();
        }

        public static void GenerateAllMazeCombinations(int minRows, int maxRows, int minColumns, int maxColumns)
        {
            for (int row = minRows; row <= maxRows; row++)
            {
                for (int column = minColumns; column <= maxColumns; column++)
                {
                    GenerateMaze(row, column);

                    FindLongestPath(0, 0, 'S');

                    List<Maze> mazes = GetAllMazes(row, column, longestPaths);

                    SaveAllMazeCombinations(mazes);
                }

                longestPathCount = 0;
            }
        }
    }
}