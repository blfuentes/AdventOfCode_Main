﻿module day14_part02

open AdventOfCode_Utilities
open AdventOfCode_2023.Modules

type FloorType = Rounded | Damaged | Empty

type Tile = { FloorType:FloorType; X:int; Y:int; InitialLoad: int }

let charToTile (c: char) (row: int) (col: int) (length: int)=
    let tTile =
        match c with
        | 'O' -> Rounded
        | '#' -> Damaged
        | _ -> Empty
    { FloorType = tTile; X = row; Y = col; InitialLoad = length - row }

let tileToChar (t:Tile) =
    match t.FloorType with
    | Rounded -> 'O'
    | Damaged -> '#'
    | Empty -> '.'

let parseGroup (lines:string list) =
    let rows = lines.Length
    let cols = lines.[0].Length
    let group = Array2D.zeroCreate<Tile> rows cols
    for i in 0..rows - 1 do
        for j in 0..cols - 1 do
            let value = lines.[i].[j]
            let tile = charToTile value i j rows
            group.[i,j] <- tile
    group

let gridToList (group: Tile[,]) =
    let rows = group.GetLength(0)
    let cols = group.GetLength(1)
    let list = List.init rows (fun i -> List.init cols (fun j -> group.[i,j]))
    list

let printGroup (group:Tile[,]) =
    for i in 0..group.GetLength(0)-1 do
        for j in 0..group.GetLength(1)-1 do
            printf "%c" (tileToChar group.[i,j])
        printfn ""

let rec findNearestTile (from: Tile) (tiles: Tile list) =
    match tiles with
    | head :: tail ->
        if head.FloorType = Empty then
            findNearestTile head tail
        else
            from
    | [] -> from

let getNewPos (tile: Tile) (map: Tile[,]) (direction: int[]) =
    match direction with
    | [| r; c |] when r = -1 && c = 0 ->
        let fullCol = map.[*, tile.Y] |> Array.filter (fun t -> t.X < tile.X) |> Array.toList |> List.rev
        findNearestTile tile fullCol
    | [| r; c |] when r = 1 && c = 0 ->
        let fullCol = map.[*, tile.Y] |> Array.filter (fun t -> t.X > tile.X) |> Array.toList
        findNearestTile tile fullCol
    | [| r; c |] when r = 0 && c = -1 ->
        let fullRow = map.[tile.X, *] |> Array.filter (fun t -> t.Y < tile.Y) |> Array.toList |> List.rev
        findNearestTile tile fullRow
    | [| r; c |] when r = 0 && c = 1 ->
        let fullRow = map.[tile.X, *] |> Array.filter (fun t -> t.Y > tile.Y) |> Array.toList
        findNearestTile tile fullRow
    | _ -> failwith "Not implemented"

let rec moveOnDirection (group:Tile[,]) (remainingTiles: Tile list) (x:int) (y:int) =
    match remainingTiles with
    | [] -> group
    | head :: tail ->
        let group' = group |> Array2D.copy
        let newPos = getNewPos head group [| x; y |]
        group'.[head.X, head.Y] <- { FloorType = Empty; X = head.X; Y = head.Y; InitialLoad = head.InitialLoad }
        group'.[newPos.X, newPos.Y] <- { FloorType = head.FloorType; X = newPos.X; Y = newPos.Y; InitialLoad = newPos.InitialLoad }
        moveOnDirection group' tail x y

let rec performCycle (group:Tile[,]) (roundedTiles: Tile list) (numOfCycles: int) =
    if numOfCycles = 0 then
        group
    else
        //printfn "Initial state"
        //printGroup group

        let afterNorth = moveOnDirection group roundedTiles -1 0
        let northRoundedTiles = gridToList afterNorth |> List.concat |> List.filter (fun t -> t.FloorType = Rounded)
        //printfn "After north"
        //printGroup afterNorth

        let afterWest = moveOnDirection afterNorth northRoundedTiles 0 -1
        let westRoundedTiles = gridToList afterWest |> List.concat |> List.filter (fun t -> t.FloorType = Rounded)
        //printfn "After west"
        //printGroup afterWest

        let afterSouth = moveOnDirection afterWest (westRoundedTiles |> List.rev) 1 0  
        let southRoundedTiles = gridToList afterSouth |> List.concat |> List.filter (fun t -> t.FloorType = Rounded)
        //printfn "After south"
        //printGroup afterSouth

        let afterEast = moveOnDirection afterSouth (southRoundedTiles |> List.rev) 0 1
        let eastRoundedTiles = gridToList afterEast |> List.concat |> List.filter (fun t -> t.FloorType = Rounded)
        //printfn "After east %i" numOfCycles
        //printGroup afterEast
        printfn "Reamianing cycles %i\n" numOfCycles
        performCycle afterEast eastRoundedTiles (numOfCycles - 1)

let execute =
    let path = "day14/test_input_01.txt"
    //let path = "day14/day14_input.txt"
    let lines = LocalHelper.ReadLines path |> Seq.toList
    let map = parseGroup lines
    //printGroup map
    let roundedTiles = gridToList map |> List.concat |> List.filter (fun t -> t.FloorType = Rounded)
    //let result = moveOnDirection map roundedTiles 0 0
    let result = performCycle map roundedTiles 1000000000
    let newRoundedTiles = gridToList result |> List.concat |> List.filter (fun t -> t.FloorType = Rounded)
    //printfn ""
    //printGroup result
    newRoundedTiles |> List.sumBy _.InitialLoad