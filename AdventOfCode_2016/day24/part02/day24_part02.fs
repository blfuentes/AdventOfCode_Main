﻿module day24_part02

open AdventOfCode_2016.Modules
open AdventOfCode_Utilities.Utilities

open System.Collections
open System.Collections.Generic

type CellKind =
    | Empty
    | Wall

type Cell = {
    Row: int
    Col: int
    Kind: CellKind
}

type CheckPoint = {
    Name: int
    Pos: Cell
}

let parseContent(lines: string array) =
    let (maxrows, maxcols) = (lines.Length, lines[0].Length)
    let map = Array2D.init maxrows maxcols (fun r c -> { Row = r; Col = c; Kind = Empty })

    let checkpoints =
        [for row in 0..maxrows-1 do
            for col in 0..maxcols-1 do
                if lines[row][col] = '#' then
                    map[row, col] <- { map[row, col] with Kind = Wall }
                elif lines[row][col] <> '.' then
                    let name = (lines[row][col]).ToString()
                    yield { Name = (int)name; Pos = map[row, col] }
        ]
    (map, checkpoints)

let findShortestPath (graph: Cell[,]) (start: Cell) (goal: Cell) =
    let maxRows = graph.GetLength(0)
    let maxCols = graph.GetLength(1)

    let isInBoundaries (row: int) (col: int) =
        row >= 0 && col >= 0 && row < maxRows && col < maxCols

    let directions = [ (-1, 0); (1, 0); (0, 1); (0, -1) ]

    let queue = Queue<Cell * int>()
    let visited = HashSet<Cell>()

    queue.Enqueue((start, 0))

    let rec bfs () =
        if queue.Count = 0 then None
        else
            let (current, path) = queue.Dequeue()
            if current = goal then
                Some(path)
            else
                if not (visited.Contains(current)) && current.Kind.IsEmpty then
                    let _ = visited.Add(current)

                    for (dRow, dCol) in directions do
                        let nextRow = current.Row + dRow
                        let nextCol = current.Col + dCol
                        if isInBoundaries nextRow nextCol then
                            let neighbor = graph[nextRow, nextCol]
                            if not (visited.Contains(neighbor)) then
                                queue.Enqueue((neighbor, path+1))
                bfs ()

    bfs ()

let findAllWays(map: Cell[,]) (checkpoints: CheckPoint list) =
    let startzero = checkpoints |> List.find(fun c -> c.Name = 0)
    let possiblepaths = 
        permutations checkpoints
        |> List.filter(fun p ->
            p.Item(0).Name = 0 
        )
    
    possiblepaths
    |> List.map(fun p ->

        List.pairwise (p @ [startzero])
        |> List.map(fun (s, g) -> 
            let steps = findShortestPath map s.Pos g.Pos
            steps
        )
        |> List.sumBy(fun e -> if e.IsSome then e.Value else System.Int32.MaxValue)
    )
    |> List.min

let execute =
    let path = "day24/day24_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let (map, checkpoints) = parseContent content
    findAllWays map checkpoints