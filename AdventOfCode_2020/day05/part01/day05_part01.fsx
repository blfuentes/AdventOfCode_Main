open System.IO
open System.Collections.Generic
open System

#load @"../../Model/CustomDataTypes.fs"
#load @"../../Modules/Utilities.fs"

open Utilities
open CustomDataTypes

let file = "test_input.txt"
//let file = "test_input_complete.txt"
//let file = "day05_input.txt"
let path = __SOURCE_DIRECTORY__ + @"../../" + file
let inputLines = GetLinesFromFileFSI2(path)

let rec calculateSeat minRowCur maxRowCur minColCur maxColCur (index:int) (seatdefinition:string)=
    match index < seatdefinition.Length with
    | true -> 
        match seatdefinition.[index] with
        | 'F' -> calculateSeat minRowCur (minRowCur + (maxRowCur - minRowCur) / 2)  minColCur maxColCur (index + 1) seatdefinition
        | 'B' -> calculateSeat (minRowCur + (maxRowCur + 1 - minRowCur) / 2) maxRowCur minColCur maxColCur (index + 1) seatdefinition
        | 'L' -> calculateSeat minRowCur maxRowCur minColCur (minColCur + (maxColCur - minColCur) / 2) (index + 1) seatdefinition
        | 'R' -> calculateSeat minRowCur maxRowCur (minColCur + (maxColCur + 1 - minColCur) / 2) maxColCur  (index + 1) seatdefinition
        | _ -> 0
    | false -> minRowCur * 8 + minColCur

let numberOfRows = 128

let maxSeat = inputLines |> List.ofArray |> List.map (fun s -> calculateSeat 0 127 0 7 0 s) |> List.max
maxSeat

calculateSeat 0 127 0 7 0 inputLines.[0]
calculateSeat 0 127 0 7 0 "BFFFBBFRRR"
calculateSeat 0 127 0 7 0 "FFFBBBFRRR"
calculateSeat 0 127 0 7 0 "BBFFBBFRLL"
