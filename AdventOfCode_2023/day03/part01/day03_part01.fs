module day03_part01

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_2023.Modules

type FoundNum = {
    number: string
    row: int
    col: int
}

let (|Int|_|) (str:string) =
    match System.Int32.TryParse str with
    | true,int -> Some int
    | _ -> None

let isSymbol (input: string): bool =
    match input with
    |Int i -> false 
    |_ -> if input = "." then false else true

let buildSchematicEngine (schematic: string[,]) (lines: string[]) =
    for i in 0..lines.Length-1 do
        for j in 0..lines.[0].Length-1 do
            schematic.[i,j] <- lines.[i].[j].ToString()

let checkSymbol(schematic: string[,]) (row: int) (col: int) =
    if (row >= 0 && col >= 0 && row < schematic.GetLength(0) && col < schematic.GetLength(1)) then
        let element = schematic.[row, col]
        if isSymbol element then 
            true 
        else 
            false
    else
        false

let isLinked (schematic: string[,]) (row: int) (col: int) (number: string) : bool =
    let currentSurrounding =
        seq {
            for j in (col)..(col + number.Length - 1) do
                // left up
                if checkSymbol schematic (row - 1) (j - 1) then
                    yield true
                // up
                if checkSymbol schematic (row - 1) (j) then
                    yield true
                // right up
                if checkSymbol schematic (row - 1) (j + 1) then
                    yield true
                // left
                if checkSymbol schematic (row) (j - 1) then
                    yield true
                // right
                if checkSymbol schematic (row) (j + 1) then
                    yield true
                // left down
                if checkSymbol schematic (row + 1) (j - 1) then
                    yield true
                // down 
                if checkSymbol schematic (row + 1) (j) then
                    yield true
                // right down 
                if checkSymbol schematic (row + 1) (j + 1) then
                    yield true
                    
        } |> Seq.toList
    currentSurrounding.Length > 0

let processSchematic (schematic: string[,]) =
    let regex = Regex("(\d+)")
    let numbers =
        seq {
            for i in 0..schematic.GetLength(0) - 1 do
                let matches = regex.Matches(String.Join("",schematic.[i,*]))
                let numbers = matches |> Seq.map(fun m -> { number = m.Value; row = i; col = m.Index })
                
                for num in numbers do
                    if isLinked schematic num.row num.col num.number then
                        let foundNumber = (int)num.number
                        yield foundNumber
        }
    numbers

let execute =
    let path = "day03/day03_input.txt"
    let lines = Utilities.GetLinesFromFile path
    let engineSchematic = Array2D.create lines.Length lines.[0].Length ""
    buildSchematicEngine engineSchematic lines
    let linkedNumbers = processSchematic engineSchematic
    linkedNumbers |> Seq.sum