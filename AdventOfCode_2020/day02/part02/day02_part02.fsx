open System.IO

#load @"../../Model/CustomDataTypes.fs"
#load @"../../Modules/Utilities.fs"

open Utilities
open CustomDataTypes

//let file = "test_input.txt"
let file = "day02_input.txt"
let path = __SOURCE_DIRECTORY__ + @"../../" + file

let inputLines = List.ofSeq <| GetLinesFromFileFSI(path)
let extract l =
    match l with
    | Regex @"(?<min>\d+)-(?<max>\d+) (?<elem>\w): (?<code>\w+)" [m; M; e; c] -> Some {min = m |> int; max = M |> int; element = e; code = c }
    | _ -> None

let inputCheck = inputLines |> List.map extract
 
let passwordIsValid(check: PasswordPolicy) : bool =
    let checkCode = check.code |> Array.ofSeq
    (checkCode.[check.min-1] = (check.element |> char)) ^@ (checkCode.[check.max-1] = (check.element |> char))

let numberOfValids = inputCheck |> List.filter (fun check -> passwordIsValid check.Value) |> List.length