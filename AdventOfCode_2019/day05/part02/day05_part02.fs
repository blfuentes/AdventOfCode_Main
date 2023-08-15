module day05_part02
open System.IO

let filepath = __SOURCE_DIRECTORY__ + @"../../day05_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input.txt"
let values = File.ReadAllText(filepath).Split(',')
                |> Array.map int

let performOperation(input:int, idx: int, opDef: string array) (output: int)=
    let op = int(opDef.[4]) + int(opDef.[3]) * 10
    let param1Mode = int opDef.[2]
    let param2Mode = int opDef.[1]
    let param3Mode = int opDef.[0]

    match op with
    | 1 -> // ADD
        let parameters = 
            match (param1Mode, param2Mode) with
            | (0, 0) -> (values.[values.[idx + 1]], values.[values.[idx + 2]])
            | (0, 1) -> (values.[values.[idx + 1]], values.[idx + 2])
            | (1, 0) -> (values.[idx + 1], values.[values.[idx + 2]])
            | (1, 1) -> (values.[idx + 1], values.[idx + 2])
            | (_, _) -> (0, 0)
        Array.set values values.[idx + 3] (fst parameters + snd parameters)
        ((4, true), output)
    | 2 -> // MULTIPLY
        let parameters = 
            match (param1Mode, param2Mode) with
            | (0, 0) -> (values.[values.[idx + 1]], values.[values.[idx + 2]])
            | (0, 1) -> (values.[values.[idx + 1]], values.[idx + 2])
            | (1, 0) -> (values.[idx + 1], values.[values.[idx + 2]])
            | (1, 1) -> (values.[idx + 1], values.[idx + 2])
            | (_, _) -> (0, 0)
        Array.set values values.[idx + 3] (fst parameters * snd parameters)
        ((4, true), output)
    | 3 -> // WRITE INPUT
        let parameters = 
            match (param1Mode, param2Mode) with
            | (0, 0) -> (values.[idx + 1], 0)
            | (_, _) -> (0, 0)
        Array.set values (fst parameters) input
        ((2, true), output)
    | 4 -> // OUTPUT
        let parameters = 
            match (param1Mode, param2Mode) with
            | (0, 0) -> (values.[values.[idx + 1]], 0)
            | (_, _) -> (0, 0)
        //printfn "%d" (fst parameters)
        ((2, true), fst parameters)
    | 5 -> // JUMP IF TRUE
        let parameters = 
            match (param1Mode, param2Mode) with
            | (0, 0) -> (values.[values.[idx + 1]], values.[values.[idx + 2]])
            | (0, 1) -> (values.[values.[idx + 1]], values.[idx + 2])
            | (1, 0) -> (values.[idx + 1], values.[values.[idx + 2]])
            | (1, 1) -> (values.[idx + 1], values.[idx + 2])
            | (_, _) -> (0, 0)

        match fst parameters with
        | 0 -> ((3, true), output)
        | _ -> (((snd parameters) - idx, true), output)
    | 6 -> // JUMP IF FALSE
        let parameters = 
            match (param1Mode, param2Mode) with
            | (0, 0) -> (values.[values.[idx + 1]], values.[values.[idx + 2]])
            | (0, 1) -> (values.[values.[idx + 1]], values.[idx + 2])
            | (1, 0) -> (values.[idx + 1], values.[values.[idx + 2]])
            | (1, 1) -> (values.[idx + 1], values.[idx + 2])
            | (_, _) -> (0, 0)

        match fst parameters with
        | 0 -> (((snd parameters) - idx, true), output)
        | _ -> ((3, true), output)
    | 7 -> // LESS THAN
        let parameters = 
            match (param1Mode, param2Mode) with
            | (0, 0) -> (values.[values.[idx + 1]], values.[values.[idx + 2]])
            | (0, 1) -> (values.[values.[idx + 1]], values.[idx + 2])
            | (1, 0) -> (values.[idx + 1], values.[values.[idx + 2]])
            | (1, 1) -> (values.[idx + 1], values.[idx + 2])
            | (_, _) -> (0, 0)

        if (fst parameters < snd parameters) then Array.set values values.[idx + 3] 1
        else Array.set values values.[idx + 3] 0
        ((4, true), output)
    | 8 -> // EQUALS
        let parameters = 
            match (param1Mode, param2Mode) with
            | (0, 0) -> (values.[values.[idx + 1]], values.[values.[idx + 2]])
            | (0, 1) -> (values.[values.[idx + 1]], values.[idx + 2])
            | (1, 0) -> (values.[idx + 1], values.[values.[idx + 2]])
            | (1, 1) -> (values.[idx + 1], values.[idx + 2])
            | (_, _) -> (0, 0)

        if (fst parameters = snd parameters) then Array.set values values.[idx + 3] 1
        else Array.set values values.[idx + 3] 0
        ((4, true), output)
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