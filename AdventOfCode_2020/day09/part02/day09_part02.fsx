open System.IO
open System.Collections.Generic
open System

#load @"../../Model/CustomDataTypes.fs"
#load @"../../Modules/Utilities.fs"

open Utilities
open CustomDataTypes

//let file = "test_input.txt"
let file = "day09_input.txt"
let path = __SOURCE_DIRECTORY__ + @"../../" + file
let inputLines = GetLinesFromFileFSI2(path) |> Array.map (fun x -> Convert.ToUInt64(x)) |> List.ofArray

let preambleSize = 25

let numberIsValid (value: uint64) (listChecker: uint64 list) =
    let permu = combination (2, listChecker)
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

let testList  = [uint64(0)..uint64(20)]
let possibleCombinations (combSize: int) (mainList: uint64 list) =
    seq {
        for init in mainList do
            let index = mainList |> List.findIndex(fun e -> e = init)
            if mainList.Length - combSize > index then
                yield mainList |> List.skip(index) |> List.take(combSize)
    } |> List.ofSeq

possibleCombinations 3 testList

let rec findWeakness (value: uint64) (combSize: int) (listChecker: uint64 list) : (uint64 * uint64) =
    let permu = possibleCombinations combSize listChecker
    let found = (permu |> List.filter(fun l -> l |> List.sum = value))
    match found.Length > 0 with
    | true -> (found.Head |> List.min, found.Head |> List.max)
    | false -> findWeakness value (combSize + 1) listChecker

let result = findInvalidValue inputLines preambleSize
let result2 = findWeakness (snd result) 2 (inputLines)
fst result2 + snd result2