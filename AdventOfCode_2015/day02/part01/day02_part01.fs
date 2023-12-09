module day02_part01

open System.IO
open AdventOfCode_Utilities

let path = "day02/day02_input.txt"
let inputLine = GetLinesFromFile(path) |> Seq.map(fun x -> x.Split('x') |> Array.map int)

let calculatePaper(d: int[]) =
    let smallest = d |> Array.sort |> Array.take(2) |> Array.fold (*) 1
    (2 * d.[0] * d.[1] + 2 * d.[1] * d.[2] + 2 * d.[2] * d.[0]) + smallest

let execute =
    inputLine |> Seq.sumBy(fun p -> calculatePaper(p))