module day03_part02

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

let isNumber (input: string): bool =
    match input with
    |Int i -> true 
    |_ -> false

let isSymbol (input: string): bool =
    match input with
    |Int i -> false 
    |_ -> if input = "." then false else true

let addToSymbols (symbols: string[]) (input: string) =
    if Array.contains input symbols then symbols
    else Array.append symbols [|input|]

let buildSchematicEngine (schematic: string[,]) (lines: string[]) (symbols: string[])=
    for i in 0..lines.Length-1 do
        for j in 0..lines.[0].Length-1 do
            schematic.[i,j] <- lines.[i].[j].ToString()

let printSchematic (schematic: string[,]) =
    for i in 0..schematic.GetLength(0)-1 do
        for j in 0..schematic.GetLength(1)-1 do
            printf "%s" schematic.[i,j]
        printfn ""

let checkSymbol(schematic: string[,]) (row: int) (col: int) =
    //printf "Check element %i %i " row col
    if (row >= 0 && col >= 0 && row < schematic.GetLength(0) && col < schematic.GetLength(1)) then
        let element = schematic.[row, col]
        //printf "Element %s " element
        if isSymbol element then 
            //printfn "is symbol"
            true 
        else 
            //printfn "is not symbol"
            false
    else
        //printfn "Out of bounds"
        false

let getGear (schematic: string[,]) (row: int) (col: int) =
    let currentSurrounding =
        seq {
            // left up
            let leftUp = schematic[row - 1,*] |> Array.sub(col - 2,)
            if checkSymbol schematic (row - 1) (col - 1) then
                yield true
            // up
            if checkSymbol schematic (row - 1) (col) then
                yield true
            // right up
            if checkSymbol schematic (row - 1) (col + 1) then
                yield true
            // left
            if checkSymbol schematic (row) (col - 1) then
                yield true
            // right
            if checkSymbol schematic (row) (col + 1) then
                yield true
            // left down
            if checkSymbol schematic (row + 1) (col - 1) then
                yield true
            // down 
            if checkSymbol schematic (row + 1) (col) then
                yield true
            // right down 
            if checkSymbol schematic (row + 1) (col + 1) then
                yield true
        } |> Seq.toList
    currentSurrounding

let processSchematic (schematic: string[,]) =
    let regex = Regex("(\*)")
    let numbers =
        seq {
            for i in 0..schematic.GetLength(0) - 1 do
                let matches = regex.Matches(String.Join("",schematic.[i,*]))
                let stars = matches |> Seq.map(fun m -> { number = m.Value; row = i; col = m.Index })
                
                for star in stars do
                    let checkGear = getGear schematic star.row star.col
                    if checkGear.Length = 2 then
                        //printfn "Found number %A" foundNumber
                        yield (checkGear |> List.reduce (*))
                    else
                        printfn "Symbol * at %i % i is not a gear" star.row star.col
        }
    numbers

let execute =
    let path = "day03/day03_input.txt"
    //let path = "day03/test_input_01.txt"
    //let path = "day03/test_input_02.txt"
    let lines = Utilities.GetLinesFromFile path
    let engineSchematic = Array2D.create lines.Length lines.[0].Length ""
    buildSchematicEngine engineSchematic lines [||]
    //printSchematic engineSchematic
    let linkedNumbers = processSchematic engineSchematic
    linkedNumbers |> Seq.sum