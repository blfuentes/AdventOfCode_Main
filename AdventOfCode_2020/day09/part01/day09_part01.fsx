open System.IO
open System.Collections.Generic
open System

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Model/CustomDataTypes.fs"
#load @"../../Modules/Helpers.fs"

open AoC_2020.Modules.Helpers
open AdventOfCode_Utilities

//let file = "test_input.txt"
let file = "day09_input.txt"
let path = "day09/" + file
let inputLines = GetLinesFromFile(path) |> Array.map (fun x -> Convert.ToUInt64(x)) |> List.ofArray

let preambleSize = 25

let numberIsValid (value: uint64) (listChecker: uint64 list) =
    let permu = combination 2 listChecker
    (permu |> List.exists(fun x -> x.Item(0) + x.Item(1) = value), value)

let rec findInvalidValue (elements: uint64 list) (preamble: int) : (bool * uint64)=
    match elements.Length = 0 with
    | true -> (false, uint64(0))
    | false ->
        let checkList = elements |> List.take(preamble + 1)
        let valid = numberIsValid (checkList |> List.rev |> List.head) (checkList |> List.take(preamble))
        match fst valid with 
        | false -> (true, snd valid)
        |  true -> findInvalidValue (elements |> List.skip(1)) preamble

let result = findInvalidValue inputLines preambleSize