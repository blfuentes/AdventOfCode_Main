module day18_part02

open AdventOfCode_2023.Modules
open System.Text.RegularExpressions
open System

let (bigint_) = bigint.Parse

type Direction = UP | RIGHT | DOWN | LEFT

type Instruction = { Direction: Direction; Steps: bigint; Color: string }

let calculateFromHex(input: string) =
    let valueTo = input.Substring(0, input.Length - 1)
    (bigint_)(Int32.Parse(valueTo, System.Globalization.NumberStyles.HexNumber).ToString())

//calculateFromHex"70c710"

let dirRows = [| -1I; 0I; 1I; 0I |]
let dirCols = [| 0I; 1I; 0I; -1I |]

let rec digHole (ans: Instruction list) (directions: string[]) 
    (area: bigint) (border: bigint) (row: bigint) (nextRow: bigint) 
    (col: bigint) (nextCol: bigint) =
    match ans with
    | head :: tail ->
        let steps = calculateFromHex(head.Color)
        let dIdx = Array.findIndex (fun d -> d = head.Color.Substring(head.Color.Length - 1)) directions
        let newRow = row + dirRows[dIdx] * steps
        let newCol = col + dirCols[dIdx] * steps
        let newArea = area + (nextRow - newRow) * (nextCol + newCol) * 1I
        let newBorder = border + steps
        let newNextRow = newRow
        let newNextCol = newCol
        digHole tail directions newArea newBorder newRow newNextRow newCol newNextCol
    | [] ->
        let resultArea = area + (nextRow - row) * (nextCol + col)
        bigint ((float)border / 2. + Math.Abs((float)resultArea) / 2. + 1. )

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
                ; Steps = bigint_ parts.Groups.[2].Value
                ; Color = parts.Groups.[3].Value
            }
    )

let execute =
    let path = "day18/day18_input.txt"
    let instructions = LocalHelper.ReadLines(path) |> List.ofSeq |> parseInput
    digHole instructions [| "3"; "0"; "1"; "2" |] 0I 0I 0I 0I 0I 0I