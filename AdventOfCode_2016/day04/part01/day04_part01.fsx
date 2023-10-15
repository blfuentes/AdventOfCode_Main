#load @"../../Modules/Utilities.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_2016.Modules

//let path = "day04/test_input_01.txt"
let path = "day04/day04_input.txt"

let getParts(input: string) =
    let input = "aaaaa-bbb-z-y-x-123[abxyz]"
    let pattern = @"^(.*?)-(\d+)\[(.*?)\]$"
    let regex = Regex(pattern)

    match regex.Match(input) with
    | matchResult when matchResult.Success ->
        let groups = matchResult.Groups
        let stringValue = groups.[1].Value
        let numberValue = groups.[2].Value
        let bracketsContent = groups.[3].Value
        printfn "String: %s, Number: %s, Content in brackets: %s" stringValue numberValue bracketsContent
    | _ ->
        printfn "No match found in the input string."


let inputLines = Utilities.GetLinesFromFile(path)
getParts "aaaaa-bbb-z-y-x-123[abxyz]"
