module day22_part01

open AdventOfCode_2024.Modules

let parseContent(lines: string array) =
    lines |> Array.map int64

let calculateSecretNumber(number: int64) (th: int) =
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
        
    let rec getSecret(currentth: int) (current: int64) =
        match currentth = th with
        | true -> 
            current
        | false ->
            let newSecret = secret current
            getSecret (currentth+1) newSecret
    
    getSecret 0 number

let execute() =
    let path = "day22/day22_input.txt"

    let content = LocalHelper.GetLinesFromFile path
    let numbers = parseContent content
    let secretnumbers =
        numbers
        |> Array.map(fun s -> calculateSecretNumber s 2000)
    secretnumbers |> Array.sum