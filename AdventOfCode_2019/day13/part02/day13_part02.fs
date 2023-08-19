module day13_part02

open System.IO
open System.Collections.Generic
open AoC_2019.Modules
open System

type BlockType = EMPTY | WALL | BLOCK | PADDLE | BALL | NONE

let toBlock(v: bigint) =
    match int(v) with
    | 0 -> EMPTY
    | 1 -> WALL
    | 2 -> BLOCK
    | 3 -> PADDLE
    | 4 -> BALL
    | _ -> NONE

let getBlocksSeq(blocksoutput: List<bigint>) =
    let result = blocksoutput 
                    |> Seq.map int 
                    |> Seq.toList 
                    |> List.chunkBySize(3) 
                    |> List.filter (fun x -> x.Length = 3) 
                    |> List.groupBy (fun b -> toBlock(bigint(b.[2])))
                    |> List.map (fun (block, coordinates) -> (block, coordinates, coordinates.Length))
    result

let rec executeNext(values: Dictionary<bigint, bigint>, relativeBase: bigint, input:bigint, idx:bigint, numberOfInputs: bigint, alloutputs: List<bigint>) =
    let outputResult = IntCodeModule.getOutput values idx relativeBase [input] true 0I
    alloutputs.Add(outputResult.Output)
    match outputResult.Continue with
    | false -> outputResult.Output
    | true -> executeNext(values, outputResult.RelativeBase, input, outputResult.Idx, 1I, alloutputs)


//let getBlockScore(blocksoutput: List<bigint>) =
//    let result = blocksoutput 
//                    |> Seq.toList 
//                    |> List.chunkBySize(3) 
//                    |> List.filter (fun x -> x.Length = 3) 
//                    |> List.map List.toArray
//                    |> List.tryFind (fun x -> x.[0] = -1I && x.[1] = 0I )
//    match result with
//    | None -> 0I
//    | Some x -> x.[2]

let round(values: Dictionary<bigint, bigint>, relativeBase: bigint, input:bigint, idx:bigint, numberOfInputs: bigint) = 
    let alloutputs = new List<bigint>()
    let result = executeNext(values, 0I, 0I, 0I, 1I, alloutputs)
    let blocktypes = getBlocksSeq(alloutputs)
    let scoreElement = blocktypes |> List.tryFind(fun (b, l, s) -> b = BlockType.NONE) //getBlockScore(alloutputs)
    let score =
        match scoreElement with
        | None -> [|0; 0; 0|]
        | Some (b, l, s) -> l.[0] |> List.toArray
    let blocks = blocktypes |> List.tryFind (fun x -> 
        let (block, _, size) = x
        block = BlockType.BLOCK)
    let numberOfBlocks =
        match blocks with 
        | None -> 0
        | Some (b, l, s) -> s
    let (paddle, paddleCoordinates, numberOfPaddles) = blocktypes |> List.find (fun x -> 
        let (block, _, size) = x
        block = BlockType.PADDLE)
    let (ball, ballCoordinates, numberOfBalls) = blocktypes |> List.find (fun x -> 
        let (block, _, size) = x
        block = BlockType.BALL)
    //let coords = paddleCoordinates.[0] |> List.toArray
    //let nextInput = 
    //    match coords.[0]  with
    //    | direction when direction = 23 -> 0I
    //    | direction when direction < 23 -> -1I
    //    | direction when direction > 23 -> 1I
    //    | _ -> 0I
    //let nextInput =
    //    match paddleCoordinates.Head.[0] - (ballCoordinates |> List.rev).Head.[0] with
    //    | direction when direction = 0 -> 0I
    //    | direction when direction < 0 -> 1I
    //    | direction when direction > 0 -> -1I
    //    | _ -> 0I

    let nextInput = (new Random()).Next(-1, 1) |> bigint
        
    printfn "Score: %A with remaining blocks %A. Paddle: %A Ball(%A): %A" score numberOfBlocks paddleCoordinates[0] numberOfBalls ballCoordinates[0]
    (score.[2], nextInput, numberOfBlocks <> 0)

let rec executeRound (valuesArray: bigint array) (values: Dictionary<bigint, bigint>) (relativeBase: bigint) (input:bigint) (idx:bigint) (numberOfInputs: bigint) 
    (doContinue: bool) (finalScore: int) =
    if doContinue then
        let (score, nextInput, numberOfBlocks) = round(values, relativeBase, input, idx, numberOfInputs)
        Array.set valuesArray 0 nextInput
        executeRound valuesArray values relativeBase valuesArray.[0] 0I 1I numberOfBlocks score
    else
        finalScore

let execute =
    let filepath = __SOURCE_DIRECTORY__ + @"../../day13_input.txt"
    //let filepath = __SOURCE_DIRECTORY__ + @"../../day13_input_2.txt"
    let alloutputs = new List<bigint>()
    let values = IntcodeComputerModule.getInputBigData filepath

    let (score, nextInput, toContinue) = round(values, 0I, 0I, 0I, 1I)
    values.[0I] <- 2I
    let valuesArray = [|nextInput|]
    executeRound valuesArray values 0I valuesArray.[0] 0I 1I toContinue score
    //let mutable continueLooping = toContinue
    //let mutable finalScore = score
    //while continueLooping do
    //    let (score, nextInput, numberOfBlocks) = round(values, 0I, valuesArray.[0], 0I, 1I)
    //    continueLooping <- numberOfBlocks
    //    finalScore <- score
    //    Array.set valuesArray 0 nextInput

    //finalScore