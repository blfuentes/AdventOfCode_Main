module day01_part02

open System
open System.Collections.Generic

open AdventOfCode_2023.Modules

let path = "day01/day01_input.txt"

let findFirstIndexOfDigit (input: string) (digit: string * string) =
    let indexDigit = input.IndexOf(fst digit)
    let indexWord = input.IndexOf(snd digit)
    match (indexDigit, indexWord) with
    | (-1, _) -> indexWord
    | (_, -1) -> indexDigit
    | (d, w) -> if d < w then  d else w

let findLastIndexOfDigit (input: string) (digit: string * string) =
    let indexDigit = input.LastIndexOf(fst digit)
    let indexWord = input.LastIndexOf(snd digit)
    match (indexDigit, indexWord) with
    | (-1, _) -> indexWord
    | (_, -1) -> indexDigit
    | (d, w) -> if d > w then  d else w

let getFirstAndLast (input: string) (digits: (string * string) list) =
    digits |> List.map (fun d -> [|(int)(fst d); findFirstIndexOfDigit input d; findLastIndexOfDigit input d|])

let buildNumber (input: int array list) =
    let first = (input |> List.filter(fun i -> i.[1] > -1) |> List.sortBy (fun i -> i.[1])).Head.[0]
    let last = (input |> List.filter(fun i -> i.[2] > -1) |> List.sortByDescending (fun i -> i.[2])).Head[0]
    first * 10 + last

let validDigits = [("1", "one"); ("2", "two"); ("3", "three"); ("4", "four"); ("5", "five"); ("6", "six"); ("7", "seven"); ("8", "eight"); ("9", "nine")]

let execute =
    let lines = Utilities.GetLinesFromFile path
    lines |> Array.map (fun l -> getFirstAndLast l validDigits) |> Array.sumBy buildNumber