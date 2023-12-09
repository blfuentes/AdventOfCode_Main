open System.IO

#load @"../../Model/CustomDataTypes.fs"
#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"

open AdventOfCode_Utilities
open CustomDataTypes

//let file = "test_input.txt"
let file = "day02_input.txt"
let path = "day02/" + file

let inputLines = List.ofSeq <| GetLinesFromFile(path)
let extract l =
    match l with
    | Regex @"(?<min>\d+)-(?<max>\d+) (?<elem>\w): (?<code>\w+)" [m; M; e; c] -> Some {min = m |> int; max = M |> int; element = e; code = c }
    | _ -> None

let inputCheck = inputLines |> List.map extract

let passwordIsValid(check: PasswordPolicy) : bool =
    let numberOfElements = check.code |> List.ofSeq |> List.map string |> List.filter (fun a -> a = check.element) |> List.length
    numberOfElements >= check.min && numberOfElements <= check.max

let numberOfValids = inputCheck |> List.filter (fun check -> passwordIsValid check.Value) |> List.length