﻿module day01_part01

open AoC_2022.Modules

let path = "day01/day01_input.txt"

let execute =
    let inputLines = GetLinesFromFile(path) |> Seq.toList
    let elvesCalories = getLinesGroupBySeparatorWithEmptySpaceFixReplace inputLines "" |> List.map(fun l -> l |> (List.map int) |> List.sum)
    elvesCalories |> List.max