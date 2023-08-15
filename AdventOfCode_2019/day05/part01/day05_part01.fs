module day05_part01

open System.IO

let filepath = __SOURCE_DIRECTORY__ + @"../../day05_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input.txt"
let values = File.ReadAllText(filepath).Split(',')
                |> Array.map int

let performOperation(input:int, idx: int, opDef: string array) (output: int) =
    let op = int(opDef.[4]) + int(opDef.[3]) * 10
    let param1Mode = int opDef.[2]
    let param2Mode = int opDef.[1]
    let param3Mode = int opDef.[0]

    match op with
    | 1 -> 
        let parameters = 
            match (param1Mode, param2Mode) with
            | (0, 0) -> (values.[values.[idx + 1]], values.[values.[idx + 2]])
            | (0, 1) -> (values.[values.[idx + 1]], values.[idx + 2])
            | (1, 0) -> (values.[idx + 1], values.[values.[idx + 2]])
            | (1, 1) -> (values.[idx + 1], values.[idx + 2])
            | (_, _) -> (0, 0)
        Array.set values values.[idx + 3] (fst parameters + snd parameters)
        ((4, true), output)
    | 2 -> 
        let parameters = 
            match (param1Mode, param2Mode) with
            | (0, 0) -> (values.[values.[idx + 1]], values.[values.[idx + 2]])
            | (0, 1) -> (values.[values.[idx + 1]], values.[idx + 2])
            | (1, 0) -> (values.[idx + 1], values.[values.[idx + 2]])
            | (1, 1) -> (values.[idx + 1], values.[idx + 2])
            | (_, _) -> (0, 0)
        Array.set values values.[idx + 3] (fst parameters * snd parameters)
        ((4, true), output)
    | 3 -> 
        let parameters = 
            match (param1Mode, param2Mode) with
            | (0, 0) -> (values.[idx + 1], 0)
            | (_, _) -> (0, 0)
        Array.set values (fst parameters) input
        ((2, true), output)
    | 4 -> 
        let parameters = 
            match (param1Mode, param2Mode) with
            | (0, 0) -> (values.[values.[idx + 1]], 0)
            | (_, _) -> (0, 0)
        ((2, true), fst parameters)
    | 99 -> ((0, false), output)
    | _ -> ((0, true), output)

let rec doStep (input: int) (continueLooping: bool) (increment: int) (idx: int) (output: int) =
    if continueLooping && idx < values.Length then
        let opDefinition = values.[idx].ToString().PadLeft(5, '0') |> Seq.toArray |> Array.map string
        let (resultOp, newOutput) = performOperation(input, idx, opDefinition) output
        doStep input (snd resultOp) (fst resultOp) (idx + (fst resultOp)) newOutput
    else
        output

let execute(input:int) =
    doStep input true 4 0 0