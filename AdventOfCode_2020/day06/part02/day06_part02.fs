﻿module day06_part02

open System.IO
open System.Collections.Generic
open Utilities

let path = "day06/day06_input.txt"

let inputLines = GetLinesFromFile(path) |> List.ofArray
let answers = getLinesGroupBySeparator2 inputLines ""

let execute =
    answers |> List.map (fun x -> commonElements2 (concatStringListSeparated x)) |> List.map (fun x -> List.length (Set.toList x)) |> List.reduce(+)