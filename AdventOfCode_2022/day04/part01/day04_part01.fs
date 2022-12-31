﻿module day04_part01

open AoC_2022.Modules

let path = "day04/day04_input.txt"

let isFullyOverlapped (lists: list<array<int>>) =
    let commonElements = Utilities.commonElements lists
    lists |> List.exists(fun l -> l.Length = commonElements.Count)    

let execute =
    let inputLines = Utilities.GetLinesFromFile(path) |> Seq.toList
    let elvesPairs = inputLines |> List.map(fun l -> [[|(int)(l.Split(',').[0].Split('-').[0])..(int)(l.Split(',').[0].Split('-').[1])|]; [|(int)(l.Split(',').[1].Split('-').[0])..(int)(l.Split(',').[1].Split('-').[1])|]])
    elvesPairs |> List.sumBy(fun e -> if (isFullyOverlapped e) then 1 else 0 )