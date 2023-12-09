#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"

open System
open System.Collections.Generic

open AdventOfCode_Utilities

let rec processHistory (history: int array) (lasNumbers: int list) =
    let diffs = history |> Array.pairwise |> Array.map (fun (a, b) -> b - a)
    if diffs |> Array.forall ((=) 0) then
        lasNumbers
    else
        processHistory diffs (lasNumbers @ [diffs.[diffs.Length - 1]])

let execute = 
    //let path = "day09/test_input_01.txt"
    let path = "day09/day09_input.txt"
    let lines = Utilities.GetLinesFromFile path
    let histories = lines 
                    |> Array.map (fun l -> l.Split(' ') |> Array.map int)

    let results = histories |> Array.map (fun h -> processHistory h [h.[h.Length - 1]])
    results |> Array.map (fun r -> r |> List.reduce(+)) |> Array.sum