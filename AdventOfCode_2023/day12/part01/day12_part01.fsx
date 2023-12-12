#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../../AdventOfCode_2023/Modules/LocalHelper.fs"
open System
open System.Collections.Generic

open AdventOfCode_2023.Modules
open AdventOfCode_Utilities

type ConditionRecord = {
    mask: char array
    definition: int array
}

let parseRecordEntry (entry: string) =
    let parts = entry.Split " "
    let mask = parts.[0].ToCharArray()
    let definition = parts.[1].Split(",") |> Array.map int
    { definition = definition; mask = mask }

let rec matchesDefinition (mask: char array) (testcase: char array) =
    match testcase.Length with
    | 0 -> true
    | _ ->
        if mask.[0] = testcase.[0] || mask.[0] = '?' then
            matchesDefinition (mask.[1..]) (testcase.[1..])
        else
            false

let rec generateCombinations (conRecord: ConditionRecord) (possibleSplits: int list list) (maxLength: int) (combinations: char array list)=
    match possibleSplits with
    | comb :: tail ->
        let start = String.replicate comb.[0] "."
        let firstPart = String.replicate conRecord.definition.[0] "#"
        let firstSplit = String.replicate comb.[1] "."
        let secondPart = String.replicate conRecord.definition.[1] "#"
        let secondSplit = String.replicate comb.[2] "."
        let thirdPart = String.replicate conRecord.definition.[2] "#"
        let ending = String.replicate comb.[3] "."
        let newDefinition = (start + firstPart + firstSplit + secondPart + secondSplit + thirdPart + ending).ToCharArray()
        let combinations' =
            if matchesDefinition conRecord.mask newDefinition then
                combinations @ [newDefinition]
            else
                combinations
        generateCombinations conRecord tail maxLength combinations'
    | [] -> 
        combinations

let getAllCombinations (lists: int list list) =
    combinationsOfLists lists

let countPossible (conRecord: ConditionRecord) (separators: int list list) =
    let maxLength = conRecord.mask.Length
    let valuesLength = conRecord.definition |> Array.sum
    let possibleSplitCombinations = separators |> List.filter(fun l -> l.[0] + l.[1] + l.[2] + l.[3] + valuesLength = maxLength)
    let validCombinations = generateCombinations conRecord possibleSplitCombinations maxLength []
    validCombinations.Length

//let sample = "???.### 1,1,3"

//matchesDefinition("???.###".ToCharArray()) ("#.#.###.".ToCharArray())

//let conRecord = parseRecordEntry sample
//let separators = getAllCombinations [[0..10];[1..10]; [1..10]; [0..10]]
//let result = countPossible conRecord separators

let execute =
    //let path = "day12/test_input_01.txt"
    //let path = "day12/test_input_02.txt"
    let path = "day12/day12_input.txt"    
    let lines = LocalHelper.ReadLines path |> Seq.map parseRecordEntry
    let separators = getAllCombinations [[0..10];[1..10]; [1..10]; [0..10]]
    let result = lines |> Seq.map(fun c -> countPossible c separators) |> Seq.sum
    result

execute