﻿module day06_part01

open System.IO
open System.Collections.Generic
open Utilities


let path = "day06/day06_input.txt"

let inputLines = GetLinesFromFile(path) |> List.ofArray
let answers = getLinesGroupBySeparator2 inputLines ""

let execute =
    answers |> List.map (fun x -> concatStringList x) |> List.map (List.distinct) |> List.map (List.length) |> List.reduce (+)