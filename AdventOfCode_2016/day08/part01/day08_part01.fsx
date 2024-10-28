#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_2016.Modules
open AdventOfCode_Utilities

type Instruction =
    | Rect of columns: int * rows: int
    | RotateColumn of column: int * distance: int
    | RotateRow of row: int * distance: int

let path = "day08/test_input_01.txt"
//let path = "day08/day08_input.txt"

let parseLines (lines: string array) : Instruction list =
    let parseRect(line: string) : Instruction =
        let pattern = "(?<type>rect|rotate) (((?<column>\d)x(?<row>\d))|((?<axis>column|row) (x|y)=(?<index>\d) by (?<steps>\d)))"
        let regex = new Regex(pattern)
        let matches = regex.Match(line)
        match matches.Groups["type"].Value with
        | "rect" ->
            Rect((int)matches.Groups["column"].Value, (int)matches.Groups["row"].Value)
        | "rotate" ->
            if matches.Groups["axis"].Value = "column" then
                RotateColumn((int)matches.Groups["index"].Value, (int)matches.Groups["steps"].Value)
            else
                RotateRow((int)matches.Groups["index"].Value, (int)matches.Groups["steps"].Value)

    let parseLine (line: string) : Instruction =
        parseRect line

    lines |> Array.map parseLine |> Array.toList

let printMap (map: string[,]) =
    for row in [0..map.GetUpperBound(0) ] do
        for column in [0..map.GetUpperBound(1) ] do
            printf "%s" map[row, column]
        printfn ""

let executeRect (width: int) (tall: int) (map: string[,]) =
    for col in [0..width - 1] do
        for row in [0..tall - 1] do
            map[row, col] <- "#"

let executeRotateColumn (column: int) (distance: int) (map: string[,]) =
    let maxRows = map.GetUpperBound(0) + 1
    printfn "max rows %i" maxRows
    for row = (maxRows - 1) downto 0 do
        let currentValue = map[row - 1, column]
        printfn "row %i col %i value %s" ((row + distance)%maxRows) column currentValue
        map[(row + distance)%maxRows, column] <- currentValue
        map[row, column] <- "."

let executeRotateRow (row: int) (distance: int) (map: string[,]) =
    let maxColumns = map.GetUpperBound(1)
    for col in [0..maxColumns] do
        for idx in [0..distance] do
            let currentValue = map[row, col]
            printfn "row %i col %i value %s" row ((col + idx) % maxColumns) currentValue
            map[row, (col + idx) % maxColumns] <- currentValue
            map[row, col] <- "."

let rec runInstructions (instructions: Instruction list) (map: string[,]) =
    match instructions with
    | instruction::rest ->
        match instruction with
        | Rect (width,tall) -> executeRect width width map
        | RotateColumn(column, distance) ->  executeRotateColumn column distance map
        | RotateRow(row, distance) -> executeRotateRow row distance map
        printMap map
        runInstructions rest map
    | [] -> map

let lines = LocalHelper.GetLinesFromFile(path)
let rows = 3
let columns = 7
let map = Array2D.create rows columns "."
printMap map
let instructions = parseLines lines

executeRect 3 2 map
executeRotateColumn 1 1 map
executeRotateRow 0 4
executeRotateColumn 1 1

runInstructions instructions map