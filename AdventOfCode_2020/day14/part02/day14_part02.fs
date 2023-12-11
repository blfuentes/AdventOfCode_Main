module day14_part02

open System.IO
open System.Collections.Generic
open System

open AdventOfCode_Utilities
open CustomDataTypes
open AdventOfCode_2020.Modules

let path = "day14/day14_input.txt"

let inputLines = LocalHelper.GetLinesFromFile(path) 

let execute =
    0