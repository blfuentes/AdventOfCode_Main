﻿module day10_part02

open AdventOfCode_2017.Modules

let rec processLengths (roundsleft: int) (state: int array) (initiallengths: int list) (lengths: int list) (currentskip: int) (currentindex: int) =
    match lengths with
    | [] -> 
        if roundsleft > 0 then
            processLengths (roundsleft - 1) state initiallengths initiallengths currentskip currentindex
        else
            state
    | totake :: rest ->
        let indexes = 
            seq {
                for idx in [currentindex..currentindex + totake - 1] do
                    yield (state[idx % state.Length], idx % state.Length)
            } |> Array.ofSeq

        Array.iter2(fun el rev -> state[snd el] <- fst rev) indexes (indexes |> Array.rev)

        processLengths roundsleft state  initiallengths rest (currentskip + 1) ((currentindex + totake + currentskip) % state.Length)

let execute =
    let path = "day10/day10_input.txt"
    let text = (LocalHelper.GetContentFromFile path)
    let extra = "17, 31, 73, 47, 23".Split(", ") |> Array.map(fun v ->  int v) |> List.ofArray
    let content = List.concat([text.ToCharArray() |> Array.map int |> List.ofArray; extra])
    let size = 256
    let rounds = 64
    let circular = 
        (Array.create size 0) 
        |> Array.mapi(fun idx _ -> idx)
    processLengths (rounds - 1)  circular content content 0 0 |> ignore
    let hexadecimals = 
        circular 
            |> Array.chunkBySize(16)    
            |> Array.map(fun c -> sprintf "%02X" (c |> Array.reduce (^^^)))
    (String.concat "" hexadecimals).ToLowerInvariant()