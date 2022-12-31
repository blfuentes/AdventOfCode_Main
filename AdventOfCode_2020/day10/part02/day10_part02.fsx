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
let compList = [0] @ inputLines @ [maxValue] |> List.pairwise |> List.map (diff) |> List.map string |> String.concat ""
let groups = compList.Split('3')
let groupsOf2 = groups |> Array.filter(fun c -> c.Length = 2) |> Array.length |> float
let groupsOf3 = groups |> Array.filter(fun c -> c.Length = 3) |> Array.length |> float
let groupsOf4 = groups |> Array.filter(fun c -> c.Length = 4) |> Array.length |> float

(2.0**groupsOf2 * 4.0**groupsOf3 * 7.0**groupsOf4) |> int64