namespace TDMazeGenerator
{
    using System;
    using System.Collections.Generic;

    public class Maze
    {
        private char[,] maze;
        private List<char> mazePath;

        public Maze(char[,] maze, List<char> mazePath)
        {
            this.MazeArray = maze;
            this.MazePath = mazePath;
        }

        public char[,] MazeArray
        {
            get { return this.maze; }
            set { this.maze = value; }
        }

        public List<char> MazePath
        {
            get { return this.mazePath; }
            set { this.mazePath = value; }
        }

        public int PathLength
        {
            get { return this.MazePath.Count; }
        }
    }
}