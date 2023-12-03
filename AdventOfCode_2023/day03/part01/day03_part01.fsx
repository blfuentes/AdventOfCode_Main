#load @"../../Modules/Utilities.fs"

open System
open System.Collections.Generic

open AdventOfCode_2023.Modules

let path = "day03/test_input_01.txt"
//let path = "day03/day03_input.txt"

let lines = Utilities.GetLinesFromFile path


let buildSchematicEngine (schematic: string[,]) (lines: string[]) =
    for i in 0..lines.Length-1 do
        for j in 0..lines.[0].Length-1 do

            schematic.[i,j] <- lines.[i].[j].ToString()

let printSchematic (schematic: string[,]) =
    for i in 0..schematic.GetLength(0)-1 do
        for j in 0..schematic.GetLength(1)-1 do
            printf "%s" schematic.[i,j]
        printfn ""

let input = ".......12.......935............184.720."
let regex = System.Text.RegularExpressions.Regex("(\d+)")
let matches = regex.Matches(input)
let numbers = [|for m in matches -> m.Index|]
numbers

let engineSchematic = Array2D.create lines.Length lines.[0].Length ""
buildSchematicEngine engineSchematic lines
printSchematic engineSchematic