﻿module day06_part01

open AdventOfCode_Utilities
open AdventOfCode_2015.Modules.LocalHelper

let parseInstruction (instruction: string) =
    instruction.Split(' ') |> fun i -> (
        let value = 
            match i.[0] with
            | "turn" -> ((if i.[1] = "on" then 1 else 0), 0)
            | "toggle" -> (-1, -1)
            | _ -> (0, 0)
        [|fst value; int(i.[2 + snd value].Split(',').[0]); int(i.[2 + snd value].Split(',').[1]); int(i.[4 + snd value].Split(',').[0]); int(i.[4 + snd value].Split(',').[1])|]
    )

let parseInstructions (instructions: string seq) =
    instructions |> Seq.map(fun i -> parseInstruction i)

let rec executeInstruction (instructions: int array list) (grid: int[,]) =
    match instructions with
    | instruction::rest ->
        for col in instruction.[1]..instruction.[3] do
            for row in instruction.[2]..instruction.[4] do
                if instruction.[0] = -1 then
                    grid[row, col] <- if grid[row, col] = 0 then 1 else 0
                else
                    grid[row, col] <- instruction.[0]
        executeInstruction rest grid
    | [] -> grid


let countMatchingValue (grid: int[,]) (valueToCount: int) =
    grid |> Seq.cast<int> |> Seq.filter (fun x -> x = valueToCount) |> Seq.length

let execute =
    let path = "day06/day06_input.txt"    
    let inputLines = GetLinesFromFile(path) |>Seq.toList
    let instructions = parseInstructions inputLines |> Seq.toList
    let grid = Array2D.create 1000 1000 0
    let result = executeInstruction instructions grid
    countMatchingValue result 1