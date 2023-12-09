module day07_part01

open System.IO
open System.Collections.Generic
open AdventOfCode_Utilities
open CustomDataTypes
open AoC_2020.Modules

let path = "day07/day07_input.txt"

let inputLines = GetLinesFromFile(path)

let elements = parseBagsInput inputLines |> List.ofSeq

let originalBag = { Name = "shiny gold"; Size = 1; Content = [] }

let execute =
    seq {
        for el in elements do
            if countBagContainers originalBag el.Content elements then
                yield el
    } |> Seq.length