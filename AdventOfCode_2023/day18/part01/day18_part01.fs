module day18_part01

open AdventOfCode_2023.Modules
open System.Text.RegularExpressions
open System

type Direction = UP | RIGHT | DOWN | LEFT

type Instruction = { Direction: Direction; Steps: int; Color: string }

let dirRows = [| -1; 0; 1; 0 |]
let dirCols = [| 0; 1; 0; -1 |]

let rec digHole (ans: Instruction list) (directions: Direction[]) 
    (area: int) (border: int) (row: int) (nextRow: int) 
    (col: int) (nextCol: int) =
    match ans with
    | head :: tail ->
        let dIdx = Array.findIndex (fun d -> d = head.Direction) directions
        let newRow = row + dirRows[dIdx] * head.Steps
        let newCol = col + dirCols[dIdx] * head.Steps
        let newArea = area + (nextRow - newRow) * (nextCol + newCol) * 1
        let newBorder = border + head.Steps
        let newNextRow = newRow
        let newNextCol = newCol
        digHole tail directions newArea newBorder newRow newNextRow newCol newNextCol
    | [] ->
        let resultArea = area + (nextRow - row) * (nextCol + col)
        int ((float)border / 2. + Math.Abs((float)resultArea) / 2. + 1. )

let parseInput (input: string list) =
    input 
        |> List.map (fun l ->
            let parts = Regex.Match(l, @"([U|D|L|R])\s([\d]+)\s\(#([0-9a-fA-F]+)\)")
            { 
                Direction = 
                    match parts.Groups.[1].Value with
                    | "U" -> UP
                    | "D" -> DOWN
                    | "L" -> LEFT
                    | "R" -> RIGHT
                    | _ -> failwith "Unknown direction"
                ; Steps = int parts.Groups.[2].Value
                ; Color = parts.Groups.[3].Value
            }
    )

let execute =
    let path = "day18/day18_input.txt"
    let instructions = LocalHelper.ReadLines(path) |> List.ofSeq |> parseInput
    digHole instructions [| UP; RIGHT; DOWN; LEFT |] 0 0 0 0 0 0