#load @"../../Modules/Utilities.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_2023.Modules

let path = "day01/test_input_02.txt"
//let path = "day01/day01_input.txt"

let lines = Utilities.GetLinesFromFile path

let findIndexOfDigit (input: string) (digit: string) =
    let matches = Regex.Matches(input, $"\d|{digit}")
    if matches.Count > 0 then
        matches |> Seq.cast<Match> |> Seq.map(fun m -> (m.Index, m.Value)) |> Seq.toList
    else
        [(-1, "")]

let validDigits = ["0"; "1"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "9"; "one"; "two"; "three"; "four"; "five"; "six"; "seven"; "eight"; "nine"]

let convertNumber (input: string) =
    match input with
    | "one" -> "1"
    | "two" -> "2"
    | "three" -> "3"
    | "four" -> "4"
    | "five" -> "5"
    | "six" -> "6"
    | "seven" -> "7"
    | "eight" -> "8"
    | "nine" -> "9"
    | _ -> input

let findNumbers input =
    let indexes = validDigits |> List.map (fun d -> findIndexOfDigit input d) |> List.concat
    indexes |> List.sortBy fst

let check = findNumbers "six7sixqrdfive3twonehsk"

//let getNumbersFromLine(line: string) =
//    let numbers = findNumbers line
//    if numbers.Length > 0 then (int)(numbers.Head + numbers.Item(numbers.Length - 1))
//    else 0

//let values = lines |> Array.map getNumbersFromLine
//values
//let totalSum = Array.sumBy getNumbersFromLine lines
//totalSum
    

