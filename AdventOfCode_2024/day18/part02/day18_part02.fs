module day18_part02

open AdventOfCode_2024.Modules
open System.Collections.Generic

type KindType =
    | Empty
    | Corrupted

type Coord ={
    X: int
    Y: int
    Kind : KindType
}

let parseContent(lines: string array) =
    let map = Array2D.init 71 71 (fun x y -> { X = x; Y = y; Kind = Empty })
    let corrupted = 
        lines
        |> Array.map(fun l ->
            { X = (int)(l.Split(",")[0]); Y =(int)(l.Split(",")[1]); Kind = Corrupted }
        ) |> List.ofArray
            
    (map, corrupted)

let printMap(graph: Coord[,]) =
    let maxX = graph.GetLength(0)
    let maxY = graph.GetLength(1)
    for row in 0..(maxX-1) do
        for col in 0..(maxY-1) do
            printf "%s" (if graph[row,col].Kind.IsCorrupted then "#" else ".")
        printfn ""

let findShortestPath (graph: Coord[,]) (start: Coord) (goal: Coord) =
    let maxY = graph.GetLength(0)
    let maxX = graph.GetLength(1)

    let isInBoundaries (x: int) (y: int) =
        x >= 0 && y >= 0 && x < maxX && y < maxY

    let directions = [ (0, 1); (1, 0); (0, -1); (-1, 0) ]

    let queue = Queue<Coord * Coord list>()
    let visited = HashSet<Coord>()

    queue.Enqueue((start, [start]))

    let rec bfs () =
        if queue.Count = 0 then None
        else
            let (current, path) = queue.Dequeue()

            if current = goal then
                Some(path)
            else
                if not (visited.Contains(current)) && current.Kind.IsEmpty then
                    let _ = visited.Add(current)

                    for (dx, dy) in directions do
                        let nextX = current.X + dx
                        let nextY = current.Y + dy
                        if isInBoundaries nextX nextY then
                            let neighbor = graph[nextX, nextY]
                            if not (visited.Contains(neighbor)) then
                                queue.Enqueue((neighbor, path @ [neighbor]))
                bfs ()

    bfs ()

let findCorrupted(map: Coord[,]) (corrupted: Coord list) (start: Coord) (goal: Coord) =
    let rec walk (remainingCorrupted: Coord list) =
        match remainingCorrupted with
        | c :: tail ->
            map[c.Y, c.X] <- { map[c.Y, c.X] with Kind = Corrupted }
            let canFinish = findShortestPath map start goal
            match canFinish with
            | Some(p) -> walk tail
            | None -> c
        | _ -> failwith "error!"
    walk corrupted

let execute() =
    let path = "day18/day18_input.txt"

    let content = LocalHelper.GetLinesFromFile path
    let (map, corrupted) = parseContent content
    let start = { X = 0; Y = 0; Kind = Empty }
    let endnode = { X = 70; Y = 70; Kind = Empty }
    let found = findCorrupted map corrupted start endnode
    sprintf "%d,%d" found.X found.Y