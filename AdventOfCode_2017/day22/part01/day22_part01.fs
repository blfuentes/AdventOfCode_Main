module day22_part01

open AdventOfCode_2017.Modules

open System.Collections.Generic

type Status =
    | Clean
    | Infected

type Direction =
    | Up
    | Down
    | Left
    | Right

let parseContent (lines: string array) =
    let (maxrows, maxcols) = (lines.Length / 2, lines[0].Length / 2)
    let map = Dictionary<(int * int), Status>()
    let (startrow, startcol) = (0 - maxrows, 0 - maxcols)
    let mutable row = 0
    let mutable col = 0
    for r' in startrow..maxrows do
        col <- 0
        for c' in startcol..maxcols do
            let status =
                match lines[row][col] with
                | '.' -> Clean
                | '#' -> Infected
                | _ -> failwith "error"
            map.Add((r', c'), status)
            col <- col + 1
        row <- row + 1

    map
let execute() =
    //let path = "day22/day22_input.txt"
    let path = "day22/test_input_22.txt"
    let content = LocalHelper.GetLinesFromFile path
    let map = parseContent content
    map.Count