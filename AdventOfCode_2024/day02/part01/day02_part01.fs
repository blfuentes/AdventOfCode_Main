module day02_part01

open AdventOfCode_2024.Modules

let parseContent (lines: string array) =
    lines
    |> Array.map(fun line -> line.Split(" ") |> Array.map int |> List.ofArray)

let areSafeInc(i: int list) =
    i |> List.pairwise |> List.forall(fun (a, b) -> b - a >= 1 && b - a <= 3)   

let areSafeDec(i: int list) =
    i |> List.pairwise |> List.forall(fun (a, b) -> a - b >= 1 && a - b <= 3)   

let execute =
    let path = "day02/day02_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let values = parseContent content
    values
    |> Array.filter(fun line -> areSafeInc <| line || line |> areSafeDec)
    |> Array.length