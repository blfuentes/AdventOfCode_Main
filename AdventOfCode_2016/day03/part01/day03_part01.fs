module day03_part01

open System
open System.Collections.Generic

open AdventOfCode_Utilities

let path = "day03/day03_input.txt"

let isValidTriangle (sides: int[]) =
    sides.[0] + sides.[1] > sides.[2] &&
    sides.[1] + sides.[2] > sides.[0] &&
    sides.[0] + sides.[2] > sides.[1]

let execute =
    let inputLines = Utilities.GetLinesFromFile(path)
    inputLines 
        |> Array.map(fun t -> [|int(t.Substring(0, 5)); int(t.Substring(5, 5)); int(t.Substring(10, 5))|]) 
        |> Array.filter(fun e -> isValidTriangle e) |> Array.length