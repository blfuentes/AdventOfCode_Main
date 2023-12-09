module day05_part01

open System.IO
open System.Collections.Generic
open AdventOfCode_Utilities
open AoC_2020.Modules


let path = "day05/day05_input.txt"
let inputLines = GetLinesFromFile(path)

let execute =
    inputLines |> List.ofArray |> List.map (fun s -> calculateBinarySeat s) |> List.max