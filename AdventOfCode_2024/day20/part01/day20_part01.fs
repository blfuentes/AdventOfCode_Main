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
            
    (map, wallspoints, startpoint, endpoint)

let neighbous (position: Coord) =
    let directions = [ (-1, 0); (1, 0); (0, 1); (0, -1) ]
    [for (dRow, dCol) in directions do
        let nextRow = position.Row + dRow
        let nextCol = position.Col + dCol
        yield (nextRow, nextCol)
    ]

let isInBoundaries (row: int) (col: int) (maxRows: int) (maxCols: int) =
    row >= 0 && col >= 0 && row < maxRows && col < maxCols


let findShortestPath (graph: Coord[,]) (wallcheat: Coord option) (visited: HashSet<Coord>) (start: Coord) (goal: Coord) =
    let maxRows = graph.GetLength(0)
    let maxCols = graph.GetLength(1)

    let queue = Queue<Coord * int>()
    let startingpoint = graph[start.Row, start.Col]
    queue.Enqueue((startingpoint, 0))

    let rec bfs () =
        if queue.Count = 0 then (None, visited)
        else
            let (current, path) = queue.Dequeue()

            if current.Row = goal.Row && current.Col = goal.Col then
                let _ = visited.Add(goal)
                (Some(path), visited)
            else
                if not (visited.Contains(current)) && (current.Kind.IsEmpty || (wallcheat.IsSome && current.Row = wallcheat.Value.Row && current.Col = wallcheat.Value.Col)) then
                    let _ = visited.Add(current)

                    let neighbours = neighbous current
                    for (nextRow, nextCol) in neighbours do
                        if isInBoundaries nextRow nextCol maxRows maxCols then
                            let neighbor = graph[nextRow, nextCol]
                            if not (visited.Contains(neighbor)) then
                                queue.Enqueue((neighbor, path+1))
                bfs ()

    bfs ()

let tryToCheat(graph: Coord[,]) (wallpoints: Coord list) (start: Coord) (goal: Coord) =
    let maxRows = graph.GetLength(0)
    let maxCols = graph.GetLength(1)

    let (initialLength, visitedlengths) = findShortestPath graph None (HashSet<Coord>()) start goal
    
    let distances = Dictionary<(int*int), int>()
    visitedlengths
    |> Seq.iteri(fun idx c -> distances.Add((c.Row, c.Col), initialLength.Value - idx))

    let cheattimes =
        [for wall in wallpoints do
            let cheatwall = wall
            //let cheatwall = { Row = 1; Col = 8; Kind = Empty}
            //let cheatwall = { Row = 7; Col = 10; Kind = Empty}
            //let cheatwall = { Row = 7; Col = 6; Kind = Empty}

            let possibleexits = neighbous cheatwall
            let visited = HashSet<Coord>()
            let (firstpartlength, n1) = findShortestPath graph (Some(cheatwall)) visited start cheatwall
            if firstpartlength.IsSome then
                for (nextRow, nextCol) in possibleexits do
                    if isInBoundaries nextRow nextCol maxRows maxCols && 
                        graph[nextRow, nextCol].Kind.IsEmpty || distances.ContainsKey((nextRow, nextCol)) then
                        //let visited = HashSet<Coord>()
                        //let secondstart = graph[nextRow, nextCol]
                        
                        if distances.ContainsKey((nextRow, nextCol)) then
                            yield 1 + firstpartlength.Value + distances[(nextRow, nextCol)]

                        //let (secondpartlength, n2) = findShortestPath graph None visited secondstart goal
                        //if secondpartlength.IsSome then
                        //    yield (1 + firstpartlength.Value + secondpartlength.Value)
        ]
    let groupOfSavings =
        cheattimes
        |> List.map(fun t -> initialLength.Value - t)
        |> List.filter(fun t -> t > 0)
        |> List.groupBy id
        |> List.sortBy fst
        |> List.map(fun (k, v) -> (k, v.Length))
    groupOfSavings
    |> List.iter(fun (k, v) -> printfn "%d cheats saving %d" v k)

    groupOfSavings
    |> List.filter(fun (k, v) -> k >= 100)
    |> List.sumBy snd

let execute() =
    let path = "day20/day20_input.txt"
    //let path = "day20/test_input_20.txt"
    let content = LocalHelper.GetLinesFromFile path
    let (map, wallpoints, startpoint, endpoint) = parseContent(content)
    tryToCheat map wallpoints startpoint endpoint
    //let path = findShortestPath map wallpoints startpoint endpoint
    //path.Value