module day01_part01

open AdventOfCode_2024.Modules
open System

let parseContent (lines: string array) =
    let pairs =
        lines
        |> Array.map(fun line -> 
            ((int)(line.Split(" ", StringSplitOptions.RemoveEmptyEntries)[0]),
                (int)(line.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1])))
    (pairs |> Array.map fst), (pairs |> Array.map snd)

let execute =
    let path = "day01/day01_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let (lefts, rights) = parseContent content
    Array.zip(lefts |> Array.sort) (rights |> Array.sort) |>  Array.sumBy (fun (a, b) -> abs (a - b))