let rec lookAndSay (round: int) (max: int) (input: string) : string =
    printf "round: %A\n" round
    let rec lookAndSay' (consumed: string) (input: string list) : string =
        //printf "consumed: %A\n" consumed
        //printf "input: %A\n" input
        if input.IsEmpty then consumed
        else
            let initial = input |> List.takeWhile(fun x -> x = input.Head)
            //printfn "initial: %A" initial
            let remaining = input |> List.skipWhile(fun x -> x = input.Head)
            //printfn "remaining: %A" remaining
            let consumed' = initial.Length.ToString() + input.Head
            lookAndSay' (consumed + consumed') remaining
    if round = max then input
    else
        lookAndSay (round + 1) max (lookAndSay' "" (input.ToCharArray() |> Array.map string |> List.ofArray))

let result = lookAndSay 0 40 "1321131112"
printf "%i" result.Length