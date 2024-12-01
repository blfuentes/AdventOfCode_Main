module day01_part01

open AdventOfCode_2024.Modules
open System

let parseContent (lines: string array) =
    let pairs =
        lines
        |> Array.map(fun line -> 
            ((int)(line.Split(" ", StringSplitOptions.RemoveEmptyEntries)[0]),
                (int)(line.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1])))
    pairs

let execute =
    let path = "day01/day01_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let pairs = parseContent content
    let lefts = pairs |> Array.map(fun p -> fst p) |> Array.sort
    let rights = pairs |> Array.map(fun p-> snd p) |> Array.sort

    let pairs = (lefts, rights)
    Array.zip(fst pairs) (snd pairs) |>  Array.sumBy(fun a -> abs((fst a) - (snd a)))