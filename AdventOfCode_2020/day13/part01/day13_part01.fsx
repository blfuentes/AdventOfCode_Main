open System.Collections.Generic

#load @"../../Model/CustomDataTypes.fs"
#load @"../../Modules/Utilities.fs"

open Utilities
open CustomDataTypes

//let file = "test_input.txt"
let file = "day13_input.txt"
let path = __SOURCE_DIRECTORY__ + @"../../" + file
let inputLines = GetLinesFromFileFSI2(path)

let initTime = inputLines.[0] |> int
let buses = inputLines.[1].Split(',') |> Array.filter(fun b -> b <> "x") |> Array.map int
let times = buses |> Array.map(fun b -> ((initTime / b) * b) + b)
let minTime = times |> Array.min
let bus = buses.[times |> Array.findIndex(fun t -> t = minTime)]
let result = bus * (minTime - initTime)