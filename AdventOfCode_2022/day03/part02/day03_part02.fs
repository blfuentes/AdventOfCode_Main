﻿module day03_part02

open AdventOfCode_Utilities

let path = "day03/day03_input.txt"

let getValueOfElement (input: char) =
    let minorletters = ['a'..'z'] |> List.mapi(fun idx c -> (c, idx + 1))
    let capitalleters = ['A'..'Z'] |> List.mapi(fun idx c -> (c, idx + 27))
    snd ((minorletters @ capitalleters) |> List.find(fun e -> (fst e) = input))

let execute =
    let inputLines = Utilities.GetLinesFromFile(path) |> Array.toList
    let groupOfRucksacks = inputLines |> List.chunkBySize 3 |> List.map(fun l -> l |> List.map(fun s -> s.ToCharArray()))
    groupOfRucksacks |> 
                    List.map Utilities.commonElements |> 
                    List.map(fun c -> c |> Seq.toList) |> 
                    List.sumBy(fun l -> l |> List.sumBy(fun s -> getValueOfElement s))