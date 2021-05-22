using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum WallState {
    LEFT = 1, // 0001
    RIGHT = 2, // 0010
    UP = 4, // 0100
    DOWN = 8, // 1000

    VISITED = 128 // 1000 0000
}

public struct Position {
    public int X;
    public int Y;
}

public struct Neighbor {
    public Position Position;
    public WallState SharedWall;
}

public static class MazeGenerator {
    private static WallState GetOppositeWall(WallState wall) {
        switch (wall) {
            case WallState.RIGHT: return WallState.LEFT;
            case WallState.LEFT: return WallState.RIGHT;
            case WallState.UP: return WallState.DOWN;
            case WallState.DOWN: return WallState.UP;
            default: return WallState.LEFT;
        }
    }

    private static WallState[,] RecursiveBacktracker(WallState[,] maze, uint width, uint height) {
        var rng = new System.Random();

        var positionStack = new Stack<Position>();
        var position = new Position { X = 0, Y = 0 };

        maze[position.X, position.Y] |= WallState.VISITED; // 1000 1111
        positionStack.Push(position);

        while (positionStack.Count > 0) {
            var current = positionStack.Pop();
            var neighbors = GetUnvisitedNeighbors(current, maze, width, height);

            if (neighbors.Count > 0) {
                positionStack.Push(current);

                var randIndex = rng.Next(0, neighbors.Count);
                var randomNeighbor = neighbors[randIndex];

                var nPosition = randomNeighbor.Position;

                maze[current.X, current.Y] &= ~randomNeighbor.SharedWall;
                maze[nPosition.X, nPosition.Y] &= ~GetOppositeWall(randomNeighbor.SharedWall);

                maze[nPosition.X, nPosition.Y] |= WallState.VISITED;

                positionStack.Push(nPosition);
            }
        }
        
        return maze;
    }

    private static List<Neighbor> GetUnvisitedNeighbors(Position p, WallState[,] maze, uint width, uint height) {
        List<Neighbor> list = new List<Neighbor>();

        if (p.X > 0) { // left
            if (!maze[p.X - 1, p.Y].HasFlag(WallState.VISITED)) {
                list.Add(new Neighbor {
                    Position = new Position {
                        X = p.X - 1,
                        Y = p.Y
                    },
                    SharedWall = WallState.LEFT
                });
            }
        }

        if (p.Y > 0) { // down
            if (!maze[p.X, p.Y - 1].HasFlag(WallState.VISITED)) {
                list.Add(new Neighbor {
                    Position = new Position {
                        X = p.X,
                        Y = p.Y - 1
                    },
                    SharedWall = WallState.DOWN
                });
            }
        }

        if (p.Y < height - 1) { // up
            if (!maze[p.X, p.Y + 1].HasFlag(WallState.VISITED)) {
                list.Add(new Neighbor {
                    Position = new Position {
                        X = p.X,
                        Y = p.Y + 1
                    },
                    SharedWall = WallState.UP
                });
            }
        }

        if (p.X < width - 1) { // right
            if (!maze[p.X + 1, p.Y].HasFlag(WallState.VISITED)) {
                list.Add(new Neighbor {
                    Position = new Position {
                        X = p.X + 1,
                        Y = p.Y
                    },
                    SharedWall = WallState.RIGHT
                });
            }
        }

        return list;
    }
        
    public static WallState[,] Generate(uint width, uint height) {
        WallState[,] maze = new WallState[width, height];

        WallState init = WallState.RIGHT | WallState.LEFT | WallState.UP | WallState.DOWN;

        for (int x = 0; x < width; ++x) {
            for (int y = 0; y < height; ++y) {
                maze[x, y] = init;
            }
        }

        return RecursiveBacktracker(maze, width, height);
    }

}