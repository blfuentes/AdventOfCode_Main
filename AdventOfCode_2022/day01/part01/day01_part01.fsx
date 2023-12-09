open System.IO

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
open AdventOfCode_Utilities

// let path = "day01/test_input_01.txt"
let path = "day01/day01_input.txt"

let inputLines = GetLinesFromFile(path) |> Seq.toList
let elvesCalories = getLinesGroupBySeparatorWithEmptySpaceFixReplace inputLines "" |> List.map(fun l -> l |> (List.map int) |> List.sum)
elvesCalories |> List.max