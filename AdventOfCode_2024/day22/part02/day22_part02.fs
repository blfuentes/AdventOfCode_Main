module day22_part02

open AdventOfCode_2024.Modules
open System.Collections.Generic

let parseContent(lines: string array) =
    lines |> Array.map int64

let mix (value: int64) (cSecret: int64) = value ^^^ cSecret
let prune (value: int64) = value%16777216L  
let mix_prune v s = (mix v s) |> prune

let mul64 (value:int64) =
    mix_prune (value * 64L) value

let div32 (value: int64) =
    let tofloor = (float)(value / 32L)
    mix_prune ((int64)(floor tofloor)) value

let mul2048 (value: int64) =
    mix_prune (value * 2048L) value

let secret (value: int64) =
    value |> mul64 |> div32 |> mul2048

let calculateSecretNumber(number: int64) (th: int) (index: int) (mapOfBananaPrices: (int64*int64)[,])=
    let mix (value: int64) (cSecret: int64) = value ^^^ cSecret
    let prune (value: int64) = value%16777216L  
    let mix_prune v s = (mix v s) |> prune

    let mul64 (value:int64) =
        mix_prune (value * 64L) value

    let div32 (value: int64) =
        let tofloor = (float)(value / 32L)
        mix_prune ((int64)(floor tofloor)) value

    let mul2048 (value: int64) =
        mix_prune (value * 2048L) value

    let secret (value: int64) =
        value |> mul64 |> div32 |> mul2048
    
    let rec getSecret(currentth: int) (prev: int64) (current: int64) =
        match currentth = th with
        | true -> 
            current
        | false ->
            let newSecret = secret current
            let price = newSecret % 10L
            mapOfBananaPrices[index, currentth] <- (price, (price - prev))
            getSecret (currentth+1) price newSecret
    
    getSecret 0 0 number

let getMostBananasOverall(mapOfBananaPrices: (int64*int64)[,]) =
    let mapOfSeqs = Dictionary<(int64*int64*int64*int64), int64[]>()
    let (maxrows, maxcols) = (mapOfBananaPrices.GetLength(0), mapOfBananaPrices.GetLength(1))
    for row in 0..(maxrows-1) do
        let mutable (first, second, third) = 
            (snd mapOfBananaPrices[row, 0], snd mapOfBananaPrices[row, 1], snd mapOfBananaPrices[row, 2])
        for col in 3..(maxcols-1) do
            let fourth = snd mapOfBananaPrices[row, col]
            let key = (first, second, third, fourth)
            if not (mapOfSeqs.ContainsKey(key)) then
                mapOfSeqs.Add(key, Array.create maxrows 0L)
            if mapOfSeqs[key][row] = 0L then
                mapOfSeqs[key][row] <- fst mapOfBananaPrices[row, col]
            first <- second
            second <- third
            third <- fourth

    let mutable maxNumber = 0L
    for numbers in mapOfSeqs.Values do
        let sumofSeq = numbers |> Array.sum
        if sumofSeq > maxNumber then maxNumber <- sumofSeq

    maxNumber

let execute() =
    let path = "day22/day22_input.txt"

    let content = LocalHelper.GetLinesFromFile path
    let numbers = parseContent content
    
    let mapOfBananaPrices = Array2D.create content.Length 2000 (0L, 0L)
    
    let _ =
        numbers
        |> Array.mapi(fun idx s -> calculateSecretNumber s 2000 idx mapOfBananaPrices)
    getMostBananasOverall mapOfBananaPrices