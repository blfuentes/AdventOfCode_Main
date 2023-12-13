#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../../AdventOfCode_2023/Modules/LocalHelper.fs"

open System
open System.Collections.Generic

open AdventOfCode_2023.Modules
open AdventOfCode_Utilities

//let path = "day13/test_input_01.txt"
let path = "day13/day13_input.txt"

let parseGroup (line:string list) =
    let group = Array2D.create (line.Length - 1) (line.Length - 1) '0'
    for i in 0..group.GetLength(0) - 1 do
            for j in 0..group.GetLength(1) - 1 do
                if i < group.GetLength(0) - 1 && j < group.GetLength(1) - 1 then
                    group.[i,j] <- line.[i].[j]
    group

let printGroup (group:char[,]) =
    for i in 0..group.GetLength(0)-1 do
        for j in 0..group.GetLength(1)-1 do
            printf "%c" group.[i,j]
        printfn ""

let execute =
    let path = "day13/test_input_01.txt"
    //let path = "day13/day13_input.txt"
    let lines = LocalHelper.ReadLines path |> Seq.toList
    let groups = getGroupsOnSeparator lines System.Environment.NewLine
    let maps = groups |> List.map parseGroup
    maps |> List.iter printGroup
    0