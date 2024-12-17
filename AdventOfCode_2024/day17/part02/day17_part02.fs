module day17_part02
open AdventOfCode_2024.Modules
open System.Collections.Generic

type OpCode =
    | Adv
    | Bxl
    | Bst
    | Jnz
    | Bxc
    | Out
    | Bdv
    | Cdv

type Register = {
    Name: string
    Value: int64
}

let opType(v: int64) =
    match v with
    | 0L -> Adv
    | 1L -> Bxl
    | 2L -> Bst
    | 3L -> Jnz
    | 4L -> Bxc
    | 5L -> Out
    | 6L -> Bdv
    | 7L -> Cdv
    | _ -> failwith "error"

let revOpType(v: OpCode) =
    match v with
    | Adv -> 0L
    | Bxl -> 1L
    | Bst -> 2L
    | Jnz -> 3L
    | Bxc -> 4L
    | Out -> 5L
    | Bdv -> 6L
    | Cdv -> 7L

let parseContent(lines: string) =
    let partregisters = lines.Split("\r\n\r\n")[0]
    let partprogram = lines.Split("\r\n\r\n")[1]
    let registers = Dictionary<string,int64>()
    let r' =
        partregisters.Split("\r\n")
        |> Array.iter(fun r ->
            let (n, v) = ((r.Split(" ")[1]).Replace(":",""),
                (int64)(r.Split(" ")[2]))
            registers.Add(n, v)
        )
    let ops = 
        (partprogram.Split(" ")[1]).Split(",")
        |> Array.map int64 |> Array.map opType
    (registers, ops)

let comboOperand(op: OpCode) (registers: Dictionary<string,int64>) =
    match op with
    | Adv -> 0L
    | Bxl -> 1L
    | Bst -> 2L
    | Jnz -> 3L
    | Bxc -> registers["A"]
    | Out -> registers["B"]
    | Bdv -> registers["C"]
    | Cdv -> failwith "error"

let literalOperand(op: OpCode) =
    match op with
    | Adv -> 0
    | Bxl -> 1
    | Bst -> 2
    | Jnz -> 3
    | Bxc -> 4
    | Out -> 5
    | Bdv -> 6
    | Cdv -> 7
    
let performOp(op: OpCode) (opOp: OpCode) (registers: Dictionary<string,int64>) (pIdx: int)=
    let mutable pointerIdx = pIdx + 2
    let mutable output = -1L
    if op.IsAdv then
        let numerator = (float)(registers["A"])
        let denominator = System.Math.Pow(2, (float)(comboOperand opOp registers))
        registers["A"] <- (int64)(numerator / denominator)
    elif op.IsBxl then
        registers["B"] <- registers["B"] ^^^ (literalOperand opOp)
    elif op.IsBst then
        let numerator = comboOperand opOp registers
        registers["B"] <- numerator % 8L
    elif op.IsJnz then
        if registers["A"] <> 0 then
            pointerIdx <- literalOperand opOp
    elif op.IsBxc then
        registers["B"] <- registers["B"] ^^^ registers["C"]
    elif op.IsOut then  
        output <- (comboOperand opOp registers) % 8L
    elif op.IsBdv then
        let numerator = (float)(registers["A"])
        let denominator = System.Math.Pow(2, (float)(comboOperand opOp registers))
        registers["B"] <- (int64)(numerator / denominator)
    elif op.IsCdv then
        let numerator = (float)(registers["A"])
        let denominator = System.Math.Pow(2, (float)(comboOperand opOp registers))
        registers["C"] <- (int64)(numerator / denominator)

    (pointerIdx, output)

let runProgram(ops: OpCode array)(registers: Dictionary<string,int64>) (ops2: int64 list) (ignorecheck: bool)  =
    let mutable pIdx = 0
    let mutable shouldContinue = true
    let mutable outputValues = []
    let mutable opIdx = 0
    while pIdx < ops.Length && shouldContinue do
        let (newPidx, output) = performOp ops[pIdx] ops[pIdx+1] registers pIdx
        pIdx <- newPidx
        if output <> -1 then
            if ignorecheck then
                outputValues <- outputValues @ [output]
                opIdx <- opIdx + 1
            else
                if output = ops2.Item(opIdx) then
                    outputValues <- outputValues @ [output]
                    opIdx <- opIdx + 1
                else
                    shouldContinue <- false
    outputValues

let rec findNewRegisterA (originalValue: int64) (currentValue: int64) (ops: OpCode array) (ops2: int64 list) (registers: Dictionary<string,int64>) =
    let newRegisters = Dictionary(registers)
    newRegisters["A"] <- currentValue
    if currentValue <> originalValue then
        let result = runProgram ops newRegisters ops2 false
        if result = ops2 then
            currentValue
        else
            findNewRegisterA originalValue (currentValue + 1L) ops ops2 newRegisters
    else
        findNewRegisterA originalValue (currentValue + 1L) ops ops2 newRegisters


let execute() =
    let path = "day17/day17_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let (registers, ops) = parseContent content
    let ops2 = ops |> Array.map revOpType |> List.ofArray
    let registerA = findNewRegisterA registers["A"] System.Int32.MaxValue ops ops2 registers
    registerA
