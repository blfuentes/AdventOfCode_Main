namespace AoC_2019.Modules

open System.Collections.Generic
open System.Numerics

[<AutoOpen>]
module IntCodeModule = 
    type IntCodeResult = 
        {
            Idx: bigint
            Pause: bool
            Continue: bool
            LoopMode: bool
            Output: bigint
            Input: bigint
            RelativeBase: bigint
            AvailableInputs: bigint
        }

    let getInput(path: string) =
        let values = System.IO.File.ReadAllText(path).Split(',') |> Array.map (fun x -> BigInteger.Parse(x))
        let dataContainer = new Dictionary<bigint, bigint>()
        for idx in [|0..values.Length - 1|] do
            dataContainer.Add(bigint idx, values.[idx])
        dataContainer

    let getValue(values: Dictionary<bigint, bigint>, idx: bigint) =
        let found, value = values.TryGetValue idx
        match found with
        | true -> value
        | false -> 
            values.Add(idx, 0I)
            0I

    let getOperatorValue (values: Dictionary<bigint, bigint>) (relativeBase: bigint) (idx: bigint) (mode: int) =
        match mode with
        | 0 -> getValue(values, getValue(values, idx))
        | 1 -> getValue(values, idx)
        | 2 -> getValue(values, relativeBase + getValue(values, idx))
        | _ -> 0I

    let getOperatorAddress(values: Dictionary<bigint, bigint>) (relativeBase: bigint) (idx: bigint) (mode: int) =
        match mode with
        | 0 -> getValue(values, idx)
        | 1 -> idx
        | 2 -> relativeBase + getValue(values, idx)
        | _ -> 0I

    let setValue (values: Dictionary<bigint, bigint>) (idx: bigint) (newValue: bigint) =
        let found, value = values.TryGetValue idx
        match found with
        | true -> values.[idx] <- newValue
        | false -> values.Add(idx, newValue)

    let performOperation 
        (values: Dictionary<bigint, bigint>) (opDef: string array) (idx: bigint) 
        (relativeBase: bigint) (phase: bigint) (input:bigint) (availableInputs: bigint)
        (loopMode: bool) (output: bigint) =

        let op = int(opDef.[4]) + int(opDef.[3]) * 10
        let param1Mode = int opDef.[2]
        let param2Mode = int opDef.[1]
        let param3Mode = int opDef.[0]
    
        match op with
        | 1 -> // ADD
            let parameters = 
                (getOperatorValue values relativeBase (idx + 1I) param1Mode, getOperatorValue values relativeBase (idx + 2I) param2Mode)
            let writeAddress = getOperatorAddress values relativeBase (idx + 3I) param3Mode
            setValue values writeAddress (fst parameters + snd parameters)           
            { Idx = idx + 4I; Pause = false; Continue = true; LoopMode = loopMode; Output = output; Input = input; RelativeBase = relativeBase; AvailableInputs = availableInputs }

        | 2 -> // MULTIPLY
            let parameters = 
                (getOperatorValue values relativeBase (idx + 1I) param1Mode, getOperatorValue values relativeBase (idx + 2I) param2Mode)
            let writeAddress = getOperatorAddress values relativeBase (idx + 3I) param3Mode
            setValue values writeAddress (fst parameters * snd parameters)  
            { Idx = idx + 4I; Pause = false; Continue = true; LoopMode = loopMode; Output = output; Input = input; RelativeBase = relativeBase; AvailableInputs = availableInputs }

        | 3 -> // WRITE INPUT
            let writeAddress = getOperatorAddress values relativeBase (idx + 1I) param1Mode
            if availableInputs > 0I then
                setValue values writeAddress phase
                { Idx = idx + 2I; Pause = false; Continue = true; LoopMode = loopMode; Output = output; Input = input; RelativeBase = relativeBase; AvailableInputs = (availableInputs - 1I) }
            else
                { Idx = idx; Pause = true; Continue = true; LoopMode = loopMode; Output = output; Input = input; RelativeBase = relativeBase; AvailableInputs = availableInputs }

        | 4 -> // OUTPUT
            let output = getOperatorValue values relativeBase (idx + 1I) param1Mode
            { Idx = idx + 2I; Pause = loopMode; Continue = true; LoopMode = loopMode; Output = output; Input = input; RelativeBase = relativeBase; AvailableInputs = availableInputs }

        | 5 -> // JUMP IF TRUE
            let operator1 = getOperatorValue values relativeBase (idx + 1I) param1Mode
            let operator2 = getOperatorValue values relativeBase (idx + 2I) param2Mode
            match int(operator1) with
            | 0 -> 
                { Idx = idx + 3I; Pause = false; Continue = true; LoopMode = loopMode; Output = output; Input = input; RelativeBase = relativeBase; AvailableInputs = availableInputs }
            | _ -> 
                { Idx = operator2; Pause = false; Continue = true; LoopMode = loopMode; Output = output; Input = input; RelativeBase = relativeBase; AvailableInputs = availableInputs }

        | 6 -> // JUMP IF FALSE
            let operator1 = getOperatorValue values relativeBase (idx + 1I) param1Mode
            let operator2 = getOperatorValue values relativeBase (idx + 2I) param2Mode
            match int(operator1) with
            | 0 -> 
                { Idx = operator2; Pause = false; Continue = true; LoopMode = loopMode; Output = output; Input = input; RelativeBase = relativeBase; AvailableInputs = availableInputs }
            | _ -> 
                { Idx = idx + 3I; Pause = false; Continue = true; LoopMode = loopMode; Output = output; Input = input; RelativeBase = relativeBase; AvailableInputs = availableInputs }

        | 7 -> // LESS THAN
            let operator1 = getOperatorValue values relativeBase (idx + 1I) param1Mode
            let operator2 = getOperatorValue values relativeBase (idx + 2I) param2Mode
            let writeAddress = getOperatorAddress values relativeBase (idx + 3I) param3Mode
            if (operator1 < operator2) then setValue values writeAddress 1I
            else setValue values writeAddress 0I
            { Idx = idx + 4I; Pause = false; Continue = true; LoopMode = loopMode; Output = output; Input = input; RelativeBase = relativeBase; AvailableInputs = availableInputs }

        | 8 -> // EQUALS
            let operator1 = getOperatorValue values relativeBase (idx + 1I) param1Mode
            let operator2 = getOperatorValue values relativeBase (idx + 2I) param2Mode
            let writeAddress = getOperatorAddress values relativeBase (idx + 3I) param3Mode
            if (operator1 = operator2) then setValue values writeAddress 1I
            else setValue values writeAddress 0I
            { Idx = idx + 4I; Pause = false; Continue = true; LoopMode = loopMode; Output = output; Input = input; RelativeBase = relativeBase; AvailableInputs = availableInputs }
        
        | 9 -> // ADJUST RELATIVE BASE
            let operator1 = getOperatorValue values relativeBase (idx + 1I) param1Mode
            { Idx = idx + 2I; Pause = false; Continue = true; LoopMode = loopMode; Output = output; Input = input; RelativeBase = relativeBase + operator1; AvailableInputs = availableInputs }

        | 99 -> // EXIT
            { Idx = idx; Pause = false; Continue = false; LoopMode = loopMode; Output = output; Input = input; RelativeBase = relativeBase; AvailableInputs = availableInputs }

        | _ -> 
            { Idx = idx; Pause = false; Continue = false; LoopMode = loopMode; Output = output; Input = input; RelativeBase = relativeBase; AvailableInputs = availableInputs }

    let rec getOutput 
        (values: Dictionary<bigint, bigint>) (idx: bigint) 
        (relativeBase: bigint) (phase:bigint) (input:bigint) (availableInputs:bigint) (fixAvInput: bool)
        (loopMode: bool) (currentOutput:bigint) =
        let opDefinition = values.[idx].ToString().PadLeft(5, '0') |> Seq.toArray |> Array.map string 
        let resultOp = performOperation values opDefinition idx relativeBase phase input availableInputs loopMode currentOutput
        match resultOp.Continue with
        | true -> 
            getOutput values resultOp.Idx resultOp.RelativeBase phase input (if fixAvInput then 2I else resultOp.AvailableInputs) fixAvInput resultOp.LoopMode resultOp.Output
        | false -> 
            resultOp.Output