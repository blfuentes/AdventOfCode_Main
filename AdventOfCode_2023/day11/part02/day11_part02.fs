module day11_part02

open System
open System.Collections.Generic

open AdventOfCode_2023.Modules
open AdventOfCode_Utilities

let getExpandPoints (input: string list) =
    let allVerticalIdx = 
        seq {
            for col in [0..input.[0].Length - 1] do
                let allDots = input |> List.forall(fun l -> l.[col] = '.')
                if allDots then yield col
        } |> Seq.toList
    let allHorizontalIdx = 
        seq {
            for row in [0..input.Length - 1] do
                let allDots = input.[row].ToCharArray() |> Array.forall ((=) '.')
                if allDots then yield row
        } |> Seq.toList
    let generateCombinations (list1: int list) (list2: int list) =
        [for x in list1 do
            for y in list2 do
                yield (x, y)]
    (allHorizontalIdx, allVerticalIdx)
    //let expandPoints = generateCombinations allHorizontalIdx allVerticalIdx 
    //expandPoints |> List.map (fun p -> [|fst p; snd p|])

let buildMap (lines: string list) =
    let array = Array2D.create (lines.Length) (lines.[0].Length) '.'
    let galaxies = new Dictionary<int array, int>()
    for rowIdx in 0..lines.Length - 1 do
        for colIdx in 0..lines.[0].Length - 1 do
            let value = lines.[rowIdx].[colIdx]
            array.[rowIdx, colIdx] <- value
            if value = '#' then
                printfn "adding galaxy at %i, %i" rowIdx colIdx
                galaxies.Add([|rowIdx; colIdx|], galaxies.Count + 1)
    galaxies, array

let printMap (map: char[,]) =
    for rowIdx in 0..map.GetLength(0) - 1 do
        for colIdx in 0..map.GetLength(1) - 1 do
            printf "%c" map.[rowIdx, colIdx]
        printfn ""

let distance (a: int array) (b: int array) =
    Math.Abs(a.[0] - b.[0]) + Math.Abs(a.[1] - b.[1])

let calculateDistance (start: int array) (target: int array) (points: int list * int list) (range: int) =
    //let collisions = (points |> List.filter (fun p -> p.[0] >= start.[0] && p.[0] <= target.[0] && p.[1] >= start.[1] && p.[1] <= target.[1])).Length
    let vExpanded = ((snd points) |> List.filter (fun p -> p >= start.[1] && p <= target.[1])).Length
    let hExpanded = ((fst points) |> List.filter (fun p -> p >= start.[0] && p <= target.[0])).Length
    //let hCollisions = (points |> List.filter (fun p -> p.[0] >= start.[0] && p.[0] <= target.[0]).Length
    let newStart = [|start.[0]; start.[1]|]
    let newTarget = [|target.[0] + ((range - hExpanded) * hExpanded) ; target.[1] + ((range - vExpanded) * vExpanded)|]
    distance newStart newTarget

let execute =
    let path = "day11/test_input_01.txt"
    //let path = "day11/day11_input.txt"
    let lines = LocalHelper.ReadLines path |> List.ofSeq
    let pointsOfDouble = getExpandPoints lines
    let (galaxies, map) = buildMap lines
    //printMap map

    let combinations = Utilities.comb 2 (galaxies.Keys |> Seq.toList)
    let distances = combinations |> Seq.map (fun x -> calculateDistance x.[0] x.[1] pointsOfDouble 10) |> Seq.sum
    distances
    //printMap map