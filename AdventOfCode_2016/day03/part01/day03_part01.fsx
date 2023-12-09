#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"

open System
open System.Collections.Generic

open AdventOfCode_Utilities

// let path = "day03/test_input_01.txt"
let path = "day03/day03_input.txt"

let isValidTriangle (sides: int[]) =
    sides.[0] + sides.[1] > sides.[2] &&
    sides.[1] + sides.[2] > sides.[0] &&
    sides.[0] + sides.[2] > sides.[1]

let inputLines = Utilities.GetLinesFromFile(path)
"  145  233  150"
inputLines 
    |> Array.map(fun t -> [|int(t.Substring(0, 5)); int(t.Substring(5, 5)); int(t.Substring(10, 5))|]) 
    |> Array.filter(fun e -> isValidTriangle e) |> Array.length