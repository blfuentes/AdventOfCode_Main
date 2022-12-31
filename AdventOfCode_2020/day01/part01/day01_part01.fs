﻿module day01_part01
open System.IO
open Utilities

let path = "day01/day01_input.txt"
let inputLines = GetLinesFromFile(path) |> List.ofArray |> List.map (int)

let execute =
    let pairs = combination(2, inputLines)
    let pair2020 =  pairs |> List.find (fun ele -> List.sum ele = 2020)
    pair2020 |> List.fold (*) 1