open System.IO
open System.Collections.Generic

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Model/CustomDataTypes.fs"
#load @"../../Modules/Helpers.fs"

open AoC_2020.Modules.Helpers
open AdventOfCode_Utilities

//let file = "test_input.txt"
let file = "day04_input.txt"
let path = "day04/" + file
let inputLines = GetLinesFromFile(path) |> List.ofArray

let allFields = [|"byr"; "iyr"; "eyr"; "hgt"; "hcl"; "ecl"; "pid"; "cid"|]
let requiredFields = [|"byr"; "iyr"; "eyr"; "hgt"; "hcl"; "ecl"; "pid"|]

let passportList = new List<List<string>>()
passportList.Add(new List<string>())

let values = getLinesGroupBySeparator2 inputLines ""

let passPortIsValid (credentials: string list) =
    requiredFields |> Array.forall (fun field -> credentials |> List.exists(fun cred -> cred.StartsWith(field)))

let validPassports = values |> List.filter(fun p -> passPortIsValid p) |> List.length

validPassports