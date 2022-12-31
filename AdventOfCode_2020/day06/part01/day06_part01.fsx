open System.IO
open System.Collections.Generic
open System

#load @"../../Model/CustomDataTypes.fs"
#load @"../../Modules/Utilities.fs"

open Utilities
open CustomDataTypes

let file = "test_input.txt"
//let file = "day06_input.txt"
let path = __SOURCE_DIRECTORY__ + @"../../" + file
let inputLines = GetLinesFromFileFSI2(path) |> List.ofArray

let concatStringList (list:string list) =
    seq {
        for l in list do
            yield! l.ToCharArray()
    } |> List.ofSeq

let answers = getLinesGroupBySeparator2 inputLines ""
let result = answers |> List.map (fun x -> concatStringList x) |> List.map (List.distinct) |> List.map (List.length) |> List.reduce (+)