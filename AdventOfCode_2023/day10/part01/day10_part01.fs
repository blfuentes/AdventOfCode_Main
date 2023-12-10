module day10_part01

open System
open System.Collections.Generic
open System.IO

open AdventOfCode_Utilities

let ReadLines(path: string) =
    File.ReadLines(__SOURCE_DIRECTORY__ + @"../../../" + path)

type Coord = {
    row: int
    col: int
}

type Position = {
    coord: Coord
    value: string
    connections: Position list
}

type VisitedPosition = {
    position: Position
    visited: bool
}

let getValidDirections (position: Coord) (map: string[,]) =
    let directions = [[|-1; 0|]; [|1; 0|]; [|0; -1|]; [|0; 1|]]
    let validDirections = directions |> List.filter (fun direction ->
        let row = position.row + direction.[0]
        let col = position.col + direction.[1]
        row >= 0 && row < map.GetLength(0) && col >= 0 && col < map.GetLength(1) && map.[row, col] <> "."
    )
    validDirections

let getDirectionsPositions (position: Coord) (map: string[,]) =
    let availableDirections = getValidDirections position map
    availableDirections |> List.map (fun direction ->
        let row = position.row + direction.[0]
        let col = position.col + direction.[1]
        { coord = { row = row; col = col }; value = map.[row, col]; connections = [] }        
    )

let getValueOfDirection (position: Coord) (map: string[,]) =
    if position.row >= 0 && position.row < map.GetLength(0) && position.col >= 0 && position.col < map.GetLength(1) then 
        [{ coord = position; value = map.[position.row, position.col]; connections = [] }]
    else
        []

let getStartDirections (position: Coord) (map: string[,]) =
    let availableDirections = getValidDirections position map
    let startDirections = seq {
        for d in availableDirections do
            let posToCheck = { row = position.row + d.[0]; col = position.col + d.[1] }
            let value = getValueOfDirection posToCheck map
            if value.Length > 0 then                
                match d, value.Head.value with
                | [|-1; 0|], p when ["|"; "7"; "F"] |> List.contains p -> yield d
                | [|0; -1|], p when ["-"; "L"; "F"] |> List.contains p -> yield d
                | [|0; 1|], p when ["-"; "J"; "7"] |> List.contains p -> yield d
                | [|1; 0|], p when ["|"; "J"; "L"] |> List.contains p -> yield d
                | _ -> ()
    }
    startDirections |> List.ofSeq

let getConnections (input: string) (position: Coord) (map: string[,])=
    let directions =
        match input with
        | "|" -> [[|-1; 0|]; [|1; 0|]]
        | "-" -> [[|0; -1|]; [|0; 1|]]
        | "L" -> [[|-1; 0|]; [|0; 1|]]
        | "J" -> [[|-1; 0|]; [|0; -1|]]
        | "7" -> [[|1; 0|]; [|0; -1|]]
        | "F" -> [[|1; 0|]; [|0; 1|]]
        | "S" -> getStartDirections position map
        | _ -> []

    let connections = seq {
        for d in directions do
            let posToCheck = { row = position.row + d.[0]; col = position.col + d.[1] }
            let value = getValueOfDirection posToCheck map
            if value.Length > 0 then
                yield value.Head
    }
    connections |> List.ofSeq

let buildMap (input: string list) =
    let map = new Dictionary<(int * int), Position>()
    let array = Array2D.create (input.Length) (input.[0].Length) "."
    for rowIdx in 0..input.Length - 1 do
        for colIdx in 0..input.[0].Length - 1 do
            let value = input.[rowIdx].[colIdx].ToString()
            array[rowIdx, colIdx] <- value
    for rowIdx in 0..input.Length - 1 do
        for colIdx in 0..input.[0].Length - 1 do
            let value = array[rowIdx, colIdx]
            if value <> "." then
                let coord = { row = rowIdx; col = colIdx }
                let conn = getConnections value coord array
                map.Add((rowIdx, colIdx), { coord = coord; value = value; connections = conn })
    (map, array)

let printMap (map: string[,]) =
    for row in 0..map.GetLength(0) - 1 do
        for col in 0..map.GetLength(1) - 1 do
            printf "%s" map.[row, col]
        printfn ""

let containsPosition (position: Position) (visited: VisitedPosition list) =
    visited |> List.exists (fun v -> v.position.coord.row = position.coord.row && v.position.coord.col = position.coord.col)

let getNextPosition (curentPosition: Position) (map: Dictionary<(int * int), Position>) (visited: VisitedPosition list) =
    let connection = map.Values |> Seq.toList |> List.filter (fun c -> c.coord.row = curentPosition.coord.row && c.coord.col = curentPosition.coord.col) |> List.head
    let wasVisited = containsPosition connection visited
    match wasVisited with
    | true -> failwith "all visited"
    | false -> connection

let rec travelMap (startPosition: Position) (map: Dictionary<(int * int), Position>) (visited: VisitedPosition list) =
    let nextPosition = getNextPosition startPosition map visited
    //printfn "Visiting %A" nextPosition.value
    let visited' = { position = startPosition; visited = true }::visited
    let nextPosition' = nextPosition.connections |> List.filter (fun c -> not(containsPosition c visited'))
    match nextPosition' with
    | [] -> visited'
    | head :: _ -> travelMap head map visited'

let execute =
    //let path = "day10/test_input_01.txt"
    //let path = "day10/test_input_02.txt"
    //let path = "day10/test_input_03.txt"
    let path = "day10/day10_input.txt"
    let lines = ReadLines path |> List.ofSeq
    let maps = buildMap lines
    let start = (fst maps).Values |> Seq.find (fun p -> p.value = "S")
    let visited = travelMap start.connections.Head (fst maps) [{ position = start; visited = true }]
    //printMap (snd maps)
    visited.Length / 2