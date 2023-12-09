#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"

open System
open System.Collections.Generic

open AdventOfCode_Utilities

//let path = "day04/test_input_01.txt"
let path = "day04/day04_input.txt"

let input = Utilities.GetLinesFromFile path
let groups = input |> Array.map (fun l -> l.Split(' ') |> Array.countBy id)
            |> Array.filter(fun g -> g |> Array.forall(fun (k, c) -> c = 1)) |> Array.length