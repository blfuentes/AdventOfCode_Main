module day04_part01

open System
open System.Collections.Generic

open AdventOfCode_Utilities

let path = "day04/day04_input.txt"

let execute =
    let input = Utilities.GetLinesFromFile path
    input |> Array.map (fun l -> l.Split(' ') |> Array.countBy id) 
        |> Array.filter(fun g -> g |> Array.forall(fun (k, c) -> c = 1)) |> Array.length