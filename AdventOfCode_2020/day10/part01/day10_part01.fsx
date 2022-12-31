#load @"../../Model/CustomDataTypes.fs"
#load @"../../Modules/Utilities.fs"

open Utilities
open CustomDataTypes

//let file = "test_input.txt"
let file = "day10_input.txt"
let path = __SOURCE_DIRECTORY__ + @"../../" + file
let inputLines = GetLinesFromFileFSI2(path) |> Array.map int |> Array.sort |> List.ofArray

let diff(a, b) =
    b - a

let maxValue = inputLines.[inputLines.Length - 1] + 3
let result = [0] @ inputLines @ [maxValue] |> List.pairwise |> List.map (diff) |> List.groupBy (fun x -> x)

printf "%i %A" (fst (result.Item(0))) (snd (result.Item(0))).Length
printf "%i %A" (fst (result.Item(1))) (snd (result.Item(1))).Length

(snd (result.Item(0))).Length * (snd(result.Item(1))).Length