module day14_part01

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

    | _ -> failwith "Not implemented"

let rec moveNorth (group:Tile[,]) (remainingTiles: Tile list) (x:int) (y:int) =
    match remainingTiles with
    | [] -> group
    | head :: tail ->
        let group' = group |> Array2D.copy
        let newPos = getNewPos head group [|-1;0|]
        group'.[head.X, head.Y] <- { FloorType = Empty; X = head.X; Y = head.Y; InitialLoad = head.InitialLoad }
        group'.[newPos.X, newPos.Y] <- { FloorType = head.FloorType; X = newPos.X; Y = newPos.Y; InitialLoad = newPos.InitialLoad }
        moveNorth group' tail x y

let printGroup (group:Tile[,]) =
    for i in 0..group.GetLength(0)-1 do
        for j in 0..group.GetLength(1)-1 do
            printf "%c" (tileToChar group.[i,j])
        printfn ""

let execute =
    let path = "day14/day14_input.txt"
    let lines = LocalHelper.ReadLines path |> Seq.toList
    let map = parseGroup lines
    let roundedTiles = gridToList map |> List.concat |> List.filter (fun t -> t.FloorType = Rounded)
    let result = moveNorth map roundedTiles 0 0
    let newRoundedTiles = gridToList result |> List.concat |> List.filter (fun t -> t.FloorType = Rounded)
    newRoundedTiles |> List.sumBy _.InitialLoad