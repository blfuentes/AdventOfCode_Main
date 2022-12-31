﻿open System.IO
open System.Collections.Generic
open System

#load @"../../Model/CustomDataTypes.fs"
#load @"../../Modules/Utilities.fs"

open Utilities
open CustomDataTypes

//let file = "test_input.txt"
let file = "day08_input.txt"
let path = __SOURCE_DIRECTORY__ + @"../../" + file
let inputLines = GetLinesFromFileFSI2(path)

let operations = 
    seq {
        for line in inputLines do
            let parts = line.Split(' ')
            let operation =
                match parts.[0] with
                | "acc" -> HandheldOpType.ACC
                | "jmp" -> HandheldOpType.JMP
                | "nop" -> HandheldOpType.NOP
                | _ -> HandheldOpType.MISSING
            yield {
                Op = operation;
                Offset = parts.[1] |> int
            }
    } |> Array.ofSeq

let rec calculateAccumulator (currentValue: int) (consumedOps: int list) (newOpIdx: int) (program: HandledOperation[]) =
    if consumedOps |> List.contains(newOpIdx) then
        currentValue
    else
        let newOp = program.[newOpIdx]
        match newOp.Op with
        | HandheldOpType.ACC -> calculateAccumulator (currentValue + newOp.Offset) (consumedOps @ [newOpIdx]) (newOpIdx + 1) program
        | HandheldOpType.JMP -> calculateAccumulator currentValue (consumedOps @ [newOpIdx]) (newOpIdx + newOp.Offset) program
        | HandheldOpType.NOP -> calculateAccumulator currentValue (consumedOps @ [newOpIdx]) (newOpIdx + 1) program
        | _ -> currentValue

let rec calculateAccumulatorComplex (currentValue: int) (consumedOps: int list) (newOpIdx: int) (program: HandledOperation[]) =
    if newOpIdx = program.Length then
        (true, currentValue)
    else
        match consumedOps |> List.contains(newOpIdx) with
        | true -> (false, currentValue)
        | false -> 
            let newOp = program.[newOpIdx]
            match newOp.Op with
            | HandheldOpType.ACC -> calculateAccumulatorComplex (currentValue + newOp.Offset) (consumedOps @ [newOpIdx]) (newOpIdx + 1) program
            | HandheldOpType.JMP -> calculateAccumulatorComplex currentValue (consumedOps @ [newOpIdx]) (newOpIdx + newOp.Offset) program
            | HandheldOpType.NOP -> calculateAccumulatorComplex currentValue (consumedOps @ [newOpIdx]) (newOpIdx + 1) program
            | _ -> (false, currentValue)

let checkOpIdx = operations |> Array.filter(fun o -> o.Op = HandheldOpType.JMP ||o.Op = HandheldOpType.NOP) |> Array.map(fun o -> Array.IndexOf(operations, o))

let rec loop (checkOpIdxList: int list) (program: HandledOperation[]) = 
    let op = program.[checkOpIdxList.Head]
    let newOp =
        match op.Op with
        | HandheldOpType.JMP -> { Op = HandheldOpType.NOP; Offset = op.Offset }
        | HandheldOpType.NOP -> { Op = HandheldOpType.JMP; Offset = op.Offset }
        | _ -> { Op = HandheldOpType.MISSING; Offset = op.Offset }
    let currentProgram = (updateElement checkOpIdxList.Head newOp (program |> List.ofArray)) |> Array.ofList
    let sub = calculateAccumulatorComplex 0 [] 0 currentProgram
    if not (fst sub) && checkOpIdxList.Length > 0 then 
        loop checkOpIdxList.Tail program
    else
        sub

let result = loop (checkOpIdx |> List.ofArray) operations         

let result2 =
    let rec loop (checkOpIdxList: int list) (program: HandledOperation[]) = 
        let op = program.[checkOpIdxList.Head]
        let newOp =
            match op.Op with
            | HandheldOpType.JMP -> { Op = HandheldOpType.NOP; Offset = op.Offset }
            | HandheldOpType.NOP -> { Op = HandheldOpType.JMP; Offset = op.Offset }
            | _ -> { Op = HandheldOpType.MISSING; Offset = op.Offset }
        let currentProgram = (updateElement checkOpIdxList.Head newOp (program |> List.ofArray)) |> Array.ofList
        let sub = calculateAccumulatorComplex 0 [] 0 currentProgram
        if not (fst sub) && checkOpIdxList.Length > 0 then 
            loop checkOpIdxList.Tail program
        else
            sub
    loop (checkOpIdx |> List.ofArray) operations  