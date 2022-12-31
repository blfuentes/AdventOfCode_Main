module day02_part02

open System.IO
open Utilities
open CustomDataTypes


let path = "day02/day02_input.txt"
let inputLines = List.ofArray <| GetLinesFromFile(path)

let extract l =
    match l with
    | Regex @"(?<min>\d+)-(?<max>\d+) (?<elem>\w): (?<code>\w+)" [m; M; e; c] -> Some {min = m |> int; max = M |> int; element = e; code = c }
    | _ -> None

let inputCheck = inputLines |> List.map extract
 
let passwordIsValid(check: PasswordPolicy) : bool =
    let checkCode = check.code |> Array.ofSeq
    (checkCode.[check.min-1] = (check.element |> char)) ^@ (checkCode.[check.max-1] = (check.element |> char))

let execute =
    inputCheck |> List.filter (fun check -> passwordIsValid check.Value) |> List.length