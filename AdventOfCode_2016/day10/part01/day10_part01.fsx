#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Collections.Generic
open System.Text.RegularExpressions
open AdventOfCode_Utilities
open AdventOfCode_2016.Modules
open System.Numerics

let path = "day10/test_input_01.txt"
//let path = "day10/day10_input.txt"

type targetType =
    | Bot of int
    | Output of int

type instruction =
    | Value of botid: int * value: int
    | SetBot of botid: int * lowtarget: targetType * hightarget: targetType

let parseInstructions (lines: string array) =
    let pattern = "(?<type>(value|bot)) (?<numbers>\d+)[aA-zZ ]*(?<lowtype>(bot|output)) (?<lowtarget>\d+)(([aA-zZ ]*((?<hightype>(bot|output)) (?<hightarget>\d+)))|)"
    let regex = new Regex(pattern)
    lines
    |> Array.map(fun l -> 
        let found = regex.Match(l)
        match found.Success with
        | false -> failwith "cannot parse line to instruction"
        | true ->
            let ins = found.Groups["type"].Value
            let idValue = (int)found.Groups["numbers"].Value
            let target1 = (int)found.Groups["lowtarget"].Value
            if ins = "value" then
                Value(target1, idValue)
            else
                let target1Type = if found.Groups["lowtype"].Value = "bot" then Bot(target1) else Output(target1)
                let target2 = (int)found.Groups["hightarget"].Value
                let target2Type = if found.Groups["hightype"].Value = "bot" then Bot(target2) else Output(target2)
                SetBot(idValue, target1Type, target2Type)
    )

let rec runInstructions (instructions: instruction list) =
    match

let lines = LocalHelper.GetLinesFromFile path

parseInstructions lines
