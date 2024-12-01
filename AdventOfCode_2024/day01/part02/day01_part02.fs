module day01_part02

open AdventOfCode_2024.Modules
open System

let parseContent (lines: string array) =
    let pairs =
        lines
        |> Array.map(fun line -> 
            ((int)(line.Split(" ", StringSplitOptions.RemoveEmptyEntries)[0]),
                (int)(line.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1])))
    (pairs |> Array.map fst), (pairs |> Array.map snd)
    
let countTimes (element: int*int) (searchlist: (int*int) array) =
    match searchlist |> Array.tryFind(fun a -> fst element = fst a) with
    | Some(result) -> (fst element) * (snd element)  * (snd result)
    | None -> 0

let execute =
    let path = "day01/day01_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let (lefts, rights) = parseContent content
    let groupLeft = lefts |> Array.countBy id
    let groupRight = rights |> Array.countBy id
    groupLeft |> Array.sumBy(fun l -> countTimes l groupRight)
    