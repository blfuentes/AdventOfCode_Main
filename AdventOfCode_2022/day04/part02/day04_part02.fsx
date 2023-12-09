﻿open System.IO

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
open AdventOfCode_Utilities

// let path = "day04/test_input_01.txt"
let path = "day04/day04_input.txt"
let inputLines = Utilities.GetLinesFromFile(path) |> Seq.toList

let elvesPairs = inputLines |> List.map(fun l -> [[|(int)(l.Split(',').[0].Split('-').[0])..(int)(l.Split(',').[0].Split('-').[1])|]; [|(int)(l.Split(',').[1].Split('-').[0])..(int)(l.Split(',').[1].Split('-').[1])|]])

let isOverlapped (lists: list<array<int>>) =
    let commonElements = Utilities.commonElements lists
    commonElements.Count > 0
    // lists |> List.exists(fun l -> l.Length = commonElements.Count)    

let elvesOverlapped = elvesPairs |> List.sumBy(fun e -> if (isOverlapped e) then 1 else 0 )