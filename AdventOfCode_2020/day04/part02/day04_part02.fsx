open System.IO
open System.Collections.Generic
open System

#load @"../../Model/CustomDataTypes.fs"
#load @"../../Modules/Utilities.fs"

open Utilities
open CustomDataTypes

//let file = "test_input.txt"
//let file = "test_input_invalid.txt"
//let file = "test_input_valid.txt"
//let file = "test_input_valid01.txt"
//let file = "test_input_valid02.txt"
//let file = "test_input_valid03.txt"
//let file = "test_input_valid04.txt"
let file = "day04_input.txt"
let path = __SOURCE_DIRECTORY__ + @"../../" + file
let inputLines = GetLinesFromFileFSI2(path) |> List.ofArray

let allFields = [|"byr"; "iyr"; "eyr"; "hgt"; "hcl"; "ecl"; "pid"; "cid"|]
let requiredFields = [|"byr"; "iyr"; "eyr"; "hgt"; "hcl"; "ecl"; "pid"|]

let passportList = new List<List<string>>()
passportList.Add(new List<string>())

let values = getLinesGroupBySeparator inputLines ""

let passPortIsValid (credentials: string list) =
    let allFieldsRequired = requiredFields |> Array.forall (fun field -> credentials |> List.exists(fun cred -> cred.StartsWith(field)))
    let cred = credentials |> Array.ofSeq
    let valueIsCorret = cred |> Array.forall (fun field ->
        let parts = field.Split(':')
        match parts with
        | [|"byr"; thevalue|] -> byrValid thevalue
        | [|"iyr"; thevalue|] -> iyrValid thevalue
        | [|"eyr"; thevalue|] -> eyrValid thevalue
        | [|"hgt"; thevalue|] -> hgtValid thevalue
        | [|"hcl"; thevalue|] -> hclValid thevalue
        | [|"ecl"; thevalue|] -> eclValid thevalue
        | [|"pid"; thevalue|] -> pidValid thevalue
        | _ -> true
    ) 
    valueIsCorret && allFieldsRequired

let validPassports  = values |> List.filter(fun p -> passPortIsValid p)
validPassports |> List.length