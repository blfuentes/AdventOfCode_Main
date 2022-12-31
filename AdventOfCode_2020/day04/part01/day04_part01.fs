﻿module day04_part01

open System.IO
open System.Collections.Generic
open Utilities


let path = "day04/day04_input.txt"
let inputLines = GetLinesFromFile(path) |> List.ofArray

let allFields = [|"byr"; "iyr"; "eyr"; "hgt"; "hcl"; "ecl"; "pid"; "cid"|]
let requiredFields = [|"byr"; "iyr"; "eyr"; "hgt"; "hcl"; "ecl"; "pid"|]

let values = getLinesGroupBySeparator2 inputLines ""

let passPortIsValid (credentials: string list) =
    requiredFields |> Array.forall (fun field -> credentials |> List.exists(fun cred -> cred.StartsWith(field)))

let execute =
    values |> List.filter(fun p -> passPortIsValid p) |> List.length