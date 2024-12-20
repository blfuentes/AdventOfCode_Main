module day20_part01

open AdventOfCode_2024.Modules
open System.Collections.Generic

type KindType =
    | Empty
    | Wall
    | Cheat
    | Start
    | End

type Coord ={
    Row: int
    Col: int
    Kind : KindType
}

let parseContent(lines: string array) =
    let map = Array2D.init lines.Length lines[0].Length (fun row col -> { Row = row; Col = col; Kind = Empty })
    let (maxrows, maxcols) = (map.GetLength(0), map.GetLength(1))
    let mutable startpoint = { Row = -1; Col = -1; Kind = Start }
    let mutable endpoint = { Row = -1; Col = -1; Kind = End }
    let wallspoints =
        [for row in 0..maxrows-1 do
            let line = lines[row].ToCharArray() |> Array.map string
            for col in 0..maxcols-1 do
                let value = line[col]
                match value with
                | "S" ->
                    map[row, col] <- { map[row, col] with Kind = Empty }
                    startpoint <- { startpoint with Row = row; Col = col }
                | "E" -> 
                    map[row, col] <- { map[row, col] with Kind = Empty }
                    endpoint <- { endpoint with Row = row; Col = col }
                | "#" ->
                    map[row, col] <- { map[row, col] with Kind = Wall }
                    yield map[row, col]
                | _ -> ignore()

        ]
            
    (map, wallspoints |> Set.ofList, startpoint, endpoint)

let findShortestPath (graph: Coord[,]) (wallpoints: Set<Coord>) (start: Coord) (goal: Coord) =
    let maxRows = graph.GetLength(0)
    let maxCols = graph.GetLength(1)

    let isInBoundaries (row: int) (col: int) =
        row >= 0 && col >= 0 && row < maxRows && col < maxCols

    let directions = [ (-1, 0); (1, 0); (0, 1); (0, -1) ]

    let queue = Queue<Coord * int>()
    let visited = HashSet<Coord>()
    let startingpoint = graph[start.Row, start.Col]
    queue.Enqueue((startingpoint, 0))

    let rec bfs () =
        if queue.Count = 0 then None
        else
            let (current, path) = queue.Dequeue()

            if current.Row = goal.Row && current.Col = goal.Col then
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



let execute() =
    //let path = "day20/day20_input.txt"
    let path = "day20/test_input_20.txt"
    let content = LocalHelper.GetLinesFromFile path
    let (map, wallpoints, startpoint, endpoint) = parseContent(content)
    let path = findShortestPath map startpoint endpoint
    path.Value