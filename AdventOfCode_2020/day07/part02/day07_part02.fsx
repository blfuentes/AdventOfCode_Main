﻿open System.IO
open System.Collections.Generic
open System

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Model/CustomDataTypes.fs"
#load @"../../Modules/Helpers.fs"

open AoC_2020.Modules.Helpers
open AdventOfCode_Utilities

//let file = "test_input.txt"
let file = "day07_input.txt"
let path = "day07/" + file
let inputLines = GetLinesFromFile(path)

//let testLine ="light red bags contain 1 bright white bag, 2 muted yellow bags."
//testLine.Replace("bags", "").Replace("bag", "").Split([|"contain"|], StringSplitOptions.None)

//let testLine2 = "1 bright white"
//let element = 
//    match testLine2 with
//    | Regex @"(?<size>\d+)\s(?<color>\w+\s\w+)" [s; c] -> Some { Name = c.Trim(); Size = (s |> int); Content = [||] }
//    | _ -> None

let parseInput (input:string array) =
    seq {
        for line in input do
            let parts = line.Replace("bags", "").Replace("bag", "").Split([|"contain"|], StringSplitOptions.None)
            let content = 
                seq {
                    let contentList = parts.[1].Split(',') |> Array.map (fun x -> x.Trim())
                    for content in contentList do
                        let element = 
                            match content with
                            | Regex @"(?<size>\d+)\s(?<color>\w+\s\w+)" [s; c] -> Some { Name = c.Trim(); Size = (s |> int); Content = [] }
                            | _ -> None
                        if element.IsSome then
                            yield element.Value                        
                } |> List.ofSeq
            let element = 
                Some {
                    Name = parts.[0].Trim();
                    Size = 1;
                    Content = content
                }
            yield element.Value
    }

let rec countContainers (originalBag: ChristmasBag) (childcontainers: ChristmasBag list) (allcontainers: ChristmasBag list) =
    match childcontainers.IsEmpty with
    | true -> false
    | false ->
        match (childcontainers |> List.exists (fun c -> c.Name = originalBag.Name)) with
        | true -> true
        | false -> 
            (childcontainers |> List.map(fun c -> countContainers originalBag ((allcontainers |> List.find(fun s -> s.Name = c.Name)).Content) allcontainers) |> List.exists (fun c -> c))

let elements = parseInput inputLines |> List.ofSeq

let originalBag = { Name = "shiny gold"; Size = 1; Content = [] }

let rec countBags (currentCount: int) (childcontainers: ChristmasBag list) (allcontainers: ChristmasBag list)=
    match childcontainers.IsEmpty with
    | true -> currentCount
    | false -> 
        let childrenSize = childcontainers |> List.map (fun c -> c.Size + c.Size * countBags 0 ((allcontainers |> List.find(fun s -> s.Name = c.Name)).Content) allcontainers) |> List.sum
        currentCount + childrenSize

let shinyBag = elements |> List.filter(fun b -> b.Name = "shiny gold") |> List.head
let counting = countBags 0 shinyBag.Content elements

let result = 
    seq {
        for el in elements do
            if countContainers originalBag el.Content elements then
                yield el
    } |> List.ofSeq

result.Length
    