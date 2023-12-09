﻿open System.IO
open System.Collections.Generic
open System

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Model/CustomDataTypes.fs"
#load @"../../Modules/Helpers.fs"

open AoC_2020.Modules.Helpers
open AdventOfCode_Utilities

let file = "test_input.txt"
//let file = "day06_input.txt"
let path = "day06/" + file
let inputLines = GetLinesFromFile(path) |> List.ofArray

let concatStringList (list:string list) =
    seq {
        for l in list do
            yield! l.ToCharArray()
    } |> List.ofSeq

let answers = getLinesGroupBySeparator2 inputLines ""
let result = answers |> List.map (fun x -> concatStringList x) |> List.map (List.distinct) |> List.map (List.length) |> List.reduce (+)