module day01_part02

open AdventOfCode_2024.Modules
open System

let parseContent (lines: string array) =
    let pairs =
        lines
        |> Array.map(fun line -> 
            ((int)(line.Split(" ", StringSplitOptions.RemoveEmptyEntries)[0]),
                (int)(line.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1])))
    pairs
    
let countTimes (element: int*int) (searchlist: (int*int) array) =
    match searchlist |> Array.tryFind(fun a -> fst element = fst a) with
    | Some(result) -> (fst element) * (snd element)  * (snd result)
    | None -> 0

let execute =
    let path = "day01/day01_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let pairs = parseContent content
    let groupLeft = pairs |> Array.countBy(fun a -> fst a)
    let groupRight = pairs |> Array.countBy(fun a -> snd a)
    groupLeft |> Array.sumBy(fun l -> countTimes l groupRight)
    