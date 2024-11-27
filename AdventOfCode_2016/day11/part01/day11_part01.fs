module day11_part01

open System.Text.RegularExpressions
open AdventOfCode_2016.Modules

type Element =
    | Generator of string
    | Microchip of string
    | Nothing

let buildElement (value: string) =
    let parts = value.Split(" ")
    match parts[1] with
    | "generator" -> Generator(parts[0])
    | "microchip" -> Microchip(parts[0].Split("-")[0])
    | _ -> Nothing
        

let parseLine (line: string) =
    let pattern = @"(\b[\w-]+\b)\s(?:generator|microchip|relevant)"
    let match' = Regex.Matches(line, pattern)
    let elements =
        match'
        |> Seq.map (fun m -> buildElement m.Value)
        |> List.ofSeq
    elements

let parseContent (lines: string array) =
    let elements = lines |> Array.mapi(fun idx line -> (idx, parseLine line))
    elements

let execute =
    let path = "day11/day11_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let elementsbyfloor = parseContent content
    elementsbyfloor.Length