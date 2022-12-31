﻿module day01_part02
open System.IO
open Utilities

let path = "day01/day01_input.txt"
let inputLines = GetLinesFromFile(path) |> List.ofArray |> List.map (int)

let execute =
    let comb3 = combination(3, inputLines)
    let comb2020 =  comb3 |> List.find (fun ele -> List.sum ele = 2020)
    comb2020 |> List.fold (*) 1